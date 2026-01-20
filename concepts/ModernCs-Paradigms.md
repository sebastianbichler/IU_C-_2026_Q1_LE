### Programmieren mit C# (DSPC016)


---

# 📔 Jupyter Notebook: Moderne C# Paradigmen

**Thema:** Performance, Immutability und Typsystem-Tricks

**Kontext:** Vorbereitung auf das Projekt "Grand Arcade Ecosystem"

---

## 1. Stack vs. Heap: `class` vs. `struct` (20 Min)

**Theorie:** In C# unterscheiden wir fundamental zwischen **Verweistypen** (`class`) und **Wertetypen** (`struct`).

* Klassen leben auf dem **Heap**; Variablen halten nur eine Referenz (Pointer).
* Structs leben dort, wo sie deklariert werden (meist auf dem **Stack**); Variablen halten den tatsächlichen Wert.

```csharp
// CODE-DEMO 1
public class PointClass { public int X; public int Y; }
public struct PointStruct { public int X; public int Y; }

var c1 = new PointClass { X = 10 };
var c2 = c1; // Referenz wird kopiert
c2.X = 20;
Console.WriteLine($"Class: c1.X is {c1.X}"); // Output: 20 (beide zeigen auf dasselbe Objekt)

var s1 = new PointStruct { X = 10 };
var s2 = s1; // Ganzer Wert wird kopiert
s2.X = 20;
Console.WriteLine($"Struct: s1.X is {s1.X}"); // Output: 10 (Original bleibt unverändert)

```

**✍️ ÜBUNG 1:** Erstellen Sie eine Methode `Multiply(PointStruct p)`, die X und Y verdoppelt. Rufen Sie sie auf und
prüfen Sie das Original. Warum ändert sich das Original nicht? Nutzen Sie danach das Schlüsselwort `ref`, um das
Verhalten zu ändern.

---

## 2. Immutability & Records (30 Min)

**Theorie (nach Kotz/Wenz Kap. 8):** Records sind eine der wichtigsten Neuerungen der letzten Jahre. Sie sind optimiert
für **Daten-Container**.

* **Value Equality:** Zwei Records sind gleich, wenn ihre Daten gleich sind (nicht die Speicheradresse).
* **Immutability:** Durch `init` statt `set` oder die primäre Konstruktor-Syntax werden Daten unveränderlich.

```csharp
// CODE-DEMO 2
public record Player(string Name, int Score);

var p1 = new Player("Alice", 100);
var p2 = new Player("Alice", 100);

Console.WriteLine($"Gleichheit: {p1 == p2}"); // True! (Bei Klassen wäre es False)

// "Destruktive Änderung" via with-Expression
var p3 = p1 with { Score = 200 };
Console.WriteLine(p3);

```

**✍️ ÜBUNG 2:** Definieren Sie einen `record` für ein Spiel-Asset (Name, Pfad, Größe). Versuchen Sie, den Namen nach der
Erstellung zu ändern. Nutzen Sie die `with`-Expression, um eine Kopie mit veränderter Größe zu erstellen.

---

## 3. Tuples & Deconstruction (20 Min)

**Theorie:** Manchmal ist ein Record zu "schwer". Tuples erlauben es, Werte ad-hoc zu gruppieren, ohne einen Typnamen zu
vergeben.

```csharp
// CODE-DEMO 3
// Methode mit Tuple-Rückgabe
(int Min, int Max) GetMinMax(int[] numbers) => (numbers.Min(), numbers.Max());

var stats = GetMinMax(new[] { 1, 2, 3, 4, 5 });
Console.WriteLine($"Min: {stats.Min}, Max: {stats.Max}");

// Deconstruction (Auspacken)
var (min, max) = GetMinMax(new[] { 10, 20, 30 });

```

**✍️ ÜBUNG 3:** Schreiben Sie eine Klasse `Monster` mit `Name` und `Health`. Implementieren Sie eine Methode
`public void Deconstruct(out string n, out int h)`, damit man ein Monster-Objekt so auspacken kann:
`var (name, health) = myMonster;`.

---

## 4. High-Performance: `ref` & `Span<T>` (40 Min)

**Theorie (Master-Level):** `Span<T>` ist das Werkzeug für "Zero-Allocation". Es erlaubt uns, Teile eines Arrays zu
bearbeiten, ohne neue Teil-Arrays auf dem Heap zu erzeugen. Es ist ein `ref struct`, darf also nur auf dem Stack
existieren.

```csharp
// CODE-DEMO 4
int[] daten = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

// Ein Fenster auf die Zahlen 4, 5, 6 erzeugen (KEINE KOPIE!)
Span<int> slice = daten.AsSpan(3, 3);
slice[0] = 99; // Ändert das Original-Array!

Console.WriteLine($"Original an Index 3: {daten[3]}"); // 99

```

**✍️ ÜBUNG 4 (Die "Arcade" Challenge):** Sie erhalten einen String `"SCORE:1500;NAME:PLAYER1"`.

Nutzen Sie `ReadOnlySpan<char>`, um den Score-Wert (1500) zu extrahieren und in einen `int` umzuwandeln, **ohne**
`string.Split()` oder `Substring()` zu verwenden (da diese neue Strings auf dem Heap allokieren würden).
*Hinweis: Nutzen Sie `data.AsSpan().IndexOf(':')` und `int.Parse()` mit dem Span.*

---

## 5. Transfer zum Projekt (10 Min)

Diskutieren Sie im Plenum:

1. Warum sollte **Gruppe 1** (Performance) eher `Span` statt `string.Split` nutzen?
2. Warum sollte **Gruppe 4** (Persistence) `Records` für die Savegames nutzen?
3. Wie hilft uns **Deconstruction** beim Auslesen von Highscores aus dem Dashboard?

---

### Hilfreiche Shortcuts für die Vorlesung:

* **F5 / Run Cell:** Code sofort ausführen.
* **Kotz/Wenz S. 154:** Details zu Wertetypen.
* **Kotz/Wenz S. 412:** Vertiefung zu Records und `init`-only Properties.

**Möchten Sie, dass ich die Musterlösung für Übung 4 (den Span-Parser) bereits vorbereite?**
