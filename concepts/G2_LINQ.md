### Programmieren mit C# (DSPC016)

---

## Gruppe 2: Deklarative Programmierung & LINQ-Provider

**Der Fokus:** Logik als Datenstruktur behandeln, um sie analysierbar und transformierbar zu machen.

### Die C#-Besonderheit: Expression Trees

Ein normaler Lambda-Ausdruck `Func<int, bool> f = x => x > 5;` ist fertig kompilierter Code (IL). Der Computer kann ihn
ausführen, aber das Programm kann nicht "hineinschauen", um zu verstehen, was er tut.

* **Was ist ein Expression Tree?** Wenn Sie `Expression<Func<T, bool>>` schreiben, erzeugt der Compiler keinen Code,
  sondern einen **Abstract Syntax Tree (AST)**.
* **Warum ist das mächtig?** Sie können diesen Baum zur Laufzeit durchlaufen (mittels `ExpressionVisitor`), ihn
  verändern (z.B. eine zusätzliche Sicherheitsprüfung einbauen) oder ihn in eine andere Sprache (wie SQL oder ein
  Spiele-Skript) übersetzen.

### Code-Beispiel: Dynamische Achievement-Regeln

Ein Dashboard möchte prüfen, ob ein Spieler ein Achievement erhält, ohne dass das Dashboard den Code des Spiels kennt.

```csharp
// Die Regel wird als DATENSTRUKTUR definiert
Expression<Func<Player, bool>> achievementRule = p => p.Gold > 100 && p.Level > 10;

// Wir können den Baum analysieren
if (achievementRule.Body is BinaryExpression binary)
{
    Console.WriteLine($"Die Regel nutzt den Operator: {binary.NodeType}"); // Gibt "AndAlso" aus
}

// Und am Ende für die Ausführung kompilieren
var checkFunc = achievementRule.Compile();
bool hasAchievement = checkFunc(currentPlayer);

```

---
