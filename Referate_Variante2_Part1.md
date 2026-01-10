### Programmieren mit C# (DSPC016)

---

# # Referatsunterlagen Variante 2

Dies stellt die zweite Variante für die Anforderungen an die Prüfung bzw. den Referaten. Diese Variante ist
anspruchsvoller als die erste, da sie spezifischer
wissenschaftlichen Fragestellungen und Anwendungsfällen vorsieht. Welche Variante zum Einsatz kommt, werden wir
besprechen.

---

# 🕹️ Rahmenprojekt: The Grand Arcade Ecosystem (GAE)

## 1) Beschreibung der Rahmenaufgabe

Das Ziel ist die Entwicklung eines modularen Spiele-Hubs. Anstatt isolierte Spiele zu bauen, erschaffen die Studierenden
ein Ökosystem, in dem ein zentrales **Arcade-Dashboard (Host)** zur Laufzeit verschiedene Spiele-Module lädt.

- **Das "Plug-and-Play"-Prinzip:** Der Host kennt die konkreten Spiele nicht. Er erkennt sie über gemeinsame
  Schnittstellen (`IAracdeGame`), die in einer `Shared.Core`-Bibliothek definiert sind.

- **Gemeinsame Ressourcen:** Alle Gruppen nutzen und erweitern die `Shared.Core` und `Shared.Data` Bibliotheken. Wenn
  Gruppe 1 einen extrem schnellen Buffer-Mechanismus schreibt, müssen die anderen Gruppen diesen für ihre Highscores
  oder Asset-Ladeprozesse nutzen.

- **Das Dashboard:** Ein zentrales UI-Modul (Avalonia UI), das Statistiken über alle Spiele anzeigt (z.B. globale
  Highscores, Speicherverbrauch pro Spiel, aktive Spieler-Sessions).

---

## 2) Übersicht über die Referatsthemen

| **Gruppe** | **Forschungsfeld**              | **Technischer C# Schwerpunkt**      | **Modul-Beitrag zum GAE**                  |
|------------|---------------------------------|-------------------------------------|--------------------------------------------|
| **1**      | **Memory Safety & Performance** | `Span<T>`, `Memory<T>`, Pointers    | **High-Performance Asset Loader**          |
| **2**      | **Declarative Engines**         | LINQ Provider, Expression Trees     | **Queryable Game Rule Engine**             |
| **3**      | **Scalable Concurrency**        | `Channels<T>`, Async State Machines | **Multiplayer & Global Game Loop**         |
| **4**      | **DDD & Persistence**           | EF Core 8, Value Objects, Records   | **Persistent Player & Savegame Core**      |
| **5**      | **Meta-Programming**            | Source Generators, Reflection       | **Plugin Discovery & Dashboard Telemetry** |

---

## 3) Zusammenarbeit & Workflow

Da alle am selben Ökosystem arbeiten, ist Kommunikation essenziell:

1. **Shared-First Entwicklung:** In den ersten Wochen definieren alle Gruppen gemeinsam die Interfaces in `Shared.Core`.

2. **Pull Request Kultur:** Wer eine Änderung an der `Shared.Core` vornimmt, muss die Approvals der anderen Gruppen
   einholen (Peer Review).

3. **Integration:** Einmal wöchentlich wird der aktuelle Stand im Dashboard zusammengeführt.

4. **Wissenschaftlicher Diskurs:** In der Abschlusspräsentation muss jede Gruppe zeigen, wie ihre "wissenschaftliche
   Komponente" die Performance oder Stabilität der Spiele der anderen Gruppen verbessert hat.

---

## 4) Detaillierte Beschreibung der Referatsthemen

### Gruppe 1: Der "Zero-Allocation" Asset Manager

- **Fokus:** Minimierung des Garbage Collectors in ressourcenintensiven Anwendungen.

- **Wissenschaftliche Frage:** "Inwieweit lässt sich die Frame-Stabilität (Vermeidung von GC-Rucklern) durch konsequente
  Nutzung von Stack-Allokation und Memory-Pooling in C# optimieren?"

- **C#-Spezialität:** `Span<T>`, `ArrayPool<T>`, `Unsafe`-Code für direkten Speicherzugriff.

- **Auftrag:** Entwickelt eine Komponente, die Game-Assets (Bilder, Map-Daten) lädt und verarbeitet, ohne den Heap zu
  belasten. Stellt den anderen Gruppen "Buffer-Schnittstellen" zur Verfügung.

### Gruppe 2: Der "Logic-as-Data" Rule Engine

- **Fokus:** Deklarative Programmierung zur Laufzeit-Modifikation von Spielregeln.

- **Wissenschaftliche Frage:** "Wie können Expression Trees genutzt werden, um Spielregeln (z.B. 'Wenn Gold > 100 UND
  Level < 5') zur Laufzeit sicher zu evaluieren, ohne Performance-Einbußen durch klassische Interpretation?"

- **C#-Spezialität:** Custom LINQ Provider oder Expression Tree Visitor Pattern.

- **Auftrag:** Erstellt ein System, mit dem Spiele ihre Regeln deklarativ definieren können. Das Dashboard nutzt dies,
  um spielübergreifende Erfolge (Achievements) zu filtern.

### Gruppe 3: Der "Async Multi-Agent" Controller

- **Fokus:** Beherrschung von Nebenläufigkeit und komplexen Zuständen.

- **Wissenschaftliche Frage:** "Vergleich von klassischen Locks gegenüber asynchronen Channels bei der Synchronisation
  von hochfrequenten Spielzuständen in einer Multi-User-Umgebung."

- **C#-Spezialität:** `System.Threading.Channels`, `TaskCompletionSource`, Async Iterators.

- **Auftrag:** Implementiert den globalen Game-Loop und ein einfaches Netzwerk-Protokoll (simuliert oder via SignalR),
  das mehrere Spielinstanzen synchronisiert.

### Gruppe 4: Das "Impedance-Balanced" Savegame-System

- **Fokus:** Saubere Trennung von Domänenlogik und Datenbanktechnologie (DDD).

- **Wissenschaftliche Frage:** "Wie unterstützen C# Records und Non-Nullable Reference Types die Implementierung von '
  Always-Valid' Domain Models in komplexen Persistenz-Szenarien?"

- **C#-Spezialität:** Entity Framework Core 8 (Value Converters, Owned Types), C# Records.

- **Auftrag:** Erstellt die Persistenzschicht für Spielerprofile, Inventare und Spielstände. Achtet darauf, dass die
  Spiel-Logik absolut unabhängig von der SQLite-Datenbank bleibt.

### Gruppe 5 (Zusatz/Vertiefung): Der "Auto-Discovery" Orchestrator

- **Fokus:** Erhöhung der Start-Performance und Typsicherheit durch Kompilierzeit-Logik.

- **Wissenschaftliche Frage:** "Vorteile von Source Generators gegenüber Runtime-Reflection bei der dynamischen
  Modul-Entdeckung in Plug-and-Play-Architekturen."

- **C#-Spezialität:** .NET Source Generators (IIncrementalGenerator), Diagnostics.

- **Auftrag:** Schreibt einen Source Generator, der automatisch alle Klassen findet, die `IArcadeGame` implementieren,
  und den Boilerplate-Code für das Dashboard-Menü generiert.

---

### Mein Tipp für die Umsetzung:

Lassen Sie die Studierenden als konkretes Beispiel ein einfaches **"Snake"** oder **"Minesweeper"** implementieren, um
das Framework zu testen. Gruppe 2 könnte dann zeigen, wie sie Minesweeper-Regeln via LINQ ändert, während Gruppe 1
demonstriert, dass die Highscores der Snake-KI keinerlei Garbage Collection verursachen.

**Soll ich Ihnen für eines dieser Themen (z.B. Gruppe 1 oder 3) bereits ein konkretes Code-Skelett für die `Shared.Core`
erstellen?**

--

Für das Projekt **"The Grand Arcade Ecosystem" (GAE)** ist eine saubere physikalische Trennung der Projekte in der
Solution entscheidend. Dies erzwingt die Einhaltung von Architekturregeln (wie die *Dependency Rule* der Clean
Architecture) und ermöglicht das "Plug-and-Play"-Prinzip.

Hier ist das vorgeschlagene Layout für Ihre .NET-Solution:

### Die GAE Solution-Struktur

Plaintext

```
GAE.Solution
├── 01_Shared
│   ├── GAE.Shared.Core         <-- Das Herzstück: IArcadeGame, Enums, Basis-Interfaces
│   ├── GAE.Shared.Kernel       <-- Gruppe 1 & 3: High-Perf-Buffer, Async-Schnittstellen
│   └── GAE.Shared.Domain       <-- Gruppe 4: Domain Models (Records), Repositories
│
├── 02_Infrastructure
│   ├── GAE.Infra.Persistence   <-- Gruppe 4: EF Core Umsetzung, SQLite-Anbindung
│   ├── GAE.Infra.Assets        <-- Gruppe 1: Asset-Loading-Logik (Span<T>, Memory<T>)
│   └── GAE.Infra.Rules         <-- Gruppe 2: LINQ-Provider & Expression-Evaluation
│
├── 03_Modules (Die konkreten Spiele/Features)
│   ├── GAE.Module.Snake        <-- Beispiel-Spiel (Nutzt Shared.Core)
│   ├── GAE.Module.Minesweeper  <-- Beispiel-Spiel
│   └── GAE.Module.Telemetry    <-- Gruppe 5: Dashboard-Metriken & Source Generators
│
├── 04_Host
│   └── GAE.Host.Dashboard      <-- Avalonia UI App: Lädt Module dynamisch zur Laufzeit
│
└── tests
    ├── GAE.Tests.Unit
    └── GAE.Tests.Integration
```

---
