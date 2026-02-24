# Referat: Declarative Engines, LINQ & Expression Trees (Gruppe 2)

Ziel unserer Arbeit ist die **Analyse, Konzeption und prototypische Umsetzung** einer deklarativen Engine auf Basis von **LINQ** und **Expression Trees**. Die Engine soll im Spielehub-Kontext generisch wiederverwendbar sein und *Queryable Game Engine Rules* über ein zentrales Interface anbieten (z. B. für Achievements, Highscores, Rankings).

**Rahmenbedingungen aus dem Gesamtprojekt**

- **Plug-and-Play:** Der Host kennt konkrete Spiele nicht, sondern nutzt gemeinsame Schnittstellen (z. B. `IArcadeGame`) aus der Shared-Core-Bibliothek.
- **Gemeinsame Ressourcen:** Alle erstellten Kernbibliotheken sollen innerhalb Shared.Data und Shared.Core bereitgestellt werden.

## 1. Einleitung und Motivation

### 1.1 Thema, Problem- und Zielstellung

Ein Spielehub vereint mehrere, heterogene Spiele unter einer gemeinsamen Oberfläche. Dadurch entstehen Anforderungen, die in einzelnen Spielen oft „lokal“ (und imperativ) gelöst würden, im Hub aber **zentral, generisch und erweiterbar** bereitgestellt werden sollen – insbesondere rund um *Abfragen* auf Spielzustand oder Spielereignisse.

**Warum „deklarativ“?**

- Wiederverwendbare Query-Logik lässt sich besser als *„Was möchte ich wissen?“* ausdrücken – statt *„Wie iteriere ich über Daten und filtere sie?“*.
- Deklarative Regeln/Queries sind oft **lesbarer** und transportieren Intention klarer; dadurch können auch Nicht-„Engine“-Entwickler (z. B. Game Design) Logik leichter nachvollziehen und iterieren (vgl. [Zichao_Handout.md](Zichao_Handout.md), Abschnitt 1).
- LINQ ist in C# ein etabliertes deklaratives Modell; über `IQueryable<T>` lassen sich Abfragen als **Expression Trees** repräsentieren und z. B. in eine Engine- oder Storage-spezifische Ausführung übersetzen.
- Gegenüber imperativen Implementierungen (Querverweis/Abgleich mit anderen Gruppen) soll gezeigt werden, welche **Stärken/Schwächen** ein Provider-/Expression-Tree-Ansatz im Projektkontext hat.

**Problemstatement**

- Es wird eine Lösung benötigt, die *Regeln/Abfragen* für hub-weite Funktionen (Achievements/Highscores/Rankings/Filter) **deklarativ definierbar** macht.
- Die Lösung muss **generisch nutzbar** sein (von anderen Gruppen/Spielen), ohne dass Spielehub oder Engine konkrete Spiele kennen.

**Zielstellung**

- Entwurf und prototypische Implementierung einer **P/LINQ-basierten** deklarativen Engine für *Queryable Game Rules*.
- Bereitstellung von Schnittstellen/Query-APIs, die andere Gruppen im Hub verwenden können.

Dabei steht **P/LINQ** in unserem Kontext für *Provider-LINQ*: LINQ-Abfragen über `IQueryable<T>`, die als **Expression Trees** erfasst und durch einen Provider in eine ausführbare Form übersetzt werden.

**Qualitätsziele (ISO/IEC 25010:2023 – Auswahl)**

- **Funktionelle Eignung:** Regeln/Abfragen liefern korrekte Ergebnisse für den vorgesehenen Zweck.
- **Modifizierbarkeit (Erweiterbarkeit):** Neue Regeltypen (z. B. Rankings, Event-Filter) sollen ohne invasive Änderungen integrierbar sein.
- **Modularität:** Minimale Kopplung, klare Verantwortlichkeiten, austauschbare Provider/Backends.

**MVP-Fokus**

Das MVP muss mindestens **Achievement-Management oder Highscore-Management** unterstützen und daraus abgeleitet folgende funktionale Anforderungen erfüllen:

- **F01:** Regeldefinitionen deklarativ ermöglichen.
- **F02:** P/LINQ (Provider) als Abstraktionsmechanismus nutzen.
- **F03:** Generische, von anderen Gruppen nutzbare Query-Logiken bzw. Schnittstellen bereitstellen.

Erweiterungen (nach MVP): Rankings, Highscores, Event-Filter, weitere Storage-/Provider-Backends.

### 1.2 Ausblick / Anschlussfähigkeit

Damit die README später als „Drehscheibe“ funktioniert, planen wir folgende Ergänzungen (Platzhalter):

- **Diagramm:** Architektur-Skizze „Host ↔ Shared.Core ↔ Query Engine ↔ Provider/Backend“.
- **Tabelle:** Vergleich deklarativer Ansätze (LINQ/Provider vs. Rule Engines/Rete vs. State-Effect-Pattern).
- **Querverweise:** Verlinkung auf Planungs-/Konzeptdokumente anderer Gruppen und auf unsere Detailnotizen.

## 2. Theoretische Grundlagen

### 2.1 Deklarativ vs. imperativ (unabhängig von C#)

- **Imperativ:** beschreibt *Ablauf/Schritte* („wie“) – z. B. Schleifen, Mutationen, Kontrollfluss.
- **Deklarativ:** beschreibt *Eigenschaft/Ziel* („was“) – z. B. Prädikate, Constraints, Regeln, Abfragen.

Die Einordnung samt Beispiel (foreach vs. `Where/OrderBy/Take`) ist in der Vorarbeit gut illustriert: vgl. [Zichao_Handout.md](Zichao_Handout.md), Abschnitt 1.

Im Projektkontext ist der Nutzen deklarativer Ansätze vor allem:

- **Komponierbarkeit:** Regeln/Abfragen lassen sich kombinieren (Filter, Projektionen, Aggregationen).
- **Trennung von Spezifikation und Ausführung:** dieselbe Query kann je nach Backend anders ausgeführt werden.
- **Potenzial für Optimierung:** Ausführung kann optimiert (oder parallelisiert) werden, ohne die Regeldefinition zu ändern.

### 2.2 LINQ als deklaratives Query-Modell

LINQ („Language Integrated Query“) beschreibt Abfragen in C# so, dass Entwickler primär das *Was* ausdrücken:

- Filtern (`Where`), Projizieren (`Select`), Aggregieren (`GroupBy`, `Count`, …)
- Pipeline-Stil, häufig als „Query Expression“ oder Method Syntax.

Wichtig ist dabei die Unterscheidung:

- `IEnumerable<T>`: Abfragen laufen typischerweise **in-memory** und werden als Delegates/Iteratoren ausgeführt.
- `IQueryable<T>`: Abfragen werden als **Expression Trees** modelliert und können von einem Provider interpretiert/übersetzt werden (z. B. in SQL, in eine eigene Regel-Engine, in einen optimierten Evaluator).

Diese Trennung (LINQ to Objects vs. LINQ to SQL, inkl. Deferred Execution) wird in der Vorarbeit explizit herausgearbeitet: vgl. [Zichao_Handout.md](Zichao_Handout.md), Abschnitt 2.

**Kurze Gegenüberstellung**

| Aspekt | `IEnumerable<T>` (LINQ to Objects) | `IQueryable<T>` (Provider-LINQ) |
|---|---|---|
| Repräsentation | Delegates/Iteratoren | Expression Trees (`Expression`) |
| Typische Datenquelle | In-Memory Collections | Externe Datenquellen oder eigene Engine |
| Ausführung | i. d. R. direkt im Prozess | Übersetzung + Ausführung durch Provider |
| Optimierung | begrenzt (primär durch .NET) | möglich (Rewrite/Optimize vor Execute) |
| Fehlerbilder | meist zur Laufzeit/Logikfehler | häufig „nicht übersetzbar“/Validierungsfehler |
| Typische Nutzung im Projekt | lokale Auswertung, kleine Hilfsqueries | zentrale, deklarative Regeln + austauschbare Backends |

### 2.3 LINQ Provider & Expression Trees

Ein LINQ-Provider ist im Wesentlichen ein Übersetzer + Executor:

- **Capture:** Query-Operationen werden als Expression Tree aufgebaut.
- **Analyse/Rewrite:** Ausdruck wird geprüft, ggf. normalisiert/optimiert.
- **Ausführung:** Ausdruck wird auf ein Zielsystem angewendet (In-Memory, Datenbank, Engine).

Expression Trees sind dabei die zentrale Repräsentation:

- Sie erlauben, Code als Datenstruktur zu inspizieren (Operatoren, Member-Zugriffe, Konstanten, Lambda-Ausdrücke).
- Sie sind die Grundlage, um „deklarative Regeln“ **nicht nur auszuführen**, sondern **zu interpretieren**.

Für die technische Intuition (AST-Struktur, `Expression<Func<...>>`) und die Provider-Pipeline (Parse/Validate/Optimize/Translate/Execute/Map) siehe auch: [Zichao_Handout.md](Zichao_Handout.md), Abschnitt 3–4.

Expression Trees kann man als „Code als Datenstruktur“ verstehen. Häufige Knotenarten sind z. B. `BinaryExpression` (Operatoren wie `&&`, `>=`), `MemberExpression` (Property-Zugriff), `ConstantExpression` (Konstanten) und `ParameterExpression`.

Wichtig für Provider-LINQ:

- **Compile vs. Translate:** `Compile()` macht aus einem Expression Tree einen Delegate und führt ihn In-Memory aus; ein Provider versucht dagegen, den Tree in eine Zielrepräsentation zu **übersetzen** (z. B. SQL oder eine eigene Query-Engine).
- **Übersetzbarkeitsgrenzen:** Nicht jeder .NET-/C#-Ausdruck ist sinnvoll übersetzbar (z. B. beliebige Methodenaufrufe, I/O, Closures). Deshalb ist eine **Validate**-Phase zentral.
- **Deferred Execution:** Viele Queries werden erst bei Enumeration/Materialisierung ausgeführt (z. B. `ToList()`), was für Debugging und Performance relevant ist.

```mermaid
flowchart LR
  A[LINQ Query in C#] --> B[Expression Tree]
  B --> C[Validate]
  C --> D[Rewrite / Optimize]
  D --> E[Translate]
  E --> F[Execute]
  F --> G[Materialize / Map Results]
```

### 2.4 Performance und Trade-offs

Deklarative Abfragen sind nicht „automatisch schneller“. Typische Trade-offs:

- **Overhead** durch Interpretation/Übersetzung (Provider) und ggf. Materialisierung.
- **Deferred Execution:** Viele LINQ-Abfragen werden erst bei Enumeration ausgeführt; das ist mächtig, kann aber zu unerwartetem Verhalten führen.
- **Nebenwirkungen:** Deklarative Regeln funktionieren am besten, wenn Datenquellen/Operationen möglichst *pure* sind.

Für Games ist besonders relevant, dass LINQ (v. a. LINQ-to-Objects) in Hot-Paths messbaren Overhead verursachen kann; als Faustregel eignet es sich eher für UI/Business-Logik/seltene Queries, aber nicht für `Update()`/FixedUpdate-Loops (vgl. [Zichao_Handout.md](Zichao_Handout.md), Abschnitt 5).


### 2.5 Verwandte/alternative Ansätze

Zur Einordnung (und für spätere tabellarische Gegenüberstellung) betrachten wir weitere deklarative bzw. regelbasierte Ansätze:

- **Rete-Algorithmus** (Kawakami & Yanagisawa, 2015): effiziente Auswertung von IF-THEN-Regeln durch Kompilierung von Bedingungen, Caching von Zwischenergebnissen und inkrementelle Updates.
- **TED & Bottom-Up** (Horswill & Hill, 2023): alternative, performante Strategien zur regel-/ereignisgetriebenen Auswertung.
- **SGL / State-Effect-Pattern** (Sowell et al., 2009):
  - **Query:** liest State-Variablen (read-only)
  - **Effect:** schreibt Effekte (write-only)
  - **Update:** Engine berechnet neuen Zustand
  - Kerngedanke: Entwickler schreiben lokal imperativ, die Engine orchestriert deklarativ.

**Vergleich**

| Ansatz | Was wird beschrieben? | Optimierbarkeit | Stärken im Hub-Kontext | Typische Risiken/Trade-offs |
|---|---|---|---|---|
| LINQ / Provider (`IQueryable<T>`) | Queries über Daten/State | hoch (Rewrite/Optimize möglich) | wiederverwendbare Regeln, Backend-Austauschbarkeit, C#-integriert | Übersetzbarkeitsgrenzen, Debugging, Provider-Komplexität |
| Rule Engine / Rete | IF-THEN Regeln / Pattern Matching | hoch (inkrementell, Caching) | effiziente Regel-Auswertung, viele Regeln | Modellierungsaufwand, Datenmodell/Working Memory |
| State-Effect-Pattern | getrennte Query/Effect-Phasen | mittel (Engine-Orchestrierung) | klare Trennung von Lesen/Schreiben, deterministischer Update-Loop | Architektur-Disziplin, Expressivität vs. Einfachheit |

### 2.6 PLINQ (Parallel LINQ) – Möglichkeiten und Grenzen

PLINQ ist eine parallele Ausführungsvariante des LINQ-Musters, die versucht, Abfragen über mehrere Threads zu verteilen.

- Funktioniert auf `IEnumerable<T>`-Datenquellen; startet i. d. R. bei Enumeration (deferred).
- Kann bei geeigneten Workloads (CPU-bound, ausreichend große Datenmengen, geringe Synchronisation) Performance bringen.
- Grenzen/typische Probleme:

  - **Externe Datenquellen/I/O** und `IQueryable<T>`-Provider sind oft nicht sinnvoll parallelisierbar.
  - **Nicht Thread-sicherer oder mutabler Shared State** verhindert korrekte Parallelisierung.
  - **Reihenfolgeabhängige Operatoren** (z. B. `First`) oder globale Koordination (z. B. `GroupBy`) können Parallelisierung erschweren oder unperformant machen.

**Beispiel: CPU-bound Normalisierung (LINQ vs. PLINQ)**

Wenn pro Eintrag eine teure, rein CPU-lastige Berechnung anfällt (und die Funktion thread-safe/pure ist), kann PLINQ gegenüber sequenziellem LINQ sinnvoll sein.

Sequenziell (LINQ):

```csharp
var expensiveComputation = scoreEntries
  .Select(s => new
  {
    s.PlayerName,
    ComputedValue = ExpensiveScoreNormalization(s.Points)
  })
  .ToList();
```

Parallel (PLINQ):

```csharp
var expensiveComputation = scoreEntries
  .AsParallel()
  .Select(s => new
  {
    s.PlayerName,
    ComputedValue = ExpensiveScoreNormalization(s.Points)
  })
  .ToList();
```

Hinweise:

- PLINQ ist v. a. bei **großen Collections** und **teuren, CPU-bound** Operationen hilfreich; bei kleinen Datenmengen dominiert oft der Parallelisierungs-Overhead.
- Die Ergebnisreihenfolge ist bei PLINQ nicht garantiert; falls relevant, explizit ordnen (z. B. `AsOrdered()` oder `OrderBy(...)`).
- `ExpensiveScoreNormalization` sollte keine geteilten, mutablen Zustände nutzen (Thread-Safety).

## 3. Konzeption

Die Konzeption wird im Wesentlichen durch folgende Diagramme getragen (jeweils mit grober Einordnung):

- [Diagrams/Classes.md](Diagrams/Classes.md): Typen/Verantwortlichkeiten und Abhängigkeiten (Contracts in `Shared.Core` vs. Implementierung in `GAE.Query`).
- [Diagrams/Activity.md](Diagrams/Activity.md): rekursive AST-Traversierung zur Operator-Analyse/Validierung (entspricht der „Validate/Analyze“-Phase).
- [Diagrams/Rules.md](Diagrams/Rules.md): Lebenszyklus einer Regel (Discovery → Analyse → Caching/Compile → Execute) + Plugin-Kommunikation (Dashboard ↔ Engine ↔ Game-Module).

## 4. Umsetzung

### 4.1 Demos & API-Lieferumfang (Demo)

**RuleEngineDemo** ([../Demo.Console/Query/RuleEngineDemo.cs](../Demo.Console/Query/RuleEngineDemo.cs))

- Definiert Spiel-spezifische Regel-Provider (`IRuleProvider<Highscore>`) mit mehreren Achievements (`IRule<Highscore>`), deren `Criteria` als `Expression<Func<Highscore,bool>>` formuliert ist.
- Discovering/Plug-in-Prinzip wird demonstriert, indem Regel-Provider via Reflection aus der Assembly gefunden und instanziiert werden.
- Die Engine (`RuleEngine`) evaluiert Regeln gegen Mockdaten (Highscores) und zeigt dabei zentrale Mechanismen:
  - **Analyse/Validierung** des Expression Trees (z. B. erlaubte Operatoren)
  - **Kompilierung** einer Regel zu einem Delegate
  - **Caching** kompilierter Regeln (Key: `rule.Criteria.ToString()`)
  - Fehlerfall: `InvalidOperationException` bei nicht erlaubten Operatoren

**FluentApiDemo** ([../Demo.Console/Query/FluentApiDemo.cs](../Demo.Console/Query/FluentApiDemo.cs))

- Zeigt eine konsumierbare „Query-API“ über Extension Methods (z. B. globale Top-N, Durchschnittswerte pro Spiel) auf Basis eines `IHighscoreProvider`.
- Fokus liegt auf „klassischem“ LINQ-to-Objects/Aggregationen (Dashboard-nahe Auswertungen), nicht auf Expression-Tree-Analyse.

**Was wird dabei in Shared.Core ausgeliefert?**

- `IRule<T>` ([../Shared.Core/IRule.cs](../Shared.Core/IRule.cs)): Regelbeschreibung + Expression-Tree-Kriterium.
- `IRuleProvider<T>` ([../Shared.Core/IRuleProvider.cs](../Shared.Core/IRuleProvider.cs)): Plugin-Schnittstelle, über die Spiele Regeln bereitstellen.
- `IQueryEngine` ([../Shared.Core/IQueryEngine.cs](../Shared.Core/IQueryEngine.cs)): minimaler Contract, um Regeln auf `IQueryable<T>` anzuwenden.
- `IHighscoreProvider` + `LocalHighscoreService` ([../Shared.Core/IHighscoreProvider.cs](../Shared.Core/IHighscoreProvider.cs), [../Shared.Core/LocalHighscoreService.cs](../Shared.Core/LocalHighscoreService.cs)): Datenzugriff abstrahiert + In-Memory-Default für Demos.

## 5. Evaluation und Ausblick

t.b.d.

---
**Querverweise**

- Vorarbeit/Handout (Zichao): [Zichao_Handout.md](Zichao_Handout.md)

**Zusätzliche Quellen**

