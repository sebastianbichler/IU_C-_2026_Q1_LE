Für Gruppe 3 ist das Thema **„Reflective Programming & Pattern Matching“** wie geschaffen. Während Gruppe 5 den statischen Weg (Source Generators) geht, übernimmt Gruppe 3 den **dynamischen Part**.

In einem Game-Hub ist Reflection unverzichtbar, um zur Laufzeit (Runtime) Erweiterungen zu finden, die erst nach der Kompilierung des Hubs hinzugefügt wurden.

---

### 1. Theoretisches Konzept (10 Min)

Gruppe 3 fungiert als **„Detektiv“** des Systems.

- **Introspektion:** Das System untersucht sich selbst: „Welche Klassen sind geladen? Welche davon haben das Attribut `[ArcadeGame]`?“

- **Pattern Matching:** Sobald ein Objekt gefunden wurde, nutzt die Gruppe modernes C# Pattern Matching, um sicher und elegant mit den Typen umzugehen (z. B. um zwischen verschiedenen Spielmodi oder Schwierigkeitsgraden zu unterscheiden).

---

### 2. Praktische Umsetzung: Der Plugin-Loader (40 Min)

Die Gruppe baut den Mechanismus, der eine DLL scannt und die darin enthaltenen Spiele instanziiert.

**C#-Beispiel für Gruppe 3 (The Reflection Engine):**

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

Gruppe 3 soll in ihren Artikeln prüfen: **„Reflection vs. Static Binding“**.

- **Problem:** Reflection ist langsamer.

- **Lösungsidee:** Sie implementieren einen Cache. Einmal gefundene Typen werden in einem `Dictionary<string, Type>` gespeichert, damit nicht bei jedem Menüaufruf die gesamte DLL gescannt werden muss.

---

### 4. Aufgaben für die heutige Reststunde (Interaktiv)

1. **Assembly-Scan:** Erstellt eine kleine Konsolen-App, die den eigenen `bin`-Ordner nach Klassen scannt, die `IArcadeGame` implementieren.

2. **Attribut-Check:** Implementiert eine Logik, die prüft, ob das `[ArcadeGame]`-Attribut (von Gruppe 1 definiert) vorhanden ist, und liest den Namen aus.

3. **Pattern Matching:** Schreibt eine Methode, die ein Spiel-Objekt entgegennimmt und je nach Typ (z. B. ob es ein zusätzliches Interface `IMultiplayer` implementiert) unterschiedliche Start-Optionen zurückgibt.

---

### Verknüpfung im Game-Hub:

Gruppe 3 liefert die **Flexibilität**. Während Gruppe 5 die "fest verbauten" Spiele zur Kompilierzeit registriert, erlaubt Gruppe 3, dass man einfach eine neue DLL in einen Ordner wirft und das Spiel im Dashboard von Gruppe 4 erscheint.

**Soll ich Gruppe 3 zeigen, wie sie "Duck Typing" mittels Reflection simulieren können, um auch Spiele zu unterstützen, die das Interface vielleicht nicht perfekt implementieren?**
