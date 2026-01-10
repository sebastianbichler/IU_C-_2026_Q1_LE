using Demo.Console.Game01;
using Demo.Console.Game02;

public class Launcher
{
    public static void Main(string[] args)
    {
        // var app = new MyApp();
        // app.Run();

        var myPacManApp = new MyPacManApp();
        myPacManApp.StartEngine();

    }
}
