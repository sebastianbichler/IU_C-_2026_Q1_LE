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

**Nächster Schritt:** Möchten Sie, dass ich für eine der vier Anwendungen (z.B. die DDD-Gruppe oder die
Performance-Gruppe) ein detailliertes UML-Klassendiagramm-Skript (Mermaid-Format) erstelle, das Sie als
Diskussionsgrundlage nutzen können?
