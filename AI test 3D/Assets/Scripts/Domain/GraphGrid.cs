using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GraphGrid : Graph
{
    public GameObject obstaclePrefab;
    public string mapName = "arena.map";
    public bool get8Vicinity = false;
    public float cellSize = 1f;
    [Range(0, Mathf.Infinity)]
    public float maximumCost = Mathf.Infinity;
    string mapsDir = "Maps";
    int numCols;
    int numRows;
    GameObject[] vertexObjs;
    bool[,] mapVertices;
    private float defaultCost = 1.0f;

    private int GridToId(int x, int y) => Mathf.Max(numRows, numCols) * y + x;
    private Vector2 IdToGrid(int id)
    {
        Vector2 location = Vector2.zero;
        location.y = Mathf.Floor(id / numCols);
        location.x = Mathf.Floor(id % numCols);
        return location;
    }

    private void LoadMap(string filename)
    {
        var path = Path.Combine(Application.dataPath, mapsDir, filename);
        try
        {
            var streamReader = new StreamReader(path);
            using (streamReader)
            {
                var j = 0;
                var i = 0;
                var id = 0;
                var line = default(string);
                var position = Vector3.zero;
                var scale = Vector3.zero;

                line = streamReader.ReadLine();
                line = streamReader.ReadLine();
                numRows = int.Parse(line.Split(' ')[1]);
                line = streamReader.ReadLine();
                numCols = int.Parse(line.Split(' ')[1]);
                line = streamReader.ReadLine();

                vertices = new List<Vertex>(numRows * numCols);
                neighbours = new List<List<Vertex>>(numRows * numCols);
                costs = new List<List<float>>(numRows * numCols);
                vertexObjs = new GameObject[numRows * numCols];
                mapVertices = new bool[numRows, numCols];
                for (i = 0; i < numRows; i++)
                {
                    line = streamReader.ReadLine();
                    for (j = 0; j < numCols; j++)
                    {
                        var isGround = true;
                        if (line[j] != '.')
                            isGround = false;
                        mapVertices[i, j] = isGround;

                        position.x = j * cellSize;
                        position.z = i * cellSize;
                        id = GridToId(j, i);
                        if (isGround)
                        {
                            vertexObjs[id] = Instantiate(vertexPrefab, position, Quaternion.identity) as GameObject;
                        }
                        else
                        {
                            vertexObjs[id] = Instantiate(obstaclePrefab, position, Quaternion.identity) as GameObject;
                        }

                        vertexObjs[id].name = vertexObjs[id].name.Replace("(Clone)", id.ToString());
                        var v = vertexObjs[id].AddComponent<Vertex>();
                        v.id = id;
                        vertices.Add(v);
                        neighbours.Add(new List<Vertex>());
                        costs.Add(new List<float>());
                        var y = vertexObjs[id].transform.localScale.y;
                        scale = new Vector3(cellSize, y, cellSize);
                        vertexObjs[id].transform.localScale = scale;
                        vertexObjs[id].transform.parent = gameObject.transform;
                    }
                }

                for (i = 0; i < numRows; i++)
                {
                    for (j = 0; j < numCols; j++)
                    {
                        SetNeighbours(j, i);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }
    }

    private void SetNeighbours(int x, int y, bool get8 = false)
    {
        var col = x;
        var row = y;
        int i, j;
        var vertexId = GridToId(x, y);
        neighbours[vertexId] = new List<Vertex>();
        costs[vertexId] = new List<float>();
        var pos = new Vector2[0];

        if (get8)
        {
            pos = new Vector2[8]; int c = 0;
            for (i = row - 1; i <= row + 1; i++)
            {
                for (j = col - 1 ; j <= col; j++)
                {
                    pos[c] = new Vector2(j, i);
                    c++;
                }
            }
        }
        else
        {
            pos = new Vector2[4];
            pos[0] = new Vector2(col, row - 1);
            pos[1] = new Vector2(col - 1, row);
            pos[2] = new Vector2(col + 1, row);
            pos[3] = new Vector2(col, row + 1);
        }
        foreach (Vector2 p in pos)
        {
            i = (int)p.y;
            j = (int)p.x;
            if (i < 0 || j < 0)
                continue;
            if (i >= numRows || j >= numCols)
                continue;
            if (i == row && j == col)
                continue;
            if (!mapVertices[i, j])
                continue;
            var id = GridToId(j, i);
            neighbours[vertexId].Add(vertices[id]);
            costs[vertexId].Add(defaultCost);
        }
    }

    public override void LoadGraph()
    {
        LoadMap(mapName);
    }

    //public override Vertex GetNearestVertex(Vector3 position)
    //{
    //    position.x = Mathf.Floor(position.x / cellSize);
    //    position.y = Mathf.Floor(position.z / cellSize);
    //    int col = (int)position.x;
    //    int row = (int)position.z;
    //    int id = GridToId(col, row);
    //    return vertices[id];
    //}

    public override Vertex GetNearestVertex(Vector3 position)
    {
        //position.x = Mathf.Floor(position.x / cellSize);
        //position.y = Mathf.Floor(position.z / cellSize);
        int col = (int)position.x;
        int row = (int)position.z;
        var p = new Vector2(col, row);
        var explored = new List<Vector2>();
        var queue = new Queue<Vector2>();
        queue.Enqueue(p);
        do
        {
            p = queue.Dequeue();
            col = (int)p.x;
            row = (int)p.y;
            int id = GridToId(col, row);
            if (mapVertices[row, col])
                return vertices[id];
            if (!explored.Contains(p))
            {
                explored.Add(p);
                int i, j;
                for (i = row - 1; i <= row + 1; i++)
                {
                    for (j = col - 1; j <= col + 1; j++)
                    {
                        if (i < 0 || j < 0)
                            continue;
                        if (j >= numCols || i >= numRows)
                            continue;
                        if (i == row && j == col)
                            continue;
                        queue.Enqueue(new Vector2(j, i));
                    }
                }
            }
        } while (queue.Count != 0);
        throw new NotImplementedException();
    }
}
