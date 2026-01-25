---

Hier ist die Übersicht der wichtigsten .NET-Collections mit Fokus auf Zeitkomplexität, Speicherverhalten und
Anwendungsfall.

---

# 🚀 C# Collection Cheat-Sheet: Performance & Big O

### 1. Die Standard-Werkzeuge (Generics)

*Namespace: `System.Collections.Generic*`

| Collection            | Zugriff (Index) | Suche (Value) | Einfügen | Besonderheit                                       |
|-----------------------|-----------------|---------------|----------|----------------------------------------------------|
| **`List<T>`**         |                 |               |          | Standard-Wahl. Nutzt intern ein Array.             |
| **`Dictionary<K,V>`** | (via Key)       |               |          | Extrem schnell für Key-Lookup. Braucht mehr RAM.   |
| **`HashSet<T>`**      | n/a             |               |          | Wie Dictionary, aber nur für eindeutige Werte.     |
| **`Queue<T>`**        | n/a             |               |          | FIFO (First-In-First-Out). Ideal für Event-Buffer. |
| **`Stack<T>`**        | n/a             |               |          | LIFO (Last-In-First-Out). Gut für Undo-Funktionen. |

*Am Ende der Liste. Wenn das interne Array voll ist, erfolgt ein Resize ().*

---

### 2. Thread-Sichere Alternativen

*Namespace: `System.Collections.Concurrent*`

Nutzen Sie diese im GAE, wenn mehrere Spiele-Module gleichzeitig Daten an den Server senden.

* **`ConcurrentDictionary<K,V>`**: Erlaubt paralleles Lesen/Schreiben ohne manuellen `lock`.
* **`ConcurrentQueue<T>`**: Lock-frei implementiert, sehr performant für Producer-Consumer-Szenarien.
* **`ConcurrentBag<T>`**: Optimiert für Szenarien, in denen derselbe Thread Daten produziert und auch wieder konsumiert.

---

### 3. Entscheidungshilfe für das GAE-Projekt

| Wenn du ...                    | ... dann nimm:          | Warum?                                                               |
|--------------------------------|-------------------------|----------------------------------------------------------------------|
| **Highscores nach ID suchst**  | `Dictionary<Guid, int>` | Sofortiger Zugriff auf den Score via Spieler-ID.                     |
| **Einfache Listen renderst**   | `List<T>`               | Geringster Overhead, schnellste Iteration für die UI.                |
| **Eindeutige Tags speicherst** | `HashSet<string>`       | Verhindert Duplikate automatisch (z.B. Genres: "Action", "Classic"). |
| **Network-Pakete pufferst**    | `ConcurrentQueue<T>`    | Mehrere Netzwerk-Threads können sicher Daten einreihen.              |

---

### 4. Memory-Check: Werte- vs. Referenztypen

Erinnern Sie sich an den Kernaspekt der Vorlesung: Speicherlayout und Cache-Lokalität!

* **`List<int>` / `List<MyStruct>**`:
* **Speicher:** Liegt kompakt („contiguous“) im RAM.
* **CPU:** Extrem schnell durch L1/L2 Cache-Hits.


* **`List<MyClass>`**:
* **Speicher:** Liste enthält nur Pointer. Die Objekte liegen verstreut auf dem Heap.
* **CPU:** Langsamer durch „Pointer-Chasing“ (Cache-Misses).

---

### 💡 Goldene Regel für die Übung:

> "Wähle die Collection nach dem häufigsten Zugriffsmuster. Musst du suchen? Nimm ein Dictionary. Gehst du nur die Liste
> von oben nach unten durch? Nimm eine `List<T>`."
