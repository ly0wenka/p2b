using System;
using System.Collections;
using System.Linq;
using BoardNS;
using TileNS;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TestUtilsNS
{
    public static class TestUtils
    {
        private const int ComponentAmount = 5;

        public static IEnumerator LoadSceneCoroutineAsync(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                Debug.Log(message: $"{operation.progress:P}");
                yield return null;
            }
        }

        public static void CreateBoard()
        {
            var board = CreateComponent<Board>();
            Board.Instance = board;

            Set5TilesIn5Rows(ref board);

            board.CreatingTiles.CreateTiles();
            board.SettingRandomTiles.SetRandomTiles(MockSetItem);
        }

        public static void MockSetItem(Tile tile)
        {
            tile.Item = ItemDatabase.Items[ItemIndexes[tile.y, tile.x]];
        }

        private static int[,] ItemIndexes => new[,]
        {
            { 0, 1, 2, 3, 4 },
            { 0, 1, 2, 3, 4 },
            { 0, 1, 2, 3, 4 },
            { 0, 1, 2, 3, 4 },
            { 0, 1, 2, 3, 4 }
        };

        public static (int, int)[,] ItemPoses => new[,]
        {
            { (0, 0), (1, 0), (2, 0), (3, 0), (4, 0) },
            { (0, 1), (1, 1), (2, 1), (3, 1), (4, 1) },
            { (0, 2), (1, 2), (2, 2), (3, 2), (4, 2) },
            { (0, 3), (1, 3), (2, 3), (3, 3), (4, 3) },
            { (0, 4), (1, 4), (2, 4), (3, 4), (4, 4) }
        };

        private static T CreateComponent<T>() where T : Behaviour
        {
            var gameObject = new GameObject();
            var monoBehaviour = gameObject.AddComponent<T>();
            return monoBehaviour;
        }

        private static void Set5TilesIn5Rows(ref Board board)
        {
            board.rows = Create5Component<Row>();
            Array.ForEach(board.rows, row => { row.tiles = Create5Tiles(); });
        }

        private static T[] Create5Component<T>() where T : MonoBehaviour =>
            Enumerable.Range(0, ComponentAmount).Select(i => CreateComponent<T>()).ToArray();

        private static Tile[] Create5Tiles() =>
            Enumerable.Range(0, ComponentAmount).Select<int, Tile>(i => CreateTile()).ToArray();

        private static Tile CreateTile()
        {
            var tile = CreateComponent<Tile>();
            tile.icon = CreateComponent<Image>();
            tile.button = CreateComponent<Button>();
            tile.image = CreateComponent<Image>();
            return tile;
        }

        public static Vector3 OldPos(Tile tile, out Vector3 oldPosIconN)
        {
            var position = tile.icon.transform.position;
            var positionN = tile.NeighbourTiles.Neighbours[3].icon.transform.position;

            var oldPosIconTile = new Vector3(position.x, position.y, position.z);
            oldPosIconN = new Vector3(positionN.x, positionN.y, positionN.z);
            return oldPosIconTile;
        }
    }
}