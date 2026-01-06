namespace Demo.Console.Game01;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public enum CellType
{
    Empty,
    Player,
    Sun,
    Cloud,
    Stone
}

public class GameState
{
    public int Width { get; } = 16;
    public int Height { get; } = 8;
    public (int X, int Y) PlayerPos { get; set; }
    public CellType[,] Grid { get; }
    public int Score { get; set; } = 0;
    public int Lives { get; set; } = 3;

    public GameState()
    {
        Grid = new CellType[Width, Height];
        PlayerPos = (0, 0);
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        var rnd = new Random();
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                int val = rnd.Next(0, 100);
                if (val < 10) Grid[x, y] = CellType.Sun;
                else if (val < 20) Grid[x, y] = CellType.Cloud;
                else if (val < 25) Grid[x, y] = CellType.Stone;
            }
    }

    public void Move(Direction dir)
    {
        int newX = PlayerPos.X;
        int newY = PlayerPos.Y;

        if (dir == Direction.Up) newY--;
        if (dir == Direction.Down) newY++;
        if (dir == Direction.Left) newX--;
        if (dir == Direction.Right) newX++;

        // Bounds Check & Stone Check
        if (newX >= 0 && newX < Width && newY >= 0 && newY < Height && Grid[newX, newY] != CellType.Stone)
        {
            PlayerPos = (newX, newY);
            HandleCollision(newX, newY);
        }
    }

    private void HandleCollision(int x, int y)
    {
        if (Grid[x, y] == CellType.Sun)
        {
            Score += 10;
            Grid[x, y] = CellType.Empty;
        }

        if (Grid[x, y] == CellType.Cloud)
        {
            Lives--;
            Grid[x, y] = CellType.Empty;
        }
    }
}
