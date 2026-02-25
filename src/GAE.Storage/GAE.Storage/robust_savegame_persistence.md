# Robust and Efficient SaveGame Persistence

## Concurrency, Cancellation, and Hashing in DDD-based Systems

---

## Abstract

In modernen Computerspielen ist die Persistenz von Spielständen („SaveGames") ein zentraler Bestandteil der
Systemarchitektur. Aufbauend auf einem Domain-Driven Design (DDD)-Ansatz mit immutable Records und Value Objects sowie
einer EF Core-basierten Persistenz- und Repository-Schicht untersucht dieser Beitrag, wie CancellationTokens,
hash-basiertes Write-Caching, optimistische Concurrency-Kontrolle und Thread-Safety-Mechanismen kombiniert werden
können, um eine robuste und performante SaveGame-Verwaltung zu realisieren.

Ziel ist es, eine Persistenzschicht zu entwerfen, die:

- parallele Zugriffe korrekt behandelt
- unnötige Schreiboperationen vermeidet
- inkonsistente Zwischenzustände verhindert
- sich sauber in die bestehende DDD-Architektur integriert

---

## 1. Einführung

SaveGames sind komplexe Aggregate, bestehend aus:

- PlayerProfile
- Inventar
- Metadaten (z. B. SavedAtUtc)
- ggf. Spielweltzustand

### Anforderungen

**Konsistenz**  
Ein SaveGame darf niemals „halb" gespeichert werden.

**Performance**  
Auto-Saves oder Cloud-Sync erzeugen häufige Schreiboperationen.

**Concurrency**  
Mehrere Threads oder Prozesse können gleichzeitig speichern.

**Robustheit**  
Systemabbruch oder Spielende dürfen keine Datenkorruption erzeugen.

---

## 2. Asynchrone Persistenz mit CancellationToken

Asynchrone Programmierung (async/await) ermöglicht nicht blockierende SaveGame-Operationen. Mit einem CancellationToken
können diese Vorgänge kooperativ abgebrochen werden, wenn z. B. ein Spieler das Spiel beendet oder ein Auto-Save
unterbrochen werden soll.

Beispiel mit Timeout:

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

try
{
    await repository.SavePlayerProfileAsync(profile, cts.Token);
}
catch (OperationCanceledException)
{
    _logger.LogWarning("Save operation cancelled.");
}
```

Optional mit Transaktion:

```csharp
await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
await _dbContext.SaveChangesAsync(cancellationToken);
await transaction.CommitAsync(cancellationToken);
```

- EF Core reagiert auf das Token, unterbricht I/O und wirft eine OperationCanceledException.
- In Verbindung mit Transaktionen garantiert dies, dass die Datenbank nie in einem halbfertigen Zustand verbleibt.

---

## 3. Hash-basiertes Write-Caching

Um unnötige Schreiboperationen zu vermeiden, kann ein SaveGame mit einem Hashwert versehen werden. Vor jedem
Persistieren wird der aktuelle Hash mit dem letzten gespeicherten verglichen:

```csharp
namespace MyGame.Persistence;

public static class SaveGameHasher
{
    public static string ComputeHash(SaveGame saveGame)
    {
        var json = JsonSerializer.Serialize(saveGame);
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToHexString(bytes);
    }
}
```

Anwendung:

```csharp
var newHash = SaveGameHasher.ComputeHash(saveGame);

if (newHash == saveGame.StoredHash)
{
    return; // Skip DB write
}

saveGame = saveGame with { StoredHash = newHash };
await repository.SaveSaveGameAsync(saveGame, cancellationToken);
```

Vorteile:

- Reduziert Schreib-I/O und Locking.
- Unterstützt Delta- oder inkrementelle Saves.
- Erhöht die Performance in Multi-Save-Szenarien (z. B. Autosave + manuelles Save).

Herausforderungen:

- Hash-Berechnung ist CPU-gebunden, bei großen Aggregates kann sie merklich sein.
- Reihenfolge der serialisierten Properties muss konsistent sein, um falsche Unterschiede zu vermeiden.

---

## 4. Concurrency & Thread-Safety

Die Kombination aus Async + CancellationToken + Shared DbContext birgt Risiken:

1. DbContext ist nicht threadsafe – paralleler Zugriff führt zu Race Conditions.
2. Mutable Domain Objects können inkonsistente Zustände erzeugen, wenn mehrere Threads darauf zugreifen.

### Problem: Last Write Wins

```csharp
await Task.WhenAll(
    SavePlayerProfileAsync(profileA, cancellationToken),
    SavePlayerProfileAsync(profileB, cancellationToken)
);
```

- Zwei parallele Saves überschreiben sich potenziell.
- DbContext wirft potentiell Exceptions bei parallelem Zugriff.

### Scoped DbContext

```csharp
using var scope = _scopeFactory.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<GameDbContext>();

context.Update(profile);
await context.SaveChangesAsync(cancellationToken);
```

- Jeder Task arbeitet auf eigenem DbContext → keine Konflikte.

### Optimistic Concurrency Control

```csharp
public sealed class PlayerProfileEntity
{
    public Guid Id { get; init; }
    public int Level { get; init; }

    [Timestamp]
    public byte[] RowVersion { get; init; }
}
```

Exception Handling:

```csharp
try
{
    await _dbContext.SaveChangesAsync(cancellationToken);
}
catch (DbUpdateConcurrencyException)
{
    _logger.LogWarning("Concurrency conflict detected.");
    throw;
}
```

### Serialisierung via Semaphore

```csharp
private readonly SemaphoreSlim _semaphore = new(1, 1);

public async Task SaveAsync(SaveGame game, CancellationToken cancellationToken)
{
    await _semaphore.WaitAsync(cancellationToken);
    try
    {
        await _repository.SaveSaveGameAsync(game, cancellationToken);
    }
    finally
    {
        _semaphore.Release();
    }
}
```

---

## 5. Rolle der Immutability

- Records garantieren Always-Valid Models in der Domain.
- Zusammen mit Nullable Reference Types werden Compile-Time-Fehler für potenzielle Null-Referenzen erkannt.
- Unterstützt Thread-Sicherheit, da Daten nicht mehr nachträglich verändert werden können.
- Kombiniert mit CancellationToken und Hash-Checks → robuste, performante Persistenzschicht.

---

## 6. Gesamtsystem-Absicherung

| Ebene         | Schutzmechanismus |
|---------------|-------------------|
| Domain        | Immutable Records |
| Application   | Hash-Check        |
| Infrastruktur | Scoped DbContext  |
| Datenbank     | RowVersion (OCC)  |
| Execution     | CancellationToken |

Die Kombination aus:

1. DDD-basierter Domain (Records, Value Objects)
2. Asynchroner Persistenz mit CancellationToken
3. Hash-basiertem Write-Caching
4. Thread-sicherem Zugriff auf DbContext

ermöglicht skalierbare, sichere SaveGame-Systeme, die:

- Ressourcenschonend sind
- Keine teilweise gespeicherten Daten erzeugen
- Robust gegen parallele Zugriffe und Abbrüche sind

---

## 7. Fazit

- CancellationTokens: kontrollierter Abbruch ohne Datenverlust.
- Hash-basiertes Caching: vermeidet unnötige DB-Writes.
- Thread-Safety & Immutability: schützt Domain und Persistenz vor Race Conditions.
- Records & Nullable Types: garantieren typsichere, valide Domain-Objekte.

Diese Techniken bilden zusammen eine robuste, performante und wartbare Persistenzschicht für SaveGames im Kontext
moderner C#-Architekturen.
