### Programmieren mit C# (DSPC016)

---

Für Gruppe 5 ist das Thema **„Reflective Programming & Pattern Matching“** (Source Generators).

In einem Game-Hub ist Reflection unverzichtbar, um zur Laufzeit (Runtime) Erweiterungen zu finden, die erst nach der
Kompilierung des Hubs hinzugefügt wurden.

---

### 1. Theoretisches Konzept

Gruppe 5 fungiert als **„Detektiv“** des Systems.

- **Introspektion:** Das System untersucht sich selbst: „Welche Klassen sind geladen? Welche davon haben das Attribut
  `[ArcadeGame]`?“

- **Pattern Matching:** Sobald ein Objekt gefunden wurde, nutzt die Gruppe modernes C# Pattern Matching, um sicher und
  elegant mit den Typen umzugehen (z. B. um zwischen verschiedenen Spielmodi oder Schwierigkeitsgraden zu
  unterscheiden).

---

### 2. Praktische Umsetzung: Der Plugin-Loader

Die Gruppe baut den Mechanismus, der eine DLL scannt und die darin enthaltenen Spiele instanziiert.

**C#-Beispiel für Gruppe 5 (The Reflection Engine):**

C#

```
using System.Reflection;
using GAE.Core;

namespace GAE.Services;

public class PluginLoader
{
    public void LoadGamesFromDll(string dllPath)
    {
        // Lädt die Assembly (DLL) zur Laufzeit
        Assembly assembly = Assembly.LoadFrom(dllPath);

        // Suche alle Typen, die IArcadeGame implementieren
        var gameTypes = assembly.GetTypes()
            .Where(t => typeof(IArcadeGame).IsAssignableFrom(t) && !t.IsInterface);

        foreach (var type in gameTypes)
        {
            // Nutze Reflection für die Instanziierung
            if (Activator.CreateInstance(type) is IArcadeGame game)
            {
                // Pattern Matching zur Typprüfung und Registrierung
                RegisterGame(game);
            }
        }
    }

    private void RegisterGame(IArcadeGame game)
    {
        // Nutze Pattern Matching (C# 9.0+) für Metadaten-Checks
        string category = game switch
        {
            { MetadataName: var name } when name.Contains("Retro") => "Classic",
            _ => "Modern"
        };

        Console.WriteLine($"[Registry] Spiel geladen: {game.MetadataName} (Kategorie: {category})");
        GameRegistry.Instance.Register(game);
    }
}
```

---

### 3. Die „Wissenschaftliche“ Komponente: Performance & Type Safety

Gruppe 5 soll in ihren Artikeln prüfen: **„Reflection vs. Static Binding“**.

- **Problem:** Reflection ist langsamer.

- **Lösungsidee:** Sie implementieren einen Cache. Einmal gefundene Typen werden in einem `Dictionary<string, Type>`
  gespeichert, damit nicht bei jedem Menüaufruf die gesamte DLL gescannt werden muss.

---

### 4. Hilfestellung

1. **Assembly-Scan:** Erstellt eine kleine Konsolen-App, die den eigenen `bin`-Ordner nach Klassen scannt, die
   `IArcadeGame` implementieren.

2. **Attribut-Check:** Implementiert eine Logik, die prüft, ob das `[ArcadeGame]`-Attribut vorhanden ist, und liest den
   Namen aus.

3. **Pattern Matching:** Schreibt eine Methode, die ein Spiel-Objekt entgegennimmt und je nach Typ (z. B. ob es ein
   zusätzliches Interface `IMultiplayer` implementiert) unterschiedliche Start-Optionen zurückgibt.

---

