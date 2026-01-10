namespace Demo.Console.Game02;

public class GameEngine
{
    private readonly TileType[,] _grid;
    private readonly Player _player;
    private readonly IRenderer _renderer;
    public bool IsRunning { get; private set; } = true;

    public GameEngine(IRenderer renderer)
    {
        _renderer = renderer;
        // Einfaches Labyrinth: 0=Empty, 1=Wall, TileType.Point=Point
        _grid = new TileType[5, 20] {
            { TileType.Wall,TileType.Wall, TileType.Wall,  TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall },
            { TileType.Wall,TileType.Point,TileType.Point, TileType.Point,TileType.Point,TileType.Wall, TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Wall, TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Wall },
            { TileType.Wall,TileType.Point,TileType.Wall,  TileType.Wall, TileType.Point,TileType.Wall, TileType.Point,TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Point,TileType.Wall, TileType.Point,TileType.Wall, TileType.Wall, TileType.Wall, TileType.Point,TileType.Wall },
            { TileType.Wall,TileType.Point,TileType.Point, TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Point,TileType.Wall },
            { TileType.Wall,TileType.Wall, TileType.Wall,  TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall }
        };
        _player = new Player { X = 1, Y = 1 };
    }

    public void Setup()
    {
        for (int r = 0; r < _grid.GetLength(0); r++)
            for (int c = 0; c < _grid.GetLength(1); c++)
                DrawTile(r, c);
    }

    public void UpdateInput(Direction dir) => _player.CurrentDirection = dir;

    public void Update()
    {
        int nextX = _player.X;
        int nextY = _player.Y;

        switch (_player.CurrentDirection)
        {
            case Direction.Up: nextX--; break;
            case Direction.Down: nextX++; break;
            case Direction.Left: nextY--; break;
            case Direction.Right: nextY++; break;
        }

        // Kollisionsprüfung mit Wänden
        if (_grid[nextX, nextY] != TileType.Wall)
        {
            _renderer.ClearTile(_player.Y, _player.X); // Alte Pos löschen
            _player.X = nextX;
            _player.Y = nextY;

            // Punkte sammeln
            if (_grid[nextX, nextY] == TileType.Point)
            {
                _player.Score += 10;
                _grid[nextX, nextY] = TileType.Empty;
            }
        }

        _renderer.DrawTile(_player.Y, _player.X, '@', ConsoleColor.Yellow);
        _renderer.DrawScore(_player.Score, _player.Lives);
    }

    private void DrawTile(int x, int y)
    {
        char s = _grid[x, y] switch
        {
            TileType.Wall => '#',
            TileType.Point => '.',
            _ => ' '
        };
        _renderer.DrawTile(y, x, s, ConsoleColor.Gray);
    }

    public void StopGame()
    {
        IsRunning = false;
    }
}
