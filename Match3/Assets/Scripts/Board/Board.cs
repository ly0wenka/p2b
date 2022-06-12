using System.Collections.Generic;
using BoardNS.SelectionNS;
using TileNS;
using UnityEngine;

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