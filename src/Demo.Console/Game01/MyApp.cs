namespace Demo.Console.Game01;

public class MyApp
{
    GameState gameState = new();

    public void Run()
    {
        while (gameState.Lives > 0)
        {
            System.Console.Clear();
            System.Console.WriteLine($"Score: {gameState.Score} | Lives: {gameState.Lives}");
            for (int y = 0; y < gameState.Height; y++)
            {
                for (int x = 0; x < gameState.Width; x++)
                {
                    if (x == gameState.PlayerPos.X && y == gameState.PlayerPos.Y) System.Console.Write("P ");
                    else
                        System.Console.Write(gameState.Grid[x, y] switch
                        {
                            CellType.Sun => "S ",
                            CellType.Cloud => "C ",
                            CellType.Stone => "# ",
                            _ => ". "
                        });
                }

                System.Console.WriteLine();
            }

            var key = System.Console.ReadKey().Key;
            if (key == ConsoleKey.UpArrow) gameState.Move(Direction.Up);
            else if (key == ConsoleKey.DownArrow) gameState.Move(Direction.Down);
            else if (key == ConsoleKey.LeftArrow) gameState.Move(Direction.Left);
            else if (key == ConsoleKey.RightArrow) gameState.Move(Direction.Right);
        }
    }
}

