### Programmieren mit C# (DSPC016)

---

# Monaden Part2 – Von der Kategorientheorie zu LINQ

---

## 1. Die Theorie: Was ist eine Monade? (30 Min)

### Mathematischer Ursprung

Monaden stammen aus der **Kategorientheorie**. In der Informatik nutzen wir sie, um Seiteneffekte (wie I/O, State,
Exceptions) in rein funktionalen Umgebungen zu kapseln.

Eine Monade ist im Grunde ein **Wrapper** (ein Kontext) für einen Wert. Damit ein Typ als Monade gilt, muss er drei
Dinge besitzen:

1. **Typ-Konstruktor:** Ein Weg, einen Typ `T` in den Kontext zu setzen (z. B. `M<T>`).
2. **Unit (oder Return/Pure):** Eine Funktion, die einen nackten Wert in die Monade hebt.

* *Signatur:*


3. **Bind (oder FlatMap):** Eine Operation, die eine Funktion auf den inneren Wert anwendet, wobei die Funktion selbst
   wieder eine Monade zurückgibt.

* *Signatur:*

### Die Monadengesetze

Damit sich eine Monade vorhersehbar verhält, muss sie drei Regeln folgen:

* **Links-Identität:** `Bind(Unit(x), f) == f(x)`
* **Rechts-Identität:** `Bind(m, Unit) == m`
* **Assoziativität:** Das Hintereinanderschalten von Binds ist unabhängig von der Klammerung.

---

## 2. Monaden programmiersprachen-übergreifend (20 Min)

Monaden heißen überall anders, folgen aber demselben Muster:

* **Haskell:** Die Mutter der Monaden (`Maybe`, `IO`, `Either`). Hier heißt die Bind-Operation `>>=`.
* **JavaScript:** `Promise` ist fast eine Monade. `.then()` fungiert hier oft sowohl als `Map` als auch als `FlatMap`.
* **Java:** `Optional<T>` und `Stream<T>`. `flatMap` ist hier das Äquivalent zu Bind.
* **C#:** Wir kennen sie primär als `IEnumerable<T>`, `Task<T>` oder `Nullable<T>`.

---

## 3. C#-Spezifisch: Die LINQ-Verbindung (40 Min)

In C# ist das monadische Muster tief in **LINQ** verwurzelt.

* **Unit** entspricht in LINQ oft einer einfachen Initialisierung oder `Enumerable.Repeat`.
* **Bind** entspricht in C# der Methode **`SelectMany`**.

### Die Maybe-Monade in C#

Ein klassisches Problem: Null-Prüfungen. Die "Maybe-Monade" (in C# oft als `Option<T>` implementiert) kapselt das Risiko
eines fehlenden Wertes.

---

## 4. Übungen & Musterlösungen (30 Min)

### Übung 1: Die manuelle Maybe-Monade (Einfach)

**Aufgabe:** Implementieren Sie eine einfache Klasse `Maybe<T>`, die einen Wert oder "Nichts" enthalten kann.
Implementieren Sie eine `Bind`-Methode.

**Musterlösung:**

```csharp
public class Maybe<T>
{
    private readonly T _value;
    public bool HasValue { get; }

    private Maybe(T value, bool hasValue)
    {
        _value = value;
        HasValue = hasValue;
    }

    public static Maybe<T> Some(T value) => new Maybe<T>(value, true);
    public static Maybe<T> None() => new Maybe<T>(default, false);

    // Dies ist die BIND-Operation
    public Maybe<U> Bind<U>(Func<T, Maybe<U>> func)
    {
        return HasValue ? func(_value) : Maybe<U>.None();
    }
}

```

---

### Übung 2: Chaining mit der Monade (Mittel)

**Aufgabe:** Nutzen Sie die `Maybe`-Klasse aus Übung 1. Gegeben sind drei Funktionen:

1. `GetPlayer(int id)` -> `Maybe<Player>`
2. `GetTeam(Player p)` -> `Maybe<Team>`
3. `GetCoach(Team t)` -> `Maybe<Coach>`
   Verkappen Sie diese Aufrufe so, dass am Ende ein `Maybe<Coach>` herauskommt, ohne explizite `if (x == null)`
   -Prüfungen.

**Musterlösung:**

```csharp
Maybe<Coach> coach = GetPlayer(1)
    .Bind(player => GetTeam(player))
    .Bind(team => GetCoach(team));

// Wenn irgendwo in der Kette ein "None" auftritt,
// wird der Rest der Kette übersprungen (Short-Circuiting).

```

---

### Übung 3: LINQ als Monaden-Syntax (Schwer)

**Aufgabe:** Wussten Sie, dass man in C# LINQ-Query-Syntax für eigene Monaden nutzen kann? Damit das funktioniert, muss
eine Erweiterungsmethode für `SelectMany` mit einer speziellen Signatur geschrieben werden. Implementieren Sie diese für
`Maybe<T>`.

**Musterlösung:**

```csharp
public static class MaybeExtensions
{
    // Erforderliche Signatur für LINQ-Query-Syntax Support
    public static Maybe<V> SelectMany<T, U, V>(
        this Maybe<T> source,
        Func<T, Maybe<U>> selector,
        Func<T, U, V> resultSelector)
    {
        return source.Bind(t =>
            selector(t).Bind(u =>
                Maybe<V>.Some(resultSelector(t, u))));
    }
}

// Anwendung:
var result = from p in GetPlayer(1)
             from t in GetTeam(p)
             from c in GetCoach(t)
             select c;

```

---

## 📋 Zusammenfassung für die Studierenden

1. **Monaden sind Kontext-Container:** Sie erlauben es uns, Werte zu transformieren, während der Kontext (
   Fehlerbehandlung, Asynchronität, Listen-Logik) automatisch mitgeschleift wird.
2. **C# ist monadisch:** Jedes Mal, wenn Sie `SelectMany` in LINQ oder `await` bei Tasks nutzen, nutzen Sie monadisches
   Verhalten.
3. **Vorteil:** Code wird deklarativer. Wir beschreiben, *was* mit dem Wert passieren soll, nicht *wie* wir den
   Container (Null-Checks, Task-Handling) auspacken.

**Nächster Schritt:** Möchten Sie, dass wir uns anschauen, wie man das monadische Prinzip nutzt, um **Logging** in eine
Pipeline zu integrieren, ohne die Geschäftslogik zu verändern?
