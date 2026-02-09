Das ist eine hervorragende Situation: Der Übergang von der Theorie (wissenschaftliche Artikel) zur Praxis (Game-Hub) ist der Moment, in dem "Software Engineering" greifbar wird. Um in der verbleibenden Stunde heute und dem morgigen Tag eine lauffähige Minimalversion (**MVP - Minimum Viable Product**) zu erstellen, nutzen wir ein **Plugin-basiertes Design**, das auf **Source Generators** (für die automatische Integration) und **Interfaces** (für die Autarkie) basiert.

Hier ist der Masterplan, um die Gruppen von der Literatur direkt in den Code zu führen:

---

## Die Vision: "The GAE Plugin Architecture"

Das verbindende Element ist ein **Shared Kernel** (eine kleine Klassenbibliothek), die die Spielregeln definiert. Jede Gruppe arbeitet an einem Modul, das sich per **Attribut-Markierung** automatisch beim Hub anmeldet.

### Das architektonische Konzept

Wir nutzen das **Strategy Pattern** (für die Spiellogik) und das **Observer Pattern** (für das Event-Handling). Die automatische Integration erfolgt über einen **Source Generator**, den Gruppe 5 entwickelt.

---

## Aufgabenstellung für die 5 Gruppen

### Gruppe 1: Core & Interfaces (Der Gesetzgeber)

**Konzept:** Definition des Lebenszyklus eines Spiels.

- **Heute:** Erstellen des Interfaces `IArcadeGame`. Welche Methoden brauchen alle? (`Initialize()`, `Update(double deltaTime)`, `Render()`, `HandleInput()`).

- **Morgen:** Implementierung eines `GameState`-Objekts, das zwischen Hub und Spiel ausgetauscht wird (Punkte, Leben, Status).

- **Wissenschaftlicher Bezug:** Schnittstellen-Design und "Contract-First" Development.

### Gruppe 2: Input & Event System (Die Nerven)

**Konzept:** Abstraktion der Hardware. Ein Spiel sollte nicht wissen, ob eine Taste oder ein Joystick gedrückt wird.

- **Heute:** Definition eines `InputState`-Records (Werte-Typ!). Collections für aktuell gedrückte Tasten.

- **Morgen:** Ein Event-Bus (Observer Pattern), über den der Hub Eingaben an das aktive Spiel pusht.

- **Wissenschaftlicher Bezug:** Event-driven Architecture und Latenzminimierung.

### Gruppe 3: Registry & Discovery (Das Gehirn)

**Konzept:** Wie weiß der Hub, welche Spiele existieren?

- **Heute:** Erstellung des `[ArcadeGame(string Name, string Author)]` Custom Attributes.

- **Morgen:** Implementierung einer `GameRegistry` (Singleton), in der Instanzen der Spiele verwaltet werden.

- **Wissenschaftlicher Bezug:** Reflection vs. Service Discovery.

### Gruppe 4: UI Framework & Dashboard (Das Gesicht)

**Konzept:** Das Hauptmenü, das die Liste der Spiele anzeigt.

- **Heute:** Ein einfaches Konsolen- oder Minimal-UI-Menü, das die `GameRegistry` ausliest.

- **Morgen:** Implementierung des Wechsels zwischen Menü-Modus und Spiel-Modus (State Pattern).

- **Wissenschaftlicher Bezug:** Human-Computer Interaction (HCI) und State Management.

### Gruppe 5: Automatisierung & Source Generation (Der Mechaniker)

**Konzept:** Die "Magie", die alles verbindet.

- **Heute:** Setup des Source Generator Projekts. Ziel: Suche alle Klassen mit `[ArcadeGame]` und generiere eine Methode `RegisterAll()`.

- **Morgen:** Integration des Generators, sodass Gruppe 1-4 nur eine neue Klasse erstellen müssen und diese sofort im Menü von Gruppe 4 erscheint.

- **Wissenschaftlicher Bezug:** Metaprogrammierung und Compile-time Safety.

---

## Fahrplan für die verbleibende Stunde heute (Interaktiv)

1. **Das "Interface-Agreement" (15 Min):**
   
   Versammeln Sie alle Gruppen. Gruppe 1 präsentiert am Beamer den Entwurf des `IArcadeGame`. Alle müssen zustimmen, da dies der "Vertrag" ist, an den sich alle halten.

2. **Repo-Setup (10 Min):**
   
   Ein zentrales Git-Repository mit 5 Ordnern. Jede Gruppe erstellt ein Projekt.

3. **Die "Hello Arcade" Challenge (35 Min):**
   
   Jede Gruppe muss bis zum Ende der Stunde ein "Skelett" ihrer Aufgabe programmiert haben, das zumindest kompiliert. Gruppe 3 und 5 müssen sich hier eng abstimmen (Attribut-Definition).

---

## Ausblick auf Morgen: Die Integration

Morgen starten wir mit einem **"Integration Sprint"**.

- **Phase 1 (Vormittag):** Jede Gruppe stellt sicher, dass ihr Modul die Schnittstellen von Gruppe 1 korrekt bedient.

- **Phase 2 (Mittag):** Der Source Generator von Gruppe 5 wird "scharf" geschaltet.

- **Phase 3 (Abschluss):** Jede Gruppe schreibt ein winziges "Dummy-Spiel" (z.B. ein Spiel, das nur "Hello World" ausgibt), um zu beweisen, dass die automatische Integration funktioniert.

**Der Clou für die Studierenden:**

Wenn sie morgen fertig sind, haben sie ein System, bei dem man einfach eine neue Klasse schreibt, sie mit einem Attribut versieht, und – *zack* – taucht sie im Dashboard auf, ist steuerbar und trackt Highscores. Das motiviert enorm für die kommenden Referate.

**Soll ich für Gruppe 1 ein konkretes C#-Interface-Beispiel formulieren, das Sie jetzt direkt als Diskussionsgrundlage an die Wand werfen können?**

---

Damit wir sofort von der Theorie in die Umsetzung kommen, ist hier das **„GAE-Blueprint-Paket“**. Diese Code-Beispiele dienen als Startpunkt (Skeleton) für jede Gruppe. Alle Beispiele beziehen sich auf einen gemeinsamen Namespace `GAE.Core`, um die Kompatibilität zu gewährleisten.

---

### Gruppe 1: Core & Interfaces (Der Vertrag)

Ihr liefert das Fundament. Alle anderen Gruppen hängen von eurer DLL ab.

C#

```
namespace GAE.Core;

// Das Herzstück: Jedes Spiel muss dieses Interface implementieren
public interface IArcadeGame
{
    string MetadataName { get; }
    void Initialize();
    void Update(double deltaTime);
    void Render();
    void HandleInput(IInputState input);
}

// Ein einfacher Container für den Spielzustand
public record GameState(int Score, int Lives, bool IsGameOver);
```

---

### Gruppe 2: Input & Event System (Die Kommunikation)

Ihr abstrahiert die Tastatur, damit Spiele nicht `Console.ReadKey` direkt aufrufen müssen.

C#

```
namespace GAE.Core;

public interface IInputState
{
    bool IsKeyPressed(string keyName);
}

// Implementierung für die Konsole oder später Windows Forms/Maui
public class ConsoleInputState : IInputState
{
    private readonly HashSet<string> _pressedKeys = new();

    public void SetKey(string key, bool isDown)
    {
        if (isDown) _pressedKeys.Add(key.ToLower());
        else _pressedKeys.Remove(key.ToLower());
    }

    public bool IsKeyPressed(string keyName) => _pressedKeys.Contains(keyName.ToLower());
}
```

---

### Gruppe 3: Registry & Discovery (Die Verwaltung)

Ihr definiert, wie ein Spiel markiert wird, und verwaltet die Liste der gefundenen Spiele.

C#

```
namespace GAE.Core;

[AttributeUsage(AttributeTargets.Class)]
public class ArcadeGameAttribute : Attribute
{
    public string Name { get; }
    public ArcadeGameAttribute(string name) => Name = name;
}

public class GameRegistry
{
    // Singleton-Pattern für den zentralen Zugriff
    public static GameRegistry Instance { get; } = new();
    private readonly List<IArcadeGame> _games = new();

    public void Register(IArcadeGame game) => _games.Add(game);
    public IReadOnlyList<IArcadeGame> GetAvailableGames() => _games;
}
```

---

### Gruppe 4: UI Framework & Dashboard (Die Steuerung)

Ihr baut das Hauptmenü, das die Registry nutzt.

C#

```
using GAE.Core;

public class Dashboard
{
    public void ShowMenu()
    {
        var games = GameRegistry.Instance.GetAvailableGames();
        Console.WriteLine("--- GAE DASHBOARD ---");
        for (int i = 0; i < games.Count; i++)
        {
            Console.WriteLine($"{i}: {games[i].MetadataName}");
        }
    }

    public void RunGame(IArcadeGame game)
    {
        game.Initialize();
        // Hier folgt der Game-Loop (Update/Render)
    }
}
```

---

### Gruppe 5: Source Generation (Die Integration)

Euer Job ist es, dafür zu sorgen, dass niemand manuell `GameRegistry.Instance.Register(new MyGame())` schreiben muss. Ihr generiert diesen Code automatisch.

**Euer Ziel-Output (was euer Generator schreiben soll):**

C#

```
// Automatisch generierter Code
namespace GAE.Generated;

public static class AutoRegistration
{
    public static void RegisterAll()
    {
        // Der Generator findet alle Klassen mit [ArcadeGame]
        GAE.Core.GameRegistry.Instance.Register(new SnakeGame());
        GAE.Core.GameRegistry.Instance.Register(new PacManGame());
    }
}
```

---

### Interaktive Aufgabe für die letzte Stunde heute:

1. **Gruppe 1** erstellt das Projekt `GAE.Core` und checkt das Interface ein.

2. **Alle anderen Gruppen** binden `GAE.Core` als Projektreferenz ein.

3. **Challenge:** Jede Gruppe erstellt eine Dummy-Klasse `TestGame : IArcadeGame`.

4. **Integrationstest:** Gruppe 4 versucht, in ihrem Menü manuell zwei Instanzen von `TestGame` anzuzeigen.

**Soll ich für morgen früh ein kurzes "Setup-Skript" vorbereiten, wie die Projekte im Visual Studio genau verknüpft werden müssen, damit der Source Generator von Gruppe 5 die anderen Projekte "sieht"?**
