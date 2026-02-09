Ja, für fast alle Projekte ist **Class Library** (Klassenbibliothek) der richtige Startpunkt. Aber Vorsicht: Es gibt ein
paar feine Unterschiede, damit das „Plug-and-Play“-Prinzip am Ende auch wirklich funktioniert.

Hier ist die Template-Zuweisung für Ihre Struktur:

---

## 1. Die "Shared" & "Engine" Projekte

**Template:** `Class Library`
**Target Framework:** `.NET 8.0`

Das gilt für:

* `GAE.Core` & `GAE.Data`
* `GAE.Engine.Memory`, `.Query`, `.Async`, `.Storage`
* `GAE.Module.Snake`, etc.

> **Warum?** Diese Projekte enthalten Logik, Interfaces und Datenstrukturen. Sie erzeugen eine `.dll`, die vom Host
> geladen wird.

---

## 2. Der Sonderfall: GAE.Engine.Gen (Gruppe 5)

**Template:** `Class Library`
**Target Framework:** **`netstandard2.0`** (🚨 WICHTIG!)

Dies ist die größte Stolperfalle bei Source Generators. Da der C#-Compiler (Roslyn) selbst auf verschiedenen Plattformen
läuft, müssen Source Generator-Projekte zwingend `netstandard2.0` targeten, nicht `.NET 8.0`.

**Zusatzschritt für Gruppe 5:**
In der `.csproj` müssen folgende Pakete manuell hinzugefügt werden:

* `Microsoft.CodeAnalysis.CSharp`
* `Microsoft.CodeAnalysis.Analyzers`

---

## 3. Der Host: GAE.Host.Dashboard

**Template:** `Console App` (für den schnellen Erfolg) oder `Avalonia Application` (für die UI).

Da Sie morgen die Integration starten, empfehle ich für den Host zunächst eine **Console App**. Das macht das Debugging
der Plugin-Ladeprozesse für die Studierenden wesentlich transparenter. Ein UI-Framework wie Avalonia bringt oft eigene
Komplexität beim Threading mit, die morgen nur ablenken würde.

---

# 🛠️ Schnellanleitung für morgen früh

Wenn Sie die Projekte über das Terminal (CLI) anlegen, können Sie diesen Befehlssatz nutzen, um die Struktur
blitzschnell zu erzeugen:

```bash
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

Sagen Sie den Gruppen, sie sollen beim Erstellen der Projekte darauf achten, dass die **Namespaces** exakt den
Projektverzeichnissen entsprechen. Nichts hält morgen mehr auf als ein Projekt, das in `src/02_Engine/Memory` liegt,
aber den Namespace `MyCoolGame` nutzt.

**Soll ich Ihnen noch die 3 wichtigsten Zeilen für die `.csproj` von Gruppe 5 (Source Generator) schicken, damit der
Generator auch wirklich Code in die anderen Projekte „einspritzt“?**
