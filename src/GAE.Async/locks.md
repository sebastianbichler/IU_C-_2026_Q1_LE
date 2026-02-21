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

Monitor.Enter(lockObject);
try
{
    // Zugriff auf geschützte Ressourcen
}
finally
{
    Monitor.Exit(lockObject);
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

## Vor- und Nachteile von Locks

| **Vorteile**                              | **Nachteile**                             |
|-------------------------------------------|-------------------------------------------|
| **Einfachheit**: Leicht zu implementieren. | **Wartezeit**: Threads können blockiert werden. |
| **Sicherheit**: Verhindert Race Conditions. | **Leistungsprobleme**: Bei hoher Konkurrenz können Deadlocks auftreten. |
| **Klarheit**: Der Code ist verständlich. | **Komplexität**: Erfordert sorgfältiges Design. |

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
