namespace GameOfBichler.Gui.Models
{
    public class EmptyTile : IGridObject
    {
        public bool IsWalkable => true;
        public void OnPlayerEnter(Player player, GameBoard board) { }
    }

    public class Stone : IGridObject
    {
        public bool IsWalkable => false;
        public void OnPlayerEnter(Player player, GameBoard board) { }
    }

    public class Sun : IGridObject
    {
        public bool IsWalkable => true;

        public void OnPlayerEnter(Player player, GameBoard board)
        {
            player.AddScore(1);
            board.SendMessage($"Sonne gesammelt! Score: {player.Score}");
            board.ClearTile(player.Position);
        }
    }

    public class EndGoal : IGridObject
    {
        public bool IsWalkable => true;

        public void OnPlayerEnter(Player player, GameBoard board)
        {
            board.SendMessage("GEWONNEN! Du hast das Ziel erreicht.");
            board.IsGameOver = true;
        }
    }

    public class Arrow : IGridObject
    {
        public Direction ForceDirection { get; }

        public Arrow(Direction direction)
        {
            ForceDirection = direction;
        }

        public bool IsWalkable => true;

        public async void OnPlayerEnter(Player player, GameBoard board)
        {
            await Task.Delay(200);
            board.MovePlayer(ForceDirection);
        }
    }
}
