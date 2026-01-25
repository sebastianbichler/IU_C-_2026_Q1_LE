
---

# Vorlesung: Architekturen von Datenstrukturen in .NET 8

**Dozent:** [Ihr Name] | **Zeit:** 120 Minuten
**Leitspruch:** *"Wer Collections nur als Listen versteht, hat die Hardware noch nicht begriffen."*

---

## I. Die Hardware-Ebene: Warum C#-Collections anders sind (30 Min)

### 1. Reified Generics (C#) vs. Type Erasure (Java)

* **Java:** Verwendet *Type Erasure*. Zur Laufzeit weiß eine `ArrayList<Integer>` nicht mehr, dass sie Integer enthält.
  Alles wird zu `Object`. Folge: **Boxing/Unboxing** und enorme Performance-Einbußen bei Primitiven.
* **C#:** Verwendet *Reified Generics*. Der JIT-Compiler generiert für `List<int>` spezialisierten Maschinencode.
* **Python:** Listen sind Arrays von Zeigern auf Objekte (`PyObject*`). Jeder Zugriff ist ein Pointer-Hopping (Cache
  Misses vorprogrammiert).

### 2. Speicher-Layout: Das Inline-Prinzip

In C# liegen `structs` in einer `List<T>` **zusammenhängend (flat)** im Speicher.

* **Java/Python:** Eine Liste von "Punkten" (X,Y) ist ein Array von Zeigern auf verstreute Objekte auf dem Heap.
* **C#:** Eine `List<PointStruct>` ist ein einziger Speicherblock.
* **GAE-Bezug:** Im Arcade-System müssen wir tausende Partikel oder Highscore-Einträge rendern. Ein "Cache-Miss" durch
  verstreute Java-Objekte würde die Framerate halbieren.

---

## II. Deep Dive: Dictionary & Hashing (25 Min)

### 1. Internals des `Dictionary<TKey, TValue>`

* Wie funktioniert die Kollisionsauflösung? C# nutzt *Chaining* mit einem internen Array von "Buckets" und "Entries".
* **Python-Vergleich:** Pythons `dict` nutzt *Open Addressing* (sehr schnell, aber empfindlich bei hohem Füllgrad).
* **Performance:** Warum ist `Enum` als Key in C# tückisch? (Spoiler: Boxing vor .NET Core, jetzt optimiert).

### 2. Speicher-Komplexität

* Wann wächst eine Collection? C# verdoppelt die Kapazität bei Erreichen des Limits (`Capacity = Capacity * 2`).
* **Übung 1:** Berechnen Sie den Speicher-Overhead einer `List<int>` mit 1.000 Elementen, wenn sie gerade von 512 auf
  1024 gewachsen ist. (Kotz/Wenz Kap. 4).

---

## III. Concurrency: Wenn das GAE-Dashboard brennt (35 Min)

### 1. Das Problem der Threadsicherheit

* **Python:** Das *Global Interpreter Lock (GIL)* verhindert echte Parallelität.
* **C#/Java:** Echte Parallelität möglich, aber gefährlich. Ein Standard-`Dictionary` fliegt Ihnen bei gleichzeitigem
  Schreibzugriff um die Ohren (`InvalidOperationException`).

### 2. ConcurrentDictionary vs. Locking

* Warum nicht einfach `lock(myDict)`? Antwort: **Lock Contention**. Wenn 10 Spiele-Module gleichzeitig Highscores
  senden, warten 9 immer auf den einen.
* `ConcurrentDictionary` nutzt feingranulares Locking (Striped Locking) und lock-freie Lesevorgänge.

### 3. Bounded vs. Unbounded (Das GAE-Infrastruktur-Dilemma)

* **Unbounded:** Eine `ConcurrentQueue` wächst unendlich. Wenn der Highscore-Server laggt, füllt sich der RAM, bis das
  Dashboard abstürzt (Memory Leak).
* **Bounded (Channels):** Ein `System.Threading.Channels.BoundedChannel<T>` hat eine Obergrenze. Ist er voll, wird der
  Producer (das Spiel) gebremst oder Nachrichten werden verworfen (**Backpressure**).

---

## IV. Modern .NET: Zero-Allocation Collections (20 Min)

### 1. Span<T> und Memory<T> (Der "Heilige Gral")

* Früher: `string.Substring()` erzeugt einen neuen String auf dem Heap.
* Heute: `ReadOnlySpan<char>` zeigt auf den bestehenden Speicher. Keine Allokation, kein GC-Druck.
* **GAE-Szenario:** Wir parsen Netzwerk-Pakete der Arcade-Automaten. Mit `Span<T>` verarbeiten wir 1 GB Daten, ohne dass
  der Garbage Collector auch nur einmal aufwacht.

---

## V. Übungen für die Studierenden (Interaktiv)

### Übung A: Das "Race Condition" Experiment (20 Min)

* **Aufgabe:** Starten Sie 100 `Tasks`, die jeweils 1.000 Einträge in eine globale `List<int>` schreiben.
* **Beobachtung:** Warum fehlen am Ende Einträge? (Interleaving von `_size++`).
* **Lösung:** Umbau auf `ConcurrentBag<T>` oder `lock`.

### Übung B: GAE-Infrastruktur Design (15 Min)

* **Szenario:** Ein Spiele-Modul schickt alle 10ms ein `PlayerState`-Objekt (Class).
* **Frage:** Wie optimieren wir das für 100 parallele Spieler?
* **Diskussion:** Umwandlung in `struct`, Nutzung eines `ArrayPool<T>` (Kotz/Wenz Kap. 16), Empfang via `Channel`.

---

## VI. Zusammenfassung & Referenz auf Materialien

1. **Kotz/Wenz (Kapitel 4 & 16):** Lesen Sie die Abschnitte zur TPL und zum Memory Management.
2. **System.Collections (PDF):** Ignorieren Sie `ArrayList` und `Hashtable` (Legacy). Konzentrieren Sie sich auf die
   Generics.
3. **GAE-Projekt:** In der nächsten Phase wird Ihre Performance gemessen. Wer `List<T>` im `Update`-Loop allokiert,
   verliert Punkte.

---

### Dozenten-Tipp für den Abschluss:

*"In Java ist das System für den Programmierer da, in Python für die Bequemlichkeit, aber in C# gehört das System Ihnen.
Nutzen Sie den Speicher direkt, aber nutzen Sie ihn weise."*

**Soll ich für die Übung A den C#-Code für das "Race Condition"-Experiment vorbereiten, damit Sie ihn morgen live
vorführen können?**
