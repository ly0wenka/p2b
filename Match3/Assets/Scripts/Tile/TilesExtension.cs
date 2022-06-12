using System.Linq;
using BoardNS;
using DG.Tweening;
using UnityEngine;

namespace TileNS
{
    public static class TilesExtension
    {
        public static Sequence InflateSequence(this Tile tile)
        {
            var inflateSequence = DOTween.Sequence();
            foreach (var connectedTile in tile.ConnectedTiles)
            {
                SetRandomItem(connectedTile);
                inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, Board.TweenDuration));
            }

            return inflateSequence;
        }

        public static void SetRandomItem(this Tile tile)
        {
            tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
        }

        public static bool HasPoppedTiles(this Tile tile)
        {
            return tile.FindingConnectedTiles.FindConnectedTiles().Skip(1).Count() < 2;
        }

        public static Sequence DeflateSequence(this Tile tile)
        {
            var deflateSequence = DOTween.Sequence();
            foreach (var connectedTile in tile.ConnectedTiles)
            {
                deflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, Board.TweenDuration));
            }

            return deflateSequence;
        }
    }
}