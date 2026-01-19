### Programmieren mit C# (DSPC016)

---

# Vorlesung: Ziele und Semesterplan

## 🎯 Kursziel & Philosophie

Im Fokus stehen nicht Syntax-Details, sondern die **Analyse von Paradigmen, Entwurfsmustern und
Architektur-Entscheidungen**. Wir verfolgen einen "Code-as-Data"-Ansatz und nutzen wissenschaftliche Publikationen als
Grundlage für technologische Evaluationen.

- **Wissenschaftlich getrieben:** Jede Design-Entscheidung muss methodisch begründet sein.
- **Hands-on Forschung:** Entwicklung von Prototypen zur Validierung theoretischer Konzepte.
- **Peer-Review:** Zusammenarbeit via GitHub Pull Requests und Code-Reviews.

---

## 📅 Semesterplan (15 Einheiten à 3 UE)

Der folgende Plan ist eine vorläufige Übersicht der Themen und Schwerpunkte pro Einheit. Änderungen sind
möglich, um auf aktuelle Entwicklungen und Interessen der Studierenden einzugehen.
(work in progress)

### Einfache Übersicht

| **Nr.** | **Thema**                | **Fokus**                                    | **Literatur (Kern)** |
| ------- | ------------------------ | -------------------------------------------- | -------------------- |
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

### Detaillierter Plan

| **Einheit** | **Thema**                             | **Fokus & Methodik**                                                                                  | **Literatur-Bezug**                |
| ----------- | ------------------------------------- | ----------------------------------------------------------------------------------------------------- | ---------------------------------- |
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

## 📖 Literatur & Quellen

### Kernliteratur

- **Kotz, J., Wenz, C. (2024):** *C# und .NET 8*, Hanser Verlag.
- **IU Lernskript:** *Objektorientierte Programmierung mit C#* (DLBAVROOPC01).

### Wissenschaftliche Leitartikel (Einstieg)

- Meijer et al. (2006): *LINQ: Reconciling Object, Relations and XML*.
- Bierman et al. (2012): *Pause 'n' Play: Formalizing Asynchronous Programming in C#*.
- Evans, E. (2003): *Domain-Driven Design* (Reference Material).

---

### Tools zur Modellierung

- Empfohlen wird ein Markdown-Editor mit Diagramm-Support, z. B. **Obsidian** (https://obsidian.md/) mit dem **Mermaid**-Plugin.
- **draw.io:** (https://app.diagrams.net/)
- **PlantUML:** https://plantuml.com/de/
- **graphviz:** (https://pypi.org/project/graphviz/) (GUI: https://vincenthee.github.io/DotEditor/ , Online: https://dreampuf.github.io/GraphvizOnline/)
- **Excalidraw:** https://excalidraw.com/
- **StarUML:** http://staruml.io/
- **Lucidchart:** https://www.lucidchart.com/pages/examples/uml-diagram-tool
- **Visual Paradigm:** https://www.visual-paradigm.com/
- **Microsoft Visio:** https://www.microsoft.com/de-de/microsoft-365/vis
- etc.

Die Dokumentation in Kommentaren und Daten sollte in **Markdown** verfasst werden.
Englisch sollte dabei durchgängig verwendet werden.

### Weitere Dokumentationen & Ressourcen

- **Markdown Guide:** https://www.markdownguide.org/
- **GitHub Docs:** https://docs.github.com/en
- **Visual Studio Documentation:** https://docs.microsoft.com/en-us/visualstudio/
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
