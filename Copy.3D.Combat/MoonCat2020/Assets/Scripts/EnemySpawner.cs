using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    private Object _enemyPrefab;

    public Random Random { get; set; }
    private float _timeSinceLastSpawn;
    private float _spawnRate;
    private readonly RangeInt _degree = new RangeInt(0, 361);
    private float _radius;
    
    private void Update()
    {
        Random ??= new Random();
        if (!(_timeSinceLastSpawn >= _spawnRate)) return;
        
        var enemy = PrefabUtility.InstantiatePrefab(_enemyPrefab) as GameObject;
        if (!enemy) return;

        var degrees = Random.Next(_degree.start, _degree.end);
        enemy.transform.position = CreatePosition(degrees);
        _timeSinceLastSpawn += Time.deltaTime;
    }

    public void Constructor(Object enemyPrefab, float spawnRate, float radius)
    {
        _enemyPrefab = enemyPrefab;
        _spawnRate = spawnRate;
        _radius = radius;
    }

    private Vector3 CreatePosition(int degrees)
    {
        var x = SetX(degrees);
        var y = SetY(degrees);

        return new Vector3(x, 0, y);
    }

    private float SetX(int degrees)
    {
        var x = _radius * Mathf.Cos(degrees * Mathf.Deg2Rad);
        if (Mathf.Abs(x) < 0.01f)
        {
            x = 0;
        }

        return x;
    }

    private float SetY(int degrees)
    {
        var y = _radius * Mathf.Cos(degrees * Mathf.Deg2Rad);
        if (Mathf.Abs(y) < 0.01f)
        {
            y = 0;
        }

        return y;
    }
}
