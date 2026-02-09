### Programmieren mit C# (DSPC016)

---

## Gruppe 5: Meta-Programming & Source Generation (The Orchestrator)

Gruppe 5, die "Spezialeinheit", die für die Infrastruktur des Arcade-Systems fungiert. Dieses Thema ist besonders
spannend, da es die Grenze zwischen "Code schreiben" und "Code schreiben lassen" überschreitet.

**Das Problem:** In einem Plug-and-Play-System wie unserem Arcade-Hub müssen Module (Spiele) zur Laufzeit entdeckt
werden. Der klassische Weg über **Runtime Reflection** (`Assembly.GetTypes()`) hat zwei Nachteile:

1. **Performance:** Das Scannen aller geladenen DLLs beim Start kostet Zeit (besonders auf schwächeren Geräten oder bei
   vielen Modulen).
2. **Linker-Sicherheit:** Moderne .NET-Features wie *Native AOT* (Ahead-of-Time Compilation) entfernen ungenutzten Code.
   Reflection "versteckt" Abhängigkeiten vor dem Compiler, was dazu führen kann, dass benötigte Spiele-Klassen einfach
   wegoptimiert werden.

**Die Lösung:**
Wir verlagern die Entdeckung der Spiele in den Kompilierzeitpunkt. Ein **Source Generator** liest den Code während des
Tippens/Kompilierens und schreibt den "Kleber-Code" (Boilerplate) für das Menü automatisch.

---

### 📚 Wissenschaftliche Publikationen (Literaturliste)

Hier sind gezielte Quellen, um die theoretische Basis für die Überlegenheit von Source Generation zu untermauern:

1. **"Design and Implementation of Source Generators in .NET"** (Microsoft Research / Dev Blogs)

* *Warum:* Es erklärt den Wechsel von der alten `ISourceGenerator`-API zur performanten `IIncrementalGenerator`-API, die
  inkrementelles Caching nutzt, um die IDE nicht zu verlangsamen.


2. **"The Cost of Reflection in Managed Runtimes"** (Diverse Informatik-Journals, z.B. ACM Digital Library)

* *Warum:* Diese Studien quantifizieren den Overhead (Speicher und CPU), den Reflection beim Durchsuchen von Metadaten
  verursacht.


3. **"Ahead-of-Time Compilation for Managed Languages"**

* *Warum:* Wichtig für die Argumentation, warum moderne Apps (Cloud-Native/Mobile) Reflection vermeiden müssen, um "
  Native AOT"-kompatibel zu bleiben.


4. **"Type-Safe Metaprogramming"** (z.B. Arbeiten von Sheard oder Taha)

* *Warum:* Theoretische Grundlagen über Metaprogrammierung, die garantiert, dass der generierte Code typsicher ist und
  nicht erst zur Laufzeit abstürzt.

---

### 🛠️ C#-Spezialität: `IIncrementalGenerator`

Im Gegensatz zu normalem Code arbeitet ein Source Generator mit dem **Roslyn-Syntaxbaum**. Er "sieht" das Projekt nicht
als ausgeführtes Programm, sondern als Text-Struktur.

#### Code-Beispiel: Der Game-Discovery Generator

Ein verkürztes Beispiel, wie Gruppe 5 den Code für das Dashboard generiert:

```csharp
[Generator]
public class ArcadeDiscoveryGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 1. Suche alle Klassen, die IArcadeGame implementieren
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (s, _) => s is ClassDeclarationSyntax, // Grobfilter
                transform: (ctx, _) => GetSemanticTarget(ctx))    // Semantische Prüfung
            .Where(target => target is not null);

        // 2. Generiere den Code
        context.RegisterSourceOutput(classDeclarations.Collect(), (spc, classes) => {
            var code = $@"
namespace GAE.Generated;
public static class GameRegistry {{
    public static void LoadAll() {{
        // Dieser Code wird AUTOMATISCH generiert:
        {(string.Join("\n", classes.Select(c => $"Dashboard.Register(new {c}());")))}
    }}
}}";
            spc.AddSource("GameRegistry.g.cs", code);
        });
    }

    private static string? GetSemanticTarget(GeneratorAttributeSyntaxContext ctx) {
        // Hier wird geprüft, ob die Klasse wirklich IArcadeGame implementiert...
        return ctx.TargetSymbol.Name;
    }
}

```

---

### Die Master-Challenge für Gruppe 5

Die Gruppe muss nicht nur den Generator bauen, sondern auch **Diagnostics** (Compiler-Warnungen) implementieren:

* **Wissenschaftlicher Aspekt:** "Wie können wir die Korrektheit von Plugins bereits beim Kompilieren erzwingen?"
* **Praktische Aufgabe:** Wenn ein Student eine Klasse `IArcadeGame` nennt, aber keinen parameterlosen Konstruktor
  anbietet, soll der Source Generator eine **eigene Fehlermeldung im Visual Studio (Error List)** anzeigen (z.B.
  `GAE001: Games must have a public parameterless constructor`).

### Zusammenfassung des Workflows für die Gruppe:

1. **Analyse:** Wie lange dauert der Start des Dashboards mit Reflection (Gruppe 5 misst das!).
2. **Implementation:** Bau des `IIncrementalGenerator`.
3. **Validierung:** Nachweis, dass das Dashboard nun *ohne* Reflection alle Spiele findet und die Startzeit (
   Startup-Latenz) gegen Null sinkt.
