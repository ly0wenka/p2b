using Assets.Scripts.Domain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyAI.Vector2WithColorDirection;

public class EnemyAI : Movement
{
    public new Rigidbody2D rigidbody2D;
    public float horizontal, vertical;
    [SerializeField]
    public LayerMask blockingLayer;
    public new
        //Direction direction = Direction.Up;
        Renderer renderer;

    public Transform target;

    public class Vector2Direction
    {
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Down = new Vector2(0, -1);
    }
    
    void Start()
    {
        renderer = GetComponent<Renderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        RandomDirection();
    }
    
    public void RandomDirection()
    {      
        renderer.material.color = Color.magenta;

        CancelInvoke(nameof(RandomDirection));
        print($"{ target.position.x}, { target.position.y}");
        var lottery = new List<Vector2WithColor>();
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + Right.Vector2, blockingLayer))
        {
            lottery.Add(Right);
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + Left.Vector2, blockingLayer))
        {
            lottery.Add(Left);
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + Up.Vector2, blockingLayer))
        {
            lottery.Add(Up);
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + Down.Vector2, blockingLayer))
        {
            lottery.Add(Down);
        }

        var selection = lottery[Random.Range(0, lottery.Count)];
        vertical = selection.Vector2.x;
        horizontal = selection.Vector2.y;
        renderer.material.color = selection.Color;

        Invoke(nameof(RandomDirection), Random.Range(3, 6));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var mementoColor = renderer.material.color;
        RandomDirection();
        //renderer.material.color = mementoColor;
    }

    private void FixedUpdate()
    {
        if (vertical != 0 && isMoving == false)
        {
            StartCoroutine(MoveVertical(vertical, rigidbody2D));
        }
        else if (horizontal != 0 && isMoving == false)
        {
            StartCoroutine(MoveHorizontal(horizontal, rigidbody2D));
        }
    }
    
    public class Vector2WithColor
    {
        public Vector2 Vector2 { get; set; }
        public Color Color { get; set; }
    }
    
    public class Vector2WithColorDirection
    {
        public static readonly Vector2WithColor Right = new Vector2WithColor
        {
            Vector2 = new Vector2(1, 0),
            Color = Color.red
        };
        public static readonly Vector2WithColor Left = new Vector2WithColor
        {
            Vector2 = new Vector2(-1, 0),
            Color = Color.blue
        };
        public static readonly Vector2WithColor Up = new Vector2WithColor
        {
            Vector2 = new Vector2(0, 1),
            Color = Color.yellow
        };
        public static readonly Vector2WithColor Down = new Vector2WithColor
        {
            Vector2 = new Vector2(0, -1),
            Color = Color.green
        };
    }
}