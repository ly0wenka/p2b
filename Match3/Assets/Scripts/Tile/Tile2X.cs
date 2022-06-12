using System.Collections.Generic;
using System.Threading.Tasks;
using BoardNS;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TileNS
{
    public class Tile2X
    {
        private readonly Transform _icon1Transform;
        private readonly Transform _icon2Transform;
        private readonly Image _icon1;
        private readonly Image _icon2;

        public static Tile2X CreateInstance(List<Tile> tiles)
        {
            return new Tile2X(tiles[0], tiles[1]);
        }

        private Tile2X(Tile tile1, Tile tile2)
        {
            Tile1 = tile1;
            Tile2 = tile2;
            _icon1 = tile1.icon;
            _icon2 = tile2.icon;
            _icon1Transform = _icon1.transform;
            _icon2Transform = _icon2.transform;
        }

        private Tile Tile1 { get; }
        private Tile Tile2 { get; }

        public async Task PlaySwapSequence()
        {
            var sequence = DOTween.Sequence();
            sequence.Join(_icon1Transform.DOMove(_icon2Transform.position, Board.TweenDuration))
                .Join(_icon2Transform.DOMove(_icon1Transform.position, Board.TweenDuration));
            await sequence.Play().AsyncWaitForCompletion();
        }

        public void SwapParent()
        {
            _icon1Transform.SetParent(Tile2.transform);
            _icon2Transform.SetParent(Tile1.transform);
        }

        public void SwapIcons()
        {
            Tile1.icon = _icon2;
            Tile2.icon = _icon1;
        }

        public void SwapItem()
        {
            (Tile1.Item, Tile2.Item) = (Tile2.Item, Tile1.Item);
        }
    }
}