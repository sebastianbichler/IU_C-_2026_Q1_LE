using System.Numerics;

namespace GameOfBichler.Gui.Models
{
    public interface IGridObject
    {
        bool IsWalkable { get; }
        void OnPlayerEnter(Player player, GameBoard board);
    }
}
