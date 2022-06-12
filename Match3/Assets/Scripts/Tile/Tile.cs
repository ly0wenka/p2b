using System;
using System.Collections.Generic;
using BoardNS;
using UnityEngine;
using UnityEngine.UI;

namespace TileNS
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public sealed class Tile : MonoBehaviour
    {
        public int x;
        public int y;
        public Image image;

        public Tile()
        {
            FindingConnectedTiles = new FindingConnectedTiles(this);
            NeighbourTiles = new NeighbourTiles(this);
        }

        public Item Item
        {
            get => _item;
            set
            {
                if (_item == value)
                {
                    return;
                }

                _item = value;
                icon.sprite = _item.sprite;

                SetImageColor();

                void SetImageColor()
                {
                    var color = _item.color;
                    image.color = new Color(color.r, color.g, color.b);
                }
            }
        }

        public Image icon;
        public Button button;
        private Item _item;

        public override string ToString()
        {
            return $"{nameof(Tile)}: {x}, {y}";
        }

        public List<Tile> ConnectedTiles { get; set; }

        public FindingConnectedTiles FindingConnectedTiles { get; set; }

        public NeighbourTiles NeighbourTiles { get; }

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Start()
        {
            button.onClick.AddListener(() => Board.Instance.Selection.Select(this));
        }
    }
}