using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Tile2X
{
    private readonly Transform _icon1Transform;
    private readonly Transform _icon2Transform;
    private readonly Image _icon1;
    private readonly Image _icon2;

    public static Tile2X CreateInstance(Tile tile1, Tile tile2)
    {
        return new Tile2X(tile1, tile2);
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

    private Tile Tile1 { get; set; }
    private Tile Tile2 { get; set; }

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

public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    [SerializeField] private AudioSource audioSource;
    public Row[] rows;
    public Tile[,] Tiles { get; private set; }
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    [SerializeField] private List<Tile> selection = new List<Tile>();
    public const float TweenDuration = 0.25f;
    private void Awake() => Instance = this;

    private void Start()
    {
        Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                SetRandomTile(y, x);
            }
        }
    }

    private void SetRandomTile(int y, int x)
    {
        var tile = rows[y].tiles[x];
        tile.x = x;
        tile.y = y;
        tile.SetRandomItem();
        Tiles[x, y] = tile;
    }

    public async void SelectAsync(Tile tile)
    {
        AddSelected(tile);

        if (selection.Count < 2)
        {
            return;
        }
#if UNITY_EDITOR
        Debug.Log($"Selected tiles at {selection[0]} and {selection[1]}");
#endif
        var tile2X = Tile2X.CreateInstance(selection[0], selection[1]);
        await SwapAsync(tile2X);
        if (CanPop())
        {
            await PopAsync();
        }
        else
        {
            await SwapAsync(Tile2X.CreateInstance(selection[0], selection[1]));
        }

        selection.Clear();
    }

    private void AddSelected(Tile tile)
    {
        if (selection.Contains(tile)) return;
        if (IsNeighbour() || !selection.Any())
        {
            selection.Add(tile);
        }

        bool IsNeighbour() => selection.Any() && Array.IndexOf(selection[0].Neighbours, tile) != -1;
    }

    private async Task SwapAsync(Tile2X tile2X)
    {
        await tile2X.PlaySwapSequence();
        tile2X.SwapParent();
        tile2X.SwapIcons();
        tile2X.SwapItem();
    }

    private bool CanPop()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (!Tiles[x, y].HasPoppedTiles())
                {
                    return true;
                }
            }
        }

        return false;
    }

    private async Task PopAsync()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];
                if (tile.HasPoppedTiles())
                {
                    continue;
                }

                //tile.FindConnectedTiles();
                await DeInflateSequence(tile);

                x = 0;
                y = 0;
            }
        }
    }

    private async Task DeInflateSequence(Tile tile)
    {
        var deflateSequence = tile.DeflateSequence();
        BeforeDeflateSequence(tile);
        await deflateSequence.Play().AsyncWaitForCompletion();

        var inflateSequence = tile.InflateSequence();
        await inflateSequence.Play().AsyncWaitForCompletion();
    }

    private void BeforeDeflateSequence(Tile tile)
    {
        PlayCollectedSound(tile);
        AddScore(tile);
    }

    private void PlayCollectedSound(Tile tile) 
        => audioSource.PlayOneShot(tile.Item.collectSound);

    private static void AddScore(Tile tile) 
        => ScoreCounter.Instance.Score += tile.Item.value * tile.ConnectedTiles.Count;
}
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
        return tile.FindConnectedTiles().Skip(1).Count() < 2;
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