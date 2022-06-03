using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using static NUnit.Framework.Assert;

public class NeighboursTest
{
    private const int ComponentAmount = 5;

    [Test]
    public void NeighboursLeftTest()
    {
        var tile = CreateComponent<Tile>();
        var board = CreateComponent<Board>();

        board.rows = Create5Component<Row>();;
        
        Set5TilesInRows(board);
        
        board.CreateTiles();
        board.SetRandomTiles();
        tile.x = 1;
        tile.y = 1;
        NotNull(tile);
        NotNull(tile.Neighbours);
        Equals(0, tile.Neighbours[0].x);
        Equals(1, tile.Neighbours[0].y);
    }

    private static IEnumerable<Tile[]> Set5TilesInRows(Board board) => 
        board.rows.Select(r => r.tiles = Create5Component<Tile>());

    private static Item[] CreateItems() => 
        Enumerable.Range(0, ComponentAmount).Select(i=> ScriptableObject.CreateInstance<Item>()).ToArray();

    private static T[] Create5Component<T>() where T : MonoBehaviour => 
        Enumerable.Range(0, ComponentAmount).Select(i=> CreateComponent<T>()).ToArray();
    
    private static T CreateComponent<T>() where T : MonoBehaviour
    {
        var gameObject = new GameObject();
        return gameObject.AddComponent<T>();
    }
}
