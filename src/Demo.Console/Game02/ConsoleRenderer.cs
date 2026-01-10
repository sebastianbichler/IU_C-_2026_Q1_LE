// VIEW

//using PacMan.Core;
using Demo.Console.Game02;

namespace PacMan.ConsoleUI;

public class ConsoleRenderer : IRenderer
{
    public ConsoleRenderer()
    {
        Console.CursorVisible = false;
        Console.Clear();
    }

    public void DrawTile(int x, int y, char symbol, ConsoleColor color)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(symbol);
    }

    public void ClearTile(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(' ');
    }

    public void DrawScore(int score, int lives)
    {
        Console.SetCursorPosition(0, 12);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Score: {score} | Lives: {lives}   ");
    }

    public void Message(string text)
    {
        Console.SetCursorPosition(0, 14);
        Console.WriteLine(text);
    }
}
