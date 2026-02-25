# C# – Domain Driven Design (DDD) & Persistence

## Forschungsfrage

**Wie unterstützen C# Records und Non-Nullable Reference Types die Implementierung von „Always-Valid“ Domain Models in komplexen Persistenz-Szenarien?**

Beispiel: Persistente Speicherung eines modellhaften Spielers mit Inventar.

---

# 1. Grundlagen DDD nach Evans

Begründer von DDD ist Eric Evans mit seinem Werk "Domain Driven Design"

DDD ist:

- Kein technisches Pattern-Set
- Ein modellzentrierter Ansatz
- Fokus auf Fachdomäne statt Infrastruktur
- Enge Kopplung zwischen Fachsprache und Code (Ubiquitous Language)

Zentrale Konzepte:

- **Entities**
- **Value Objects**
- **Aggregates**
- **Invarianten**

## Aggregate

Ein Aggregat ist eine Gruppe zusammengehöriger Objekte, die als konsistente Einheit behandelt werden.

Wichtig:

- Es gibt genau **eine Aggregate Root**
- Alle Änderungen laufen über diese Root
- Nur dort werden Invarianten garantiert

Beispiel:

Spieler → Level, Inventar, Items  
Aggregate Root: `PlayerProfile`

Warum?

- Spieler existiert eigenständig
- Inventar existiert nicht ohne Spieler
- Items existieren nicht ohne Spieler
- Alle fachlichen Regeln gehören zum Spieler

---

# 2. Always-Valid Domain Model

Always Valid bedeutet:

> Ein Objekt darf zu keinem Zeitpunkt einen ungültigen Zustand besitzen.

Ungültig = Verstoß gegen fachliche Regeln (Invarianten)

Invarianten:

- Sind fachliche Geschäftsregeln
- Müssen zu jedem Zeitpunkt wahr sein
- Sind Teil der Modellstruktur, nicht externe Validierung

Weiter ausgearbeitet u.a. von Vaughn Vernon in "Implementing Domain Driven Design"

## Warum ist das wichtig?

Persistenzprobleme:

- Daten können manipuliert oder korrupt sein
- Rehydration kann Konstruktorlogik umgehen
- Versionierung kann alte, ungültige Zustände enthalten

Ohne Always Valid:

- Verteilte Validierung
- Inkonsistente Aggregate
- Hohe Fehleranfälligkeit

---

# 3. Records in C# und ihre Rolle im DDD

Records besitzen folgende Eigenschaften:

- Wertbasierte Gleichheit
- Immutability (init-only / with-Expressions)
- Geeignet für Value Objects

## Value Objects im DDD

- Keine Identität
- Unveränderlich
- Gleichheit über Zustand
- Enthalten eigene Invarianten

Records unterstützen:

- Konstruktorvalidierung
- Strukturelle Gleichheit
- Immutable Zustand

→ Ideal für Always-Valid Value Objects

Wenn Validierung im Konstruktor erfolgt, kann das Objekt nie ungültig existieren.

---

# 4. Non-Nullable Reference Types (NNRT)

Vor C# 8:

- Jede Referenz war implizit nullable
- Fehler erst zur Laufzeit
- Pflichtfelder nicht garantiert

Seit C# 8:

- Explizite Unterscheidung zwischen `string` und `string?`
- Compiler erzwingt Initialisierung
- Fehler werden in die Compile-Zeit verschoben

## Beitrag zu Always Valid

Always Valid verlangt:

- Keine fehlenden Pflichtfelder
- Keine impliziten Null-Zustände
- Vollständig initialisierte Aggregate

NNRT:

- Verhindert uninitialisierte Felder
- Erzwingt vollständige Konstruktion
- Reduziert NullReferenceExceptions

---

## Kombination Records + NNRT

Records:
- Sichern interne Objektkonsistenz

NNRT:
- Sichern strukturelle Vollständigkeit

Gemeinsam:
- Compile-Time Sicherheit
- Immutable Domain Model
- Strukturelle Invariantendurchsetzung

---

# 5. Invarianten im Savegame

Im Modell existieren Invarianten auf mehreren Ebenen.

## PlayerId (Value Object)

```csharp
public readonly record struct PlayerId(Guid Value)
{
    public static PlayerId New() => new(Guid.NewGuid());
}
```

**Invariante:** Immutable Identität, keine nachträgliche Veränderung.

---

## PlayerName (Value Object mit Invariante)

```csharp
public sealed record PlayerName
{
    public string Value { get; }

    public PlayerName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Player name must not be empty.");

        if (value.Length > 32)
            throw new ArgumentException("Player name too long.");

        Value = value;
    }
}
```

**Invarianten:**

- Name darf nicht leer sein
- Maximal 32 Zeichen
- Objekt kann nie ungültig existieren

---

## InventoryItem (Value Object)

```csharp
public sealed record InventoryItem
{
    public string ItemCode { get; }
    public int Quantity { get; }

    public InventoryItem(string itemCode, int quantity)
    {
        if (string.IsNullOrWhiteSpace(itemCode))
            throw new ArgumentException("Item code required.");

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        ItemCode = itemCode;
        Quantity = quantity;
    }

    public InventoryItem Add(int amount) =>
        amount <= 0
            ? throw new ArgumentOutOfRangeException(nameof(amount))
            : this with { Quantity = Quantity + amount };
}
```

**Invarianten:**

- ItemCode darf nicht leer sein
- Quantity > 0
- Keine Mutation → neue Instanz durch `with`

---

## Inventory (Teil des Aggregats)

```csharp
public sealed class Inventory
{
    private readonly List<InventoryItem> _items = [];

    public IReadOnlyCollection<InventoryItem> Items => _items.AsReadOnly();

    public void AddItem(string itemCode, int quantity)
    {
        var existing = _items.FirstOrDefault(i => i.ItemCode == itemCode);

        if (existing is null)
        {
            _items.Add(new InventoryItem(itemCode, quantity));
            return;
        }

        _items.Remove(existing);
        _items.Add(existing.Add(quantity));
    }
}
```

**Wichtig:**

- Keine externe Mutation möglich
- Mengen werden konsistent zusammengeführt
- Invarianten der `InventoryItem` greifen automatisch

---

## PlayerProfile (Aggregate Root)

```csharp
public sealed class PlayerProfile
{
    public Inventory Inventory { get; } = new();
    public PlayerId Id { get; }
    public PlayerName Name { get; }
    public int Level { get; private set; }

    public PlayerProfile(PlayerId id, PlayerName name)
    {
        Id = id;
        Name = name;
        Level = 1;
    }

    public void LevelUp() => Level++;
}
```

**Warum Aggregate Root?**

- Zentraler Einstiegspunkt
- Inventory existiert nicht ohne Player
- Änderungen laufen über diese Instanz
- Konsistenzgrenze des Modells

---

## SaveGameSnapshot (Persistenzmodell)

```csharp
public sealed record SaveGameSnapshot(
    PlayerId PlayerId,
    string PlayerName,
    int Level,
    IReadOnlyCollection<InventoryItem> Inventory,
    DateTimeOffset SavedAtUtc);
```

**Wichtig:**

- Reines DTO
- Keine Geschäftslogik
- Trennt Domäne von Persistenz

Beim Rehydrieren müssen Value Objects neu erzeugt werden, damit Invarianten erneut greifen.

---

# Fazit

C# Records und Non-Nullable Reference Types unterstützen Always-Valid Domain Models durch:

- Immutable Value Objects
- Konstruktorbasierte Invariantendurchsetzung
- Compile-Time Null-Sicherheit
- Strukturelle Konsistenz von Aggregaten
