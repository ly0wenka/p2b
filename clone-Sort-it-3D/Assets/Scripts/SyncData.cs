using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncData
{
    public Vector3Int[] Positions;
    public Color[] Colors;

    public static byte[] Serialize(object obj)
    {
        SyncData data = obj as SyncData;

        var result = new byte[
            12 * data.Positions.Length + 16 * data.Colors.Length 
        ];

        for (var i = 0; i < data.Positions.Length; i++)
        {
            BitConverter.GetBytes(data.Positions[i].x).CopyTo(result, 12 * i);
            BitConverter.GetBytes(data.Positions[i].y).CopyTo(result, 12 * i + 4);
            BitConverter.GetBytes(data.Positions[i].z).CopyTo(result, 12 * i + 8);
        }

        for (var i = 0; i < data.Colors.Length; i++)
        {
            BitConverter.GetBytes(data.Colors[i].r).CopyTo(result, 16 * i);
            BitConverter.GetBytes(data.Colors[i].g).CopyTo(result, 16 * i + 4);
            BitConverter.GetBytes(data.Colors[i].b).CopyTo(result, 16 * i + 8);
            BitConverter.GetBytes(data.Colors[i].a).CopyTo(result, 16 * i + 12);
        }

        return result;
    }

    public static object Deserialize(byte[] bytes)
    {
        var data = new SyncData();

        var length = bytes.Length / 28;
        
        data.Positions = new Vector3Int[length];
        data.Colors = new Color[length];
        
        for (var i = 0; i < data.Positions.Length; i++)
        {
            data.Positions[i].x = BitConverter.ToInt32(bytes,  12 * i);
            data.Positions[i].y = BitConverter.ToInt32(bytes,  12 * i + 4);
            data.Positions[i].z = BitConverter.ToInt32(bytes,  12 * i + 8);
        }

        for (var i = 0; i < data.Colors.Length; i++)
        {
            data.Colors[i].r = BitConverter.ToSingle(bytes,16 * i); 
            data.Colors[i].g = BitConverter.ToSingle(bytes,16 * i + 4); 
            data.Colors[i].b = BitConverter.ToSingle(bytes,16 * i + 8); 
            data.Colors[i].a = BitConverter.ToSingle(bytes,16 * i + 12); 
        }

        return data;
    }
}
