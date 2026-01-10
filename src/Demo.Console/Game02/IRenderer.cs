// Enums and interface for rendering the game state

namespace Demo.Console.Game02;
//namespace PacMan.Core;

public enum TileType { Empty, Wall, Point, PowerUp }
public enum Direction { Up, Down, Left, Right, None }

public interface IRenderer
{
    void DrawTile(int x, int y, char symbol, ConsoleColor color);
    void DrawScore(int score, int lives);
    void ClearTile(int x, int y);
    void Message(string text);
}
