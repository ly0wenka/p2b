using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Transform border;
    public Transform star;
    public Transform enemy;
    public Transform powerUp;
    //public Transform obstacle;
    public Transform wall;
    public Transform player;

    [HideInInspector] public List<Transform> walls = new List<Transform>();
    [HideInInspector] public List<Transform> borders = new List<Transform>();
    [HideInInspector] public List<Transform> enemies = new List<Transform>();
    [HideInInspector] public List<Transform> powerUps = new List<Transform>();
    [HideInInspector] public List<Transform> stars = new List<Transform>();
    //[HideInInspector] public List<Transform> obstacles = new List<Transform>();

    public int minX, maxX, minY, maxY;
    public int minRandEnemies, maxRandEnemies;
    public int minRandPowerUps, maxRandPowerUps;
    //public int minRandObstacles, maxRandObstacles;

    void Start()
    {
        enemy.GetComponent<EnemyAI>().target = player;
        BuildLevel();
        //StartCoroutine(GenerateObstacles());
    }

    private void BuildLevel()
    {
        Generate1LevelMazeWalls();
        GenerateBorders();
        GenerateEnemies();
        GeneratePowerUps();
        GenerateStars();
    }

    //private IEnumerator GenerateObstacles()
    //{
    //    minRandObstacles = 2; maxRandObstacles = 10;
    //    GenerateObjects("obstacles", minRandObstacles, maxRandObstacles, obstacle, obstacles);
    //    yield return new WaitForFixedUpdate();
    //}

    private void GenerateObjects(string containerName, int minRandObject, int maxRandObject,
        Transform @object, List<Transform> objects)
    {
        var container = new GameObject() { name = containerName };
        container.transform.SetParent(transform);

        Enumerable.Range(0, Random.Range(minRandObject, maxRandObject)).ToList().ForEach((_) =>
        {
            var clone = Instantiate(@object, RandomPosition(), Quaternion.identity, container.transform);
            objects.Add(clone.transform);
        });
    }

    private Vector2 RandomPosition()
    {
        var x = RandomRange(minX, maxX);
        var y = RandomRange(minY, maxY);
        var result = new Vector2(x, y);
        if (enemies.Any(enemy => enemy.position.x == result.x && enemy.position.y == result.y)
            //|| obstacles.Any(obstacle => obstacle.position.x == result.x && obstacle.position.y == result.y)
            || powerUps.Any(powerUp => powerUp.position.x == result.x && powerUp.position.y == result.y)
            || walls.Any(wall => wall.position.x == result.x && wall.position.y == result.y)
            )
            return RandomPosition();
        return result;
    }

    private void GeneratePowerUps()
    {
        GenerateObjects("powerUps", minRandPowerUps, maxRandPowerUps, powerUp, powerUps);
    }

    private void GenerateStars()
    {
        var voidContainer = new GameObject() { name = nameof(stars) };
        voidContainer.transform.SetParent(transform);
        foreach (var position in GeneratePositions().ToArray()
            .Except(Generate1LevelMazeWallPositions().ToArray()
            .Except(powerUps.Select(p => p.position).ToArray())))
        {
            var clone = Instantiate(star, position, Quaternion.identity, voidContainer.transform);
            clone.name = $"star({position.x},{position.y})";
            stars.Add(clone);
        }
    }

    private IEnumerable<Vector3> GenerateBorderPositions()
    {
        for (var x = minX; x <= maxX; x++)
        {
            foreach (var position in GenerateBorderPositions(x, minX, maxX, minY, maxY))
            {
                yield return position;
            }
        }
    }

    private IEnumerable<Vector3> GeneratePositions()
    {
        for (var x = minX + 1; x < maxX; x++)
        {
            for (var y = minY + 1; y < maxY; y++)
            {
                yield return new Vector3(x, y);
            }
        }
    }

    private void GenerateEnemies()
    {
        GenerateObjects("enemies", minRandEnemies, maxRandEnemies, enemy, enemies);
    }

    private int RandomRange(int a, int b) => Random.Range(a + 1, b);
    private void GenerateBorders()
    {
        var container = new GameObject() { name = "borders" };
        container.transform.SetParent(transform);
        foreach (var position in GenerateBorderPositions())
        {
            var clone = Instantiate(border, position, Quaternion.identity, container.transform);
            clone.name = $"border({position.x},{position.y})";
            borders.Add(clone);
        }
    }

    private void Generate1LevelMazeWalls()
    {
        var container = new GameObject() { name = "walls" };
        container.transform.SetParent(transform);
        foreach (var position in Generate1LevelMazeWallPositions())
        {
            var clone = Instantiate(border, position, Quaternion.identity, container.transform);
            clone.name = $"wall({position.x},{position.y})";
        }
    }

    private IEnumerable<Vector3> Generate1LevelMazeWallPositions()
    {
        for (var x = minX + 2; x < maxX; x += 2)
        {
            for (var y = minY + 2; y < maxY; y += 2)
            {
                yield return new Vector3(x, y);
            }
        }
    }

    private IEnumerable<Vector3> GenerateBorderPositions(int i, int minX, int maxX, int minY, int maxY)
    {
        yield return new Vector3(minX, i);
        yield return new Vector3(i, minY);
        yield return new Vector3(maxX, i);
        yield return new Vector3(i, maxY);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
