using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Tile2x
{
    private Transform icon1Transform;
    private Transform icon2Transform;
    private Image icon1;
    private Image icon2;

    public static Tile2x CreateInstance(Tile tile1, Tile tile2)
    {
        return new Tile2x(tile1, tile2);
    }

    private Tile2x(Tile tile1, Tile tile2)
    {
        Tile1 = tile1;
        Tile2 = tile2;
        icon1 = tile1.icon;
        icon2 = tile2.icon;
        icon1Transform = icon1.transform;
        icon2Transform = icon2.transform;
    }

    public Tile Tile1 { get; private set; }
    public Tile Tile2 { get; private set; }

    public async Task PlaySwapSequence()
    {
        var sequence = DOTween.Sequence();
        sequence.Join(icon1Transform.DOMove(icon2Transform.position, Board.TweenDuration))
            .Join(icon2Transform.DOMove(icon1Transform.position, Board.TweenDuration));
        await sequence.Play().AsyncWaitForCompletion();
    }

    public void SwapParent()
    {
        icon1Transform.SetParent(Tile2.transform);
        icon2Transform.SetParent(Tile1.transform);
    }

    public void SwapIcons()
    {
        Tile1.icon = icon2;
        Tile2.icon = icon1;
    }

    public void SwapItem()
    {
        var tile1Item = Tile1.Item;
        Tile1.Item = Tile2.Item;
        Tile2.Item = tile1Item;
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
                SetTile(y, x);
            }
        }
    }

    private void SetTile(int y, int x)
    {
        var tile = rows[y].tiles[x];
        tile.x = x;
        tile.y = y;
        tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
        Tiles[x, y] = tile;
    }

    public async void SelectAsync(Tile tile)
    {
        if (!selection.Contains(tile))
        {
            if (selection.Count > 0)
            {
                if (Array.IndexOf(selection[0].Neighbours, tile) != -1)
                {
                    selection.Add(tile);
                }
            }
            else
            {
                selection.Add(tile);
            }
        }

        if (selection.Count < 2)
        {
            return;
        }
#if UNITY_EDITOR
        Debug.Log($"Selected tiles at {selection[0]} and {selection[1]}");
#endif
        var tile2X = Tile2x.CreateInstance(selection[0], selection[1]);
        await SwapAsync(tile2X);
        if (CanPop())
        {
            await PopAsync();
        }
        else
        {
            await SwapAsync(tile2X);
        }

        selection.Clear();
    }

    private async Task SwapAsync(Tile2x tile2X)
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
                if (Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2)
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
                var connectedTiles = tile.GetConnectedTiles();
                if (connectedTiles.Skip(1).Count() < 2)
                {
                    continue;
                }

                var deflateSequence = connectedTiles.DeflateSequence();

                audioSource.PlayOneShot(tile.Item.collectSound);
                ScoreCounter.Instance.Score += tile.Item.value * connectedTiles.Count();
                await deflateSequence.Play().AsyncWaitForCompletion();
                var inflateSequence = connectedTiles.InflateSequence();

                await inflateSequence.Play().AsyncWaitForCompletion();

                x = 0;
                y = 0;
            }
        }
    }
}
public static class TilesExtension
{
    public static Sequence InflateSequence(this IEnumerable<Tile> connectedTiles)
    {
        var inflateSequence = DOTween.Sequence();
        foreach (var connectedTile in connectedTiles)
        {
            connectedTile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
            inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, Board.TweenDuration));
        }

        return inflateSequence;
    }

    public static Sequence DeflateSequence(this IEnumerable<Tile> connectedTiles)
    {
        var deflateSequence = DOTween.Sequence();
        foreach (var connectedTile in connectedTiles)
        {
            deflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, Board.TweenDuration));
        }

        return deflateSequence;
    }
}