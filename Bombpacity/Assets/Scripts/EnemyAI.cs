using Assets.Scripts.Domain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Movement
{
    Rigidbody2D rigidbody2D;
    float horizontal, vertical;
    [SerializeField]
    LayerMask blockingLayer;
    Direction direction = Direction.Up;
    Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        RandomDirection();
    }

    public void RandomDirection()
    {
        CancelInvoke(nameof(RandomDirection));

        List<Direction> lottery = new List<Direction>();
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(1, 0), blockingLayer))
        {
            lottery.Add(Direction.Right);
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(-1, 0), blockingLayer))
        {
            lottery.Add(Direction.Left);
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, 1), blockingLayer))
        {
            lottery.Add(Direction.Up);
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, -1), blockingLayer))
        {
            lottery.Add(Direction.Down);
        }

        Direction selection = lottery[Random.Range(0, lottery.Count)];
        if (selection == Direction.Up)
        {
            vertical = 1;
            horizontal = 0;
        }
        if (selection == Direction.Down)
        {
            vertical = -1;
            horizontal = 0;
        }
        if (selection == Direction.Right)
        {
            vertical = 0;
            horizontal = 1;
        }
        if (selection == Direction.Left)
        {
            vertical = 0;
            horizontal = -1;
        }
        Invoke(nameof(RandomDirection), Random.Range(3, 6));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var mementoColor = renderer.material.color;
        renderer.material.color = Color.red;
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
}