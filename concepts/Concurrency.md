### Programmieren mit C# (DSPC016)

---

Inhaltlich stützen wir uns auf die Konzepte der **Task Parallel Library (TPL)** und **asynchronen Programmierung** aus *
*Kapitel 16** des Buches von **Kotz und Wenz**.

---

# Vorlesung: Scalable Concurrency – Maximale Performance durch Entkopplung

**Ziel:** Verständnis von asynchronen Zustandsautomaten und lock-freier Kommunikation via Channels.

---

## Teil 1: Das Problem mit Threads & die Lösung "Async" (25 Min)

### Theorie (15 Min)

* **Threads vs. Tasks:** Warum `new Thread()` im Master-Level meist falsch ist (Ressourcenverbrauch).
* **I/O-Bound vs. CPU-Bound:** Der Unterschied zwischen Warten (Datenbank, Netz) und Rechnen.
* **Zustandsautomaten:** Wie der C#-Compiler `async/await` in eine State Machine umschreibt (Kotz/Wenz Kap. 16.2).
* *Kernkonzept:* Ein Thread wird bei `await` freigegeben, um andere Arbeit zu leisten, statt blockiert zu warten.

### Demo & Übung (10 Min)

**Aufgabe:** Erstellen Sie eine Methode, die eine Datei liest, während die UI (oder Konsole) weiterhin auf
Benutzereingaben reagiert.

```csharp
// Demo: Blockierung vs. Async
public async Task ProcessDataAsync() {
    Console.WriteLine("Start Download...");
    await Task.Delay(3000); // Simuliert I/O-Warten ohne Thread-Blockierung
    Console.WriteLine("Download fertig!");
}

```

---

## Teil 2: SynchronizationContext & Task Scheduling (20 Min)

### Theorie (10 Min)

* **Deadlocks vermeiden:** Warum `Task.Wait()` oder `.Result` gefährlich sind.
* **ConfigureAwait(false):** Wann man den ursprünglichen Kontext verlassen sollte (wichtig für Bibliotheken).
* **Task.Run:** Wann es sinnvoll ist, Arbeit explizit auf den ThreadPool auszulagern (Kotz/Wenz Kap. 16.4).

### Übung (10 Min)

**Fehlersuche:** Geben Sie den Studierenden einen Code-Schnipsel mit einem klassischen Deadlock (z.B. UI-Thread wartet
auf `.Result`) und lassen Sie sie diesen mit `await` reparieren.

---

## Teil 3: Entkopplung durch Channels (35 Min)

### Theorie (15 Min)

* **Das Problem der "Shared State":** Warum `lock` langsam ist (Lock Contention).
* **System.Threading.Channels:** Das Producer-Consumer-Pattern.
* *Vorteil:* Daten werden übergeben ("Handover"), statt sie durch Sperren zu schützen.
* *Backpressure:* Wie man verhindert, dass der Producer den Consumer "überrollt" (Bounded Channels).

### Praxis & Code (20 Min)

**Übung: Der High-Speed Event Bus**
Bauen Sie einen Channel, bei dem ein "Sensor" (Producer) 1000 Datenpunkte pro Sekunde sendet und ein "Logger" (Consumer)
diese asynchron verarbeitet.

```csharp
var channel = Channel.CreateBounded<int>(100);

// Producer
_ = Task.Run(async () => {
    for(int i=0; i < 1000; i++)
        await channel.Writer.WriteAsync(i);
    channel.Writer.Complete();
});

// Consumer
await foreach (var item in channel.Reader.ReadAllAsync()) {
    // Verarbeiten...
}

```

---

## Teil 4: State Machines & Resilience (30 Min)

### Theorie (15 Min)

* **Komplexe Abläufe:** Wie man mit `Task.WhenAll` und `Task.WhenAny` parallele Workflows orchestriert.
* **CancellationToken:** Sauberes Abbrechen von asynchronen Operationen (Kotz/Wenz Kap. 16.3).
* **Fehlerbehandlung:** Exception-Handling in asynchronen Methoden.

### Übung (15 Min)

**Szenario:** Ein Multi-Agent-Crawler soll drei Webseiten gleichzeitig abfragen. Wenn die erste fertig ist, sollen die
anderen abgebrochen werden (`Task.WhenAny` + `CancellationToken`).

---

## Teil 5: Fazit & Transfer zum Projekt (10 Min)

* **Zusammenfassung:** Maximale Auslastung erreichen wir nicht durch *mehr* Threads, sondern durch *effizienteres
  Warten*.
* **Bezug zum Projekt:** Wie nutzt Gruppe 3 dieses Wissen, um den "Distributed Multi-Agent Crawler" oder den "Arcade
  Event Bus" stabil zu bauen?

---

### Literatur-Hinweise für das Referat:

1. **Kotz/Wenz:** Kapitel 16 (Asynchrone Programmierung & TPL) für die Syntax.
2. **Microsoft Learn:** "An introduction to System.Threading.Channels" (Stephen Toub).
3. **Wissenschaftlicher Fokus:** Suchen Sie nach Paper zu *"Lock-free Concurrency in Managed Runtimes"*.

**Tipp für die Präsentation:** Zeigen Sie im Task-Manager (Leistung -> CPU), wie die Auslastung bei einem blockierenden
System (viele Threads im Zustand "Waiting") im Vergleich zu einem asynchronen System (hoher Durchsatz bei wenigen
Threads) aussieht.
