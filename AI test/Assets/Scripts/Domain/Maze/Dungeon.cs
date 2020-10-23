using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public Vector2 dungeonSize;
    public float roomAreaToStop;
    public float middleTrheshold;
    public GameObject floorPrefab;
    private BSPNode root;
    private List<BSPNode> nodeList;

    public void Split()
    {
        float x, y, w, h;
        x = dungeonSize.x / 2f * -1f;
        y = dungeonSize.y / 2f * -1f;
        w = dungeonSize.x;
        h = dungeonSize.y;
        Rect rootRect = new Rect(x, y, w, h);
        root = new BSPNode(rootRect);
    }

    public void DrawNode(BSPNode n)
    {
        GameObject go = Instantiate(floorPrefab);
        var position = new Vector3(n.rect.x, 0f, n.rect.y);
        var scale = new Vector3(n.rect.width, 1f, n.rect.height);
        go.transform.position = position;
        go.transform.localScale = scale;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
