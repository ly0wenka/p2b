using BoardNS;

namespace TileNS
{
    public sealed class NeighbourTiles
    {
        private Tile _tile;

        public NeighbourTiles(Tile tile)
        {
            _tile = tile;
        }

        private Tile Left => _tile.x > 0 ? Board.Instance.Tiles[_tile.x - 1, _tile.y] : null;
        private Tile Top => _tile.y > 0 ? Board.Instance.Tiles[_tile.x, _tile.y - 1] : null;
        private Tile Right => _tile.x < Board.Instance.Width - 1 ? Board.Instance.Tiles[_tile.x + 1, _tile.y] : null;
        private Tile Bottom => _tile.y < Board.Instance.Height - 1 ? Board.Instance.Tiles[_tile.x, _tile.y + 1] : null;

        public Tile[] Neighbours => new[]
        {
            Left,
            Top,
            Right,
            Bottom
        };
    }
}