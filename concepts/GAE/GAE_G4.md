Für Gruppe 4 lautet das wissenschaftliche Oberthema **"Event-Driven Architecture (EDA) & Reactive Extensions (Rx)"**. Da diese Gruppe das **Dashboard** und das **UI-Framework** baut, ist die Entkoppelung von Logik und Anzeige entscheidend.

Das Dashboard darf nicht "wissen", was im Spiel passiert, es darf nur auf **Events** reagieren (z. B. "Score hat sich geändert", "Spiel wurde beendet").

---

### 1. Theoretisches Konzept (10 Min)

Gruppe 4 wechselt von der pull-basierten Programmierung (Fragen: "Ist der Score schon höher?") zur **push-basierten Programmierung** ("Sag mir Bescheid, wenn sich der Score ändert!").

- **Observer Pattern:** Der Hub beobachtet die Spiele.

- **Reaktivität:** Das UI aktualisiert sich automatisch, sobald Daten fließen.

- **Wissenschaftlicher Bezug:** Loslösen von harten Abhängigkeiten durch Message-Queues oder Event-Streams.

---

### 2. Praktische Umsetzung: Der Event-Monitor (40 Min)

Die Gruppe baut das Dashboard, das auf die Lebenszyklus-Events der Spiele (von Gruppe 1 definiert) reagiert.

**C#-Beispiel für Gruppe 4 (The Reactive Dashboard):**

C#

```
using GAE.Core;

namespace GAE.UI;

// Definition von Event-Argumenten für das Dashboard
public class GameEventArgs : EventArgs
{
    public string Message { get; init; } = string.Empty;
}

public class DashboardController
{
    // Das Dashboard abonniert Events, statt die Spiele direkt zu steuern
    public void SubscribeToGame(IArcadeGame game)
    {
        // Hier nutzen wir ein fiktives Event 'OnStateChanged' 
        // oder reagieren auf das Beenden via IDisposable
        Console.WriteLine($"[Dashboard] Überwachung für {game.MetadataName} gestartet.");
    }

    public void RenderMenu(IEnumerable<IArcadeGame> games)
    {
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("       GAE MAIN DASHBOARD             ");
        Console.WriteLine("======================================");

        var list = games.ToList();
        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine($"[{i}] {list[i].MetadataName}");
        }
        Console.WriteLine("======================================");
        Console.Write("Wähle ein Spiel (oder 'q' zum Beenden): ");
    }
}
```

---

### 3. Die „Wissenschaftliche“ Komponente: Reactive State

Gruppe 4 soll untersuchen, wie man den Spielzustand **observer-basiert** darstellt. Statt einer einfachen Variable nutzen sie das Konzept von `Observable` (oder in C# einfacher: `Events` oder `INotifyPropertyChanged`).

**Übungsaufgabe im Code:**

Implementiert einen `ScoreMonitor`. Dieser soll jedes Mal, wenn ein Spiel einen Punkt meldet, die Konsole in einer anderen Farbe aufleuchten lassen, ohne dass das Spiel die Konsole kennen muss.

---

### 4. Aufgaben für die heutige Reststunde (Interaktiv)

1. **Main Loop:** Erstellt die Hauptschleife des Hubs. Sie muss zwischen dem "Menü-Modus" und dem "Spiel-Modus" umschalten können (**State Pattern**).

2. **Event-Handling:** Definiert ein globales Event im `GAE.Core`, auf das das Dashboard hören kann (z. B. `OnGameExited`).

3. **Visualisierung:** Schreibt eine Methode, die die Liste der von Gruppe 3 (Reflection) gefundenen Spiele sauber untereinander ausgibt.

---

### Verknüpfung im Game-Hub:

Gruppe 4 ist der **Kleber**. Sie rufen die Registry von Gruppe 3 auf, zeigen die Spiele an und starten sie. Wenn ein Spiel läuft, überwachen sie es mit den Regeln von Gruppe 1 (Memory Safety) und zeigen die Highscores von Gruppe 2 (LINQ/Data) an.

**Soll ich Gruppe 4 zeigen, wie sie mit `Task.Run` das Spiel in einem eigenen Thread startet, damit das Dashboard (UI) weiterhin auf Eingaben reagieren kann, während das Spiel läuft?**
