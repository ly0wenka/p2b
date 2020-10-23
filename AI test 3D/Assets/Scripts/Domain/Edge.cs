using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : IComparable<Edge>
{
    public float cost;
    public Vertex vertex;

    public Edge(Vertex vertex = null, float cost = 1f)
    {
        this.vertex = vertex;
        this.cost = cost;
    }

    public int CompareTo(Edge other)
    {
        var result = cost - other.cost;
        int idA = vertex.GetInstanceID();
        int idB = other.vertex.GetInstanceID();
        if (idA == idB)
            return 0;
        return (int)result;
    }

    public override bool Equals(object obj)
    {
        return obj is Edge edge &&
               vertex.id == edge.vertex.id;
    }

    public override int GetHashCode()
    {
        return vertex.GetHashCode();
    }
}
