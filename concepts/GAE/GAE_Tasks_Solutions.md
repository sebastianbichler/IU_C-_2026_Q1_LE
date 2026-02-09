### Programmieren mit C# (DSPC016)

---

### 0. Das Fundament (GAE.Shared.Core)

*Dieses Projekt müssen alle anderen referenzieren.*

```csharp
namespace GAE.Shared.Core;

public interface IArcadeGame : IDisposable
{
    string Name { get; }
    void Initialize();
    void Update(double deltaTime);
    void Render();
    void Shutdown();
}

public record Highscore(string Player, int Value, DateTime Date, string GameName);

```

---

### Gruppe 1: Memory Safety (The "Safe-Buffer")

**Thema:** Ressourcen-Management ohne Garbage-Collection-Overhead.
**Lösung:** Ein Buffer, der `Memory<T>` nutzt, um Daten effizient zwischen Hub und Spiel zu tauschen.

```csharp
namespace GAE.Engine.Memory;

public class SafeBuffer<T> : IDisposable
{
    private Memory<T> _data;
    private bool _isDisposed = false;

    public SafeBuffer(int size) => _data = new T[size];

    public Span<T> GetSpan() => _data.Span; // High-Speed Zugriff ohne Kopie

    public void Dispose()
    {
        // Hier würde man im echten Szenario Ressourcen an einen Pool zurückgeben
        _data = Memory<T>.Empty;
        _isDisposed = true;
        Console.WriteLine("[Memory] Buffer sicher freigegeben.");
    }
}

```

---

### Gruppe 2: LINQ (The "Analytics-Provider")

**Thema:** Deklarative Datenverarbeitung.
**Lösung:** Ein Service, der Highscores filtert und aggregiert, ohne manuelle Schleifen.

```csharp
using GAE.Shared.Core;

namespace GAE.Engine.Query;

public class AnalyticsService
{
    public IEnumerable<string> GetTopPlayers(IEnumerable<Highscore> scores, int limit = 3)
    {
        return scores
            .OrderByDescending(s => s.Value)
            .Take(limit)
            .Select(s => $"{s.Player}: {s.Value} Points ({s.GameName})");
    }

    public double GetAverageScoreForGame(IEnumerable<Highscore> scores, string gameName)
    {
        return scores
            .Where(s => s.GameName == gameName)
            .DefaultIfEmpty()
            .Average(s => s?.Value ?? 0);
    }
}

```

---

### Gruppe 3: Concurrency (The "Async-Ticker")

**Thema:** Nicht-blockierende Ausführung.
**Lösung:** Ein Game-Loop, der mit dem modernen `PeriodicTimer` arbeitet.

```csharp
using GAE.Shared.Core;

namespace GAE.Engine.Async;

public class GameLoop
{
    private bool _isRunning;

    public async Task StartLoopAsync(IArcadeGame game, CancellationToken ct)
    {
        _isRunning = true;
        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(16)); // ~60 FPS

        game.Initialize();

        while (await timer.WaitForNextTickAsync(ct) && _isRunning)
        {
            game.Update(0.016);
            game.Render();
        }

        game.Shutdown();
    }

    public void Stop() => _isRunning = false;
}

```

---

### Gruppe 4: DDD & Persistence (The "Archivist")

**Thema:** Strukturierte Speicherung mit EF Core.
**Lösung:** Ein Repository-Pattern, das Domain-Models (Records) in Datenbank-Entitäten übersetzt.

```csharp
using GAE.Shared.Core;
using Microsoft.EntityFrameworkCore;

namespace GAE.Engine.Storage;

public class ArcadeDbContext : DbContext
{
    public DbSet<Highscore> Scores { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=gae.db");
}

public class ArcadeRepository
{
    public async Task SaveScoreAsync(Highscore score)
    {
        using var db = new ArcadeDbContext();
        await db.Database.EnsureCreatedAsync();
        db.Scores.Add(score);
        await db.SaveChangesAsync();
    }
}

```

---

### Gruppe 5: Metaprogrammierung (The "Discovery-Agent")

**Thema:** Automatisierung durch Code-Analyse.
**Lösung:** Ein Dienst, der via Reflection (als Vorstufe zum Source Generator) alle Spiele im System findet.

```csharp
using System.Reflection;
using GAE.Shared.Core;

namespace GAE.Engine.Gen;

public class DiscoveryService
{
    public IEnumerable<IArcadeGame> FindGames()
    {
        // Sucht in der aktuellen Assembly nach Typen, die IArcadeGame implementieren
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IArcadeGame).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in types)
        {
            yield return (IArcadeGame)Activator.CreateInstance(type)!;
        }
    }
}

```

---

### Zusammenfassung für die Gruppen

Jede Gruppe kann nun in ihrer `Program.cs` (oder in einem Unit Test) ihre Logik isoliert prüfen:

* **Gr. 1:** Erzeugt einen `SafeBuffer`, schreibt Zahlen rein und liest sie via `Span` aus.
* **Gr. 2:** Erzeugt eine `List<Highscore>` mit Testdaten und führt die Analytics-Methoden aus.
* **Gr. 3:** Startet den Loop mit einem "Dummy-Spiel" und schaut, ob die Konsole im Takt tickt.
* **Gr. 4:** Versucht, einen Highscore in die SQLite-Datei zu schreiben.
* **Gr. 5:** Erstellt eine Testklasse `MyTestGame : IArcadeGame` und prüft, ob der `DiscoveryService` sie findet.

**Möchten Sie, dass ich für eine dieser Gruppen eine etwas komplexere "Challenge"-Aufgabe formuliere, falls sie mit diesem Basisskelett zu schnell fertig sind?**
