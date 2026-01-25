## 5. Master-Lösung zu Übung 4

Hier ist die Lösung:

```csharp
string input = "ID:42;TYPE:PLAYER;POS:10,20";
ReadOnlySpan<char> span = input.AsSpan();

// 1. ID extrahieren
int startId = span.IndexOf(':') + 1;
int endId = span.IndexOf(';');
ReadOnlySpan<char> idSpan = span.Slice(startId, endId - startId);
int id = int.Parse(idSpan);

// 2. Type extrahieren
ReadOnlySpan<char> remaining = span.Slice(endId + 1);
int startType = remaining.IndexOf(':') + 1;
int endType = remaining.IndexOf(';');
ReadOnlySpan<char> type = remaining.Slice(startType, endType - startType);

Console.WriteLine($"Parsed ID: {id}, Type: {type.ToString()}");

```

---
