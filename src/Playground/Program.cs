using Playground;

class Program
{
    private static void Main()
    {
        Console.WriteLine("Willkommen im Playground!");
        Console.WriteLine("Welches Übung soll gestartet werden?");

        Console.WriteLine("1) Übung 01 - Gruppe 01a");
        Console.WriteLine("2) Übung 01 - Gruppe 01b");
        Console.WriteLine("3) Übung 01 - Gruppe 02");
        Console.WriteLine("4) Übung 01 - Gruppe 03");
        Console.WriteLine("5) Übung 01 - Gruppe 04");
        Console.WriteLine("----------------------------");
        Console.WriteLine("(6) Übung 05 - Stack vs Heap");

        string choice = Console.ReadLine() ?? string.Empty;

        Launcher launcher;
        switch (choice)
        {
            case "1":
                launcher = new Playground.E01G01a.Program();
                launcher.Run();
                break;
            case "2":
                launcher = new Playground.E01G01b.Program();
                launcher.Run();
                break;
            case "3":
                launcher = new Playground.E01G02.Program();
                launcher.Run();
                break;
            case "4":
                launcher = new Playground.E01G03.Program();
                launcher.Run();
                break;
            case "5":
                launcher = new Playground.E01G04.Program();
                launcher.Run();
                break;
            case "6":
                launcher = new Playground.E05_stack_heap.sebastian();
                launcher.Run();
                break;
            default:
                Console.WriteLine("Ungültige Wahl.");
                break;
        }
    }
}
