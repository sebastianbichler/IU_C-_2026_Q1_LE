### Programmieren mit C# (DSPC016)

---
# Zusammenarbeit zwischen den Gruppen im Grand Arcade Ecosystem (GAE)

### Gruppe 2: Deklarative Programmierung & LINQ-Provider

**Der Fokus:** Wie man Logik von der Ausführung trennt. In der Arcade-Welt wollen wir "Regeln" für Spiele (z.B. "Wer
mehr als 100 Punkte hat, ist ein Profi") nicht fest programmieren, sondern als Daten behandeln, die wir analysieren oder
sogar speichern können.

#### Die C#-Besonderheit: Expression Trees (Ausdrucksbäume)

Ein normaler Code-Schnipsel wie `x => x > 5` ist für den Computer nach dem Kompilieren eine "Black Box" – er kann ihn
ausführen, aber nicht mehr hineinschauen.

* **Was ist ein Expression Tree?** Wenn wir `Expression<Func<T, bool>>` statt `Func<T, bool>` schreiben, erstellt der
  Compiler keinen fertigen Maschinencode, sondern eine **Datenstruktur (einen Baum)**.
* **Warum ist das wichtig?** Wir können diesen Baum zur Laufzeit "begehen" (traversieren). Wir können prüfen: "Welche
  Eigenschaft wird hier verglichen?" oder "Wird ein 'Größer-als'-Zeichen verwendet?".

#### Code-Beispiel: Die "Regel-Lupe"

```csharp
// Wir definieren eine Regel als Expression (Baum), nicht als Methode
Expression<Func<Player, bool>> profiCheck = p => p.Score > 100;

// Jetzt können wir den Baum untersuchen (vereinfacht):
if (profiCheck.Body is BinaryExpression binär)
{
    Console.WriteLine($"Links steht: {binär.Left}");   // p.Score
    Console.WriteLine($"Operator ist: {binär.NodeType}"); // GreaterThan
    Console.WriteLine($"Rechts steht: {binär.Right}");  // 100
}

```

---

### Gruppe 3: Scalable Concurrency & State Machines

**Der Fokus:** Wie man tausende Dinge gleichzeitig tut, ohne dass das Programm abstürzt oder langsam wird. In der Arcade
müssen wir Highscores empfangen, die UI rendern und Netzwerkdaten prüfen – gleichzeitig.

#### Die C#-Besonderheit: `System.Threading.Channels`

Stellen Sie sich einen Briefkasten vor. Mehrere Leute (Producer) werfen Briefe ein, und einer (Consumer) arbeitet sie
nacheinander ab.

* **Das Problem:** Klassische Listen brauchen `lock`, damit zwei Leute nicht gleichzeitig denselben Platz im Speicher
  beschreiben. Das stoppt das Programm (Stau).
* **Die Lösung:** `Channels` sind wie eine Einbahnstraße für Daten. Sie sind extrem schnell und "thread-sicher", ohne
  dass man manuell Sperren (`locks`) einbauen muss. Sie nutzen die asynchrone Natur von C# perfekt aus.

#### Code-Beispiel: Der Arcade-Event-Kanal

```csharp
// Ein Kanal für Spiel-Events (z.B. "Sieg", "Tod")
var kanal = Channel.CreateUnbounded<string>();

// Producer (Das Spiel): Schickt Daten einfach ab
await kanal.Writer.WriteAsync("Spieler 1 hat gewonnen!");

// Consumer (Das Dashboard): Wartet asynchron auf Daten
await foreach (var nachricht in kanal.Reader.ReadAllAsync())
{
    Console.WriteLine($"Dashboard meldet: {nachricht}");
}

```

---

### Gruppe 4: Domain Driven Design (DDD) & Persistence

**Der Fokus:** Wie man die "echte Welt" (die Logik der Spiele) sauber von der "Technik-Welt" (Datenbanken) trennt. Wir
wollen nicht, dass ein Fehler in der Datenbank das ganze Spiel zerstört.

#### Die C#-Besonderheit: Records (Unveränderliche Daten)

In der klassischen Programmierung kann jeder einen Wert ändern: `player.Score = -500;`. Das führt zu Fehlern.

* **Was sind Records?** Sie sind "Read-Only" (unveränderlich) per Design. Wenn sich etwas ändert, erstellen wir eine
  Kopie mit dem neuen Wert.
* **Vorteil:** Man kann sich darauf verlassen, dass Daten, die man einmal geprüft hat, sich nicht "hinter dem Rücken"
  ändern. Das ist perfekt für Spielstände und Profile.

#### Code-Beispiel: Der "unbestechliche" Punktestand

```csharp
// Ein Record kann nach der Erstellung nicht mehr geändert werden
public record HighscoreEntry(string PlayerName, int Points);

var alterScore = new HighscoreEntry("Max", 100);

// Das hier geht NICHT: alterScore.Points = 110;

// Wir erstellen stattdessen eine Kopie mit einem neuen Wert:
var neuerScore = alterScore with { Points = 110 };

Console.WriteLine(alterScore.Points); // Immer noch 100 (Sicherheit!)
Console.WriteLine(neuerScore.Points); // 110

```

---

### Zusammenfassung für die Übung (Quick-Check)

| Gruppe | Technik       | Metapher                                                 | Kern-Vorteil                         |
|--------|---------------|----------------------------------------------------------|--------------------------------------|
| **1**  | `Span<T>`     | Ein Fenster auf ein Buch, statt das Buch zu kopieren.    | **Speed & weniger Müll (GC)**        |
| **2**  | `Expressions` | Ein Rezept lesen, statt direkt zu kochen.                | **Logik wird flexibel & prüfbar**    |
| **3**  | `Channels`    | Ein fließendes Fließband statt einer Stop-and-Go Ampel.  | **Keine Staus bei vielen Aufgaben**  |
| **4**  | `Records`     | Ein versiegelter Brief statt einer beschreibbaren Tafel. | **Keine Fehler durch Datenänderung** |
