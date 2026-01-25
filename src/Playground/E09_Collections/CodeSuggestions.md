
---

# Übungsprojekt: GAE Collection Lab

## Teil 1: Das Speicher-Labor (Boxing & Performance)

**Datei:** `MemoryLab.cs`

```csharp
using System.Collections;
using System.Diagnostics;

public class MemoryLab
{
    public static void Run()
    {
        const int count = 1_000_000;

        // AUFGABE 1.1: Messen Sie Zeit und Speicher für ArrayList (Boxing)
        // Bezug PPT: "Nicht-generische Collections verwalten mit dem Typ Object"
        GC.Collect();
        long startMem = GC.GetTotalMemory(true);
        var sw = Stopwatch.StartNew();

        ArrayList nonGeneric = new ArrayList();
        for (int i = 0; i < count; i++)
            nonGeneric.Add(i); // <-- Hier passiert Boxing!

        sw.Stop();
        Console.WriteLine($"ArrayList: {sw.ElapsedMilliseconds}ms, {GC.GetTotalMemory(false) - startMem} Bytes");

        // AUFGABE 1.2: Messen Sie List<int> (Generics)
        // Vergleichen Sie die Ergebnisse. Warum ist List<int> so viel schneller?
        // ... Ihr Code hier ...
    }
}

```

---

## Teil 2: Value vs. Reference Types (Cache-Lokalität)

**Datei:** `PerformanceLab.cs`

```csharp
public struct GamePosition { public int X, Y; } // Liegt direkt in der Liste
public class GamePositionObj { public int X, Y; } // Liste speichert nur Pointer

public class PerformanceLab
{
    public static void Run()
    {
        var structList = new List<GamePosition>(new GamePosition[100_000]);
        var classList = new List<GamePositionObj>();
        for(int i=0; i<100_000; i++) classList.Add(new GamePositionObj());

        // AUFGABE 2.1: Iterieren Sie über beide Listen und addieren Sie X.
        // Warum ist die struct-Liste CPU-Cache-freundlicher?
        // Bezug Kotz/Wenz: "Werte- vs. Referenztypen"
    }
}

```

---

## Teil 3: Funktionales GAE-Dashboard (LINQ)

**Datei:** `DashboardLab.cs`

```csharp
public record GameSession(string Name, int PlayerCount, bool IsActive, string Genre);

public class DashboardLab
{
    public static void Run()
    {
        var sessions = new List<GameSession> {
            new("Snake", 10, true, "Classic"),
            new("PacMan", 50, true, "Classic"),
            new("GAE-Racing", 150, false, "Sports"),
            new("SpaceInvaders", 30, true, "Classic")
        };

        // AUFGABE 3.1: Nutzen Sie Lambdas und LINQ
        // Filtern Sie alle "Classic" Spiele, die aktiv sind.
        // Projizieren Sie das Ergebnis auf einen anonymen Typ { Game = Name, Load = PlayerCount }.

        // AUFGABE 3.2: Demonstrieren Sie 'Deferred Execution'
        // Fügen Sie nach der LINQ-Definition, aber vor dem .ToList() ein neues Spiel hinzu.
        // Erscheint es im Ergebnis? Warum?
    }
}

```

---

## Teil 4: Concurrency & Backpressure

**Datei:** `EventHubLab.cs`

```csharp
using System.Threading.Channels;

public class EventHubLab
{
    public static async Task Run()
    {
        // AUFGABE 4.1: Erstellen Sie einen Bounded Channel (Kapazität 10)
        // Simulieren Sie ein Spiel, das Highscores viel schneller sendet,
        // als das Dashboard sie verarbeiten kann (Backpressure).

        var channel = Channel.CreateBounded<string>(10);

        // Producer-Task
        _ = Task.Run(async () => {
            for(int i=0; i<100; i++) {
                await channel.Writer.WriteAsync($"Highscore {i}");
                Console.WriteLine($"Sent {i}");
            }
        });

        // Consumer-Task (Simulierte Verzögerung)
        await foreach (var msg in channel.Reader.ReadAllAsync())
        {
            Console.WriteLine($"Processed {msg}");
            await Task.Delay(500); // Dashboard ist langsam!
        }
    }
}

```

---

# Hintergrundinformationen:

1. **Boxing-Visualisierung:** `ArrayList` nutzt intern ein `object[]`. Wenn ein `int` (Stack) dort
   hinein soll, muss die CLR eine "Box" auf dem Heap bauen. Das kostet Zeit und erzeugt Müll (GC-Last).
2. **Cache-Lokalität:** Bei der `struct`-Liste liegen die Daten wie Soldaten in einer Reihe. Die CPU lädt sie in "
   Cache-Lines" voraus. Bei Klassen-Listen muss die CPU für jedes Element erst die Adresse lesen und dann an eine ganz
   andere Stelle im RAM springen.
3. **LINQ & Performance:** Weisen Sie darauf hin, dass LINQ elegant ist, aber in extremen High-Performance-Loops der
   GAE-Engine (z.B. Kollisionsabfrage 60x pro Sekunde) eine einfache `for`-Schleife über ein Array oft besser ist, um
   Allokationen zu vermeiden.

