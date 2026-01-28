Hier ist ein strukturierter Übungsplan für eine 120-minütige Session. Wir beginnen bei den Grundlagen (Bezug zur PPT und
dem PDF) und arbeiten uns hoch zu komplexen architektonischen Mustern unter Einbeziehung von Performance-Vergleichen zu
Java und Python.

---

# Übungsserie: C# Collections & Functional Evolution

**Zeitrahmen:** 120 Minuten | **Material:** Kotz/Wenz, Collections_V1.pptx, Collections.pdf

---

## Block 1: Die Grundlagen & Das Erbe (20 Min)

*Schwerpunkt: Übergang von Java-Strukturen zu C# (Folie 6-7 der PPT)*

### Aufgabe 1: ArrayList vs. List<T>

In der Datei `Collections.pdf` wird die Klasse `ArrayList` als Basis-Klasse für Listen beschrieben. In der PPT wird
jedoch betont, dass C# zwischen `System.Collections` (nicht-generisch) und `System.Collections.Generic` unterscheidet.

1. Erstellen Sie eine `ArrayList` und fügen Sie einen `int`, einen `string` und ein `double` hinzu. Warum ist dies in
   C# (ähnlich wie in Python, aber unähnlich zu Java Generics) möglich?
2. Konvertieren Sie den Code in eine generische `List<int>`. Warum bricht der Code nun beim Hinzufügen des Strings ab?
3. **Vergleich:** Erläutern Sie den Unterschied zu Java. (Stichwort: *Type Erasure* vs. *Reification*).

**Musterlösung:**

```csharp
// 1. Nicht-generisch (Object-basiert)
ArrayList legacyList = new ArrayList { 1, "Text", 10.5 };

// 2. Generisch (Typsicher)
List<int> modernList = new List<int> { 1 };
// modernList.Add("Text"); // Compiler-Fehler!

```

> **Hintergrund:** C# Generics behalten ihren Typ zur Laufzeit (Reification). Java „vergisst“ ihn (*Type Erasure*).
> Python-Listen sind technisch gesehen immer wie `ArrayList<object>`, da sie nur Zeiger speichern.

---

## Block 2: Performance & Speicher (30 Min)

*Schwerpunkt: Wertetypen vs. Referenztypen (Kotz/Wenz)*

### Aufgabe 2: Der Preis der Flexibilität

In der PPT wird erwähnt, dass Collections Gruppen von Objekten verwalten.

1. Definieren Sie ein `struct PlayerStruct { public int Id; }` und eine `class PlayerClass { public int Id; }`.
2. Erstellen Sie eine `List<PlayerStruct>` und eine `List<PlayerClass>` mit 1 Mio. Einträgen.
3. Messen Sie die Zeit für eine einfache Summen-Iteration über die `Id`.
4. **Frage:** Warum ist die `struct`-Liste in C# schneller als eine Liste in Java oder Python?

**Musterlösung:**
Wertetypen (`struct`) liegen in C# „in-place“ im Array der Liste. Es gibt keine Zeiger-Indirektion. Java kann dies (bis
Project Valhalla) nicht; dort ist jede Liste ein Array von Zeigern auf Objekte. Python ist hier durch den globalen
Objekt-Overhead am langsamsten.

---

## Block 3: Sortierung & Vergleich (35 Min)

*Schwerpunkt: IComparable, IComparer & Lambdas*

### Aufgabe 3: Highscore-Sorting

Basierend auf der `Hashtable` (PDF) und `Dictionary` (PPT):

1. Erstellen Sie eine Klasse `Game` mit `Name` und `Rating`.
2. Implementieren Sie `IComparable<Game>`, um standardmäßig nach dem Namen zu sortieren.
3. Erstellen Sie eine Liste von Spielen.
4. **Der Übergang zur funktionalen Programmierung:** Sortieren Sie die Liste nun **ohne** das Interface, nur mittels
   einer Lambda-Funktion nach dem `Rating` absteigend.

**Musterlösung:**

```csharp
public class Game : IComparable<Game> {
    public string Name { get; set; }
    public int Rating { get; set; }
    public int CompareTo(Game other) => Name.CompareTo(other.Name);
}

// In der Main:
List<Game> games = new() { /*...*/ };
games.Sort(); // Nutzt IComparable (Name)

// Funktionale Variante (Lambda):
games.Sort((g1, g2) => g2.Rating.CompareTo(g1.Rating));

```

---

## Block 4: LINQ & Funktionale Transformation (35 Min)

*Schwerpunkt: Deklarative Programmierung (Das GAE Dashboard)*

### Aufgabe 4: Komplexe Abfragen

Das PDF beschreibt `Queue` und `SortedList`. Wir nutzen nun LINQ, um Datenströme zu verarbeiten.

1. Gegeben ist eine Liste von Zahlen: `var numbers = Enumerable.Range(1, 100);`
2. Nutzen Sie LINQ-Methoden-Syntax (Lambdas), um:

* Nur gerade Zahlen zu filtern.
* Jede Zahl zu quadrieren.
* Die Summe der Ergebnisse zu bilden, die größer als 500 sind.


3. **Python-Vergleich:** Wie würde das in Python mit `List Comprehensions` aussehen? Was ist lesbarer?

**Musterlösung:**

```csharp
var result = numbers
    .Where(n => n % 2 == 0)
    .Select(n => n * n)
    .Where(res => res > 500)
    .Sum();

// Python Äquivalent:
// sum([n*n for n in numbers if n % 2 == 0 and n*n > 500])

```

> **C#-Spezifik:** LINQ nutzt *Deferred Execution*. Die Berechnung startet erst, wenn `Sum()` aufgerufen wird. Python
> Comprehensions sind sofort (eager), es sei denn, man nutzt Generator Expressions.

---

## 🏁 Zusammenfassung & Vergleichstabelle

| Feature        | C#                      | Java                       | Python                      |
|----------------|-------------------------|----------------------------|-----------------------------|
| **Listen-Typ** | `List<T>` (Reified)     | `ArrayList<T>` (Erasure)   | `list` (Heterogen/Dynamic)  |
| **Wertetypen** | Ja (`struct` in Liste)  | Nein (nur Wrapper/Objects) | Nein (alles ist Objekt)     |
| **Sortierung** | Lambdas / `IComparable` | Lambdas / `Comparable`     | `sort(key=...)`             |
| **Abfragen**   | LINQ (sehr mächtig)     | Streams API (seit Java 8)  | Comprehensions / Map-Filter |

**Möchten Sie, dass ich für die nächste Einheit die "Aspektorientierung" (AOP) vorbereite, um zu zeigen, wie man Logging
automatisch in diese Collections integriert?**
