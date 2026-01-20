### Programmieren mit C# (DSPC016)

---

## Gruppe 4: DDD & Persistence

**Der Fokus:** Domänen-Logik vor technischer Komplexität (wie Datenbanken) schützen.

### Die C#-Besonderheit: Records und Value Objects

In komplexen Systemen ist "State Mutation" (das unvorhergesehene Ändern von Daten) die größte Fehlerquelle.

* **Records:** Mit C# Records führen wir echte **Immutability** ein. Ein Highscore-Objekt kann nicht mehr verändert
  werden; man erzeugt ein neues mit `with`.
* **Value Objects:** Im DDD sind Objekte wie "Währung" oder "Spieler-ID" keine einfachen Zahlen, sondern Typen. Records
  machen dies einfach, da sie standardmäßig "Value Equality" besitzen (zwei Records sind gleich, wenn ihre Inhalte
  gleich sind, nicht wenn sie an derselben Stelle im Speicher liegen).

### Code-Beispiel: Robustes Savegame-Modell

```csharp
// Unveränderlicher Datentyp (Value Object)
public record GameState(int Level, int Health, ImmutableList<string> Inventory);

public class PlayerService
{
    private GameState _currentState = new(1, 100, ImmutableList<string>.Empty);

    public void TakeDamage(int amount)
    {
        // Erzeugt einen neuen Zustand basierend auf dem alten
        _currentState = _currentState with { Health = _currentState.Health - amount };
        // Vorteil: Absolut Thread-sicher und keine Seiteneffekte in der Datenbank-Schicht!
    }
}

```

---

