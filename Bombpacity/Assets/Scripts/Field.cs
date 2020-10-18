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
    public Transform obstacle;

    [HideInInspector] public List<Transform> stars = new List<Transform>();
    [HideInInspector] public List<Transform> enemies = new List<Transform>();
    [HideInInspector] public List<Transform> powerUps = new List<Transform>();
    [HideInInspector] public List<Transform> obstacles = new List<Transform>();

    public int minX, maxX, minY, maxY;
    public int minRandEnemies, maxRandEnemies;
    public int minRandPowerUps, maxRandPowerUps;
    public int minRandObstacles, maxRandObstacles;

    void Start()
    {
        minX = -5; maxX = 5; minY = -5; maxX = 5;
        GenerateBorders();
        GenerateEnemies();
        GeneratePowerUps();
        StartCoroutine(GenerateObstacles());
        GenerateStars();
    }

    private IEnumerator GenerateObstacles()
    {
        minRandObstacles = 2; maxRandObstacles = 10;
        GenerateObjects("obstacles", minRandObstacles, maxRandObstacles, obstacle, obstacles);
        yield return new WaitForFixedUpdate();
    }

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
            || obstacles.Any(obstacle => obstacle.position.x == result.x && obstacle.position.y == result.y)
            || powerUps.Any(powerUp => powerUp.position.x == result.x && powerUp.position.y == result.y)
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
        var x = minX;
        var voidContainer = new GameObject() { name = nameof(stars) };
        voidContainer.transform.SetParent(transform);
        for (char c = 'A'; c <= 'J'; c++, x++)
            for (int j = 1, y = minY; j <= 10; j++, y++)
            {
                if (!obstacles.Any(obstacle => obstacle.position.x == x && obstacle.position.y == y)
                    && !powerUps.Any(powerUp => powerUp.position.x == x && powerUp.position.y == y))
                {
                    var clone = Instantiate(star, new Vector2(x, y), Quaternion.identity, voidContainer.transform);
                    clone.name = $"star{j}{c}";
                    stars.Add(clone);
                }
            }
    }

    private void GenerateEnemies()
    {
        int minRandEnemies = 3;
        int maxRandEnemies = 5;
        GenerateObjects("enemies", minRandEnemies, maxRandEnemies, enemy, enemies);
    }

    private int RandomRange(int a, int b) => Random.Range(a + 1, b);
    private void GenerateBorders()
    {
        var min = -5;
        var max = 5;
        var container = new GameObject() { name = "borders" };
        container.transform.SetParent(transform);
        for (var i = min; i <= max; i++)
        {
            foreach (var position in GenerateBorderPositions(i, min, max))
            {
                var clone = Instantiate(border, position, Quaternion.identity, container.transform);
                clone.name = $"border({position.x},{position.y})";
                clone.gameObject.AddComponent<BoxCollider2D>();
            }
        }
    }

    private IEnumerable<Vector3> GenerateBorderPositions(int i, int min, int max)
    {
        yield return new Vector3(min, i);
        yield return new Vector3(i, min);
        yield return new Vector3(max, i);
        yield return new Vector3(i, max);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
