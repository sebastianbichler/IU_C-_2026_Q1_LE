# Channels vs. Locks – Der Performance-Showdown im GAE

## 1. Einleitung

Nachdem wir die Mechanismen von klassischem Locking und asynchronen Channels betrachtet haben, bringen wir nun beide Welten zusammen. Für das Grand Arcade Ecosystem (GAE) lautet die Kernfrage: Welches Paradigma überlebt den Stresstest unter realen Hochlast-Bedingungen?

Um diese Frage zu beantworten, haben wir den GAE Concurrency Benchmark entwickelt, der exakt das Problem hunderter autonomer Agenten simuliert, die gleichzeitig in einen getakteten Game-Loop schreiben wollen.

## 2. Live-Demo: Der GAE Concurrency Benchmark

Schauen wir uns die Oberfläche an. Wir haben ein Setup, das ein Szenario extremer Systemauslastung simuliert:

* Wir definieren 1000 gleichzeitige User.
* Jeder User feuert schnellstmöglich 100 State-Updates in das System.
* Wir führen für beide Paradigmen (`LockBasedGame` und `ChannelBasedGame`) nacheinander eine Serie von 5 Testläufen durch, um Ausreißer zu glätten.

Sobald der Test startet, passiert in unserer `MainWindow.xaml.cs` Folgendes:

```csharp
// --- LOCK TEST ---
var lockResult = await _runner.RunSeriesAsync(new LockBasedGame(), users, 100);
UpdateResultDisplay(LblLock, LblLockHistory, lockResult);

// --- CHANNEL TEST ---
var channelResult = await _runner.RunSeriesAsync(new ChannelBasedGame(), users, 100);
UpdateResultDisplay(LblChannel, LblChannelHistory, channelResult);
```

Unser UI wertet die Dauer direkt visuell aus: Unter 30ms leuchtet das Ergebnis grün, bis 100ms orange, und alles darüber wird als kritisch rot markiert.

Dabei zeigen wir zwei entscheidende Messwerte an:
1. **Ø Lastdauer (Gesamtdurchsatz):** Dies ist die Zeit, die das System insgesamt benötigt, um alle 100.000 Netzwerk-Updates (1000 User × 100 Updates) komplett zu verarbeiten. 
2. **Ø Update (Frame-Dauer):** Dies ist die durchschnittliche Dauer eines einzelnen Durchlaufs im Game-Loop. Dieser Wert ist kritisch für das Spielgefühl: Steigt er über 16 Millisekunden, fallen wir unter 60 Bilder pro Sekunde (FPS) und das Spiel beginnt zu ruckeln.

## 3. Code-Analyse: Wie messen wir die Last?

Um zu verstehen, warum die Ergebnisse so drastisch voneinander abweichen, müssen wir einen Blick in unseren `BenchmarkRunner.cs` werfen. Wir haben den Test in zwei getrennte Bereiche aufgeteilt: die Datenverarbeitung (Consumer) und die Datengenerierung (Producer).

### Der Game-Loop (Consumer)
Hier simulieren wir das Herzstück des Spiels. Wir starten einen Hintergrund-Task, der das Spiel am Laufen hält und versucht, stabile 60 Bilder pro Sekunde (FPS) zu liefern. In diesem Loop messen wir mit einer hochpräzisen Stoppuhr exakt die Zeit, die das Spiel für die Berechnung benötigt:

```csharp
var loop = Task.Run(async () => {
    game.Initialize();
    while (!cts.Token.IsCancellationRequested)
    {
        var fSw = Stopwatch.StartNew();      // 1. Stoppuhr starten
        game.Update(0.016);                  // 2. Anstehende Daten verarbeiten
        fSw.Stop();                          // 3. Stoppuhr anhalten
        
        // 4. Dauer sicher auf die Gesamtzeit addieren
        Interlocked.Add(ref totalUpdateTicks, fSw.ElapsedTicks);
        
        await Task.Delay(16); // 5. 16ms warten, um 60 FPS zu simulieren
    }
});
```

### Die simulierte Spieler-Last (Producer)

Während der Game-Loop im 16-Millisekunden-Takt tickt, simulieren wir parallel eine hohe Anzahl an eingehenden Spieler-Updates. Der folgende Code-Block ist der eigentliche Stresstest:

```csharp
await Task.WhenAll(Enumerable.Range(0, userCount).Select(id => Task.Run(() => {
    for (int i = 0; i < updatesPerUser; i++)
        game.OnNetworkUpdateReceived(new PlayerUpdate(id, i, i));
})));
```

Wir generieren nicht einfach nacheinander Updates. Stattdessen starten wir für jeden einzelnen der 1000 User zeitgleich einen eigenen Task (`Task.Run`). Jeder dieser 1000 Tasks versucht sofort, in einer Schleife 100 Positionsdaten an den Server zu senden. 

Weil all diese Tasks im selben Millisekunden-Bruchteil auf die Speicherstrukturen des Spiels zugreifen wollen, simulieren wir eine extreme Ressourcenüberlastung. Genau hier entscheidet sich, welches Synchronisations-Modell unter echter Last zusammenbricht und welches standhält.


## 4. Auswertung

Wenn wir auf unsere Testergebnisse schauen, sehen wir bei den Locks meist rote Werte, während die Channels im grünen Bereich bleiben. Die Gegenüberstellung der jeweiligen Update-Methoden liefert die Erklärung.



### Locks (`LockBasedGame.cs`)

```csharp
public void Update(double deltaTime)
{
    lock (_syncRoot) // <-- Der Flaschenhals
    {
        while (_pendingUpdates.Count > 0)
        {
            var update = _pendingUpdates.Dequeue();
            if (GameLogic.IsMovementValid(update)) { ... }
        }
    }
}
```

Während die aufwändige Spiellogik in `IsMovementValid` läuft, hält der Game-Loop das Lock. Alle 1000 Netzwerk-Threads, die parallel `OnNetworkUpdateReceived` aufrufen, krachen gegen dieses Lock und werden blockiert. 

Das Betriebssystem wird dadurch gezwungen, die Ausführung der Threads permanent zu unterbrechen. Dabei muss es den Hardware-Zustand des blockierten Threads in den Arbeitsspeicher sichern und den Zustand des nächsten Threads laden. Die CPU verbringt am Ende mehr Zeit mit der Verwaltung als mit der Ausführung der eigentlichen Logik.

### Channels (`ChannelBasedGame.cs`)



```csharp
public void Update(double deltaTime)
{
    // TryRead ist dank SingleReader=true extrem optimiert
    while (_updateChannel.Reader.TryRead(out var update))
    {
        if (GameLogic.IsMovementValid(update)) { ... }
    }
}
```

Die Netzwerk-Threads werfen ihre Daten via `TryWrite` auf der Producer-Seite ab und sind sofort wieder frei. Der Game-Loop liest die Queue synchron aus. 

Da wir den Channel mit `SingleReader = true` instanziiert haben, weiß die .NET Runtime, dass ausschließlich der Game-Loop Daten entnimmt. Normale Warteschlangen müssen ständig absichern, dass nicht zwei Threads versehentlich denselben Datensatz auslesen. Da dieser Konflikt bei uns ausgeschlossen ist, entfällt dieser ganze Overhead. Das Auslesen wird extrem schnell. Da der Netzwerk-Empfang und die Spiel-Logik nun völlig unabhängig voneinander arbeiten, wird der Prozessor nicht mehr durch Wartezeiten ausgebremst. Die CPU kann ihre volle Leistung für die echten Berechnungen nutzen, was zu einem absolut flüssigen Spielablauf führt.

## 5. Fazit & Architektur-Entscheidung für GAE

Zu Beginn unserer Analyse haben wir basierend auf **Amdahls Gesetz** zentrale Hypothesen aufgestellt. Unser Live-Beweis bestätigt diese Theorie nun vollumfänglich:

* **Hypothese 1 bestätigt (Geringe Last < 10 User):** Hier kann das klassische `lock` leicht schneller sein. Da das System bei geringer Last noch nicht überlastet ist, bleiben die teuren Kontextwechsel aus. Zudem ist ein einfaches Lock hier im Vorteil, weil die Verwaltung der Channel-Warteschlange komplett entfällt.
* **Hypothese 2 bestätigt (Sättigung):** Sobald die Netzwerk-Updates schneller eintreffen, als das System sie verarbeiten kann, führt Locking zu massivem Ruckeln und Thread-Starvation. Die Channel-basierte Architektur ist hier überlegen, weil sie durch blockadefreies Task-Switching:
    1. Einen stabileren Frame-Takt (weniger Jitter) gewährleistet.
    2. Den CPU-Durchsatz maximiert, indem sie blockierte Threads verhindert.

**Die finale Empfehlung:**
Für alle eingehenden Spielerdaten im GAE setzen wir klar auf die Channel-Architektur. 

Um abschließend auch unseren dritten Hypothesen-Punkt zu garantieren, müssen wir für den echten Live-Betrieb nur noch eine Anpassung vornehmen: Statt einer endlos wachsenden Warteschlange nutzen wir ein festes Limit. Das verhindert, dass bei dauerhafter Überlastung der Arbeitsspeicher vollläuft. Wenn der Platz knapp wird, greift der eingebaute Backpressure-Mechanismus und lässt alte, unwichtige Positionsdaten einfach fallen, um das System dauerhaft stabil und reaktionsschnell zu halten.
