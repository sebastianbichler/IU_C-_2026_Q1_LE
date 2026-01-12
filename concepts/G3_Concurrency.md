### Programmieren mit C# (DSPC016)

---

## Gruppe 3: Scalable Concurrency & State Machines

**Der Fokus:** Maximale CPU-Auslastung durch Entkopplung von Datenströmen.

### Die C#-Besonderheit: `System.Threading.Channels`

Klassisches Threading nutzt oft `lock(obj)`, um Daten zu schützen. Das führt zu "Lock Contention": Threads stehen im
Stau und warten aufeinander.

* **Was sind Channels?** Ein Channel ist eine typsichere Pipe zwischen einem "Producer" (z.B. dem Spiel) und einem "
  Consumer" (z.B. dem Telemetrie-Modul).
* **Der Vorteil:** Sie nutzen das "Pub/Sub"-Modell. Der Producer schiebt Daten asynchron hinein und vergisst sie. Der
  Consumer arbeitet sie ab, wenn er bereit ist. Dies nutzt die `async/await` State Machine von .NET perfekt aus, um
  Threads frei zu machen, während gewartet wird.

### Code-Beispiel: Asynchroner Game-Event-Bus

In der Arcade feuern viele Spiele gleichzeitig Events (ScoreUp, GameOver, Collision).

```csharp
public class GameEventHub
{
    // Ein Channel fungiert als Puffer
    private readonly Channel<GameEvent> _queue = Channel.CreateUnbounded<GameEvent>();

    public async ValueTask Publish(GameEvent e) => await _queue.Writer.WriteAsync(e);

    public async Task ProcessEventsAsync(CancellationToken ct)
    {
        // Nutzt Async-Iteratoren für extrem effiziente Abarbeitung
        await foreach (var e in _queue.Reader.ReadAllAsync(ct))
        {
            await HandleEvent(e); // Verarbeitet Events ohne die Spiele zu blockieren
        }
    }
}

```

---
