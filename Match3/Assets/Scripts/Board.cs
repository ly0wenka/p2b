using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    [SerializeField] private AudioSource audioSource;
    public Row[] rows;
    public Tile[,] Tiles { get; private set; }
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    [SerializeField] private List<Tile> selection = new List<Tile>();
    internal const float TweenDuration = 0.25f;
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
        await SwapAsync(selection[0], selection[1]);
        if (CanPop())
        {
            await PopAsync();
        }
        else
        {
            await SwapAsync(selection[0], selection[1]);
        }

        selection.Clear();
    }

    private async Task SwapAsync(Tile tile1, Tile tile2)
    {
        var icon1 = tile1.icon;
        var icon2 = tile2.icon;
        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;
        var sequence = DOTween.Sequence();
        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
            .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));
        await sequence.Play().AsyncWaitForCompletion();
        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);
        tile1.icon = icon2;
        tile2.icon = icon1;
        var tile1Item = tile1.Item;
        tile1.Item = tile2.Item;
        tile2.Item = tile1Item;
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