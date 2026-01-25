### Programmieren mit C# (DSPC016)

---

# Ergänzende wissenschaftliche Artikel (für Referate)

Für eine wissenschaftlich fundierte Auseinandersetzung im höheren Semester ist es wichtig, dass die Studierenden über
die reine Dokumentation hinausgehen und Primärquellen der Sprachentwicklung sowie Informatik-Theorie nutzen.

Hier sind ergänzende wissenschaftliche Artikel, sortiert nach den vier Themenschwerpunkten der Gruppenarbeit. Diese
eignen sich hervorragend als Einstieg für die Literaturrecherche.

---

## Gruppe 1: Memory Safety & Low-Level Performance

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

- Gordon, A. & Syme, D. () Typing a Multi-Language Intermediate Code. https://dl.acm.org/doi/epdf/10.1145/373243.360228
---

## Gruppe 2: Deklarative Programmierung & LINQ-Provider

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

## Gruppe 3: Scalable Concurrency & State Machines

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

## Gruppe 4: Domain Driven Design (DDD) & Persistence

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

## Gruppe 5: Der "Auto-Discovery" Orchestrator

*Fokus: Metaprogrammierung, Source Generators und Reflection.*

"Design and Implementation of Source Generators in .NET" (Microsoft Research / Dev Blogs)

    Warum: Es erklärt den Wechsel von der alten ISourceGenerator-API zur performanten IIncrementalGenerator-API, die inkrementelles Caching nutzt, um die IDE nicht zu verlangsamen.

"The Cost of Reflection in Managed Runtimes" (Diverse Informatik-Journals, z.B. ACM Digital Library)

    Warum: Diese Studien quantifizieren den Overhead (Speicher und CPU), den Reflection beim Durchsuchen von Metadaten verursacht.

"Ahead-of-Time Compilation for Managed Languages"

    Warum: Wichtig für die Argumentation, warum moderne Apps (Cloud-Native/Mobile) Reflection vermeiden müssen, um "Native AOT"-kompatibel zu bleiben.

"Type-Safe Metaprogramming" (z.B. Arbeiten von Sheard oder Taha)

    Warum: Theoretische Grundlagen über Metaprogrammierung, die garantiert, dass der generierte Code typsicher ist und nicht erst zur Laufzeit abstürzt.

--

## Tipps für die Studierenden zur weiteren Recherche:

Um im Rahmen der Referate die geforderte wissenschaftliche Strenge zu erreichen, werden folgende
Quellenpfade empfohlen:

* **Microsoft Research (MSR):** Suchen Sie nach den Namen *Don Syme*, *Anders Hejlsberg* und *Erik Meijer*. Viele
  Features von C# (Generics, LINQ, Async) wurden dort zuerst als Paper veröffentlicht.
* **ACM Digital Library / IEEE Xplore:** Suchen Sie nach dem Schlagwort *"C# semantics"* oder *"High-performance .NET"*.
* **Language Specification:** Die **ECMA-334** (C# Language Specification) ist die ultimative wissenschaftliche Quelle
  für die Funktionsweise der Sprache.
