using Demo.Console.Game01;
using Demo.Console.Game02;
using Demo.Console.G5Integration;

public class Launcher
{
    public static void Main(string[] args)
    {
        // Optional for Gruppe 5 demos:
        // G5DiscoveryShowcase.Run();

        // var app = new MyApp();
        // app.Run();

        var myPacManApp = new MyPacManApp();
        myPacManApp.StartEngine();

    }
}
