### Programmieren mit C# (DSPC016)

---

## Gruppe 1: Memory Safety & Low-Level Performance

**Der Fokus:** Das Überwinden des "Managed-Overheads" ohne die Sicherheit von C# aufzugeben.

### Die C#-Besonderheit: `Span<T>` und `Memory<T>`

Normalerweise erzeugt jede Operation auf einem Array oder String (wie `Substring`) ein neues Objekt auf dem Heap. Das
kostet CPU-Zeit für die Allokation und später für die Garbage Collection.

* **Was ist `Span<T>`?** Es ist eine "View" (Sicht) auf einen zusammenhängenden Speicherbereich. Es kann auf den Stack (
  `stackalloc`), den Heap (Array) oder nativen Speicher (Pointer) zeigen.
* **Der Clou:** Ein `Span` kopiert keine Daten. Wenn Sie einen Teilbereich (Slice) nehmen, zeigen Sie einfach nur auf
  einen anderen Startpunkt mit einer anderen Länge. Da es ein `ref struct` ist, lebt es nur auf dem Stack und belastet
  den GC gar nicht.

### Code-Beispiel: High-Performance Highscore-Parser

Stellen Sie sich vor, ein Spiel sendet hunderte Highscore-Daten als String: `"Player:Score;Player:Score"`.

```csharp
// Klassisch: Erzeugt viele Strings durch Split() -> GC Pressure
string[] parts = data.Split(';');

// Modern mit Span: Null Allokationen
public void ProcessScores(ReadOnlySpan<char> data)
{
    int start = 0;
    while (start < data.Length)
    {
        int separatorIndex = data[start..].IndexOf(';');
        int length = (separatorIndex == -1) ? data.Length - start : separatorIndex;

        // Slice erzeugt KEINE Kopie, nur ein neues "Fenster" auf den Speicher
        ReadOnlySpan<char> entry = data.Slice(start, length);

        // Weiterverarbeitung...
        start += length + 1;
    }
}

```

---
