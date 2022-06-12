using System.Linq;
using TileNS;

namespace BoardNS
{
    public sealed class CreatingTiles
    {
        private readonly Board _board;

        public CreatingTiles(Board board)
        {
            _board = board;
        }

        public void CreateTiles() => _board.Tiles = new Tile[_board.rows.Max(row => row.tiles.Length), _board.rows.Length];
    }
}