# Locks in C# und System.Threading.Monitor

## Einführung in Locks in C#

**Locks** sind eine zentrale Synchronisationsmethode zur Steuerung des Zugriffs auf Ressourcen in mehrfädigen Umgebungen. Durch den Einsatz von Locks wird sichergestellt, dass Race Conditions vermieden werden, wenn mehrere Threads auf dieselbe Ressource zugreifen.

---

## Was sind Locks?

Ein Lock in C# wird durch das **`lock`-Schlüsselwort** implementiert. Die Syntax sieht folgendermaßen aus:

```cs
private readonly object lockObject = new();
lock (lockObject)
{
    // Zugriff auf geschützte Ressourcen
}

// wird vom kompiler in folgendes übersetzt

object __lockObj = lockObject;
bool __lockWasTaken = false;
try
{
    System.Threading.Monitor.Enter(__lockObj, ref __lockWasTaken);
    // Zugriff auf geschützte Ressourcen
}
finally
{
    if (__lockWasTaken) System.Threading.Monitor.Exit(__lockObj);
}
```

---

## Funktionsweise von Locks

1. **Lock Acquisition (Erwerb des Locks)**: 
   - Ein Thread fordert ein Lock an. Wenn das Lock von einem anderen Thread gehalten wird, wird der anfordernde Thread blockiert.

2. **Lock Holding (Halten des Locks)**:
   - Der Thread hat exklusiven Zugriff auf die geschützten Ressourcen.

3. **Lock Release (Freigabe des Locks)**:
   - Der Lock wird beim Verlassen des Codeblocks automatisch freigegeben.
---

## Interne Funktionsweise von Locks

### Objekt Header

- Jedes verwaltete Objekt hat einen Header
- umfangreiche Lock‑Metadaten liegen außerhalb in einer SyncBlock‑Struktur, auf die der Header bei Bedarf verweist

### Thin Locks

- CLR versucht zunächst das Lock im Header, per Compare And Swap Operation (atomar) zu setzen => _sehr schnellen, kernel‑losen Zugriff_

### Fat Locks

- Thin Locks versagen bei Kontention oder speziellen Operationen wie Monitor.Wait, Thread‑Abbruch oder wenn Header‑Platz nicht reicht => _`GetHashCode()` Aufruf_
- In diesen Fällen erweitert die CLR das Lock auf einen Sync Block in einer zentralen SyncBlock‑Tabelle => **Fat Lock**
- Enthält den Owner (zugreifender Thread), eine Warteliste für blockierte Threads, Rekursionstiefe und ggf. Wait‑Handles für Kernel‑Blocking

|                      Method |        Mean |    StdDev |
|---------------------------- |------------ |---------- |
|                NodeWithLock | 152.2947 ms | 1.4895 ms |
|              NodeWithNoLock | 149.5015 ms | 2.7289 ms |
|  NodeWithLockAndGetHashCode | 541.6314 ms | 4.0445 ms |

[Quelle Tabelle](https://devblogs.microsoft.com/premier-developer/managed-object-internals-part-2-object-header-layout-and-the-cost-of-locking/)

---

## Beispiel Spiel Implementierung

[LockBasedGame](./GAE.Async/LockBasedGame.cs)

---

## Die neue Lock-Klasse in .NET 9

Mit **.NET 9** und C# 13 wurde eine neue `Lock`-Klasse eingeführt, die die Verwendung herkömmlicher Lock-Objekte verbessert.

### Unterschiede zur traditionellen Lock-Implementierung

- **Standardisierung**: Anstelle von `object` für das Locking bietet `Lock` eine dedizierte Struktur.
- **Bessere Lesbarkeit**: Erhöht die Lesbarkeit des Codes, da die Absicht klarer wird.

### Beispiel für die Verwendung der neuen Lock-Klasse

Die Syntax sieht folgendermaßen aus:

```cs
private static Lock myLock = new();

lock (_lockObj)
{
    // Zugriff auf geschützte Ressourcen
}

using (_lockObj.EnterScope())
{
    // Zugriff auf geschützte Ressourcen
}

_lockObj.Enter();
try
{
    // Zugriff auf geschützte Ressourcen
}
finally { _lockObj.Exit(); }

if (_lockObj.TryEnter())
{
    try
    {
        // Zugriff auf geschützte Ressourcen
    }
    finally { _lockObj.Exit(); }
}
```

Diese Klasse vereinfacht das Locking, indem sie den `Monitor` unter dem Deckmantel abstrahiert und gleichzeitig eine sichere Verwendung des `using`-Patterns bietet.

---

## Vor- und Nachteile von Locks

| **Vorteile**                              | **Nachteile**                             |
|-------------------------------------------|-------------------------------------------|
| **Einfachheit**: Leicht zu implementieren. | **Wartezeit**: Threads können blockiert werden. |
| **Sicherheit**: Verhindert Race Conditions. | **Leistungsprobleme**: Bei hoher Konkurrenz können Deadlocks auftreten. |
| **Klarheit**: Der Code ist verständlich. | **Komplexität**: Erfordert sorgfältiges Design. |
| | **Async/Await**: Unterstützt keine Asynchronen Aufrufe (Semaphore erlauben dies) |

## Fazit

Die **System.Threading.Monitor**-Klasse ist wesentlicher Bestandteil der Multithread-Programmierung in C#. Die Einführung der neuen `Lock`-Klasse in .NET 9 verbessert die Synchronisation weiter, indem sie eine standardisierte, lesbare und effiziente Möglichkeit zur Verwaltung von Locks bietet. 

## Quellen

| **Titel** | **URL** |
|-----------|---------|
| **The lock statement - ensure exclusive access to a shared resource** | [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock) |
| **Lock keyword gets an upgrade in .NET9** | [Medium Artikel](https://mareks-082.medium.com/new-lock-object-and-history-d69877f46521) |
| **Lock Class** | [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/api/system.threading.lock?view=net-11.0) |
| **What's new in C# 13** | [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13) |
| **Better Locking with System.Threading.Lock** | [Medium Artikel](https://dogukanuhn.medium.com/better-locking-with-system-threading-lock-421a378ed3fe) |
| **Managed object internals, Part 2. Object header layout and the cost of locking** | [Microsoft Devblog](https://devblogs.microsoft.com/premier-developer/managed-object-internals-part-2-object-header-layout-and-the-cost-of-locking/) |
| **monitor.c** | [monitor.c](https://github.com/dotnet/runtime/blob/main/src/mono/mono/metadata/monitor.c) |
