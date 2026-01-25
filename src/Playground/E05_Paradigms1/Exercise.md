### Programmieren mit C# (DSPC016)


---

# Moderne C# Paradigmen Part 1

**Thema:** Performance, Immutability und Typsystem-Tricks

**Kontext:** Vorbereitung auf das Projekt "Grand Arcade Ecosystem"

---

## 1. Stack vs. Heap: `class` vs. `struct` (20 Min)

**Theorie:** In C# unterscheiden wir fundamental zwischen **Verweistypen** (`class`) und **Wertetypen** (`struct`).

* Klassen leben auf dem **Heap**; Variablen halten nur eine Referenz (Pointer).
* Structs leben dort, wo sie deklariert werden (meist auf dem **Stack**); Variablen halten den tatsächlichen Wert.

```csharp
// CODE-DEMO 1
public class PointClass { public int X; public int Y; }
public struct PointStruct { public int X; public int Y; }

var c1 = new PointClass { X = 10 };
var c2 = c1; // Referenz wird kopiert
c2.X = 20;
Console.WriteLine($"Class: c1.X is {c1.X}"); // Output: 20 (beide zeigen auf dasselbe Objekt)

var s1 = new PointStruct { X = 10 };
var s2 = s1; // Ganzer Wert wird kopiert
s2.X = 20;
Console.WriteLine($"Struct: s1.X is {s1.X}"); // Output: 10 (Original bleibt unverändert)

```

**✍️ ÜBUNG:** Erstellen Sie eine Methode `Multiply(PointStruct p)`, die X und Y verdoppelt. Rufen Sie sie auf und
prüfen Sie das Original. Warum ändert sich das Original nicht? Nutzen Sie danach das Schlüsselwort `ref`, um das
Verhalten zu ändern.

---
