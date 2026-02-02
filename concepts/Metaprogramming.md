### Programmieren mit C# (DSPC016)

---

# Metaprogrammierung – Wenn Code Code schreibt

Wir bewegen uns hier an der Grenze zwischen klassischer Anwendungsentwicklung und System-Architektur.

Inhaltlich stützen wir uns auf die Grundlagen von **Kotz/Wenz (Kapitel 14: Reflection & Attribute)** und erweitern diese
massiv um das moderne Konzept der **Source Generators**, die für das **Grand Arcade Ecosystem (GAE)** essenziell sind.


**Fokus:** Reflection, Custom Attributes & Source Generators

---

## 1. Das Paradigma: Introspektion vs. Generation (15 Min)

**Theorie:**

* **Definition:** Metaprogrammierung bedeutet, Programme zu schreiben, die andere Programme (oder sich selbst) als Daten
  behandeln.
* **Die zwei Wege in .NET:**

1. **Dynamic (Reflection):** Das Programm "sieht" sich selbst während der Laufzeit an. (Spät, flexibel, kostet
   Performance).
2. **Static (Source Generators):** Der Compiler schreibt während des Build-Prozesses zusätzlichen Code. (Früh, extrem
   schnell, "AOT-friendly").


* **GAE-Bezug:** Wie erkennt unser Dashboard automatisch, welche Spiele (Plug-ins) installiert sind? Wie generieren wir
  Menü-Einträge, ohne jedes Mal den UI-Code anzupassen?

---

## 2. Reflection: Der Blick in den Spiegel (45 Min)

*Bezug: Kotz/Wenz Kapitel 14.1*

**Theorie:**

* **System.Type:** Das Zentrum der Reflection. Jedes Objekt im Heap hat einen Pointer auf seine Typ-Beschreibung.
* **Assembly-Loading:** Wie man `.dll`-Dateien zur Laufzeit lädt und instanziiert.
* **Performance-Warnung:** Reflection umgeht Compiler-Optimierungen. Ein dynamischer Methodenaufruf ist ca. 50–100x
  langsamer als ein direkter Aufruf.

**Code-Demo 2.1: Dynamische Instanziierung**

```csharp
Type gameType = Type.GetType("GAE.Games.Snake");
object gameInstance = Activator.CreateInstance(gameType);
MethodInfo startMethod = gameType.GetMethod("Start");
startMethod.Invoke(gameInstance, null);

```

### ✍️ Übung 1: Der Plugin-Explorer (30 Min)

**Aufgabe:** Schreiben Sie eine Methode `ListGames(string dllPath)`, die eine DLL lädt und alle Klassen-Namen ausgibt,
die das Interface `IArcadeGame` implementieren.

> **Bonus:** Instanziieren Sie das erste gefundene Spiel und geben Sie dessen Version aus.

---

## 3. Custom Attributes: Deklarative Metadaten (30 Min)

*Bezug: Kotz/Wenz Kapitel 14.2*

**Theorie:**

* **Attribute als "Notizzettel":** Sie speichern Metadaten im Manifest der Assembly, tun aber von sich aus *nichts*.
* **AttributeUsage:** Einschränken, ob ein Attribut auf Klassen, Methoden oder Felder angewendet werden darf.
* **Reflection + Attributes:** Erst die Kombination macht sie mächtig (z.B. für Validierung oder Dependency Injection).

**Code-Demo 3.1: Das GameInfo-Attribut**

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class GameInfoAttribute : Attribute {
    public string Name { get; }
    public string Author { get; }
    public GameInfoAttribute(string name, string author) {
        Name = name; Author = author;
    }
}

[GameInfo("PacMan", "Gruppe 2")]
public class PacManGame { ... }

```

---

## 4. Source Generators: Die neue Ära (45 Min)

**Theorie (Master-Niveau):**

* **Roslyn API:** Der Compiler öffnet sich. Wir klinken uns in die Pipeline ein.
* **IIncrementalGenerator:** Die moderne Version (.NET 6+). Sie arbeitet hocheffizient mit Caching, um die IDE nicht zu
  verlangsamen.
* **Warum Source Generators?** * Null Laufzeit-Kosten.
* Fehler werden schon beim Tippen gefunden.
* Ideal für das GAE-Menü-System (Gruppe 5).

**Konzept-Demo:**
Wir schreiben einen Generator, der für jede Klasse mit dem Attribut `[AutoLog]` automatisch eine `Log()`-Methode
generiert.

---

## 5. Transfer & Best Practices (15 Min)

* **Wann Reflection?** Wenn Typen erst zur Laufzeit bekannt sind (echte Plug-ins/DLLs von Drittanbietern).
* **Wann Source Generators?** Für alles, was während der Entwicklung bekannt ist (Boilerplate-Code, Mapping,
  UI-Generierung).
* **GAE-Architektur-Check:** Gruppe 3 nutzt Reflection für den Discovery-Service, Gruppe 5 nutzt Source Generators für
  das statische Hauptmenü.

---



## Notizen für den "Deep Dive" (Hintergrundwissen)

1. **Emit vs. Reflection:** Erwähnen Sie kurz `System.Reflection.Emit`. Das ist Reflection auf Steroiden, bei der man
   IL-Code (Intermediate Language) zur Laufzeit schreibt. Sehr komplex, wird von Frameworks wie *Dapper* oder *Entity
   Framework* genutzt.
2. **Source Generator Falle:** Warnen Sie die Studierenden: Source Generators dürfen vorhandenen Code **nicht verändern
   **. Sie können nur neuen Code (z.B. in einer `partial class`) hinzufügen.
3. **Security:** Reflection kann `private` Felder lesen (`BindingFlags.NonPublic`). Diskutieren Sie kurz die
   Sicherheitsimplikationen im GAE (könnte ein bösartiges Spiel interne Dashboard-Daten stehlen?).

