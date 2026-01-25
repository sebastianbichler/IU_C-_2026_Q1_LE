# Musterlösungen

### Zu 1.1 (Boxing)

```csharp
// ArrayList speichert 'object' -> int (Wertetyp) muss in ein Objekt-Gehäuse verpackt werden.
// List<int> nutzt 'Reified Generics' -> direkter int-Array im Speicher.
ArrayList list1 = new ArrayList(); // Langsam, GC-Druck
List<int> list2 = new List<int>(); // Schnell, effizient

```

### Zu 2.1 (Speicher)

* **Lösung:** Bei `List<struct>` liegen die Daten flach hintereinander im RAM. Die CPU kann den nächsten Wert
  vorhersagen. Bei `List<class>` liegen nur Zeiger (8 Byte) hintereinander, die tatsächlichen Daten liegen verstreut auf
  dem Heap -> "Cache Miss".

### Zu 3.1 (LINQ)

```csharp
var topGames = sessions
    .Where(s => s.IsActive)
    .OrderByDescending(s => s.PlayerCount)
    .Take(3)
    .Select(s => s.GameName)
    .ToList(); // Hier passiert die tatsächliche Ausführung

```
