### Übung 1: Das `ref` Keyword (Wertetypen manipulieren)

**Ziel:** Verstehen, wie man Wertetypen (`struct` oder Primitive) per Referenz übergibt, um das Original zu verändern.

```csharp
// Musterlösung Übung 1
void ChangeYear(ref int year)
{
    year += 1; // Ändert den Wert an der ursprünglichen Speicheradresse
}

int currentYear = 2024;
Console.WriteLine($"Vorher: {currentYear}");

ChangeYear(ref currentYear); // Das 'ref' muss beim Aufruf explizit angegeben werden

Console.WriteLine($"Nachher: {currentYear}");
// Erwartete Ausgabe: 2025

```

---

### Übung 2: Records & Immutability (HighScores)

**Ziel:** Einsatz von Records für unveränderliche Datenmodelle und Nutzung der `with`-Expression.

```csharp
// Musterlösung Übung 2
public record HighScore(string PlayerName, int Points);

// 1. Liste erstellen
var scores = new List<HighScore>
{
    new HighScore("Alice", 1200),
    new HighScore("Bob", 850)
};

// 2. Einen Score "aktualisieren" (Eigentlich: Kopie mit neuem Wert erstellen)
var originalScore = scores[0];
var updatedScore = originalScore with { Points = 1350 };

Console.WriteLine($"Original: {originalScore}");
Console.WriteLine($"Update:   {updatedScore}");
Console.WriteLine($"Sind es verschiedene Objekte? {!ReferenceEquals(originalScore, updatedScore)}");

```

---

### Übung 3: Eigene Deconstruct-Logik

**Ziel:** Eine Klasse befähigen, ihre Member direkt in lokale Variablen zu "entpacken".

```csharp
// Musterlösung Übung 3
public class Monster
{
    public string Name { get; set; }
    public int HP { get; set; }
    public int Level { get; set; }

    public Monster(string name, int hp, int level)
    {
        Name = name; HP = hp; Level = level;
    }

    // Die Deconstruct-Methode ermöglicht das Tuple-ähnliche Auspacken
    public void Deconstruct(out string name, out int hp)
    {
        name = Name;
        hp = HP;
    }
}

var myMonster = new Monster("Stein-Golem", 500, 15);

// Hier passiert die Deconstruction
var (n, h) = myMonster;

Console.WriteLine($"Monster-Name: {n}, Lebenspunkte: {h}");

```

---

### Übung 4: Der High-Performance Parser mit `Span<T>`

**Ziel:** Datenextraktion ohne Erzeugung neuer Strings auf dem Heap (Zero-Allocation).

```csharp
// Musterlösung Übung 4
string input = "ID:42;TYPE:PLAYER;POS:10,20";
ReadOnlySpan<char> span = input.AsSpan();

// 1. ID finden und parsen
int idKeyEnd = span.IndexOf(':');
int idValueEnd = span.IndexOf(';');

// Slice von Index nach ':' bis zum ';'
ReadOnlySpan<char> idValueSpan = span.Slice(idKeyEnd + 1, idValueEnd - idKeyEnd - 1);
int id = int.Parse(idValueSpan);

// 2. TYPE finden
// Wir nehmen den Rest des Spans nach dem ersten ';'
ReadOnlySpan<char> remaining = span.Slice(idValueEnd + 1);
int typeKeyEnd = remaining.IndexOf(':');
int typeValueEnd = remaining.IndexOf(';');

ReadOnlySpan<char> typeValueSpan = remaining.Slice(typeKeyEnd + 1, typeValueEnd - typeKeyEnd - 1);

Console.WriteLine($"Ergebnis:");
Console.WriteLine($"- ID (als int): {id}");
Console.WriteLine($"- Type (als Span): {typeValueSpan.ToString()}");

```

---

### Kurzer Hinweis für die Vorlesung:

Wenn Sie diese Lösungen in **PyCharm** mit dem .NET Kernel vorführen, weisen Sie die Studierenden darauf hin, dass
`ToString()` bei einem `Span` nur für die Anzeige in der Konsole genutzt wird. In einer echten
High-Performance-Schleife (wie in **Projekt-Gruppe 1**) würde man den `Span` direkt weiterverarbeiten, um die Allokation
des Strings zu vermeiden.

**Tipp nach Kotz/Wenz:** Erwähnen Sie, dass `int.Parse()` in modernen .NET-Versionen Überladungen besitzt, die direkt
`ReadOnlySpan<char>` akzeptieren – das ist der Schlüssel für die Performance-Gewinne!
