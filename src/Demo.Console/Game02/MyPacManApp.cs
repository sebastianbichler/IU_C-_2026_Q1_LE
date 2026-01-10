using Demo.Console.Game02;
//using PacMan.Core;
using PacMan.ConsoleUI;

public class MyPacManApp
{
    private readonly GameEngine _engine;

    public MyPacManApp()
    {
        _engine = new GameEngine(new ConsoleRenderer()); // later: use IRenderer and pass the Concrete Renderer via Constructor
        _engine.Setup();
    }

    public void StartEngine()
    {
        // Game Loop
        while (_engine.IsRunning)
        {
            // 1. Input Handling (Nicht-blockierend!)
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true).Key;
                if (key == ConsoleKey.Escape)
                {
                    _engine.StopGame();
                }
                else
                {
                    var dir = key switch
                    {
                        ConsoleKey.UpArrow => Direction.Up,
                        ConsoleKey.DownArrow => Direction.Down,
                        ConsoleKey.LeftArrow => Direction.Left,
                        ConsoleKey.RightArrow => Direction.Right,
                        _ => Direction.None
                    };
                    _engine.UpdateInput(dir);
                }
            }

            // 2. Update Logik
            _engine.Update();

            // 3. Geschwindigkeit (ca. 10 Frames pro Sekunde)
            Thread.Sleep(100);
        }
    }
}
