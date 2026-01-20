//Uebung 1 vom 20.01.26

public class PointClass { public int X; public int Y; }
public struct PointStruct { public int X; public int Y; }


public static class Paradigms
{
    public static void Show()
    {
        // Klassen sind Referenztypen
        PointClass pc1 = new() { X = 10 };
        PointClass pc2 = pc1; // pc2 verweist auf dasselbe Objekt wie pc1
        pc2.X = 20;
        // pc1.X ist jetzt auch 30, da pc1 und pc2 dasselbe Objekt referenzieren

        // Strukturen sind Werttypen
        PointStruct ps1 = new() { X = 10 };
        PointStruct ps2 = ps1; // ps2 ist eine Kopie von ps1
        ps2.X = 20;
        // ps1.X bleibt 10, da ps1 und ps2 unterschiedliche Objekte sind

        System.Console.WriteLine($"PointClass pc1: X={pc1.X}, Y={pc1.Y}"); // X=30, Y=20
        System.Console.WriteLine($"PointStruct ps1: X={ps1.X}, Y={ps1.Y}"); // X=10, Y=20
    }

    public static void Multiply(ref PointStruct P)
    {
        P.X *= 2;
        P.Y *= 2;
    }

    // Aufruf der Methode und Überprüfung des Originals
    public static void TestMultiply()
    {
        PointStruct point = new PointStruct { X = 5, Y = 10 };
        Multiply(ref point);
        // Überprüfung des Originals
        System.Console.WriteLine($"Original: X = {point.X}, Y = {point.Y}"); // Sollte X = 10, Y = 20 sein
    }

    public static void Main(string[] args)
    {
        TestMultiply();
    }
}

