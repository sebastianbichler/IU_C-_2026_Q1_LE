### Programmieren mit C# (DSPC016)

---

# # Referatsunterlagen Variante 2
Dies stellt die zweite Variante für die Anforderungen an die Prüfung bzw. den Referaten. Diese Variante ist anspruchsvoller als die erste, da sie spezifischer
wissenschaftlichen Fragestellungen und Anwendungsfällen vorsieht. Welche Variante zum Einsatz kommt, werden wir besprechen.

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

Um im Rahmen der Referate die geforderte wissenschaftliche Strenge zu erreichen, werden folgende
Quellenpfade empfohlen:

* **Microsoft Research (MSR):** Suchen Sie nach den Namen *Don Syme*, *Anders Hejlsberg* und *Erik Meijer*. Viele
  Features von C# (Generics, LINQ, Async) wurden dort zuerst als Paper veröffentlicht.
* **ACM Digital Library / IEEE Xplore:** Suchen Sie nach dem Schlagwort *"C# semantics"* oder *"High-performance .NET"*.
* **Language Specification:** Die **ECMA-334** (C# Language Specification) ist die ultimative wissenschaftliche Quelle
  für die Funktionsweise der Sprache.

---

Für eine Master-Lehrveranstaltung im 10. Semester ist es sinnvoll, eine **Rahmenarchitektur** vorzugeben, die
industrielle Standards widerspiegelt.
Die Studierenden sollen nicht „irgendetwas“ programmieren, sondern eine konsistente, schichtweise Architektur (N-Tier
oder Clean Architecture) umsetzen.

Hier ist die Definition der Rahmenaufgabe und die vier darauf aufbauenden Spezialszenarien.

---

## Die Rahmenaufgabe: "The Modular Enterprise Core"

Jede Gruppe muss eine Anwendung erstellen, die folgende **Basiskomponenten** nativ mit .NET 8 Mitteln abbildet:

1. **Identity & Security:** Umsetzung von Login und Userverwaltung.

- *Nativ:* Verwendung von **Microsoft.AspNetCore.Identity** (auch in Desktop-Apps nutzbar) oder **IdentityReboot**.
  Passwort-Hashing via `IPasswordHasher<T>`.

2. **Persistence Layer:** Datenbankanbindung mit **Entity Framework Core 8**.

- *Anforderung:* Nutzung von Migrations und ein relationales Modell (SQLite als portable Datei im Repo).

3. **Cross-Platform GUI:** Da Windows-spezifisches WPF ausscheidet, ist die Empfehlung **Avalonia UI** (sehr nah an WPF,
   läuft nativ auf Linux/Mac) oder **Uno Platform**.
4. **Cross-Cutting Concerns:** * *Logging:* **Microsoft.Extensions.Logging** (Abstraktion) mit **Serilog** (Senke).

- *DI-Container:* Nativer **Microsoft.Extensions.DependencyInjection**.

---

## Die 4 Anwendungsszenarien mit fachspezifischem Fokus

### Gruppe 1: "High-Performance Telemetry Analyzer" (Fokus: Memory & Performance)

**Szenario:** Ein System zur Echtzeit-Analyse von Sensordaten (z.B. aus einer Industrie-4.0-Fertigung).

- **Die Aufgabe:** Verarbeitung von massiven Datenströmen (Simulations-Input), die in einer Dashboard-GUI visualisiert
  werden.
- **Wissenschaftlicher Fokus:** Implementierung von Verarbeitungsalgorithmen unter strikter Vermeidung von
  GC-Allokationen.
- **C# Spezialität:** Einsatz von `struct`, `ref struct`, `Span<T>` und `ArrayPool<T>`. Vergleich der
  CPU-Zyklen/Speicherlast zwischen einem "naiven" OOP-Ansatz (Klassen) und einem performanten Low-Level-Ansatz.

### Gruppe 2: "Global Trade Intelligence Engine" (Fokus: LINQ & Abstraktion)

**Szenario:** Ein Analysetool für weltweite Handelsdaten (Import/Export), das komplexe Filterungen über verschiedene
Datenquellen erlaubt.

- **Die Aufgabe:** Entwicklung einer Abfrageschnittstelle, die Daten aus SQL (EF Core), JSON-Files und einer externen
  Web-API zusammenführt.
- **Wissenschaftlicher Fokus:** Erstellung eines eigenen "Query-Transformers". Die Studierenden sollen zeigen, wie
  LINQ-Abfragen zur Laufzeit analysiert werden können.
- **C# Spezialität:** Deep-Dive in `IQueryable`, `Expression Trees` und die funktionale Komposition von Abfragen.

### Gruppe 3: "Distributed Multi-Agent Crawler" (Fokus: Async & Concurrency)

**Szenario:** Ein System, das Informationen aus verschiedenen wissenschaftlichen Repositorien gleichzeitig abfragt und
mittels KI (oder Heuristiken) klassifiziert.

- **Die Aufgabe:** Hunderte von parallelen HTTP-Requests steuern, ohne den GUI-Thread zu blockieren oder Ressourcen zu
  erschöpfen (Throttling).
- **Wissenschaftlicher Fokus:** Analyse von Race Conditions und Deadlocks in asynchronen Systemen. Formale Beschreibung
  des Task-Lebenszyklus.
- **C# Spezialität:** `async/await`, `CancellationToken`, `Channels<T>` für Producer-Consumer-Szenarien und
  `Parallel.ForEachAsync`.

### Gruppe 4: "Digital Twin Supply Chain Manager" (Fokus: DDD & Architektur)

**Szenario:** Abbildung einer komplexen Logistikkette (Lager, Transport, Zoll) als "Digitaler Zwilling".

- **Die Aufgabe:** Fokus auf die korrekte Abbildung der Geschäftslogik. Ein Paket darf z.B. nicht "geliefert" werden,
  wenn es nicht vorher "verzollt" wurde.
- **Wissenschaftlicher Fokus:** Umsetzung von **Domain-Driven Design (DDD)**. Trennung von Domain-Logik und
  Infrastruktur. Vermeidung des "Anemic Domain Models".
- **C# Spezialität:** Nutzung von `Records` für Value Objects, `Required Members` für Validierung und komplexe Mappings
  in EF Core 8 (Value Converters, Owned Types).

---

## Entwurf und Modellierung (Anforderung an die Studierenden)

Für alle Gruppen ist folgendes Abgabeschema (im GitHub Repo) verpflichtend:

1. **Requirements Spec:** User Stories und funktionale Anforderungen.
2. **Analysemuster:** UML-Klassendiagramm des Domänenmodells.
3. **Verhaltensmodellierung:** Ein Sequenzdiagramm für einen komplexen Prozess (z.B. der asynchrone Datenbezug oder der
   Login-Flow mit Verschlüsselung).
4. **Implementierungsmodell:** Darstellung der Schichtenarchitektur (welches Projekt in der Solution darf auf welches
   zugreifen).
5. **Wissenschaftliches Abstract:** Eine 2-seitige Begründung, warum für ein spezifisches Problem genau dieses
   Entwurfsmuster (z.B. Strategy oder Observer) gewählt wurde.

---

## Umsetzung von Standardaufgaben (Login/User)

Um den Fokus auf der Wissenschaft zu halten, sollten die Studierenden hier **"Best Practices"** von .NET nutzen, statt
das Rad neu zu erfinden:

- **Authentifizierung:** Nutzung des `Microsoft.Extensions.Identity.Core` Pakets. Dies stellt die Klassen `UserManager`
  und `SignInManager` bereit.
- **Datenhaltung:** Ein `IdentityDbContext` in EF Core legt automatisch die Tabellen für User, Rollen und Claims an.
- **GUI-Integration:** In Avalonia UI wird ein `LoginView` erstellt, der gegen ein `LoginViewModel` (MVVM-Pattern!)
  bindet, welches wiederum den Identity-Service aufruft.

---
