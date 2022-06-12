using System.Collections.Generic;
using BoardNS;
using NUnit.Framework;
using TileNS;
using UnityEngine;
using static NUnit.Framework.Assert;
using static TestUtilsNS.TestUtils;

public class NeighboursTest
{
    [Test]
    public void Neighbours0LeftTest()
    {
        ItemDatabase.Items = Resources.LoadAll<Item>("Items/");
        CreateBoard();

        var tile = Board.Instance.Tiles[1, 1];

        NotNull(tile);
        NotNull(tile.NeighbourTiles.Neighbours);
        AreEqual(0, tile.NeighbourTiles.Neighbours[0].x);
        AreEqual(1, tile.NeighbourTiles.Neighbours[0].y);
    }

    [Test]
    public void Neighbours1UpTest()
    {
        ItemDatabase.Items = Resources.LoadAll<Item>("Items/");
        CreateBoard();

        var tile = Board.Instance.Tiles[1, 1];

        NotNull(tile);
        NotNull(tile.NeighbourTiles.Neighbours);
        AreEqual(1, tile.NeighbourTiles.Neighbours[1].x);
        AreEqual(0, tile.NeighbourTiles.Neighbours[1].y);
    }

    [Test]
    public void Neighbours2RightTest()
    {
        ItemDatabase.Items = Resources.LoadAll<Item>("Items/");
        CreateBoard();

        var tile = Board.Instance.Tiles[1, 1];

        NotNull(tile);
        NotNull(tile.NeighbourTiles.Neighbours);
        AreEqual(2, tile.NeighbourTiles.Neighbours[2].x);
        AreEqual(1, tile.NeighbourTiles.Neighbours[2].y);
    }

    [Test]
    public void Neighbours3BottomTest()
    {
        ItemDatabase.Items = Resources.LoadAll<Item>("Items/");
        CreateBoard();

        var tile = Board.Instance.Tiles[1, 1];

        NotNull(tile);
        NotNull(tile.NeighbourTiles.Neighbours);
        AreEqual(1, tile.NeighbourTiles.Neighbours[3].x);
        AreEqual(2, tile.NeighbourTiles.Neighbours[3].y);
    }

    [Test]
    public void Neighbours3SwapBottomTest()
    {
        ItemDatabase.Items = Resources.LoadAll<Item>("Items/");
        CreateBoard();

        var tile = Board.Instance.Tiles[1, 1];
        
        NotNull(tile);
        NotNull(tile.NeighbourTiles.Neighbours);
        AreEqual(1, tile.NeighbourTiles.Neighbours[3].x);
        AreEqual(2, tile.NeighbourTiles.Neighbours[3].y);
        
        Board.Instance.selection = new List<Tile>() { tile, tile.NeighbourTiles.Neighbours[3]};
        Board.Instance.Selection.Select(tile);
        
        IsTrue(Board.Instance.Popping.CanPop());
        
        AreEqual(1, tile.x);
        AreEqual(2, tile.y);
    }
}