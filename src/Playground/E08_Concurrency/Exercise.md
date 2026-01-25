Diese Übungsreihe ist als „Deep Dive“ für die Gruppenarbeit im **Grand Arcade Ecosystem (GAE)** konzipiert. Sie
verbindet die Theorie aus Kotz/Wenz (Kapitel 16: Asynchrone Programmierung) mit den spezifischen Anforderungen des
Projekts (Infrastruktur, Performance, Entkopplung).

---

# Übungsserie: Infrastruktur-Kern für das GAE


**Themenschwerpunkt:** Scalable Concurrency, Channels & State Machines

## Szenario

Das GAE-Dashboard muss Telemetriedaten von verschiedenen Spiele-Modulen empfangen (z. B. Highscores, Kollisionsevents,
Systemstatus). Da hunderte Events pro Sekunde auftreten können, darf das Dashboard nicht blockiert werden. Ihr
entwickelt nun den asynchronen "Event-Hub".

---

### Aufgabe 1: Der asynchrone Telemetrie-Channel (60 Min)

Entwickeln Sie eine Klasse `ArcadeEventHub`. Diese soll als zentraler Vermittler zwischen den Spielen (Producern) und
dem Dashboard-UI (Consumer) dienen.

1. **Struktur:** Nutzen Sie einen `System.Threading.Channels.Channel<T>`. Verwenden Sie einen **Bounded Channel** mit
   einer Kapazität von 100 Nachrichten.
2. **Producer-Logik:** Schreiben Sie eine Methode `SendEventAsync(string message)`, die Nachrichten in den Channel
   schreibt. Simulieren Sie eine "Überlastung", indem Sie 150 Nachrichten sehr schnell hintereinander senden.
3. **Consumer-Logik:** Schreiben Sie eine Methode `StartProcessingAsync(CancellationToken ct)`, die die Nachrichten
   mittels `ReadAllAsync` liest und auf der Konsole ausgibt. Jede Verarbeitung soll künstlich 50ms dauern (simulierte
   UI-Arbeit).
4. **Reflektion:** Beobachten Sie, was passiert, wenn der Channel voll ist (Backpressure).

### Aufgabe 2: Ausfallsicherheit & Cancellation (30 Min)

In der Arcade kann ein Spiel abstürzen oder beendet werden. Der Telemetrie-Stream muss sauber abgebrochen werden können.

1. Erweitern Sie den Consumer aus Aufgabe 1 so, dass er auf einen `CancellationToken` reagiert.
2. Implementieren Sie einen Mechanismus, der nach 5 Sekunden das Processing hart abbricht, auch wenn noch Nachrichten im
   Channel sind.

### Aufgabe 3: Der "Fast-Lane" Aggregator (30 Min - Master-Level)

Manchmal sind bestimmte Events wichtiger als andere (z. B. "System_Crash" vor "Player_Moved").

1. **Theorie-Transfer:** Wie könnte man mit zwei verschiedenen Channels (Priorität Hoch/Niedrig) und `Task.WhenAny`
   einen prioritätsgesteuerten Consumer bauen?
2. **Skizze/Code:** Implementieren Sie einen Consumer, der bevorzugt Nachrichten aus dem "High-Priority"-Channel liest.

---

---

