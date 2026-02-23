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

Locks in .NET werden nicht einfach als Feld im Objekt gespeichert. Jedes verwaltete Objekt hat einen kleinen Header — er enthält den MethodTable‑Pointer und einige Flags oder einen SyncBlock‑Index. Der Header ist sehr kompakt; umfangreiche Lock‑Metadaten liegen außerhalb in einer SyncBlock‑Struktur, auf die der Header bei Bedarf verweist.

### Thin Locks

Die CLR versucht zunächst, ein Lock direkt im Objekt‑Header zu setzen — das nennt man Thin Lock oder Fast‑Path. Das geschieht per CAS‑Operation. Gelingt das, hat der Thread sehr schnellen, kernel‑losen Zugriff. Im Header werden typischerweise der Owner‑Thread, Rekursionslevel und einige Flags kodiert. Thin Locks sind ideal für kurzlebige, uncontended Fälle.

### Sync Blocks

Thin Locks versagen bei Kontention oder speziellen Operationen wie Monitor.Wait, Thread‑Abbruch oder wenn Header‑Platz nicht reicht. In diesen Fällen "inflates" die CLR das Lock — also sie wandelt den Thin Lock in eine schwergewichtigere Struktur um.
Bei Inflation wird ein Sync Block in einer zentralen SyncBlock‑Tabelle verwendet. Der Sync Block enthält den Owner, eine Warteliste für blockierte Threads, Rekursionstiefe und ggf. Wait‑Handles für Kernel‑Blocking. Sync Blocks werden außerdem für andere Laufzeitbedürfnisse wie HashCode oder COM‑Interop verwendet.

---

## Beispiel Spiel Implementierung

[LockBasedGame](./GAE.Async/LockBasedGame.cs)

## Vor- und Nachteile von Locks

| **Vorteile**                              | **Nachteile**                             |
|-------------------------------------------|-------------------------------------------|
| **Einfachheit**: Leicht zu implementieren. | **Wartezeit**: Threads können blockiert werden. |
| **Sicherheit**: Verhindert Race Conditions. | **Leistungsprobleme**: Bei hoher Konkurrenz können Deadlocks auftreten. |
| **Klarheit**: Der Code ist verständlich. | **Komplexität**: Erfordert sorgfältiges Design. |
| | **Async/Await**: Unterstützt keine Asynchronen Aufrufe |

---

## Die neue Lock-Klasse in .NET 9

Mit **.NET 9** und C# 13 wurde eine neue `Lock`-Klasse eingeführt, die die Verwendung herkömmlicher Lock-Objekte verbessert.

### Unterschiede zur traditionellen Lock-Implementierung

- **Standardisierung**: Anstelle von `object` für das Locking bietet `Lock` eine dedizierte Struktur.
- **Bessere Lesbarkeit**: Erhöht die Lesbarkeit des Codes, da die Absicht klarer wird.
- **Performance**: Die neue `Lock`-Klasse verringert die Thread-Kontention und verbessert die Effizienz.

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
