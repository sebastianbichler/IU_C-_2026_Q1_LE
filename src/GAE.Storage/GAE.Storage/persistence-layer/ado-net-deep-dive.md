# ADO.NET: Der hardwarenahe Datenzugriff

ADO.NET ist die Kern-Bibliothek für den Datenzugriff in .NET. In unserem Projekt dient es als Referenz für maximale Performance, zeigt aber gleichzeitig die architektonischen Herausforderungen auf.

## Implementierungs-Analyse
Beim manuellen Mapping müssen wir die Domänen-Struktur "aufbrechen" (**Flattening**), um sie in flache SQL-Tabellen zu pressen.

### Herausforderungen:
1. **Bruch der Kapselung:** Wir müssen auf private Details des Inventars zugreifen, um SQL-Parameter zu befüllen.
2. **SQL-Abhängigkeit:** Die Strings sind fest an den SQLite-Dialekt gebunden.
3. **Manueller Overhead:** Transaktionen und das Löschen/Neu-Einfügen von Relationen müssen händisch gesteuert werden.

## Code-Beispiel (The Hard Way)
```csharp
// Beispiel für manuelles Mapping eines komplexen Profils
public async Task SavePlayerHardWayAsync(PlayerProfile player, string connStr)
{
    using var conn = new SqliteConnection(connStr);
    await conn.OpenAsync();
    using var transaction = conn.BeginTransaction();
    
    // 1. Root-Update
    // 2. Altes Inventar löschen (Relationaler Zwang)
    // 3. Schleife über Items für manuelle Inserts
    // ...
}
