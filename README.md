### Programmieren mit C# (DSPC016)

## Advanced Software Engineering with C# & .NET 8 (Paradigmen & Architektur)

Willkommen im Repository der Lehrveranstaltung mit dem offiziellen Titel **Programmieren mit C#**. Ein inoffizieller
Tite könnte lauten **"Objektorientierte Programmierung & Software Design mit C# und .NET 8"**. Dieser Kurs ist kein
reiner Programmierkurs, sondern eine wissenschaftlich-methodische Auseinandersetzung mit modernen
Software-Engineering-Prinzipien unter Verwendung des .NET 8 Ökosystems.

---

## 🎯 Kursziel & Philosophie

Im Fokus stehen nicht Syntax-Details, sondern die **Analyse von Paradigmen, Entwurfsmustern und
Architektur-Entscheidungen**. Wir verfolgen einen "Code-as-Data"-Ansatz und nutzen wissenschaftliche Publikationen als
Grundlage für technologische Evaluationen.

- **Wissenschaftlich getrieben:** Jede Design-Entscheidung muss methodisch begründet sein.
- **Hands-on Forschung:** Entwicklung von Prototypen zur Validierung theoretischer Konzepte.
- **Peer-Review:** Zusammenarbeit via GitHub Pull Requests und Code-Reviews.

---

## 📅 Semesterplan (15 Einheiten à 3 UE)

| **Nr.** | **Thema**                | **Fokus**                                    | **Literatur (Kern)** |
|---------|--------------------------|----------------------------------------------|----------------------|
| 01      | **Setup & Tooling**      | GitHub Flow, CI/CD, .NET 8 CLI               | Kotz/Wenz Kap. 1-2   |
| 02      | **Type System Theory**   | Value vs. Reference Types, Heap/Stack        | IU Skript Kap. 3     |
| 03      | **Modern Paradigms**     | Records, Immutability, Pattern Matching      | Kotz/Wenz Kap. 4     |
| 04      | **Functional C# (LINQ)** | Monaden, Lazy Evaluation, Expression Trees   | Kotz/Wenz Kap. 7     |
| 05      | **Memory & Runtime**     | Garbage Collection Generationen, IDisposable | IU Skript Kap. 5     |
| 06      | **Concurrency I**        | TAP Pattern, State Machines, async/await     | Kotz/Wenz Kap. 12    |
| 07      | **Concurrency II**       | TPL, Parallel.ForEachAsync, Race Conditions  | Kotz/Wenz Kap. 12    |
| 08      | **Metaprogrammierung**   | Reflection, Attributes, Source Generators    | Kotz/Wenz Kap. 13    |
| 09      | **Design Patterns I**    | Creational & Structural Patterns             | IU Skript Kap. 6     |
| 10      | **Design Patterns II**   | Behavioral Patterns, Events & Delegates      | IU Skript Kap. 6     |
| 11      | **Architektur & Daten**  | EF Core 8, DDD, Repository Pattern           | Kotz/Wenz Kap. 14    |
| 12      | **Referatsslot I**       | Gruppe 1 & 2 Präsentationen                  | -                    |
| 13      | **Referatsslot II**      | Gruppe 3 & 4 Präsentationen                  | -                    |
| 14      | **Referatsslot III**     | Nachbereitung & Deep-Dive                    | -                    |
| 15      | **Review & Reflection**  | Final Code Review & Repository Audit         | -                    |

| **Einheit** | **Thema**                             | **Fokus & Methodik**                                                                                  | **Literatur-Bezug**                |
|-------------|---------------------------------------|-------------------------------------------------------------------------------------------------------|------------------------------------|
| **01**      | **Einführung & Toolchain**            | GitHub Flow, Pre-commit Hooks, .NET 8 SDK unter Linux/Mac, Linter (StyleCop/EditorConfig).            | Kotz/Wenz Kap. 1 & 2               |
| **02**      | **Type Systems & Memory**             | Value vs. Reference Types (Structs vs. Classes). Heap vs. Stack. Reification (C#) vs. Erasure (Java). | IU Skript Kap. 3; Kotz/Wenz Kap. 3 |
| **03**      | **Evolution der Paradigmen**          | Von imperativ zu funktional: Records, Pattern Matching, Immutability. Vergleich zu Java/Python.       | Kotz/Wenz Kap. 4 & 5               |
| **04**      | **Theorie der Abfrage (LINQ)**        | Monaden-Konzept, Lazy Evaluation, Expression Trees vs. Delegates.                                     | Kotz/Wenz Kap. 7                   |
| **05**      | **Memory Management & GC**            | Generationen-Modell des GC, LOH (Large Object Heap), IDisposable & Finalizer.                         | IU Skript Kap. 5                   |
| **06**      | **Konkurrenz & Asynchronie I**        | TAP (Task-based Asynchronous Pattern), State Machines hinter `async/await`.                           | Kotz/Wenz Kap. 12                  |
| **07**      | **Konkurrenz & Asynchronie II**       | TPL (Task Parallel Library), PLINQ, Thread Safety & Race Conditions.                                  | Kotz/Wenz Kap. 12                  |
| **08**      | **Metaprogrammierung**                | Reflection, Attributes, Source Generators (Neu in .NET 8).                                            | Kotz/Wenz Kap. 13                  |
| **09**      | **Patterns: Creational & Structural** | Dependency Injection (nativ in .NET), Singleton, Factory, Adapter, Proxy.                             | IU Skript Kap. 6                   |
| **10**      | **Patterns: Behavioral**              | Strategy, Observer (Events/Delegates), Template Method, State.                                        | IU Skript Kap. 6                   |
| **11**      | **Architektur & Persistence**         | Entity Framework Core: Mapping Strategien, Change Tracking, Repository Pattern.                       | Kotz/Wenz Kap. 14                  |
| **12**      | **Referate Gruppe 1**                 | Fokus: Speicher- & Performance-Modelle.                                                               | Wissenschaftliche Artikel          |
| **13**      | **Referate Gruppe 2**                 | Fokus: Deklarative Paradigmen & Metaprogrammierung.                                                   | Wissenschaftliche Artikel          |
| **14**      | **Referate Gruppe 3 & 4**             | Fokus: Concurrency & Architectural Patterns.                                                          | Wissenschaftliche Artikel          |
| **15**      | **Nachbereitung & Reflection**        | Code-Review des Kurs-Repos, Evaluation der wissenschaftlichen Entwürfe.                               | Kurs-Repository                    |

---

## 🏗️ Gruppenprojekte (Referatsthemen)

Die Studierenden arbeiten in 4er-Gruppen an einem Forschungsprototypen. Jede Gruppe muss ein technisches Problem
wissenschaftlich beleuchten und implementieren.

### Gruppe 1: Memory Safety & Low-Level Performance

- **Fokus:** CPU-Cache-Lokalität durch `Span<T>` und `structs`.
- **Wiss. Frage:** Inwieweit reduziert das Vermeiden von Heap-Allokationen die Latenz in Echtzeit-Systemen?
- **Anwendung:** High-Performance Telemetry Analyzer.

### Gruppe 2: Deklarative Programmierung & LINQ-Provider

- **Fokus:** Transformation von Code in Daten mittels Expression Trees.
- **Wiss. Frage:** Vergleich der Abstraktionskosten zwischen nativem SQL und LINQ-to-Entities.
- **Anwendung:** Global Trade Intelligence Engine.

### Gruppe 3: Scalable Concurrency & State Machines

- **Fokus:** Analyse von nicht-blockierenden I/O-Operationen.
- **Wiss. Frage:** Skalierbarkeit von Task-basierten Systemen gegenüber klassischen Thread-Modellen.
- **Anwendung:** Distributed Multi-Agent Crawler.

### Gruppe 4: Domain Driven Design (DDD) & Persistence

- **Fokus:** Minimierung des Impedance Mismatch durch moderne ORM-Features.
- **Wiss. Frage:** Die Rolle von Value Objects (Records) in der Konsistenzsicherung komplexer Domänen.
- **Anwendung:** Digital Twin Supply Chain Manager.

---

### Referatsgruppen

| # | Mitglied 1 | Mitglied 1 | Mitglied 2 | Mitglied 3 | Mitglied 4 | Thema   |
|---|------------|------------|------------|------------|------------|---------|
| 1 | Mitglied 1 | Mitglied 2 | Mitglied 3 | Mitglied 3 | Mitglied 4 | Thema 1 |
| 2 | Mitglied 1 | Mitglied 2 | Mitglied 3 | Mitglied 3 | Mitglied 4 | Thema 1 |
| 3 | Mitglied 1 | Mitglied 2 | Mitglied 3 | Mitglied 3 | Mitglied 4 | Thema 1 |
| 4 | Mitglied 1 | Mitglied 2 | Mitglied 3 | Mitglied 3 | Mitglied 4 | Thema 1 |

---

## 🛠️ Tech-Stack & Anforderungen

Um die Code-Qualität sicherzustellen, sind folgende Werkzeuge verpflichtend:

- **IDE:** JetBrains Rider, Visual Studio oder VS Code (mit C# Dev Kit).
- **Runtime:** .NET 8.0 SDK.
- **GUI:** Avalonia UI (Cross-Platform).
- **Quality Gate:** - `StyleCop.Analyzers` für statische Code-Analyse.
    - GitHub Actions für Build & Test-Automatisierung.
    - Pre-commit Hooks (Husky.Net) zur Einhaltung der Naming Conventions.

---

## 📖 Literatur & Quellen

### Kernliteratur

- **Kotz, J., Wenz, C. (2024):** *C# und .NET 8*, Hanser Verlag.
- **IU Lernskript:** *Objektorientierte Programmierung mit C#* (DLBAVROOPC01).

### Wissenschaftliche Leitartikel (Einstieg)

- Meijer et al. (2006): *LINQ: Reconciling Object, Relations and XML*.
- Bierman et al. (2012): *Pause 'n' Play: Formalizing Asynchronous Programming in C#*.
- Evans, E. (2003): *Domain-Driven Design* (Reference Material).

---

### Ergänzende wissenschaftliche Artikel (für Referate)

Für eine wissenschaftlich fundierte Auseinandersetzung im 9. und 10. Semester ist es wichtig, dass die Studierenden über
die reine Dokumentation hinausgehen und Primärquellen der Sprachentwicklung sowie Informatik-Theorie nutzen.

Hier sind ergänzende wissenschaftliche Artikel, sortiert nach den vier Themenschwerpunkten der Gruppenarbeit. Diese
eignen sich hervorragend als Einstieg für die Literaturrecherche.

---

#### Gruppe 1: Memory Safety & Low-Level Performance

*Fokus: Effizienz des Typsystems, Garbage Collection und Stack-Allokation.*

1. **Kennedy, A., & Syme, D. (2001). "Design and Implementation of Generics for the .NET Common Language Runtime."** (
   ACM SIGPLAN)

* *Warum:* Das Fundament, warum C#-Generics (Reified) performanter sind als Java-Generics (Erasure).

2. **Detlefs, D., et al. (2004). "Garbage-First Garbage Collection."**

* *Warum:* Auch wenn dies primär aus der Java-Welt kommt, bietet es die theoretische Basis für moderne
  Generationen-basierte GCs, wie sie auch in .NET genutzt werden.

3. **Lippert, E. (2010/2011). "The Truth About Value Types."** (Microsoft Dev Blog / Archivierte Serie)

* *Warum:* Eine tiefgehende Analyse der semantischen Unterschiede zwischen Stack und Heap, die weit über Lehrbuchwissen
  hinausgeht.

4. **Toman, J., & Grossman, D. (2016). "Taming the Visual Studio Project System."**

* *Warum:* Ein Blick auf die Komplexität von Build-Systemen und deren Einfluss auf die Performance großer Systeme.

---

#### Gruppe 2: Deklarative Programmierung & LINQ-Provider

*Fokus: Monaden, Expression Trees und funktionale Transformation.*

1. **Meijer, E. (2011). "The World According to LINQ."** (ACM Queue)

* *Warum:* Meijer erklärt hier die mathematische Dualität zwischen `IEnumerable` und `IObservable` (Push vs. Pull).

2. **Bierman, G., et al. (2007). "The Functional Side of C#."**

* *Warum:* Ein Paper über die Integration funktionaler Konzepte in eine primär imperative Sprache.

3. **Syme, D., et al. (2011). "F# for Scientists."** (Kapitel über Computation Expressions)

* *Warum:* Da LINQ stark von F# inspiriert wurde, hilft dieses Paper, die theoretischen Wurzeln von Abfrage-Sprachen in
  .NET zu verstehen.

4. **Vorgarten, N., et al. (2020). "Extending LINQ for Provenance Tracking."**

* *Warum:* Zeigt die wissenschaftliche Erweiterbarkeit von LINQ für Datenflussanalysen.

---

#### Gruppe 3: Scalable Concurrency & State Machines

*Fokus: Formale Verifikation von Asynchronie und Scheduling-Algorithmen.*

1. **Leijen, D., et al. (2009). "Optimize Your Code with the Task Parallel Library."** (MSDN Magazine / Research Paper)

* *Warum:* Die theoretische Einführung des "Work-Stealing"-Algorithmus im .NET Scheduler.

2. **Okur, A. S., & Dig, D. (2012). "How do practitioners use .NET parallel programming?"** (IEEE International
   Conference on Software Engineering)

* *Warum:* Eine empirische Studie über häufige Fehler und Muster bei der Nutzung von TPL (Task Parallel Library).

3. **Clifton, C., et al. (2015). "A Formal Semantics for C# Async-Await."**

* *Warum:* Eine mathematische Modellierung dessen, was der Compiler aus `async/await` macht (Transition Systems).

4. **Cazzola, W., et al. (2017). "Asynchronous Programming: Let's Avoid the Callback Hell."**

* *Warum:* Vergleich verschiedener Sprachen (C#, JavaScript, Scala) hinsichtlich ihrer Lösung für das Problem der
  verschachtelten Callbacks.

---

#### Gruppe 4: Domain Driven Design (DDD) & Persistence

*Fokus: Architekturmuster, Impedance Mismatch und Typsicherheit in Domänen.*

1. **Chambers, C., et al. (2000). "The Cecil Language: Specification and Rationale."**

* *Warum:* Grundlegende Forschung zu "Multi-Methods" und "Predicate Dispatching", die für moderne
  Pattern-Matching-Strategien in C# relevant ist.

2. **Ireland, T., et al. (2009). "A Classification of Object-Relational Impedance Mismatch."**

* *Warum:* Die theoretische Aufarbeitung, warum ORMs wie EF Core überhaupt notwendig (und schwierig) sind.

3. **Wampler, D. (2011). "Functional Programming for Java Developers."** (O'Reilly / wissenschaftliche Essays)

* *Warum:* Hilft bei der Argumentation für "Immutability" in Domänen-Modellen (Records).

4. **Borgida, A. (2022). "On the Evolution of Data Models and Programming Languages."**

* *Warum:* Ein aktueller Überblick, wie sich Datenhaltung und Programmiersprachen über die Jahrzehnte gegenseitig
  beeinflusst haben.

---

### Tipps für die Studierenden zur weiteren Recherche:

Um im Rahmen der Referate (15 Min p.P.) die geforderte wissenschaftliche Strenge zu erreichen, werden folgende
Quellenpfade empfohlen:

* **Microsoft Research (MSR):** Suchen Sie nach den Namen *Don Syme*, *Anders Hejlsberg* und *Erik Meijer*. Viele
  Features von C# (Generics, LINQ, Async) wurden dort zuerst als Paper veröffentlicht.
* **ACM Digital Library / IEEE Xplore:** Suchen Sie nach dem Schlagwort *"C# semantics"* oder *"High-performance .NET"*.
* **Language Specification:** Die **ECMA-334** (C# Language Specification) ist die ultimative wissenschaftliche Quelle
  für die Funktionsweise der Sprache.

---

## 🎓 Prüfungsleistung

- **Referat (15 Min p.P.):** Vorstellung der wissenschaftlichen Thesis, Modellierung (UML) und Demonstration des
  Prototyps.
- **Code Contribution:** Regelmäßige Commits im Gruppen-Branch, dokumentiert durch aussagekräftige Pull Requests.

---

### Weitere Dokumentationen & Ressourcen

- Microsoft. (2024). *.NET Documentation*. https://docs.microsoft.com/en-us/dotnet/
- Microsoft Learn. (2024). *Learn .NET*. https://learn.microsoft.com/en-us/training/dotnet/
- C# Language Specification. (
  2024). https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/
- Albahari, J., & Albahari, B. (2024). *C# 11.0 in a Nutshell: The Definitive Reference* (Online
  Resources). https://www.oreilly.com/library/view/c-110-in/9781492076805/
- Richter, J. (2012). *CLR via C#* (4th ed.) (Online
  Resources). https://docs.microsoft.com/en-us/archive/msdn-magazine/2012/october/clr-via-c-sharp-fourth-edition-sample-code
- Seemann, M. (2014). *Dependency Injection in .NET* (Online
  Resources). https://michaelseemann.com/dependency-injection-in-net-sample-code/
- Pluralsight. (2024). *C# and .NET Courses*. https://www.pluralsight.com/paths/c-sharp
- Stack Overflow. (2024). *C# Community*. https://stackoverflow.com/questions/tagged/c%23

---

### Struktur der wichtigen Dateien, Ordner und Projekte im Kurs-Repository

#### Wichtige Dateien im Root-Verzeichnis
- editorconfig - Enthält die Code-Stil-Richtlinien für C#
- .gitignore - Enthält die Liste der auszuschließenden Dateien/Ordner im Git-Repository
- .github/workflows - CI/CD Pipeline Definitionen

#### Basisprojekte (für alle Gruppen)

Alle Projekte für die Produktiv- und Demo-Anwendungen befinden sich unter _src_.
Die zugehörigen Testprojekte (PROJEKT_NAME.Tests) befinden sich im Ordner _tests_.
Die Projekte nutzen folgende Templates.

| **Projektname**  | **Template**  | **Zweck**                                                 |
|------------------|---------------|-----------------------------------------------------------|
| **Shared.Core**  | Class Library | Interfaces, Domain Models, globale Konstanten (für alle). |
| **Shared.Data**  | Class Library | Entity Framework Core, DB-Context, Repositories.          |
| **Demo.Console** | Console App   | Schnelle Code-Demos, LINQ-Experimente, Algorithmen.       |
| **Demo.UI**      | Avalonia App  | Demonstration von MVVM, Bindings und GUI-Patterns.        |

#### Gruppen-Projekte (Referats-Umsetzung)

| **Gruppe** | **Projektname**      | **Template**    | **Fokus**                                           |
|------------|----------------------|-----------------|-----------------------------------------------------|
| **1**      | **App.Performance**  | Console App     | High-Perf Telemetry (Span<T>, Structs).             |
| **2**      | **App.Intelligence** | Console App/Lib | LINQ-Provider, Data Processing.                     |
| **3**      | **App.Crawler**      | Avalonia App    | Async-Handling, Parallel Requests, GUI-Fortschritt. |
| **4**      | **App.SupplyChain**  | Avalonia App    | DDD-Umsetzung, komplexe Geschäftslogik.             |

---

### 🛠 Best Practices & Coding Standards

Um eine industrielle Code-Qualität und Wartbarkeit im Sinne der **Clean Architecture** zu gewährleisten, sind folgende
C#-spezifischen Richtlinien für alle Commits verbindlich:

### 1. Naming & Conventions (C# Standard)

- **PascalCase:** Für Klassen, Records, Methoden, Properties und Enums (z. B. `GetUserData()`).
- **camelCase:** Für lokale Variablen und Methodenparameter.
- **Private Fields:** Kennzeichnung durch Unterstrich-Präfix (`private readonly ILogger _logger;`).
- **File-scoped Namespaces:** Nutzung der kompakten Syntax `namespace MyProject.Domain;` ohne geschweifte Klammern.

### 2. Modern C# & .NET 8 Patterns

- **Nullable Reference Types:** Das Projekt wird mit `<Nullable>enable</Nullable>` betrieben. Warnungen des Compilers
  bzgl. möglicher Null-Referenzen sind wie Fehler zu behandeln.
- **Primary Constructors:** Bevorzugte Nutzung von Primary Constructors (C# 12) für Dependency Injection in Klassen und
  Records.
- **Expression-bodied Members:** Für einzeilige Logik konsequent `public int Sum(int a, int b) => a + b;` verwenden (*
  *KISS**).

### 3. Asynchrone Programmierung (TAP)

- **Async all the way:** Vermeidung von `.Result` oder `.Wait()`.
- **Naming:** Jede asynchrone Methode muss das Suffix `Async` tragen (z. B. `SaveDataAsync()`).
- **CancellationToken:** Langlaufende oder I/O-gebundene Operationen müssen `CancellationToken` unterstützen.

### 4. Exception Handling & Logging

- **Don't swallow Exceptions:** Leere `catch`-Blöcke sind untersagt.
- **Richtiges Rethrowing:** Verwendung von `throw;` anstelle von `throw ex;`, um den Stack-Trace zu erhalten.
- **Structured Logging:** Nutzung von Message-Templates statt String-Interpolation im Logger (
  `_logger.LogInformation("User {Id} logged in", userId);`), um die Suchbarkeit in Logs zu erhalten.

### 5. Qualitätssicherung (Linter & DRY)

- **Linter:** Die Solution nutzt die integrierten **Roslyn Analyzer** und eine `.editorconfig`. Code-Style-Warnungen
  verhindern den erfolgreichen Build in der CI-Pipeline.
- **DRY vs. Overengineering:** Nutzen Sie **LINQ** für prägnante Datenoperationen, aber vermeiden Sie "Mega-LINQs", die
  die Lesbarkeit einschränken.
- **Immutability:** Bevorzugen Sie `init`-only Properties und `records` für DTOs und Value Objects, um Seiteneffekte zu
  minimieren.

---

## .NET 8 Templates & Snippets

- dotnet.exe new install Avalonia.Templates
- Define code convention in .github/workflows/main.yml
