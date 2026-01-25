---

# Übungsserie: Deep Dive Collections & Memory Performance

**Zeitrahmen:** 120 Minuten | **Fokus:** GAE (Grand Arcade Ecosystem)

## Teil 1: Die Evolution – Von Java-Style zu C# Generics (30 Min)

*Bezug: Collections_V1.pptx (Folie 6-8) & PDF (ArrayList vs. List)*

### Aufgabe 1.1: Das Boxing-Problem (Theorie & Code)

In der Präsentation wird erwähnt, dass C# `System.Collections` (nicht-generisch) und `System.Collections.Generic` bietet.

1. Erstellen Sie eine `ArrayList` (nicht-generisch) und eine `List<int>` (generisch).
2. Fügen Sie 1.000.000 Integer hinzu.
3. **Diskussion:** Warum verbraucht die `ArrayList` deutlich mehr Speicher auf dem Heap und belastet den Garbage Collector (GC) stärker?
4. **C#-Spezifik:** Erklären Sie den Begriff **Boxing** im Kontext von Wertetypen (`int`) und dem `object`-Interface der `ArrayList`.

---

## Teil 2: Performance & Speicher-Layout (30 Min)

*Bezug: Kotz/Wenz (Referenztypen) & PDF (Hashtable vs. Dictionary)*

### Aufgabe 2.1: Structs vs. Classes in Collections

Im GAE müssen wir Spieler-Positionen speichern.

1. Definieren Sie `public struct PositionS { public int X, Y; }` und `public class PositionC { public int X, Y; }`.
2. Erstellen Sie jeweils eine `List<T>` für beide Typen.
3. **Analyse:** Warum ist die Iteration über `List<PositionS>` schneller als über `List<PositionC>`?

* *Hinweis:* Stichwort **Cache-Lokalität** und **Indirektion**.


4. **C#-Besonderheit:** Nutzen Sie `Span<T>`, um einen Teilbereich der Liste zu verarbeiten, ohne eine Kopie zu
   erstellen.

---

## Teil 3: Funktionale Programmierung & LINQ (30 Min)

*Bezug: Moderne C# Entwicklung & GAE-Logik*

### Aufgabe 3.1: Das Dashboard-Filter-System

Das GAE-Dashboard erhält eine Liste von `GameSession`-Objekten (Properties: `GameName`, `PlayerCount`, `IsActive`).

1. Schreiben Sie eine LINQ-Abfrage (Fluent Syntax), die:

* Alle aktiven Spiele filtert.
* Diese nach `PlayerCount` absteigend sortiert.
* Nur die Namen der Top 3 Spiele zurückgibt.


2. **Lambda-Check:** Verwenden Sie eine anonyme Funktion innerhalb von `.Select()`.
3. **Lazy Evaluation:** Beweisen Sie durch ein `Console.WriteLine` innerhalb eines Filters, dass die Abfrage erst
   ausgeführt wird, wenn `.ToList()` oder `foreach` aufgerufen wird.

---

## Teil 4: Thread-Sicherheit & Channels (30 Min)

*Bezug: PDF (Queue/Hashtable) & Scalable Concurrency*

### Aufgabe 4.1: Der Event-Bus

Mehrere Spiele senden gleichzeitig Highscores an das Dashboard. Eine normale `List<T>` würde hier eine
`InvalidOperationException` werfen.

1. Vergleichen Sie `System.Collections.Concurrent.ConcurrentQueue<T>` mit der normalen `Queue<T>` aus dem PDF.
2. Implementieren Sie einen minimalen "Producer-Consumer"-Worker:

* Task A (Producer) schreibt 100 Nachrichten.
* Task B (Consumer) liest sie aus.


3. **Master-Frage:** Warum ist ein `Channel<T>` (aus .NET 8) oft die bessere Wahl für eine asynchrone Pipeline als eine
   `ConcurrentQueue`? (Thema: Asynchrones Warten statt Polling).

---


### Zu 4.1 (Thread-Sicherheit)

```csharp
var queue = new ConcurrentQueue<string>();
// Sicherer Zugriff ohne explizites lock(obj)
if (queue.TryDequeue(out string result)) {
    Console.WriteLine(result);
}

```

---

## Zusammenfassung für die Studierenden (Handout-Stichpunkte)

* **Wähle `List<T>**`, wenn du schnellen Index-Zugriff brauchst.
* **Wähle `Dictionary<K,V>**`, wenn du Elemente über einen Key suchen willst (O(1)).
* **Vermeide `ArrayList**` (Legacy aus Java-Zeiten, siehe PPT).
* **Nutze `struct**` für kleine Datenmengen in Listen, um den Heap zu entlasten.
* **Nutze `LINQ**` für deklarativen, lesbaren Code, aber achte auf die Performance in engen Schleifen.

**Möchten Sie, dass ich für eine dieser Übungen ein detailliertes Code-Gerüst erstelle, das die Studierenden als
Startpunkt nutzen können?**
