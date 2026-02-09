### Programmieren mit C# (DSPC016)

---

Um das Spiel von der Konsole aus zu testen, betrachten wir die Konsole einfach als den ersten **„Host“**. Später wird
Avalonia diesen Host ersetzen, aber die Logik der Spiele bleibt identisch.

Hier ist ein simpler Test-Aufbau, mit dem du ein Spiel in der Konsole „taktst“ (Update/Render-Zyklus).

### 1. Ein einfaches Test-Spiel (MockGame)

Zuerst brauchen wir eine konkrete Klasse, die wir ausführen können. Dieses Spiel zählt einfach einen Zähler hoch, wenn
wir eine Taste drücken.

```csharp
using GAE.Shared.Core;

namespace GAE.Modules.Test;

public class ConsoleCounterGame : IArcadeGame
{
    public string Name => "Console Counter Strike";
    private int _count = 0;
    private bool _needsRedraw = true;

    public void Initialize() => Console.WriteLine($"{Name} initialisiert!");

    public void Update(double deltaTime)
    {
        // In der Konsole prüfen wir, ob eine Taste gedrückt wurde (Non-blocking)
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.Spacebar)
            {
                _count++;
                _needsRedraw = true;
            }
        }
    }

    public void Render()
    {
        if (_needsRedraw)
        {
            Console.SetCursorPosition(0, 5); // Damit wir nicht den ganzen Screen scrollen
            Console.WriteLine($"Aktueller Score: {_count} | Drücke [Leertaste] zum Zählen!");
            _needsRedraw = false;
        }
    }

    public void Shutdown() => Console.WriteLine("Spiel beendet.");
    public void Dispose() => Shutdown();
}

```

---

### 2. Der Console-Host (Program.cs)

Jetzt verknüpfen wir die Engines der Gruppen (Async-Loop von Gruppe 3) mit dem Spiel.

```csharp
using GAE.Shared.Core;
using GAE.Modules.Test;
using GAE.Engine.Async; // Von Gruppe 3

Console.Clear();
Console.WriteLine("=== GAE CONSOLE HOST STARTING ===");

// 1. Instanziierung
using var myGame = new ConsoleCounterGame();
var engine = new GameLoop(); // Die Klasse von Gruppe 3
var cts = new CancellationTokenSource();

// 2. Start-Meldung
Console.WriteLine("Drücke [ESC] zum Beenden des Hosts.");

// 3. Den Loop in einem Task starten (Concurrency - Gruppe 3)
Task gameTask = engine.StartLoopAsync(myGame, cts.Token);

// 4. Den Host am Leben erhalten, bis ESC gedrückt wird
while (!cts.Token.IsCancellationRequested)
{
    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
    {
        cts.Cancel();
    }
    Thread.Sleep(100); // Entlastet die CPU
}

await gameTask;
Console.WriteLine("Host sauber beendet.");

```

---

### Das Architektur-Prinzip (Der Weg zu Avalonia)

Der große Vorteil dieses Aufbaus ist die **Abstraktion**.

* **Heute (Konsole):** Dein "Host" ruft `Update` und `Render` in einer Text-Umgebung auf.
* **Morgen (Avalonia):** Dein "Host" ist ein Windows-Fenster.
* `Update` wird über den `CompositionTarget.Rendering`-Event von Avalonia getaktet.
* `Render` zeichnet nicht mit `Console.WriteLine`, sondern gibt die Daten an ein `Canvas` oder ein `ViewModel` weiter.
* `HandleInput` nutzt die `OnKeyDown`-Events des Fensters statt `Console.KeyAvailable`.

### Was du jetzt tun kannst, um den Übergang vorzubereiten:

1. **Trennung:** Achte darauf, dass im Projekt `GAE.Modules.Snake` (oder deinem Testspiel) **kein** `Console.WriteLine`
   steht.
2. **Interface-Erweiterung:** Gib dem Spiel eine Möglichkeit, "Nachrichten" nach außen zu senden, ohne die Konsole zu
   kennen (z.B. über ein `Action<string> OnLog` Delegate).
3. **Gruppe 4 (Storage) einbinden:** Lass das Spiel beim `Shutdown()` einen Highscore in die Datenbank werfen. Das
   kannst du in der Konsole perfekt debuggen, bevor die GUI dazukommt.

**Soll ich dir zeigen, wie du eine einfache "Logger"-Klasse baust, die im Core liegt, damit Spiele Text ausgeben können,
egal ob sie in der Konsole oder in Avalonia laufen?**
