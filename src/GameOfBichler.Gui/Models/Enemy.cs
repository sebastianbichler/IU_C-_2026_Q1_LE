using System;

namespace GameOfBichler.Gui.Models
{
    public class Enemy
    {
        public Position Position { get; set; }
        public DateTime StunnedUntil { get; set; }

        public Enemy(Position startPos)
        {
            Position = startPos;
            StunnedUntil = DateTime.MinValue;
        }
    }
}
