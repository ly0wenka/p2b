using System.Collections.Generic;

namespace TileNS
{
    public sealed class FindingConnectedTiles
    {
        private Tile _tile;

        public FindingConnectedTiles(Tile tile)
        {
            _tile = tile;
        }

        public List<Tile> FindConnectedTiles(List<Tile> exclude = null)
        {
            var result = _tile.FindingConnectedTiles.NewOrAddExclude(ref exclude);

            foreach (var neighbour in _tile.NeighbourTiles.Neighbours)
            {
                var isNotNeighbourIn =
                    neighbour != null && !exclude.Contains(neighbour) && neighbour.Item == _tile.Item;
                if (isNotNeighbourIn)
                {
                    result.AddRange(neighbour.FindingConnectedTiles.FindConnectedTiles(exclude));
                }
            }

            _tile.ConnectedTiles = result;
            return result;
        }

        private List<Tile> NewOrAddExclude(ref List<Tile> exclude)
        {
            var result = new List<Tile> { _tile, };
            if (exclude != null)
            {
                exclude.Add(_tile);
            }
            else
            {
                exclude = new List<Tile> { _tile, };
            }

            return result;
        }
    }
}