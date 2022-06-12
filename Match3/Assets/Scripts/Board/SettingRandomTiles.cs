using System;
using TileNS;

namespace BoardNS
{
    public sealed class SettingRandomTiles
    {
        private readonly Board _board;

        public SettingRandomTiles(Board board)
        {
            _board = board;
        }

        public void SetRandomTiles(Action<Tile> setItem = null)
        {
            setItem ??= TilesExtension.SetRandomItem;
            for (var y = 0; y < _board.Height; ++y)
            {
                for (var x = 0; x < _board.Width; ++x)
                {
                    var tile = _board.rows[y].tiles[x];
                    tile.y = y;
                    tile.x = x;
                    setItem(tile);
                    _board.Tiles[x, y] = tile;
                }
            }
        }
    }
}