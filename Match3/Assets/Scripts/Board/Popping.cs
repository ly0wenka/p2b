using System.Threading.Tasks;
using DG.Tweening;
using TileNS;

namespace BoardNS
{
    public sealed class Popping
    {
        private readonly Board _board;

        public Popping(Board board)
        {
            _board = board;
        }
        
        public bool CanPop()
        {
            for (var y = 0; y < _board.Height; y++)
            {
                for (var x = 0; x < _board.Width; x++)
                {
                    if (!_board.Tiles[x, y].HasPoppedTiles())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task PopAsync()
        {
            for (var y = 0; y < _board.Height; y++)
            {
                for (var x = 0; x < _board.Width; x++)
                {
                    var tile = _board.Tiles[x, y];
                    if (tile.HasPoppedTiles())
                    {
                        continue;
                    }

                    await DeInflateSequence(tile)!;

                    x = 0;
                    y = 0;
                }
            }
        }

        private async Task DeInflateSequence(Tile tile)
        {
            var deflateSequence = tile.DeflateSequence();
            var inflateSequence = tile.InflateSequence();
            BeforePlayDeflateSequence(tile);
            await deflateSequence.Play().AsyncWaitForCompletion();
            await inflateSequence.Play().AsyncWaitForCompletion();
        }

        private void BeforePlayDeflateSequence(Tile tile)
        {
            _board.PlayCollectedSound(tile);
            Board.AddScore(tile);
        }
    }
}