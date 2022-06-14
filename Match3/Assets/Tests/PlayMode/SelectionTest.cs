using System.Collections;
using BoardNS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static TestUtilsNS.TestUtils;
using static NUnit.Framework.Assert;

public class SelectionTest
{
    [UnityTest]
    public IEnumerator Neighbours3SwapBottomTest()
    {
        yield return LoadSceneCoroutineAsync("SampleScene");
        
        var board = GameObject.Find("Board").GetComponent<Board>();
        board.SettingRandomTiles.SetRandomTiles(MockSetItem);
        
        yield return new WaitForSeconds(0.1f);
        
        var tile = Board.Instance.Tiles[0, 0];
        
        NotNull(tile);
        NotNull(tile.NeighbourTiles.Neighbours);
        AreEqual(0, tile.NeighbourTiles.Neighbours[3].x);
        AreEqual(1, tile.NeighbourTiles.Neighbours[3].y);
        
        var oldPosIconTile = OldPos(tile, out var oldPosIconN);

        tile.button.onClick?.Invoke();
        tile.NeighbourTiles.Neighbours[3].button.onClick?.Invoke();
        
        const float tweenDuration = Board.TweenDuration+0.01f;
        
        yield return new WaitForSeconds(tweenDuration);

        AreEqual(0, tile.x);
        AreEqual(0, tile.y);
        AreEqual(oldPosIconTile.ToString(), tile.icon.transform.position.ToString());
        AreEqual(oldPosIconN.ToString(), tile.NeighbourTiles.Neighbours[3].icon.transform.position.ToString());
        
        yield return new WaitForSeconds(3);
    }
    
    [UnityTest]
    public IEnumerator Neighbours3SwapBottomTestIconBag()
    {
        yield return LoadSceneCoroutineAsync("SampleScene");
        
        var board = GameObject.Find("Board").GetComponent<Board>();
        board.SettingRandomTiles.SetRandomTiles(MockSetItem);
        yield return new WaitForSeconds(0.1f);
        
        var tile = Board.Instance.Tiles[0, 0];
        
        NotNull(tile);
        NotNull(tile.NeighbourTiles.Neighbours);
        AreEqual(0, tile.NeighbourTiles.Neighbours[3].x);
        AreEqual(1, tile.NeighbourTiles.Neighbours[3].y);
        
        var oldPosIconTile = OldPos(tile, out var oldPosIconN);

        const float tweenDuration = 0.1f;
        tile.button.onClick?.Invoke();
        tile.NeighbourTiles.Neighbours[3].button.onClick?.Invoke();
        yield return new WaitForSeconds(tweenDuration);

        AreEqual(0, tile.x);
        AreEqual(0, tile.y);
        AreNotEqual(oldPosIconTile.ToString(), tile.icon.transform.position.ToString());
        AreNotEqual(oldPosIconN.ToString(), tile.NeighbourTiles.Neighbours[3].icon.transform.position.ToString());
        yield return new WaitForSeconds(3);
    }
}