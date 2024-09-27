using System;
using System.Collections.Generic;
using BoardNS.SelectionNS;
using TileNS;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

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

        public async void Shuffle()
        {
            List<Tile> targetTiles = new();
            foreach (var row in rows)
            {
                targetTiles.AddRange(row.tiles);
            }

            // Shuffle the list of tiles
            int n = targetTiles.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                (targetTiles[n], targetTiles[k]) = (targetTiles[k], targetTiles[n]);
            }
            // Update the positions in the 2D array
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    selection.Clear();
                    selection.Add(Tiles[x, y]);
                    selection.Add(targetTiles[x * Height + y]);
#if UNITY_EDITOR
                    Debug.Log($"Selected tiles at {selection[0]} and {selection[1]}");
#endif
                    await Swapping.SwapAsync(this);
                }
            }
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