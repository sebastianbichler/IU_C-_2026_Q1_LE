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

Die Studierenden arbeiten in 4er-Gruppen an einem Forschungsprototypen. Jede Gruppe muss ein technisches Problem wissenschaftlich beleuchten und implementieren.

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

## 🎓 Prüfungsleistung

- **Referat (15 Min p.P.):** Vorstellung der wissenschaftlichen Thesis, Modellierung (UML) und Demonstration des
  Prototyps.
- **Code Contribution:** Regelmäßige Commits im Gruppen-Branch, dokumentiert durch aussagekräftige Pull Requests.

---

### Wissenschaftliche Artikel (Beispiele)

- "The Art of Readable Code" von Dustin Boswell und Trevor Foucher
- "Effective C#: 50 Specific Ways to Improve Your C#" von Bill Wagner
- "Asynchronous Programming with async and await" von Stephen Cleary
- "Dependency Injection Principles, Practices, and Patterns" von Mark Seemann und Steven van Deursen
- "Domain-Driven Design: Tackling Complexity in the Heart of Software" von Eric Evans
- "Building Microservices: Designing Fine-Grained Systems" von Sam Newman
- "The Future Computed: AI and its Role in Society" von Satya Nadella
- "Exploring .NET 8: New Features and Improvements" von Microsoft Docs Team
- "Understanding C# 11: Latest Language Features" von John Albahari und Ben Albahari
- "CLR via C#: Advanced Topics in .NET Programming" von Jeffrey Richter
- "C# Language Specification: In-Depth Look at C# 11" von Microsoft Language Team
- "Pro .NET Memory Management: For Better Performance and Scalability" von Konrad Henning
- "Programming C# 7: Build Cloud, Web, and Desktop Applications" von Joseph Liberty
- "Programming C# 9: Build Cloud, Web, and Desktop Applications" von Joseph Liberty
- "LINQ Pocket Reference: Language Integrated Query in C# and VB.NET" von Joseph Albahari und Ben Albahari
- "Dependency Injection in .

- "Domain-Driven Design: Tackling Complexity in the Heart of Software" von Eric Evans
- "Building Microservices: Designing Fine-Grained Systems" von Sam Newman
- Fowler, M. (2003). *Patterns of Enterprise Application Architecture*. Addison-Wesley.
- Gamma, E., Helm, R., Johnson, R., & Vlissides, J. (1994). *Design Patterns: Elements of Reusable Object-Oriented
  Software*. Addison-Wesley.
- Albahari, J., & Albahari, B. (2024). *C# 11.0 in a Nutshell: The Definitive Reference*. O'Reilly Media.
- Skeet, J. (2019). *C# in Depth* (4th ed.). Manning Publications.
- Richter, J. (2012). *CLR via C#* (4th ed.). Microsoft Press.
- Torgersen, M., Golde, P., & Wiltamuth, S. (2023). *C# Language Specification* (11th ed.). Microsoft Press.
- Henning, M. (2020). *Pro .NET Memory Management: For Better Performance and Scalability*. Apress.
- Liberty, J. (2017). *Programming C# 7: Build Cloud, Web, and Desktop Applications*. O'Reilly Media.
- Liberty, J. (2021). *Programming C# 9: Build Cloud, Web, and Desktop Applications*. O'Reilly Media.
- Albahari, J., & Albahari, B. (2020). *LINQ Pocket Reference: Language Integrated Query in C# and VB.NET*. O'Reilly
  Media.
- Seemann, M. (2014). *Dependency Injection in .NET*. Manning Publications.
- Evans, E. (2004). *Domain-Driven Design: Tackling Complexity in the Heart of Software*. Addison-Wesley.
- Newman, S. (2015). *Building Microservices: Designing Fine-Grained Systems*. O'Reilly Media.
- Nadella, S. (2023). *The Future Computed: AI and its Role in Society*. Microsoft Press.

---

### Weitere Ressourcen
- Microsoft. (2024). *.NET Documentation*. https://docs.microsoft.com/en-us/dotnet/
- C# Language Specification. (
  2024). https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/
- Albahari, J., & Albahari, B. (2024). *C# 11.0 in a Nutshell: The Definitive Reference* (Online
  Resources). https://www.oreilly.com/library/view/c-110-in/9781492076805/
- Richter, J. (2012). *CLR via C#* (4th ed.) (Online
  Resources). https://docs.microsoft.com/en-us/archive/msdn-magazine/2012/october/clr-via-c-sharp-fourth-edition-sample-code
- Seemann, M. (2014). *Dependency Injection in .NET* (Online
  Resources). https://michaelseemann.com/dependency-injection-in-net-sample-code/
- Microsoft Learn. (2024). *Learn .NET*. https://learn.microsoft.com/en-us/training/dotnet/
- Pluralsight. (2024). *C# and .NET Courses*. https://www.pluralsight.com/paths/c-sharp
- Stack Overflow. (2024). *C# Community*. https://stackoverflow.com/questions/tagged/c%23

---

### Ordnerstruktur im Kurs-Repository


---

### Best Practices

- Schreibe sauberen, gut dokumentierten Code.
- Nutze Versionskontrolle (Git) konsequent.
- Führe regelmäßige Code-Reviews durch.
- Teste deinen Code umfassend (Unit Tests, Integration Tests).
- Halte dich an die Prinzipien des Clean Code.
- Verwende Design Patterns angemessen.
- Optimiere für Performance und Speicherverbrauch.
- Bleibe auf dem neuesten Stand der C#- und .NET-Entwicklungen.
- Teile dein Wissen und arbeite kollaborativ im Team.
- Reflektiere regelmäßig über deine Lernfortschritte und passe deine Lernstrategie an.
- Nutze die bereitgestellten Ressourcen und Literatur effektiv.
- Setze das Gelernte in praktischen Projekten um, um deine Fähigkeiten zu festigen.
- Sei offen für Feedback und lerne aus Fehlern.
- Engagiere dich in der Entwickler-Community, um von anderen zu lernen und dein Netzwerk zu erweitern.
- Plane deine Lernzeit effektiv und setze realistische Ziele für deinen Fortschritt im Kurs.
- Dokumentiere deine Projekte und Lernfortschritte sorgfältig, um später darauf zurückgreifen zu können.
- Nutze moderne Entwicklungswerkzeuge und -praktiken, um deine Produktivität zu steigern.
- Bleibe neugierig und experimentiere mit neuen Technologien und Ansätzen im C#-Ökosystem.
- Setze dir persönliche Herausforderungen, um deine Fähigkeiten kontinuierlich zu verbessern.
- Feiere deine Erfolge und Meilensteine im Lernprozess, um motiviert zu bleiben.
- Halte dich an ethische Standards in der Softwareentwicklung und respektiere die Privatsphäre und Sicherheit der
  Nutzer.
- Nutze automatisierte Tools zur Code-Analyse und -Optimierung, um die Qualität deines Codes zu gewährleisten.
- Entwickle ein tiefes Verständnis für die zugrunde liegenden Konzepte und Prinzipien von C# und .NET, um fundierte
  Entscheidungen bei der Softwareentwicklung treffen zu können.
- Bleibe flexibel und passe deine Lernstrategie an neue Herausforderungen und Technologien an, um stets auf dem neuesten
  Stand zu bleiben.
- Nutze Pair Programming und kollaborative Lernmethoden, um von anderen Entwicklern zu lernen und deine Fähigkeiten zu
  erweitern.
- Setze dir klare Lernziele für jede Kurseinheit und überprüfe regelmäßig deinen Fortschritt, um sicherzustellen, dass
  du auf dem richtigen Weg bist.
- Entwickle ein Portfolio von Projekten, die deine Fähigkeiten und dein Wissen in C# und .NET demonstrieren, um
  potenziellen Arbeitgebern oder Kunden zu zeigen, was du kannst.
- Bleibe geduldig und beharrlich, da das Erlernen fortgeschrittener Konzepte Zeit und Übung erfordert.
- Nutze Online-Ressourcen, Tutorials und Foren, um zusätzliche Unterstützung und Inspiration zu finden.
- Entwickle ein Verständnis für die Geschäftsanforderungen und -prozesse, um Softwarelösungen zu erstellen, die echten
  Mehrwert bieten.
- Setze auf kontinuierliches Lernen und Weiterbildung, um deine Fähigkeiten im Bereich C# und .NET ständig zu verbessern
  und auf dem neuesten Stand zu halten.
- Nutze Feedback von Mentoren, Kollegen und der Entwickler-Community, um deine Fähigkeiten zu verbessern und neue
  Perspektiven zu gewinnen.
- Entwickle ein tiefes Verständnis für die Prinzipien der Softwarearchitektur und -design, um robuste und skalierbare
  Anwendungen zu erstellen.
- Setze auf Test-Driven Development (TDD), um die Qualität und Zuverlässigkeit deines Codes zu gewährleisten.
- Nutze DevOps-Praktiken, um den Entwicklungsprozess zu optimieren und die Zusammenarbeit im Team zu verbessern.
- Entwickle ein Verständnis für Cloud-Technologien und -Dienste, um moderne Anwendungen zu erstellen, die in der Cloud
  ausgeführt werden können.
- Bleibe offen für neue Ideen und Ansätze in der Softwareentwicklung, um innovative Lösungen zu entwickeln.
- Setze auf kontinuierliche Integration und kontinuierliche Bereitstellung (CI/CD), um den Entwicklungsprozess zu
  automatisieren und die Qualität der Software zu verbessern.
- Entwickle ein Verständnis für Sicherheitspraktiken in der Softwareentwicklung, um sichere Anwendungen zu erstellen.
- Nutze Code-Refactoring, um die Wartbarkeit und Lesbarkeit deines Codes zu verbessern.
- Setze auf agiles Arbeiten und iterative Entwicklungsprozesse, um flexibel auf Veränderungen reagieren zu können.
- Entwickle ein Verständnis für Benutzererfahrung (UX) und Benutzeroberflächendesign (UI), um benutzerfreundliche
  Anwendungen zu erstellen.
- Nutze Monitoring- und Logging-Tools, um die Leistung und Stabilität deiner Anwendungen zu überwachen.
- Setze auf Dokumentation und Wissensmanagement, um dein Wissen zu organisieren und für zukünftige Projekte zugänglich
  zu machen.
- Entwickle ein Verständnis für die rechtlichen und ethischen Aspekte der Softwareentwicklung, um verantwortungsbewusst
  zu handeln.
- Nutze Mentoring und Coaching, um dein Wissen weiterzugeben und anderen Entwicklern zu helfen, ihre Fähigkeiten zu
  verbessern.
- Setze auf persönliche Entwicklung und Soft Skills, um deine Karriere als Softwareentwickler voranzutreiben.
- Entwickle ein Verständnis für die wirtschaftlichen Aspekte der Softwareentwicklung, um fundierte Entscheidungen zu
  treffen.
- Nutze Networking und berufliche Beziehungen, um deine Karrierechancen zu verbessern.
- Setze auf Work-Life-Balance, um langfristig produktiv und motiviert zu bleiben.
- Entwickle ein Verständnis für die globalen Trends und Entwicklungen in der Softwareindustrie, um zukunftsorientiert zu
  arbeiten.
