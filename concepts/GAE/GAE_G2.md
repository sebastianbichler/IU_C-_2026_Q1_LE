### Programmieren mit C# (DSPC016)

---

Das Thema **„Deklarative Programmierung & LINQ-Provider“** für Gruppe 2 ist extrem spannend, da es die Art und Weise
radikal ändert, wie Spiele auf Daten (z. B. Highscores, Spielerlisten oder Level-Daten) zugreifen.

Statt imperativ zu schreiben: *„Gehe durch die Liste, prüfe ob X > 10, füge es in eine neue Liste ein“*, schreiben wir
deklarativ: *„Gib mir alle Highscores über 10“*.

---

### 1. Theoretisches Konzept

Gruppe 2 muss verstehen, dass sie nicht nur LINQ *benutzen*, sondern die **Infrastruktur** dafür bereitstellen.

- **Deklarativ:** Der Fokus liegt auf dem „Was“ (Query), nicht auf dem „Wie“ (Schleife).

- **LINQ-Provider Idee:** Sie bauen einen zentralen `HighscoreService`, der es den anderen Gruppen erlaubt, Abfragen wie
  an eine Datenbank zu stellen, selbst wenn die Daten nur in einer `List<T>` oder einer Datei liegen.

---

### 2. Praktische Umsetzung: Der Data-Hub

Gruppe 2 erstellt eine Komponente, die den Datenzugriff für den gesamten Game-Hub vereinheitlicht.

**C#-Beispiel für Gruppe 2 (Der Highscore-Provider):**

C#

```
namespace GAE.Core;

public record Highscore(string PlayerName, int Score, DateTime AchievedAt, string GameName);

public interface IHighscoreProvider
{
    // Die Basis für LINQ-Abfragen
    IQueryable<Highscore> AllScores { get; }
    void AddScore(Highscore score);
}

public class LocalHighscoreService : IHighscoreProvider
{
    private readonly List<Highscore> _scores = new();

    // Ermöglicht es anderen Gruppen, LINQ direkt auf dem Service zu nutzen
    public IQueryable<Highscore> AllScores => _scores.AsQueryable();

    public void AddScore(Highscore score) => _scores.Add(score);
}
```

---

### 3. Die „Wissenschaftliche“ Komponente: Fluent Interface

Um den deklarativen Ansatz zu vertiefen, soll Gruppe 2 eine **Fluent API** (Extension Methods) entwerfen, mit der Spiele
ihre Statistiken abfragen können.

**Beispiel für die Erweiterung:**

C#

```
public static class HighscoreExtensions
{
    // Deklarative Filterung: "Top" statt "OrderBy...Take"
    public static IEnumerable<Highscore> GetTopPlayers(this IHighscoreProvider provider, int count)
    {
        return provider.AllScores
            .OrderByDescending(s => s.Score)
            .Take(count);
    }
}
```

---

### 4. Hilfestellung

1. **Schnittstellen-Definition:** Erstellt das Projekt `GAE.Data` (oder integriert es in den Core). Definiert das
   `Highscore`-Modell.

2. **LINQ-Integration:** Implementiert den `LocalHighscoreService`. Wichtig: Erklärt den anderen Gruppen (vor allem das
   Dashboard), wie sie mit `.Where()` und `.OrderBy()` die Bestenlisten anzeigen können.

3. **Literatur-Transfer:** Gruppe 2 hat nach Artikeln gesucht. Sie sollen kurz prüfen: Wie unterscheiden sich **Internal
   LINQ** (auf Objekten) und **External LINQ** (Provider für SQL/APIs)? (Bezug: *Kotz/Wenz*, Kapitel über LINQ).

---

### Verknüpfung im Game-Hub:

Auf dem Dashboard könnte mit Hilfe des Services von Gruppe 2 eine „Globale Hall of Fame“ anzgezeigt werden. Jedes
Spiel (von Gruppe 1 definiert) liefert nach dem `Dispose()` (Memory Safety) seinen Highscore an den Provider.
