## 3. Übung

**Aufgabe: "The Factory Generator" (60 Min)**
Schreiben Sie einen Generator, der nicht nur den Namen ausgibt, sondern eine echte **Factory-Klasse
** erstellt.

* Wenn eine Klasse `[ArcadeGame]` hat, soll eine Methode `public IArcadeGame Create[Name]()` generiert werden.

### 💡 Musterlösung

```csharp
// Generierter Code sollte so aussehen:
public class GameFactory {
    public IArcadeGame CreateSnake() => new Snake();
    public IArcadeGame CreatePacMan() => new PacMan();
}

```

**Wichtiger Hinweis für die Lösung:** Die Studierenden müssen darauf achten, dass die Zielklassen einen parameterlosen
Konstruktor haben – ein schönes Thema für die Diskussion über "Constraints".

---
