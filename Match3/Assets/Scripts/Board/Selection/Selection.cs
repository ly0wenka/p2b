using System;
using System.Linq;
using TileNS;
using UnityEngine;

namespace BoardNS.SelectionNS
{
    public sealed class Selection
    {
        private const int MaxSelection = 2;
        private readonly Board _board;

        public Selection(Board board)
        {
            _board = board;
        }

        public async void Select(Tile tile)
        {
            AddSelected(tile);

            if (!CanSwap())
            {
                return;
            }
#if UNITY_EDITOR
            Debug.Log($"Selected tiles at {_board.selection[0]} and {_board.selection[1]}");
#endif
            await Swapping.SwapAsync(_board);
            if (_board.Popping.CanPop())
            {
                await _board.Popping.PopAsync();
            }
            else if (CanSwap())
            {
                await Swapping.SwapAsync(_board);
            }

            _board.selection.Clear();

            bool CanSwap() => !(IsSelectionCountLessTwo() || IsSomeSelectionNull());
            bool IsSelectionCountLessTwo() => _board.selection.Count < MaxSelection;
            bool IsSomeSelectionNull() => _board.selection[0] == null || _board.selection[1] == null;
        }

        private void AddSelected(Tile tile)
        {
            if (_board.selection.Contains(tile)) return;
            if (IsNeighbour() || !_board.selection.Any())
            {
                _board.selection.Add(tile);
            }

            bool IsNeighbour() => _board.selection.Any() && Array.IndexOf(_board.selection[0].NeighbourTiles.Neighbours, tile) != -1;
        }
    }
}