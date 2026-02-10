# LINQ & Declarative Game Engines - Handout

**Präsentation: Deklarative Engine - LINQ & Queryable Game Rules**  

---

## 1. Deklarativ > Imperativ

Der fundamentale Unterschied zwischen imperativer und deklarativer Programmierung ist der Schlüssel zum Verständnis von LINQ. Imperative Programmierung beschreibt **wie** etwas gemacht werden soll - Schritt für Schritt. Der Entwickler gibt explizite Anweisungen: Durchlaufe diese Liste, prüfe jedes Element, füge Ergebnisse zu einer neuen Liste hinzu, sortiere die Liste. Deklarative Programmierung hingegen beschreibt **was** erreicht werden soll. Man spezifiziert das gewünschte Ergebnis und überlässt dem System die Entscheidung über die beste Ausführungsstrategie.

Ein praktisches Beispiel verdeutlicht den Unterschied:

```csharp
// IMPERATIV - Wie finde ich starke Feinde?
List<Enemy> strongEnemies = new List<Enemy>();
foreach (var enemy in allEnemies) {
    if (enemy.Health > 100 && enemy.Level >= player.Level) {
        strongEnemies.Add(enemy);
    }
}
// Manuelles Sortieren, Limitieren...

// DEKLARATIV - Was will ich finden?
var strongEnemies = allEnemies
    .Where(e => e.Health > 100)
    .Where(e => e.Level >= player.Level)
    .OrderByDescending(e => e.Health)
    .Take(5);
```

Der deklarative Code liest sich wie eine natürliche Beschreibung des Problems. Game Designer können die Logik verstehen, ohne Code-Experten zu sein. Dies führt zu besserer Kommunikation im Team, schnelleren Iterationen und weniger Bugs, da die Intention klar ausgedrückt wird. Der Code ist zudem testbar und wartbar, da jede Operation isoliert betrachtet werden kann.

## 2. LINQ Grundlagen

LINQ (Language Integrated Query) wurde 2007 mit C# 3.0 eingeführt und revolutionierte die Datenverarbeitung in C#. Vor LINQ existierte keine einheitliche Syntax - Arrays benötigten for-loops, Datenbanken SQL-Strings, XML-Dokumente XPath. Erik Meijer und sein Team bei Microsoft Research schufen mit LINQ eine typsichere, in die Sprache integrierte Query-Syntax.

LINQ existiert in zwei Varianten mit unterschiedlichen Anwendungsfällen:

**LINQ to Objects (IEnumerable<T>):**
- Arbeitet mit In-Memory Collections (List, Array, HashSet)
- Sofortige Ausführung im Speicher
- Nutzt keine Expression Trees
- Ideal für lokale Daten

```csharp
List<Player> players = GetPlayers();
var topPlayers = players.Where(p => p.Score > 1000);
// Wird sofort im Speicher ausgeführt
```

**LINQ to SQL (IQueryable<T>):**
- Arbeitet mit externen Datenquellen (Datenbanken, APIs)
- Verzögerte Ausführung (Deferred Execution)
- Nutzt Expression Trees zur Analyse
- Kann zu SQL oder anderen Sprachen übersetzt werden

```csharp
IQueryable<Player> dbPlayers = database.Players;
var topPlayers = dbPlayers.Where(p => p.Score > 1000);
// Noch NICHT ausgeführt! Wird zu SQL übersetzt bei ToList()
```

Der entscheidende Unterschied liegt darin, dass IQueryable-Queries analysiert und optimiert werden können, bevor sie ausgeführt werden. Dies ermöglicht LINQ Providern, intelligente Entscheidungen zu treffen - beispielsweise Index-basierte Lookups statt Full-Table-Scans.

## 3. Expression Trees - Das Fundament

Expression Trees sind das technische Fundament, das LINQ seine Mächtigkeit verleiht. Normalerweise wird Code vom Compiler direkt in IL (Intermediate Language) übersetzt und ist danach nicht mehr analysierbar - eine "Black Box". Expression Trees hingegen repräsentieren Code als hierarchische Datenstruktur, einen Abstract Syntax Tree (AST), bei dem jeder Knoten einen Teil des Ausdrucks repräsentiert.

Ein einfaches Beispiel verdeutlicht das Konzept:

```csharp
// Normale Lambda - direkt kompiliert
Func<int, bool> func = x => x > 10;

// Expression Tree - Code als Datenstruktur!
Expression<Func<int, bool>> expr = x => x > 10;

// Die Struktur kann analysiert werden:
//       Lambda
//          |
//    BinaryExpression (>)
//       /         \
//  Parameter(x)  Constant(10)
```

Diese Repräsentation ermöglicht völlig neue Möglichkeiten. Der Code kann zur Laufzeit analysiert werden, LINQ Provider können die Struktur durchlaufen und in andere Sprachen übersetzen (z.B. SQL), und Optimierungen können vor der Ausführung angewendet werden. Ein LINQ Provider für SQL durchläuft den Expression Tree mit einem Visitor Pattern und generiert daraus SQL-Statements.

Die theoretische Grundlage findet sich im Lambda-Kalkül von Alonzo Church (1930er Jahre) und in Haskells "Quoted Expressions". Die Forschungsarbeit "Lost in Translation" von Bierman, Meijer und Torgersen (2007) beweist mathematisch die Korrektheit dieser Query-Übersetzung.

## 4. LINQ Provider - Der Übersetzer

Ein LINQ Provider ist der "Dolmetscher" zwischen deklarativem C#-Code und der tatsächlichen Datenquelle. Er implementiert das IQueryProvider-Interface und durchläuft mehrere Phasen:

**Die Provider-Pipeline:**

1. **Parse:** Expression Tree wird rekursiv durchlaufen und analysiert
2. **Validate:** Prüfung ob Query übersetzbar ist (nicht jeder C#-Code kann zu SQL werden)
3. **Optimize:** Index-basierte Lookups statt Full-Scans, Query-Rewriting
4. **Translate:** Generierung von SQL, MongoDB Query Language, oder Custom Code
5. **Execute:** Ausführung auf der Datenquelle
6. **Map:** Ergebnisse zurück zu C#-Objekten (ORM)

Ein konkretes Beispiel zeigt die Übersetzung:

```csharp
// C# LINQ Query
var query = dbContext.Players
    .Where(p => p.Score > 1000)
    .OrderBy(p => p.Name);

// Provider analysiert Expression Tree und generiert:
// SELECT * FROM Players 
// WHERE Score > 1000 
// ORDER BY Name
```

Microsoft liefert mehrere Standard-Provider:
- **LINQ to SQL** für SQL Server (T-SQL)
- **Entity Framework Core** für verschiedene Datenbanken
- **MongoDB.Driver** für NoSQL
- **Cosmos DB Provider** für Cloud-Datenbanken

Die Forschungsarbeit "Building an IQueryable Provider" von Matt Warren (2007) ist die definitive Anleitung für Custom Provider-Implementierungen.

## 5. Performance & Trade-offs

Die Performance-Eigenschaften von LINQ sind entscheidend für die richtige Anwendung. Microsoft Research Benchmarks zeigen, dass LINQ to Objects typischerweise 5-15% Overhead im Vergleich zu for-loops hat. Der Grund liegt in der Abstraktionsschicht: LINQ nutzt Iteratoren (IEnumerator), die virtuelle Methoden-Aufrufe erfordern und Enumerator-Objekte allokieren.

Bei 60 FPS mit 16.6ms Budget pro Frame bedeutet LINQ-Overhead in Update-Loops signifikanten Performance-Verlust. Eine foreach-Schleife erstellt bei jeder Iteration über eine List ein Enumerator-Objekt (32 Bytes), das vom Garbage Collector verwaltet werden muss - bei 60 FPS summiert sich dies schnell.

**Wann LINQ verwenden:**
- Menu-Systeme und UI-Logic
- Turn-based Spiellogik
- Highscores, Rankings, Leaderboards
- Save/Load-Operationen
- Rule Engines und Business Logic
- Initialisierungs-Code
- Seltene Queries (<1x pro Sekunde)

**Wann NICHT LINQ verwenden:**
- Update() / FixedUpdate() Loops
- Collision Detection
- Performance-kritische Pfade
- Große Collections in Echtzeit (>10k Items)

## 6. Game-Hub Projekt - Queryable Game Rules

Das Game-Hub Projekt demonstriert die praktische Anwendung von LINQ für Spielregeln. Die Vision ist eine Rule Engine, bei der Game Designer Spielregeln als deklarative Queries schreiben können. Eine Regel wie "Top 10 Spieler im Ranked Mode" wird wörtlich als LINQ-Query formuliert: Filter nach Mode, sortiere nach Score, nimm 10.

**Highscore System mit LINQ:**

```csharp
public class HighscoreRules
{
    private IQueryable<ScoreEntry> _scores;
    
    // Leaderboard Query - selbsterklärend
    public IEnumerable<ScoreEntry> GetTop10(string mode)
    {
        return _scores
            .Where(s => s.GameMode == mode)
            .OrderByDescending(s => s.Score)
            .Take(10);
    }
    
    // Ranking Berechnung
    public int GetPlayerRank(string playerId, string mode)
    {
        var playerScore = _scores
            .Where(s => s.PlayerId == playerId && s.GameMode == mode)
            .Max(s => s.Score);
        
        return _scores
            .Where(s => s.GameMode == mode)
            .Count(s => s.Score > playerScore) + 1;
    }
    
    // Achievement Check mit Gruppierung
    public bool HasFirstPlace(string playerId)
    {
        return _scores
            .GroupBy(s => s.GameMode)
            .Any(g => g.OrderByDescending(s => s.Score)
                     .First().PlayerId == playerId);
    }
}
```

Ranking-Berechnungen werden mit effizienten Algorithmen durchgeführt, die die sortierte Natur der Daten ausnutzen. Das System ist modular aufgebaut mit mehreren Schichten:

- **Designer Interface:** DSL oder visueller Editor für Query-Erstellung
- **LINQ Provider Layer:** Analyse, Validierung und Optimierung der Queries
- **Game System Layer:** IQueryable-Implementierung für Highscores, Player Stats, Matchmaking
- **Data Layer:** Effiziente Strukturen mit Indizes für häufige Abfragen

---

## Zusammenfassung

LINQ und Expression Trees repräsentieren einen Paradigmenwechsel von imperativen Schleifen zu deklarativen Queries. Expression Trees ermöglichen Code-Analyse und Translation, LINQ Provider übersetzen zwischen High-Level C# und effizienter Ausführung. Für Game Development bedeutet dies Designer-freundlichere Environments und schnellere Iteration, aber Performance-Trade-offs müssen beachtet werden. Das Hybrid-Modell nutzt LINQ für Business Logic und for-loops für Update-Loops. Mit Custom Providern wie im Game-Hub Projekt können Spielregeln als lesbare Queries formuliert werden, die automatisch optimiert werden.

---

### Literatur & Ressourcen

- [1] Meijer, E., Beckman, B., & Bierman, G. (2006). The LINQ Project. Microsoft .NET Language Integrated Query. Microsoft Research PDF. https://cs.brown.edu/courses/cs295-11/2006/LINQ.pdf  
- [2] Cheney, J., Lindley, S., Radanne, G., & Wadler, P. (2013). Effective Quotation: Relating Approaches to Language-Integrated Query. arXiv. https://arxiv.org/abs/1310.4780  
- [3] López-González, J., & Serrano, J. M. (2020). The Optics of Language-Integrated Query. Science of Computer Programming. https://doi.org/10.1016/j.scico.2020.102395  
- [4] Ricciotti, W., & Cheney, J. (2020). Strongly-Normalizing Higher-Order Relational Queries. arXiv. https://arxiv.org/abs/2011.13451  
- [5] Ricciotti, W., & Cheney, J. (2021). Query Lifting: Language-Integrated Query for Heterogeneous Nested Collections. arXiv. https://arxiv.org/abs/2101.04102  
- [6] Parry, M. (n.d.). Language Integrated Query (LINQ). Microsoft Developer Documentation PDF. https://download.microsoft.com/documents/uk/msdn/events/LanguageIntegratedQueryLINQ.pdf
- [7] dotnetbenchmarks.com. A Comparison of C# Loop Constructs. https://dotnetbenchmarks.com/benchmark/1061 

