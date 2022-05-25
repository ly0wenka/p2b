using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tile : MonoBehaviour
{
    public int x;
    public int y;
    private Item _item;
    private Image _image;

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
                
            var color = _item.color;
            _image.color = new Color(color.r, color.g, color.b);
        }
    }

    public Image icon;
    public Button button;

    public override string ToString()
    {
        return $"{nameof(Tile)}: {x}, {y}";
    }

    private void Awake ()
    {
        _image = GetComponent<Image>();
    }

    private Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
    private Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
    private Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;
    private Tile Bottom => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null;

    public Tile[] Neighbours => new[]
    {
        Left,
        Top,
        Right,
        Bottom
    };

    private void Start() => button.onClick.AddListener(() => Board.Instance.SelectAsync(this));

    public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
        var result = NewOrAddExclude(ref exclude);

        foreach (var neighbour in Neighbours)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item)
            {
                continue;
            }

            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

        return result;
    }

    private List<Tile> NewOrAddExclude(ref List<Tile> exclude)
    {
        var result = new List<Tile> {this,};
        if (exclude != null)
        {
            exclude.Add(this);
        }
        else
        {
            exclude = new List<Tile> {this,};
        }

        return result;
    }
}