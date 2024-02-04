using System;
using System.Collections.Generic;
using BoardNS.SelectionNS;
using TileNS;
using UnityEngine;
using DG.Tweening;

namespace BoardNS
{
    public sealed class Board : MonoBehaviour
    {
        public static Board Instance { get; set; }
        [SerializeField] private AudioSource audioSource;
        public Row[] rows;
        public Tile[,] Tiles { get; set; }
        public int Width => Tiles.GetLength(0);
        public int Height => Tiles.GetLength(1);

        public Selection Selection { get; }
        public Popping Popping { get; }
        public SettingRandomTiles SettingRandomTiles { get; }

        public CreatingTiles CreatingTiles { get; }

        [SerializeField] public List<Tile> selection = new List<Tile>();
        public const float TweenDuration = 0.1f;

        public Board()
        {
            CreatingTiles = new CreatingTiles(this);
            Selection = new Selection(this);
            Popping = new Popping(this);
            SettingRandomTiles = new SettingRandomTiles(this);
        }

        public void Shuffle()
        {
            // Create a list of all tiles
            List<Tile> allTiles = new List<Tile>();
            foreach (var row in rows)
            {
                allTiles.AddRange(row.tiles);
            }

            // Shuffle the list of tiles
            int n = allTiles.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                Tile value = allTiles[k];
                allTiles[k] = allTiles[n];
                allTiles[n] = value;
            }
            var sequence = DOTween.Sequence();
            // Update the positions in the 2D array
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tile tile = Tiles[x, y];
                    Tile tileTarget = allTiles[x * Height + y];
                    var position = tileTarget.icon.transform.position;
                    Vector3 targetPosition = new Vector3(position.x, position.y, 0); // Assuming 2D grid, adjust if needed
                    var position1 = tile.icon.transform.position;
                    Vector3 targetPosition2 = new Vector3(position1.x, position1.y, 0); // Assuming 2D grid, adjust if needed
                    // Use DOTween to animate the movement to the new position
                    sequence.Append(
                        tile.icon.transform.DOMove(targetPosition, Board.TweenDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            // Log the position after the animation completes
#if UNITY_EDITOR
                            Debug.Log($"Tile (tile) moved to {tile.transform.position}");
#endif


                            // Update the icon's parent to the new tile
                            tile.icon.transform.SetParent(tile.transform);
                        })).Append(
                        tileTarget.icon.transform.DOMove(targetPosition2, Board.TweenDuration)
                            .SetEase(Ease.OutQuad)
                            .OnComplete(() =>
                            {
                                // Log the position after the animation completes
#if UNITY_EDITOR
                                Debug.Log($"Tile (tile) moved to {tile.transform.position}");
#endif


                                // Update the icon's parent to the new tile
                                tile.icon.transform.SetParent(tile.transform);
                            }));
                }
            }
            sequence.Play();
#if UNITY_EDITOR
            Debug.Log($"Shuffle");
#endif

        }

        private void Awake() => Instance = this;

        private void Start()
        {
            CreatingTiles.CreateTiles();
            SettingRandomTiles.SetRandomTiles();
        }

        public void PlayCollectedSound(Tile tile)
            => audioSource.PlayOneShot(tile.Item.collectSound);

        public static void AddScore(Tile tile)
            => ScoreCounter.Instance.Score += tile.Item.value * tile.ConnectedTiles.Count;
    }
}