using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab { get; set; }

    private Random _random;
    private float _timeSinceLastSpawn;
    private float _spawnRate;
    private readonly RangeInt _degree = new RangeInt(0, 361);
    private float _radius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_random == null)
            _random = new Random();

        if (_timeSinceLastSpawn >= _spawnRate)
        {
            var enemy = PrefabUtility.InstantiatePrefab(EnemyPrefab) as GameObject;
            var degrees = _random.Next(_degree.start, _degree.end);

            var x = _radius * Mathf.Cos(degrees * Mathf.Deg2Rad);
            if (Mathf.Abs(x) < 0.01f)
            {
                x = 0;
            }

            var y = _radius * Mathf.Cos(degrees * Mathf.Deg2Rad);
            if (Mathf.Abs(x) < 0.01f)
            {
                y = 0;
            }
        }
    }

}
