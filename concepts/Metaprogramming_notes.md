# Musterlösungen & Notizen

## Lösung zu Übung 1: Plugin-Explorer

Dies ist der "Klassiker" für Plugin-Systeme.

```csharp
public void ListGames(string dllPath) {
    // Assembly laden
    Assembly asm = Assembly.LoadFrom(dllPath);

    // Alle Typen filtern, die das Interface implementieren
    var gameTypes = asm.GetTypes()
        .Where(t => typeof(IArcadeGame).IsAssignableFrom(t) && !t.IsInterface);

    foreach (var type in gameTypes) {
        Console.WriteLine($"Gefundenes Spiel: {type.Name}");

        // Bonus: Instanziieren & Attribut lesen
        var info = type.GetCustomAttribute<GameInfoAttribute>();
        if (info != null) {
            Console.WriteLine($" -> Name: {info.Name}, Autor: {info.Author}");
        }
    }
}

```
