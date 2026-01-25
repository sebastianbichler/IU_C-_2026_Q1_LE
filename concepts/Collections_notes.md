

---

# High-Performance Collections & Memory

**Thema:** C# Dateneffizienz im Vergleich zu Java/Python

**Kontext:** Grand Arcade Ecosystem (GAE) Infrastruktur

---

## Modul 1: Die Hardware-Ebene (Stack vs. Heap)

**Ziel:** Verständnis für Cache-Lokalität und den Impact von Generics.

### 🎤 Sprechernotizen (Hintergrund):

* **Der Java-Vergleich:** Erklären Sie, dass Java bei Generics „Type Erasure“ nutzt. Eine `ArrayList<Integer>` speichert
  keine Zahlen, sondern Referenzen auf `Integer`-Objekte. Das bedeutet: Für jede Zahl muss der Prozessor an eine andere
  Stelle im RAM springen (Pointer-Hopping).
* **C# Besonderheit:** C# nutzt „Reified Generics“. Eine `List<int>` ist im Speicher ein echter Block aus
  4-Byte-Integern. Das ist **Cache-freundlich**. Die CPU kann diese Daten in einem Rutsch vorladen.
* **Bezug zum GAE:** Wenn wir 60 FPS (Frames per Second) erreichen wollen, haben wir nur 16,6ms Zeit pro Frame. Jede
  Speicher-Allokation und jeder Pointer-Sprung kostet wertvolle Mikrosekunden.

### 💡 Technischer Deep-Dive:

In Python sind Listen eigentlich `PyObject**`. Jedes Element ist ein Zeiger auf ein Objekt, das irgendwo im Heap liegt.
Das macht Python-Listen extrem flexibel (heterogen), aber für mathematische Operationen unbrauchbar langsam, weshalb man
dort auf C-Erweiterungen wie *NumPy* ausweicht. C# bietet diese Performance nativ.

---

## Modul 2: Dictionaries & Hashing

**Ziel:** Warum `Dictionary<TKey, TValue>` in C# so schnell ist.

### 🎤 Sprechernotizen:

* Ein Dictionary ist keine Zauberei. Es ist ein Array, dessen Index durch einen Hash-Code berechnet wird.
* **Kollisionen:** Was passiert, wenn zwei Keys den gleichen Hash haben? C# nutzt intern „Chaining“ via Indices.
* **GAE-Bezug:** Wir nutzen ein Dictionary, um die `PlayerID` (Guid) auf das `PlayerState`-Objekt abzubilden. Ein
  Zugriff dauert theoretisch . Aber Vorsicht bei großen Dictionaries: Das „Re-Hashing“ (wenn das interne Array
  vergrößert werden muss) kann kurzzeitig das ganze System einfrieren lassen.

---

## Modul 3: Concurrency & Race Conditions

**Ziel:** Warum `List<T>` im Multi-Threading gefährlich ist.

### 🎤 Sprechernotizen:

* Wenn zwei Threads gleichzeitig `list.Add()` aufrufen, lesen beide den aktuellen Index (z.B. 5). Beide schreiben ihren
  Wert an Stelle 5 und erhöhen den Index auf 6. Ein Wert geht verloren.
* **Lösung 1 (Lock):** Sicher, aber langsam. Threads müssen „anstehen“.
* **Lösung 2 (Concurrent Collections):** Nutzt „Compare-and-Swap“ (CAS) Operationen auf CPU-Ebene. Das ist fast so
  schnell wie ohne Lock, aber sicher.

---

## 🛠 Das Übungsblatt: Aufgaben & Musterlösungen

### Übung A: Das Race Condition Experiment (Task-Parallelität)

**Aufgabe:** Beweisen Sie die Unsicherheit der Standard-Liste.

```csharp
// --- AUFGABE ---
// Starten Sie 10 Tasks, die jeweils 1.000 Zahlen in eine List<int> schreiben.
// Prüfen Sie am Ende die Count-Eigenschaft.

// --- MUSTERLÖSUNG ---
List<int> unsichereListe = new();
List<Task> tasks = new();

for (int t = 0; t < 10; t++) {
    tasks.Add(Task.Run(() => {
        for (int i = 0; i < 1000; i++) unsichereListe.Add(i);
    }));
}
await Task.WhenAll(tasks);

Console.WriteLine($"Erwartet: 10000, Tatsächlich: {unsichereListe.Count}");
// Ergebnis wird fast immer < 10000 sein oder abstürzen.

// --- KORREKTUR ---
var sichereListe = new System.Collections.Concurrent.ConcurrentBag<int>();
// ... gleicher Loop ...
// Ergebnis: 10000.

```

### Übung B: Der GAE High-Performance Parser (Span<T>)

**Aufgabe:** Parsen Sie Telemetriedaten, ohne neue Strings zu erzeugen.

```csharp
// --- AUFGABE ---
// Gegeben: "PLAYER:Sebastian;SCORE:5000"
// Extrahieren Sie den Score als int via ReadOnlySpan<char>.

// --- MUSTERLÖSUNG ---
string raw = "PLAYER:Sebastian;SCORE:5000";
ReadOnlySpan<char> span = raw.AsSpan();

int scoreIndex = span.IndexOf("SCORE:") + 6;
ReadOnlySpan<char> scoreValue = span.Slice(scoreIndex);

int score = int.Parse(scoreValue);
Console.WriteLine($"Score erfolgreich geparst: {score}");

```

---

## 📋 Checkliste für den Dozenten (Zum Vorlesen am Ende)

* **Boxing vermeiden:** Benutzt immer `List<T>` (Generics) statt der alten `ArrayList` (Object). (Bezug Kotz/Wenz Kap.
  4).
* **Memory-Pressure:** Erklärt, dass der Garbage Collector (GC) in C# „Generationen“ nutzt (Gen 0, 1, 2). Wer viele
  kleine Objekte in Collections wirft, zwingt den GC zu „Stop-the-World“ Pausen.
* **GAE-Design:** In der finalen Architektur des GAE-Dashboards sollten wir **Channels** für eingehende Events nutzen
  und **ConcurrentDictionary** für die Verwaltung aktiver Spiele-Sessions.

---

### Hintergrund-Info für Sie:

Dieses Skript deckt die Brücke von den Grundlagen (Kotz/Wenz) bis hin zu Master-Level-Konzepten ab.
Sollten die Studierenden fragen: „Warum nicht einfach Python?“, lautet die Antwort: „Weil Python im GAE bei 100
parallelen Spielern aufgrund des Speicher-Managements und des GIL (Global Interpreter Lock) das Dashboard auf
Diashow-Niveau drosseln würde.“
