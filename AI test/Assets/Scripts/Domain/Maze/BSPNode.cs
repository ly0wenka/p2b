using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BSPNode
{
    public Rect rect;
    public BSPNode nodeA;
    public BSPNode nodeB;

    public BSPNode(Rect rect)
    {
        this.rect = rect;
        nodeA = null;
        nodeB = null;
    }

    public void Split(float stopArea)
    {
        if (rect.width * rect.height >= stopArea)
            return;
        bool vertSplit = Random.Range(0, 1) == 1;
        float x, y, w, h;
        Rect rectA, rectB;
        
        if (!vertSplit)
        {
            x = rect.x;
            y = rect.y;
            w = rect.width;
            h = rect.height / 2f;
            rectA = new Rect(x, y, w, h);
            y += h;
            rectB = new Rect(x, y, w, h);
        }
        else
        {
            x = rect.x;
            y = rect.y;
            w = rect.width / 2f;
            h = rect.height;
            rectA = new Rect(x, y, w, h);
            y += w;
            rectB = new Rect(x, y, w, h);
        }
    }
}
