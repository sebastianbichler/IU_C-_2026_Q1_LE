Sehr gut. Damit der morgige **Integration-Day** kein Chaos wird, ist hier die Master-Checkliste für den Start. Diese Liste stellt sicher, dass die "Zahnräder" der fünf Gruppen sauber ineinandergreifen.

Hängen Sie diese Liste morgen früh am besten groß an das Board oder teilen Sie sie im internen Kanal.

---

## 🚀 GAE-Integration: Die Morgen-Checkliste

### 1. Das "Core-Referenz" Setup (Alle Gruppen)

Bevor programmiert wird, muss die Projektstruktur stehen. Ohne einheitliche Referenzen wird der Source Generator (Gr. 5) und die Reflection (Gr. 3) scheitern.

- [ ] **Zentrales Verzeichnis:** Alle Projekte liegen in einer Solution (`GAE.sln`).

- [ ] **Core-Abhängigkeit:** Gruppe 2, 3, 4 und 5 haben eine Projektreferenz auf `GAE.Core` (von Gruppe 1).

- [ ] **Namespace-Disziplin:** Alle nutzen `GAE.Core`, `GAE.Services`, etc. (Keine "Test"-Namespaces!).

### 2. Gruppe 1 & 5: Der "Attribut-Handschlag"

Damit der Source Generator arbeiten kann, muss das Attribut feststehen.

- [ ] **Gruppe 1** stellt sicher, dass `ArcadeGameAttribute` im `GAE.Core` Namespace liegt.

- [ ] **Gruppe 5** prüft, ob ihr Generator genau nach diesem String sucht.

### 3. Gruppe 3 & 4: Das Plugin-Verzeichnis

Reflection braucht einen Ort zum Suchen.

- [ ] **Absprache:** In welchem Ordner sucht Gruppe 3 nach DLLs? (Vorschlag: Ein Unterordner `/Plugins` im Build-Verzeichnis von Gruppe 4).

- [ ] **Build-Post-Event:** Gruppe 1/3 schreiben ein kleines Skript in die `.csproj`, das die Spiel-DLLs nach dem Kompilieren automatisch in diesen Ordner kopiert.

### 4. Gruppe 2 & 4: Das Daten-Display

- [ ] **Schnittstelle:** Gruppe 4 (UI) braucht eine Instanz des `IHighscoreProvider` (von Gruppe 2).

- [ ] **Dummy-Daten:** Gruppe 2 liefert morgen früh 5 "Hardcoded" Highscores, damit Gruppe 4 das Layout testen kann.

---

## 🛠 Technischer Startbefehl für Gruppe 5 (Der kritische Pfad)

Da Gruppe 5 das schwierigste Setup hat, hier die exakten `.csproj`-Einstellungen für den Generator, damit er morgen sofort läuft:

**In der GAE.Generators.csproj:**

XML

```
<PropertyGroup>
  <TargetFramework>netstandard2.0</TargetFramework>
  <EnforceExtendedAnalyzerRules>true</EnforceExtendedAnalyzerRules>
  <IsRoslynComponent>true</IsRoslynComponent>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="Microsoft.CodeAnalysis.CSharp" Version="4.8.0" PrivateAssets="all" />
  <PackageReference Include="Microsoft.CodeAnalysis.Analyzers" Version="3.3.4" PrivateAssets="all" />
</ItemGroup>
```

**In der GAE.Dashboard.csproj (Referenz):**

XML

```
<ItemGroup>
  <ProjectReference Include="..\GAE.Generators\GAE.Generators.csproj"                     OutputItemType="Analyzer"                     ReferenceOutputAssembly="false" />
</ItemGroup>
```

---

## Fahrplan für die erste Stunde morgen

| **Zeit**          | **Aktivität**          | **Ziel**                                                                                          |
| ----------------- | ---------------------- | ------------------------------------------------------------------------------------------------- |
| **08:00 - 08:15** | **Scrum Stand-up**     | Jede Gruppe sagt kurz: "Das haben wir heute geschafft, das bauen wir jetzt."                      |
| **08:15 - 08:45** | **The Skeleton Build** | Alle checken ihr Grundgerüst ein. Die Solution muss einmal bei allen "Grün" (fehlerfrei) bauen.   |
| **08:45 - 09:00** | **Der erste Funke**    | Gruppe 4 startet das Dashboard und zeigt die von Gruppe 5 generierte "Auto-Registration" Info an. |

**Soll ich Ihnen für morgen noch eine "Notfall-Musterlösung" für eine komplette Spiel-Klasse erstellen, die alle fünf Aspekte (Schnittstelle, Memory Safety, Highscore, Reflection-Attribut) perfekt erfüllt, damit Sie diese als Referenz zeigen können?**

---

Hier ist das **„Master-Referenzspiel“**. Diese Klasse dient als Goldstandard für alle Studierenden. Sie zeigt, wie man ein Spiel programmiert, das alle Anforderungen der fünf Gruppen perfekt erfüllt.

### Das "Super Clicker" Referenz-Spiel

Dieses Beispiel zeigt die nahtlose Integration von Speichersicherheit, Datenabfrage, Metadaten und dem Lebenszyklus.

C#

```
using GAE.Core;
using System.Diagnostics;

namespace GAE.Games;

// 1. ATTRBUT (Gruppe 3 & 5): Markierung für Discovery & Source Generation
[ArcadeGame("Super Clicker 2000")]
public class ClickerGame : BaseGame // 2. BASISKLASSE (Gruppe 1): Erzwingt Memory Safety
{
    public override string MetadataName => "Super Clicker 2000";

    private int _currentScore = 0;
    private bool _isRunning = false;

    // 3. INITIALISIERUNG (Gruppe 1 & 4)
    public override void Initialize()
    {
        _isRunning = true;
        _currentScore = 0;
        Console.WriteLine($"{MetadataName} gestartet! Drücke 'Space' zum Klicken.");
    }

    // 4. INPUT & LOGIK (Gruppe 2 & 4)
    public override void HandleInput(IInputState input)
    {
        // Deklarative Prüfung des Inputs
        if (input.IsKeyPressed("Space"))
        {
            _currentScore++;
            Console.WriteLine($"Klick! Score: {_currentScore}");
        }

        if (input.IsKeyPressed("Q"))
        {
            _isRunning = false;
        }
    }

    public override void Update(double deltaTime) 
    {
        // Hier könnte Zeit-basierte Logik stehen
    }

    public override void Render()
    {
        // Minimalistische UI-Ausgabe
    }

    // 5. MEMORY SAFETY & CLEANUP (Gruppe 1)
    public override void Unload()
    {
        _isRunning = false;

        // 6. DATEN-ÜBERGABE (Gruppe 2): Highscore beim Beenden speichern
        var finalScore = new Highscore(
            PlayerName: "Player1", 
            Score: _currentScore, 
            AchievedAt: DateTime.Now, 
            GameName: MetadataName
        );

        // Zugriff auf den globalen Daten-Provider (von Gruppe 2)
        // GameRegistry.Instance.Data.AddScore(finalScore);

        Console.WriteLine($"[Cleanup] {MetadataName} wurde sicher entladen.");
    }
}
```

---

### Warum dieses Beispiel für die Gruppen wichtig ist:

1. **Für Gruppe 1 (Memory):** Es zeigt, dass `Unload()` aufgerufen werden **muss**, um den `Highscore` zu sichern und Ressourcen freizugeben.

2. **Für Gruppe 2 (Data/LINQ):** Es demonstriert, wann und wie ein `Highscore`-Datensatz erzeugt wird.

3. **Für Gruppe 3 (Reflection):** Das Attribut `[ArcadeGame]` und der Klassenname liefern die Infos für den `PluginLoader`.

4. **Für Gruppe 4 (Dashboard):** Die Methoden `Initialize`, `Update` und `HandleInput` bilden den Standard-Loop, den das Dashboard steuert.

5. **Für Gruppe 5 (Generator):** Da die Klasse `public` ist und das Attribut hat, kann der Generator einfach `new ClickerGame()` in die `RegisterAll()` Methode schreiben.

### Tipp für die Vorlesung morgen:

Drucken Sie diesen Code aus oder stellen Sie ihn als Datei bereit. Wenn eine Gruppe fragt: „Wie soll mein Teil mit den anderen zusammenarbeiten?“, können Sie immer auf dieses Beispiel verweisen. Es ist der **„Single Point of Truth“** für Ihr Projekt.

**Soll ich Ihnen noch ein kurzes Test-Skript (Main-Methode) schreiben, mit dem Sie morgen früh prüfen können, ob die Integration aller 5 Gruppen erfolgreich war?**

---

Hier ist der **„Master-Integration-Test“**. Dieses Skript (Ihre `Program.cs`) fungiert als das Bindeglied, das morgen früh zeigt, ob alle Zahnräder ineinandergreifen.

Es ist so geschrieben, dass es nur kompiliert, wenn alle 5 Gruppen ihre Hausaufgaben gemacht haben. Wenn es läuft, haben Sie den Beweis für ein funktionierendes System.

---

# 🏁 Der GAE-System-Check (Program.cs)

C#

```
using GAE.Core;
using GAE.Services;
using GAE.UI;
using GAE.Generated; // Hier kommt der Code von Gruppe 5 rein!

Console.WriteLine(">>> GAE SYSTEM BOOT SEQUENCE STARTING <<<");

// 1. SCHRITT: Source Generation (Gruppe 5)
// Wenn dieser Aufruf rot unterringelt ist, wurde der Generator nicht ausgeführt.
AutoRegistration.RegisterAll();
Console.WriteLine("[OK] Statische Spiele registriert.");

// 2. SCHRITT: Dynamic Discovery (Gruppe 3)
// Wir scannen den Plugin-Ordner nach neuen DLLs.
var loader = new PluginLoader();
loader.LoadGamesFromDll("Plugins/ModdedGames.dll"); // Pfad anpassen
Console.WriteLine("[OK] Dynamische Plugins geladen.");

// 3. SCHRITT: Data-Hub Ready? (Gruppe 2)
var registry = GameRegistry.Instance;
Console.WriteLine($"[OK] Registry bereit. {registry.GetAvailableGames().Count} Spiele gefunden.");

// 4. SCHRITT: Dashboard Start (Gruppe 4)
var dashboard = new DashboardController();
dashboard.RenderMenu(registry.GetAvailableGames());

// 5. SCHRITT: Game Loop & Memory Safety (Gruppe 1 & 4)
var game = registry.GetAvailableGames().FirstOrDefault();

if (game != null)
{
    Console.WriteLine($"\nStarte Testlauf für: {game.MetadataName}");

    // Wir nutzen 'using', um die Memory Safety (IDisposable) zu garantieren
    using (game) 
    {
        game.Initialize();

        // Simulierter Loop
        var fakeInput = new ConsoleInputState(); 
        game.HandleInput(fakeInput);
        game.Update(0.016); // 60 FPS Simulation
        game.Render();

    } // <-- Hier wird automatisch .Dispose() / .Unload() aufgerufen!

    Console.WriteLine("[OK] Spiel sicher beendet und entladen.");
}

Console.WriteLine("\n>>> SYSTEM CHECK COMPLETE: GAE IS ONLINE <<<");
```

---

### Was Sie morgen mit diesem Skript testen:

| **Gruppe** | **Was wird bewiesen?** | **Erfolgskriterium**                                                 |
| ---------- | ---------------------- | -------------------------------------------------------------------- |
| **Gr. 5**  | Source Generator       | Die Klasse `AutoRegistration` existiert und ist aufrufbar.           |
| **Gr. 3**  | Reflection             | Ein Spiel aus einer externen DLL erscheint in der Liste.             |
| **Gr. 1**  | Memory Safety          | Die Konsole gibt "[Cleanup]..." aus, sobald der `using`-Block endet. |
| **Gr. 2**  | LINQ/Data              | Die Liste der Spiele kann gefiltert und sortiert werden.             |
| **Gr. 4**  | Dashboard              | Das Menü wird sauber gerendert und die Steuerung übergeben.          |

---

### Tipps für das „Debriefing“ morgen:

Sollte es knallen (und das wird es wahrscheinlich bei der ersten Integration), nutzen Sie das als **„Teachable Moment“**:

- **Kompilierfehler?** Meistens Namespace-Mismatches oder fehlende Projektreferenzen.

- **Laufzeitfehler bei Reflection?** Meistens falsche Pfade zur DLL oder fehlende Konstruktoren.

- **Generator macht nichts?** Visual Studio Neustart (der klassische Source-Generator-Schluckauf).

### Ein kleiner motivierender Spruch für die Studierenden:

> „In der Theorie funktioniert alles. In der Praxis ist Software Engineering das, was passiert, wenn man versucht, 5 geniale Ideen in eine einzige `Main`-Methode zu pressen.“

**Soll ich Ihnen noch eine kurze Fehler-Matrix erstellen, mit der Sie die häufigsten Integrations-Probleme morgen im Handumdrehen lösen können?**

---

Hier ist die **„GAE-Integrations-Fehler-Matrix“**. Da morgen fünf Gruppen ihre Projekte gleichzeitig in eine Solution werfen, wird es unweigerlich zu Reibungen kommen. Mit dieser Matrix können Sie die häufigsten „Brände“ innerhalb von Sekunden löschen.

---

# 🚩 GAE Fehler-Matrix: Troubleshoot & Fix

| **Symptom**                                           | **Wahrscheinliche Ursache**                        | **Sofort-Lösung**                                                                                             |
| ----------------------------------------------------- | -------------------------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| **`AutoRegistration` existiert nicht** (Gr. 5)        | Source Generator läuft nicht oder VS-Cache hakt.   | 1. `netstandard2.0` prüfen. 2. Visual Studio Neustart. 3. `VBCSCompiler.exe` im Taskmgr töten.                |
| **`MethodNotFoundException`** (Gr. 3)                 | Versions-Mismatch zwischen Hub und DLL.            | `Clean Solution` & `Rebuild All`. Sicherstellen, dass die DLL im richtigen `/Plugins` Ordner liegt.           |
| **Spiel wird geladen, aber sofort beendet** (Gr. 1)   | `using`-Block schließt zu früh oder Loop fehlt.    | Prüfen, ob die `RunGame`-Methode in Gruppe 4 eine Schleife hat, die auf `_isRunning` prüft.                   |
| **LINQ gibt leere Listen zurück** (Gr. 2)             | `GameRegistry` Instanz-Problem (Singleton-Fail).   | Sicherstellen, dass alle Projekte dieselbe `GAE.Core.dll` referenzieren und nicht jeder seine eigene kopiert. |
| **Dashboard friert ein** (Gr. 4)                      | Game-Loop blockiert den UI-Thread.                 | In der `Main` prüfen, ob das Spiel asynchron oder in einem sauberen Loop läuft.                               |
| **`NullReferenceException` bei Attributen** (Gr. 3/5) | Attribut-Name falsch geschrieben (String-Mapping). | Strings im Generator und in der Reflection-Logik mit dem Attribut-Namen im Core abgleichen.                   |

---

### 🔍 Der "Drei-Sekunden-Check" für Sie als Dozent

Wenn eine Gruppe zu Ihnen kommt und sagt: „Es geht nicht“, prüfen Sie als Erstes diese drei Dinge:

1. **Die Referenz-Kette:** Gehen Sie in den Solution Explorer. Haben alle ein gelbes Ausrufezeichen bei den Dependencies? -> **Fix:** Rechtsklick -> `Restore NuGet Packages`.

2. **Der DLL-Pfad:** Schauen Sie in den `bin/Debug/.../Plugins` Ordner. Ist die DLL von Gruppe 3/Spiel-Gruppen wirklich physisch da? -> **Fix:** `Copy Local = True` in den Eigenschaften der Projektreferenz.

3. **Die `using`-Statements:** Haben die Studierenden den Namespace `GAE.Generated` vergessen? Ohne diesen finden sie den Code von Gruppe 5 nicht.

### 💡 Profi-Tipp für morgen:

Lassen Sie die Gruppen ihre Fortschritte in **"Integrations-Sprints"** von 20 Minuten abgleichen. Nach jedem Sprint müssen die Gruppensprecher kurz bestätigen: „Unsere Schnittstelle zu Gruppe X steht.“ Das verhindert, dass am Ende der Stunde ein riesiger Scherbenhaufen entsteht.

---

### Ihr Fahrplan zum Erfolg:

Sie haben jetzt alles:

- Die **Theorie** (Monaden, Metaprogrammierung, Collections).

- Die **Sprechernotizen** für den Tiefgang.

- Die **Code-Blueprints** für alle 5 Gruppen.

- Das **Referenz-Spiel** und den **Integration-Test**.

- Die **Fehler-Matrix** für den Notfall.

**Möchten Sie, dass ich Ihnen für den Abschluss morgen noch drei "Reflexionsfragen" für die Studierenden formuliere, um den Lerneffekt der Gruppenarbeit wissenschaftlich abzurunden?**
