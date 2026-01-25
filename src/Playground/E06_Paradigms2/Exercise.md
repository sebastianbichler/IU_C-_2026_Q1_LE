---

Die Inhalte beziehen sich auf die im Buch **Kotz/Wenz (Kapitel 4, 8 & 16)** behandelten Konzepte zu Speicherverwaltung und modernen Sprachfeatures.

---

# Modern C# - Performance & Immutability

**Dozent:** [Ihr Name] | **Dauer:** 120 Minuten
**Themen:** Stack/Heap, Structs, Records, Span<T>, Tuples & Deconstruction.

---

## 1. Das Fundament: Class vs. Struct (20 Min)

**Theorie:** C# unterscheidet zwischen Verweistypen (`class`) und Wertetypen (`struct`). Dies hat massive Auswirkungen auf die Performance (Garbage Collection) und das Programmverhalten.

* **Class:** Liegt auf dem **Heap**. Variablen speichern nur die Adresse (Referenz).
* **Struct:** Liegt auf dem **Stack** (oder eingebettet im Elternobjekt). Die Variable enthält die tatsächlichen Daten.

```csharp
// CODE-DEMO 1.1: Referenz- vs. Wertekopie
public class PersonClass { public string Name; }
public struct PersonStruct { public string Name; }

var c1 = new PersonClass { Name = "Alice" };
var c2 = c1;
c2.Name = "Bob"; // Ändert c1.Name mit!

var s1 = new PersonStruct { Name = "Alice" };
var s2 = s1;
s2.Name = "Bob"; // s1 bleibt "Alice"

Console.WriteLine($"Class: {c1.Name}, Struct: {s1.Name}");

```

### ✍️ Übung 1: Das `ref` Keyword

Schreiben Sie eine Methode `ChangeYear(ref int year)`, die das übergebene Jahr um 1 erhöht. Erklären Sie, warum das Jahr ohne `ref` außerhalb der Methode unverändert bliebe.

---

## 2. Immutability & Records (30 Min)

**Theorie:** In modernen Architekturen (wie DDD oder funktionaler Programmierung) ist **Unveränderlichkeit (Immutability)** ein Qualitätsmerkmal. Sie verhindert Seiteneffekte und macht Code thread-sicher.

* **Records:** Spezialisierte Klassen/Structs, die auf **Daten** optimiert sind.
* **Value Equality:** Zwei Instanzen sind gleich, wenn ihre Werte gleich sind (anders als bei Klassen).

```csharp
// CODE-DEMO 2.1: Primärer Konstruktor & init-only
public record GameAsset(string Id, string Type)
{
    // Zusätzliche Eigenschaft, nach Erstellung unveränderlich
    public int Version { get; init; } = 1;
}

var assetA = new GameAsset("Sword_01", "Weapon");
var assetB = new GameAsset("Sword_01", "Weapon");

Console.WriteLine($"Gleichheit: {assetA == assetB}"); // True

// Non-destructive Mutation mit 'with'
var assetC = assetA with { Version = 2 };
Console.WriteLine(assetC);

```

### ✍️ Übung 2: Records im Einsatz

Erstellen Sie einen `record` namens `HighScore` mit `PlayerName` und `Points`. Erstellen Sie eine Liste von Highscores und nutzen Sie `with`, um einen Score zu aktualisieren, ohne das Original-Objekt zu verändern.

---

## 3. Tuples & Deconstruction (20 Min)

**Theorie:** Tuples sind "leichtgewichtige" Datenstrukturen. Deconstruction ist der Prozess, ein Objekt in seine Einzelteile zu zerlegen.

```csharp
// CODE-DEMO 3.1: Tuples als Rückgabewert
(int x, int y) GetResolution() => (1920, 1080);

var res = GetResolution();
Console.WriteLine($"Breite: {res.x}");

// Deconstruction
var (width, height) = GetResolution();

```

### ✍️ Übung 3: Eigene Deconstruct-Logik

Fügen Sie einer Klasse `Monster` (Name, HP, Level) eine Methode `public void Deconstruct(out string n, out int h)` hinzu. Testen Sie das "Auspacken" des Monsters in zwei Variablen.

---

## 4. High-Performance mit `Span<T>` (40 Min)

**Theorie (Master-Niveau):** `Span<T>` ist eine typsichere Sicht auf Speicher. Es vermeidet Heap-Allokationen (und damit GC-Pausen) bei der Arbeit mit Teilbereichen von Daten (Slices).

* **Wichtig:** `Span<T>` ist ein `ref struct`. Es darf niemals auf den Heap (z.B. als Feld in einer Klasse).

```csharp
// CODE-DEMO 4.1: Slicing ohne Kopieren
string rawData = "CONTENT_TYPE:JSON;LENGTH:1024";
ReadOnlySpan<char> span = rawData.AsSpan();

int colonIndex = span.IndexOf(':');
int semicolonIndex = span.IndexOf(';');

// Erzeugt KEINEN neuen String auf dem Heap!
var type = span.Slice(13, (semicolonIndex - 13));
Console.WriteLine(type.ToString());

```

### ✍️ Übung 4 (Die "Arcade" Challenge): Der Parser

Sie erhalten einen String: `ID:42;TYPE:PLAYER;POS:10,20`.
Extrahieren Sie die **ID** als `int` und den **TYPE**, ohne `string.Split` oder `Substring` zu verwenden. Nutzen Sie ausschließlich `Span<T>`.

---


## 6. Fazit & Transfer zum Projekt (10 Min)

* **Wann `struct`?** Kleine Daten (< 16 Byte), hohe Frequenz, keine Vererbung.
* **Wann `Record`?** DTOs, Nachrichten, Domänen-Modelle.
* **Wann `Span`?** Parsing, Netzwerk-Buffer, Performance-kritische Pfade (Gruppe 1).

**Hinweis zum Buch:** Vertiefen Sie das Thema Speichermanagement in **Kotz/Wenz Kapitel 16** (Parallele Programmierung & Performance).
