# GAE Persistence Layer: ADO.NET vs. Entity Framework Core 8

Dieses Dokument beschreibt die architektonische Gegenüberstellung zweier Datenzugriffstechnologien im Rahmen des **Grand Arcade Ecosystems (GAE)**.

## 1. Der Impedance Mismatch
Das Kernproblem unserer Architektur ist die Inkompatibilität zwischen der objektorientierten Domäne (C#) und dem relationalen Speichermodell (SQLite).
> *"Object/relational mapping is the Vietnam of Computer Science"* – Ted Neward.

### Die Herausforderung im Projekt
- **Domäne:** Tiefe 3-stufige Hierarchie (`PlayerProfile` -> `Inventory` -> `Items`).
- **Datenbank:** Flache 2-stufige Tabellenstruktur.

## 2. Technologie-Stack
Die Persistenzschicht baut modular auf:
1. **Application/C# Code** (Domänenlogik)
2. **Entity Framework Core 8** (Abstraktionsschicht / ORM)
3. **ADO.NET** (Core-Bibliothek / Direkter DB-Zugriff)
4. **SQLite** (Datenhaltung)

## 3. Vergleich der Ansätze

| Kriterium | ADO.NET | EF Core 8 |
| :--- | :--- | :--- |
| **Performance (Insert)** | **~4.5x schneller** | Moderat |
| **Code-Länge (Read)** | 12 Zeilen | **5 Zeilen** |
| **Wartbarkeit** | Gering (Manuelles SQL) | **Hoch (LINQ / Fluent API)** |
| **Kapselung** | Bruch der Invarianten | **Vollständiger Schutz** |

## 4. Architektonische Entscheidung
Für das GAE wurde **Entity Framework Core 8** als primäres Fundament gewählt. 
**Begründung:** Die gewonnene Domain-Integrität und Zukunftssicherheit (Provider-Migration) überwiegen den Performance-Vorteil von ADO.NET in unserem Anwendungsfall.

---
👉 [Details zum ADO.NET Ansatz](./ado-net-deep-dive.md) | [Details zur EF 8 Architektur](./ef-core-8-architecture.md)
