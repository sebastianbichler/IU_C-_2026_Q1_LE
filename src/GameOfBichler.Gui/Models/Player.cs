namespace GameOfBichler.Gui.Models
{
    public class Player
    {
        public Position Position { get; set; }
        public int Score { get; private set; }

        public Player(Position startPos)
        {
            Position = startPos;
            Score = 0;
        }

        public void AddScore(int amount)
        {
            Score += amount;
        }
    }
}
