namespace Demo.Console.Game02;

public class Player : GameObject
{
    public Direction CurrentDirection { get; set; } = Direction.None;
    public int Lives { get; set; } = 3;
    public int Score { get; set; } = 0;
}
