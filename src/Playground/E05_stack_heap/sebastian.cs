namespace Playground.E05_stack_heap;

public class sebastian : Launcher
{
    public override void Run()
    {
        var c1 = new PointClass { X = 10 };
        var c2 = c1; // Referenz wird kopiert
        c2.X = 20;
        Console.WriteLine($"Class: c1.X is {c1.X}"); // Output: 20 (beide zeigen auf dasselbe Objekt)

        var s1 = new PointStruct { X = 10 };
        var s2 = s1; // Ganzer Wert wird kopiert
        s2.X = 20;
        Console.WriteLine($"Struct: s1.X is {s1.X}"); // Output: 10 (Original bleibt unverändert)

        Console.WriteLine("Multiply PointStruct:");

        var pointStruct = Multiply(s1);

        Console.WriteLine($"Modified PointStruct: X={pointStruct.X}, Y={pointStruct.Y}");
        Console.WriteLine($"Original PointStruct: X={s1.X}, Y={s1.Y}");

        Console.WriteLine("Multiply PointStruct with ref:");
        var pointStruct2 = MultiplyRef(ref s1);

        Console.WriteLine($"Modified PointStruct: X={pointStruct.X}, Y={pointStruct.Y}");
        Console.WriteLine($"Original PointStruct: X={s1.X}, Y={s1.Y}");
    }

    public PointStruct Multiply(PointStruct p)
    {
        p.X *= 2;
        p.Y *= 2;

        Console.WriteLine($"Multiply PointStruct: X={p.X}, Y={p.Y}");
        return p;
    }

    public PointStruct MultiplyRef(ref PointStruct p)
    {
        p.X *= 2;
        p.Y *= 2;

        Console.WriteLine($"Multiply PointStruct: X={p.X}, Y={p.Y}");
        return p;
    }
}
