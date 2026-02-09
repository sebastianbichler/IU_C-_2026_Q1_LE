# Lösungen zu den Übungen (Für den Dozenten)

## Lösung zu Aufgabe 1: ArcadeEventHub

Diese Lösung zeigt die korrekte Verwendung der `System.Threading.Channels` API, wie in Kotz/Wenz für Hochlastszenarien
empfohlen.

```csharp
using System.Threading.Channels;

public class ArcadeEventHub
{
    // Bounded Channel verhindert OutOfMemory bei Überlastung
    private readonly Channel<string> _eventChannel = Channel.CreateBounded<string>(100);

    public async Task SendEventAsync(string message)
    {
        // Wartet asynchron, falls der Channel voll ist (Backpressure)
        await _eventChannel.Writer.WriteAsync(message);
    }

    public async Task StartProcessingAsync(CancellationToken ct)
    {
        try
        {
            // Effizientes Auslesen via Async-Stream
            await foreach (var msg in _eventChannel.Reader.ReadAllAsync(ct))
            {
                Console.WriteLine($"[Dashboard] Verarbeite: {msg}");
                await Task.Delay(50, ct); // Simuliert UI-Last
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Processing wurde sauber abgebrochen.");
        }
    }

    public void Complete() => _eventChannel.Writer.Complete();
}

```

## Lösung zu Aufgabe 2: Zeitgesteuerter Abbruch

Hier wird die `CancellationTokenSource` verwendet, um das asynchrone Verhalten zu steuern.

```csharp
var hub = new ArcadeEventHub();
using var cts = new CancellationTokenSource();

// Starte Consumer in einem eigenen Task
var consumerTask = hub.StartProcessingAsync(cts.Token);

// Simuliere Producer
for (int i = 0; i < 150; i++)
{
    await hub.SendEventAsync($"Event {i}");
}

// Nach 5 Sekunden abbrechen
cts.CancelAfter(5000);

await consumerTask;

```

## Lösung zu Aufgabe 3: Prioritäts-Aggregator (Konzept)

Für Master-Studierende ist wichtig zu verstehen, dass man mehrere `Reader` kombinieren kann.

```csharp
public async Task PriorityConsumerAsync(ChannelReader<string> highPrio, ChannelReader<string> lowPrio)
{
    while (true)
    {
        // Versuche IMMER erst die hohe Priorität leer zu machen
        if (highPrio.TryRead(out var criticalMsg))
        {
            Console.WriteLine($"CRITICAL: {criticalMsg}");
            continue;
        }

        // Wenn nichts Kritisches da ist, warte auf IRGENDWAS
        // Ein einfacherer Ansatz im Master-Level:
        var msg = await lowPrio.ReadAsync();
        Console.WriteLine($"Normal: {msg}");
    }
}

```

### Methodischer Hinweis für die Studierenden:

Verweisen Sie während der Besprechung auf **Kotz/Wenz Kapitel 16.2**. Erklären Sie, dass der `await`-Operator hier den
Thread nicht blockiert, sondern den "Zustandsautomaten" parkt, bis der Channel wieder Platz hat. Das ist der Schlüssel
zur **Scalable Concurrency** im Projekt.
