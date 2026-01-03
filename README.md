### Programmieren mit C# (DSPC016)

---

## Advanced Software Engineering with C# & .NET 8 (Paradigmen & Architektur)

Willkommen im Repository der Lehrveranstaltung mit dem offiziellen Titel **Programmieren mit C#**. Ein inoffizieller
Tite könnte lauten **"Objektorientierte Programmierung & Software Design mit C# und .NET 8"**. Dieser Kurs ist kein
reiner Programmierkurs, sondern eine wissenschaftlich-methodische Auseinandersetzung mit modernen
Software-Engineering-Prinzipien unter Verwendung des .NET 8 Ökosystems.

---

## Unterlagen & Dokumentationen

Die wichtigsten Kursdokumente auf einen Blick:
- **Vorlesungsplan & Ziele:** [Vorlesung.md](Vorlesung.md)
- **Referatsaufgaben Variante 1:** [Referate_Variante1.md](Referate_Variante1.md)
- **Referatsaufgaben Variante 2:** [Referate_Variante2.md](Referate_Variante2.md)

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

### Struktur der wichtigen Dateien, Ordner und Projekte im Kurs-Repository

#### Wichtige Dateien im Root-Verzeichnis
- editorconfig - Enthält die Code-Stil-Richtlinien für C#
- .gitignore - Enthält die Liste der auszuschließenden Dateien/Ordner im Git-Repository
- .github/workflows - CI/CD Pipeline Definitionen
- .gitattributes - Git Attribute für Zeilenenden und Merge-Strategien

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
(work in progress)
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
- Husky:
  - dotnet new tool-manifest
  - dotnet tool install Husky
  - dotnet husky install
