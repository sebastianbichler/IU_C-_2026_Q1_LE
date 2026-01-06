using Playground.E01G01;

class Program
{
    private static void Main()
    {
        Console.WriteLine("Willkommen im Playground!");
        Console.WriteLine("Welches Übung soll gestartet werden?");

        Console.WriteLine("1) Übung 01 - Gruppe 01");
        Console.WriteLine("2) Übung 01 - Gruppe 02");
        Console.WriteLine("2) Übung 01 - Gruppe 03");
        Console.WriteLine("2) Übung 01 - Gruppe 04");

        string choice = Console.ReadLine() ?? string.Empty;

        switch (choice)
        {
            case "1":
                Playground.E01G01.Program program = new Playground.E01G01.Program();
                program.Run();
                break;
            case "2":
                //Playground.E01G02.Program program = new Playground.E01G03.Program();
                //program.Run();
                break;
            case "3":
                //Playground.E01G03.Program program = new Playground.E01G03.Program();
                //program.Run();
                break;
            case "4":
                //Playground.E01G04.Program program = new Playground.E01G04.Program();
                //program.Run();
                break;
            default:
                Console.WriteLine("Ungültige Wahl.");
                break;
        }
    }
}
