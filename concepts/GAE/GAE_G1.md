### Programmieren mit C# (DSPC016)

---

Gruppe 1 - Memory

Das Thema **Memory Safety** (Speichersicherheit) ist die perfekte Brücke zwischen der "trockenen" Theorie
wissenschaftlicher Artikel und der harten Praxis im **Game-Hub**. In Sprachen wie C oder C++ führt ein falscher
Zeigerzugriff zum Absturz oder zu Sicherheitslücken – in C# (Managed Code) nimmt uns die Runtime viel ab, aber "sicher"
bedeutet nicht "unfehlbar".

Hier ist der Schlachtplan für Gruppe 1:

---

## 1. Theoretischer Einstieg

Bevor sie programmieren, müssen sie definieren, was "Memory Safety" im Kontext des Game-Hubs bedeutet. Es sollen kurz
diese drei Säulen fixiert werden:

- **Managed vs. Unmanaged:** Wir arbeiten in der CLR (Managed), aber Spiele könnten (theoretisch) durch falsches
  Ressourcen-Management "Memory Leaks" verursachen.

- **Type Safety:** Ein Spiel darf nicht auf den Speicher eines anderen Spiels zugreifen können.

- **Ressourcen-Hygiene:** Wenn ein Spiel geschlossen wird, müssen alle Texturen, Sounds und Variablen freigegeben
  werden.

---

## 2. Praktische Umsetzung: Der "Safety Contract"

Gruppe 1 ist der "Gesetzgeber" (Core-Team). Ihr Code muss verhindern, dass unsauber programmierte Spiele das ganze
System (den Hub) mit in den Abgrund reißen.

### Die Aufgabe: IDisposable & Lifecycle

Sie müssen das `IArcadeGame` Interface so erweitern, dass Speichersicherheit durch den Lebenszyklus erzwungen wird.

**C#-Beispiel für Gruppe 1:**

C#

```
namespace GAE.Core;

// Erweiterung des Interfaces um IDisposable
// Das zwingt jedes Spiel dazu, eine "Aufräum-Logik" zu implementieren.
public interface IArcadeGame : IDisposable
{
    string MetadataName { get; }

    // Lifecycle-Methoden
    void Initialize();
    void Unload(); // Zum Freigeben von großen Assets (Bilder, Musik)
}

// Eine Basis-Klasse, die Memory-Safety-Muster vorgibt
public abstract class BaseGame : IArcadeGame
{
    public abstract string MetadataName { get; }
    public abstract void Initialize();
    public abstract void Unload();

    // Standard-Implementierung von IDisposable (Pattern aus Kotz/Wenz)
    public void Dispose()
    {
        Console.WriteLine($"[Memory Safety] Unloading {MetadataName}...");
        Unload();
        GC.SuppressFinalize(this); // Sagt dem Garbage Collector: "Alles erledigt"
    }
}
```

---

## 3. Die Forschungs-Komponente

Damit die Suche nach Artikeln nicht umsonst war, soll Gruppe 1 eine **"Resource Guard"**-Klasse skizzieren.

- **Problemstellung:** Wie verhindern wir, dass ein Spiel 10GB RAM belegt?

- **Implementierungsidee:** Ein einfacher Wrapper, der beim Laden eines Spiels den aktuellen Speicherverbrauch misst (
  `GC.GetTotalMemory`) und bei Überschreitung einer Grenze das Spiel zwangsweise via `Dispose()` beendet.

---

## 4. Hilfestellung

Checkpunkte:

1. **Code-Definition:** Finalisiert das `IArcadeGame` Interface im Core-Projekt. Stellt sicher, dass `IDisposable`
   geerbt wird.

2. **Referenz-Check:** Prüft im Buch von **Kotz/Wenz**, wie man das `Dispose`-Muster (Finalizer) korrekt implementiert,
   damit keine "Dangling Resources" übrig bleiben.

3. **Schnittstelle zum Dashboard:** Wie soll das Dashboard reagieren, wenn ein Spiel abstürzt (`try-catch` Block um den
   Game-Loop), um den Hub stabil zu halten.

### Warum das für den Game-Hub wichtig ist:

Wenn später Gruppe 2 (Input) Fehler machen, sorgt der **Memory Safety Contract** von Gruppe 1 dafür, dass der Hub nicht
abstürzt, sondern das fehlerhafte Modul einfach isoliert und entlädt.

**Möglichkeiten zur Lösung**: "Sandbox"-Überwachung bauen können, die den Speicerverbrauch des aktiven Spiels in
Echtzeit überwacht?
