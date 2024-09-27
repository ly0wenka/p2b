using System.Collections;
using BoardNS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static TestUtilsNS.TestUtils;
using static NUnit.Framework.Assert;
using System.Threading.Tasks;

public class ShuffleTest
{
    [UnityTest]
    public IEnumerator ShuffleTilesTest()
    {
        yield return LoadSceneCoroutineAsync("SampleScene");

        var board = GameObject.Find("Board").GetComponent<Board>();
        board.SettingRandomTiles.SetRandomTiles(MockSetItem);

        yield return new WaitForSeconds(0.1f);

        // Store the initial positions of tiles
        Vector3[] initialTilePositions = GetTilePositions(board);

        // Shuffle the tiles
        board.Shuffle();

        yield return new WaitForSeconds(0.1f);

        // Get the new positions of tiles after shuffling
        Vector3[] shuffledTilePositions = GetTilePositions(board);

        // Assert that the positions have changed after shuffling
        for (int i = 0; i < initialTilePositions.Length; i++)
        {
            AreNotEqual(initialTilePositions[i], shuffledTilePositions[i]);
        }
    }

    private Vector3[] GetTilePositions(Board board)
    {
        Vector3[] positions = new Vector3[board.Width * board.Height];
        int index = 0;

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                positions[index] = board.Tiles[x, y].transform.position;
                index++;
            }
        }

        return positions;
    }
}
