### Programmieren mit C# (DSPC016)

---

# Notizen: Scalable Concurrency & State Machines

## Teil 1: Warum asynchron? (Die Theorie hinter der State Machine)

**Hintergrund:**

* **Der Thread-Irrtum:** Früher dachte man: "Viel Arbeit = Viele Threads". Aber: Ein Thread kostet unter Windows ca. 1
  MB Stack-Speicher. Bei 1.000 wartenden Threads verschwenden wir 1 GB RAM nur für das "Nichtstun".
* **Das Restaurant-Beispiel:** Ein synchroner Kellner (Thread) gibt die Bestellung in der Küche ab und wartet dort, bis
  das Essen fertig ist. Er blockiert. Ein asynchroner Kellner gibt die Bestellung ab, bekommt eine Marke (Task) und
  bedient andere Tische, bis die Glocke läutet.
* **Die State Machine (Kotz/Wenz Kap. 16.2):** C# macht Magie im Hintergrund. Wenn der Compiler `await` sieht, zerlegt
  er die Methode in kleine Stücke. Er baut ein Objekt (den Zustandsautomaten), das sich merkt: "Wo war ich?" und "Welche
  Variablen hatte ich?". Sobald die Ressource (z.B. Datenbank) fertig ist, springt die State Machine zurück in den
  nächsten Zustand.

**Lösung zur Übung 1 (Async-I/O):**

```csharp
public async Task StarteLadevorgang()
{
    Console.WriteLine("Ladevorgang gestartet...");
    // Task.Delay gibt den Thread frei, die State Machine pausiert hier.
    await Task.Delay(3000);
    Console.WriteLine("Daten sind da!");
}

```

---

## Teil 2: Synchronization & Context

**Hintergrund:**

* **Kontext-Wechsel:** In einer Desktop-App (WPF/WinForms) darf nur der UI-Thread die GUI ändern. `await` sorgt
  standardmäßig dafür, dass wir in den richtigen Kontext zurückkehren.
* **Deadlocks (Die Sackgasse):** Wenn der UI-Thread auf `.Result` wartet, blockiert er. Wenn der asynchrone Teil fertig
  ist, will er zurück in den UI-Thread – aber der wartet ja noch. Zack: Stillstand. **Regel:** "Async all the way" (
  Niemals `.Result` oder `.Wait()` nutzen).
* **ConfigureAwait(false):** Erzählen Sie den Studierenden, dass dies in Bibliotheken (Infrastruktur-Layer im Projekt)
  wichtig ist. Es sagt: "Mir ist egal, auf welchem Thread ich weitermache". Das spart Performance.

---

## Teil 3: System.Threading.Channels (Der Game-Changer)

**Hintergrund:**

* **Weg von Locks:** `lock(obj)` ist wie eine rote Ampel. Alle müssen anhalten. Channels sind wie ein Fließband (
  Producer-Consumer).
* **Entkopplung:** Das Spiel (Producer) feuert Events in den Channel. Das Dashboard (Consumer) nimmt sie raus. Wenn das
  Dashboard kurz hängt, läuft das Spiel trotzdem weiter, weil der Channel puffert.
* **Bounded vs. Unbounded:** * *Unbounded:* Kann unendlich wachsen (Gefahr: OutOfMemory).
* *Bounded:* Hat ein Limit (z.B. 100 Nachrichten). Wenn voll, muss der Producer warten (Backpressure).

**Lösung zur Übung 3 (High-Speed Bus):**

```csharp
// 1. Channel erstellen (Kapazität 100)
var channel = Channel.CreateBounded<string>(100);

// 2. Producer (Sensor/Spiel)
var producer = Task.Run(async () => {
    for (int i = 1; i <= 10; i++) {
        await channel.Writer.WriteAsync($"Event {i}");
        await Task.Delay(100); // Simuliert Arbeit
    }
    channel.Writer.Complete();
});

// 3. Consumer (Logging/Dashboard)
var consumer = Task.Run(async () => {
    await foreach (var msg in channel.Reader.ReadAllAsync()) {
        Console.WriteLine($"Verarbeitet: {msg}");
    }
});

await Task.WhenAll(producer, consumer);

```

---

## Teil 4: Orchestrierung & Abbruch

**Hintergrund:**

* **CancellationToken (Kotz/Wenz Kap. 16.3):** Asynchrone Methoden sind wie Züge. Man kann sie nicht einfach "töten",
  man muss ihnen ein Signal zum Anhalten geben. Ein `CancellationToken` wird durchgereicht.
* **Task-Kombinatoren:**
* `Task.WhenAll`: "Ich warte auf alle Freunde."
* `Task.WhenAny`: "Der Erste, der fertig ist, gewinnt." (Nützlich für Timeouts oder Redundanz).

**Lösung zur Übung 4 (Crawler/Abbruch):**

```csharp
using var cts = new CancellationTokenSource();

// Simuliere zwei Aufgaben
var task1 = Task.Delay(5000, cts.Token); // Langsam
var task2 = Task.Delay(2000, cts.Token); // Schnell

// Warten, bis der ERSTE fertig ist
var completedTask = await Task.WhenAny(task1, task2);

// Den anderen abbrechen
cts.Cancel();

Console.WriteLine("Der Schnellste hat gewonnen, der Rest wurde abgebrochen.");

```

---

## Abschluss & Transfer zum Projekt

**Message:**

* **Gruppe 3 Fokus:** Ihr baut das Nervensystem. Wenn ihr blockiert, stirbt die ganze Arcade.
* **Die goldene Regel:** Nutzt asynchrone Schnittstellen von oben nach unten (Controller -> Service -> Repository ->
  Database).
* **Wissenschaftliche Tiefe:** Schaut euch an, wie `ValueTask` (Kotz/Wenz Kap. 16) noch mehr Performance rausholt, indem
  es Allokationen spart, wenn ein Ergebnis schon fertig vorliegt.

---

### Tipps, Tricks und Fragen:

1. "Was passiert, wenn der Consumer langsamer ist als der Producer?" (Stichwort: Backpressure).
2. **Visualisierung:** Zeichnen Sie das Fließband (Channel) auf.
3. **Live-Debugger:** Nutzen Sie in der IDE das Fenster "Parallel Stacks", wenn Sie mehrere Tasks laufen haben. Man
   sieht dort wunderbar, wie die Threads verteilt sind.
