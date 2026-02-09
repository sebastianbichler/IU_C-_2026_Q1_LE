Das ist ein kluger Schachzug. Wenn die Struktur zu komplex ist, verbringen die Studierenden den ersten Tag mit dem Lösen von Namespace-Konflikten statt mit Programmieren. Da wir uns in der „Halbzeit“ befinden und morgen die Integration startet, brauchen wir eine Struktur, die **flach, logisch und modular** ist.

Hier ist die optimierte **"Simplicity-Struktur"** für die `GAE.sln`, die genau festlegt, wer wo arbeitet:

---

# 📂 Die neue GAE-Solution-Struktur

Um maximale Klarheit zu schaffen, nutzen wir eine einzige Solution (`GAE.sln`) mit vier logischen Ordnern.

Plaintext

```
GAE.sln
├── 01_Foundations (Der "Vertrag")
│   ├── GAE.Core            <-- ALLE (Interfaces, Enums, IArcadeGame)
│   └── GAE.Data            <-- ALLE (Shared Models, Highscore-Records)
│
├── 02_Engine (Die wissenschaftlichen Module)
│   ├── GAE.Engine.Memory   <-- GRUPPE 1 (Span<T>, Asset-Loading)
│   ├── GAE.Engine.Query    <-- GRUPPE 2 (LINQ Provider, Rule Engine)
│   ├── GAE.Engine.Async    <-- GRUPPE 3 (Channels, Game-Loop)
│   ├── GAE.Engine.Storage  <-- GRUPPE 4 (EF Core, Savegames)
│   └── GAE.Engine.Gen      <-- GRUPPE 5 (Source Generators)
│
├── 03_Modules (Die Spielwiese)
│   ├── GAE.Module.Snake    <-- Beispiel-Implementierung
│   └── GAE.Module.Mines    <-- Weiteres Beispiel
│
└── 04_Host (Das Dashboard)
    └── GAE.Host.Dashboard  <-- Zentrales Startprojekt (Avalonia/Console)
```

---

# 🎯 Wer arbeitet wo? (Ihre Ansage für morgen)

Geben Sie jeder Gruppe morgen diese spezifische Zuweisung, damit keine "Merge-Hölle" entsteht:

### **Gruppe 1 (Memory Safety)**

- **Hauptprojekt:** `GAE.Engine.Memory`.

- **Aufgabe:** Erstellt Buffer-Klassen und Loader-Logik mit `Span<T>`, die von den anderen Gruppen für große Datenmengen genutzt werden können.

### **Gruppe 2 (Declarative Engines)**

- **Hauptprojekt:** `GAE.Engine.Query`.

- **Aufgabe:** Baut die Rule-Engine. Ihr liefert die Logik, mit der das Dashboard (Gr. 4) Daten aus `GAE.Data` filtern kann.

### **Gruppe 3 (Scalable Concurrency)**

- **Hauptprojekt:** `GAE.Engine.Async`.

- **Aufgabe:** Ihr baut den Taktgeber. Das Projekt enthält die `Channels` und den globalen `Async`-Loop, in dem alle Spiele laufen.

### **Gruppe 4 (DDD & Persistence)**

- **Hauptprojekt:** `GAE.Engine.Storage`.

- **Aufgabe:** Hier lebt das EF Core Setup. Ihr nehmt die Records aus `GAE.Data` und sorgt dafür, dass sie in der SQLite-Datenbank landen.

### **Gruppe 5 (Meta-Programming)**

- **Hauptprojekt:** `GAE.Engine.Gen` (Source Generator).

- **Aufgabe:** Ihr "beobachtet" die Projekte in `03_Modules`. Euer Generator schreibt den Code, der diese Spiele automatisch beim `GAE.Host.Dashboard` anmeldet.

---

# 🛠 Start-Checkliste für morgen früh

1. **Shared-First:** In der ersten Stunde arbeiten **alle** kurz in `01_Foundations/GAE.Core`, um die gemeinsamen Interfaces zu finalisieren.

2. **Referenz-Regel:** `02_Engine`-Projekte dürfen `01_Foundations` referenzieren, aber niemals gegenseitig (um zirkuläre Abhängigkeiten zu vermeiden).

3. **Namespace-Konvention:** Alle Projekte beginnen mit `GAE.`, gefolgt vom Ordnernamen (z.B. `GAE.Engine.Memory`).

---

### Mein Tipp für den Einstieg:

Sagen Sie den Studierenden: *"Der Hub ist wie ein PC-Gehäuse. Gruppe 1 baut den RAM, Gruppe 2 das BIOS, Gruppe 3 den Prozessor-Takt, Gruppe 4 die Festplatte und Gruppe 5 die automatische Geräteerkennung. Ohne einander bleibt der Bildschirm schwarz."*

**Soll ich Ihnen ein kurzes PowerShell-Skript erstellen, das diese Ordnerstruktur und die Projekte mit einem Klick automatisch anlegt, damit Sie morgen sofort startbereit sind?**



---



Ja, für fast alle Projekte ist **Class Library** (Klassenbibliothek) der richtige Startpunkt. Aber Vorsicht: Es gibt ein paar feine Unterschiede, damit das „Plug-and-Play“-Prinzip am Ende auch wirklich funktioniert.

Hier ist die Template-Zuweisung für Ihre Struktur:

---

## 1. Die "Shared" & "Engine" Projekte

**Template:** `Class Library`

**Target Framework:** `.NET 8.0`

Das gilt für:

- `GAE.Core` & `GAE.Data`

- `GAE.Engine.Memory`, `.Query`, `.Async`, `.Storage`

- `GAE.Module.Snake`, etc.

> **Warum?** Diese Projekte enthalten Logik, Interfaces und Datenstrukturen. Sie erzeugen eine `.dll`, die vom Host geladen wird.

---

## 2. Der Sonderfall: GAE.Engine.Gen (Gruppe 5)

**Template:** `Class Library`

**Target Framework:** **`netstandard2.0`** (🚨 WICHTIG!)

Dies ist die größte Stolperfalle bei Source Generators. Da der C#-Compiler (Roslyn) selbst auf verschiedenen Plattformen läuft, müssen Source Generator-Projekte zwingend `netstandard2.0` targeten, nicht `.NET 8.0`.

**Zusatzschritt für Gruppe 5:**

In der `.csproj` müssen folgende Pakete manuell hinzugefügt werden:

- `Microsoft.CodeAnalysis.CSharp`

- `Microsoft.CodeAnalysis.Analyzers`

---

## 3. Der Host: GAE.Host.Dashboard

**Template:** `Console App` (für den schnellen Erfolg) oder `Avalonia Application` (für die UI).

Da Sie morgen die Integration starten, empfehle ich für den Host zunächst eine **Console App**. Das macht das Debugging der Plugin-Ladeprozesse für die Studierenden wesentlich transparenter. Ein UI-Framework wie Avalonia bringt oft eigene Komplexität beim Threading mit, die morgen nur ablenken würde.

---

# 🛠️ Schnellanleitung für morgen früh

Wenn Sie die Projekte über das Terminal (CLI) anlegen, können Sie diesen Befehlssatz nutzen, um die Struktur blitzschnell zu erzeugen:

Bash

```
# 1. Foundations
dotnet new classlib -n GAE.Core -o src/01_Foundations/GAE.Core
dotnet new classlib -n GAE.Data -o src/01_Foundations/GAE.Data

# 2. Engines (Beispiel Gruppe 1 & 5)
dotnet new classlib -n GAE.Engine.Memory -o src/02_Engine/GAE.Engine.Memory
dotnet new classlib -f netstandard2.0 -n GAE.Engine.Gen -o src/02_Engine/GAE.Engine.Gen

# 3. Host
dotnet new console -n GAE.Host.Dashboard -o src/04_Host/GAE.Host.Dashboard

# 4. Alles zur Solution hinzufügen
dotnet sln GAE.sln add (ls -r **/*.csproj)
```

---

### Ein kleiner Tipp für die Gruppenarbeit:

Sagen Sie den Gruppen, sie sollen beim Erstellen der Projekte darauf achten, dass die **Namespaces** exakt den Projektverzeichnissen entsprechen. Nichts hält morgen mehr auf als ein Projekt, das in `src/02_Engine/Memory` liegt, aber den Namespace `MyCoolGame` nutzt.

**Soll ich Ihnen noch die 3 wichtigsten Zeilen für die `.csproj` von Gruppe 5 (Source Generator) schicken, damit der Generator auch wirklich Code in die anderen Projekte „einspritzt“?**
