using Demo.Console.Game02;
//using PacMan.Core;
using PacMan.ConsoleUI;

public class MyPacManApp
{
    private readonly GameEngine _engine;
    private readonly IRenderer _renderer;

    public MyPacManApp()
    {
        _renderer = new ConsoleRenderer();
        _engine = new GameEngine(_renderer);
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
                    break;
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

        // NACH der while-Schleife (engine.IsRunning == false)
        _renderer.ClearTile(0, 15); // Etwas Platz schaffen
        _renderer.Message("Spiel beendet. Auf Wiedersehen!");
        Console.CursorVisible = true;
    }
}
