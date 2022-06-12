using System.Threading.Tasks;
using TileNS;

namespace BoardNS.SelectionNS
{
    public sealed class Swapping
    {
        public static async Task SwapAsync(Board board)
        {
            await SwapAsync(Tile2X.CreateInstance(board.selection));
        }
        private static async Task SwapAsync(Tile2X tile2X)
        {
            await tile2X.PlaySwapSequence();
            tile2X.SwapParent();
            tile2X.SwapIcons();
            tile2X.SwapItem();
        }
    }
}