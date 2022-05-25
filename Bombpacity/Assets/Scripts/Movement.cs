using System;
using System.Collections;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    private const double TOLERANCE = 0.01;
    public int speed = 5;
    public Vector2 movement, endPos;

    protected bool isMoving = false;
    private float minClamp = 0f;
    private float maxClamp = 1f;
    [SerializeField] private float DeltaTimeSpeed => speed * Time.deltaTime;

    protected IEnumerator MoveHorizontal(float x, Rigidbody2D rigidBody)
    {
        isMoving = true;

        SetTransform(x);

        for (var movementProgress = 0f; movementProgress < Mathf.Abs(x);)
        {
            movementProgress += DeltaTimeSpeed;
            movementProgress = Mathf.Clamp(movementProgress, minClamp, maxClamp);
            movement = new Vector2(DeltaTimeSpeed * x, 0f);
            endPos = rigidBody.position + movement;

            if (Math.Abs(movementProgress - 1) < TOLERANCE) endPos = new Vector2(Mathf.RoundToInt(endPos.x), endPos.y);
            rigidBody.MovePosition(endPos);
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
    }

    private void SetTransform(float x)
    {
        var position = transform.position;
        position = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, -x * 90f);
    }

    protected IEnumerator MoveVertical(float y, Rigidbody2D rigidBody)
    {
        isMoving = true;

        var position = transform.position;
        position = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        transform.position = position;
        transform.rotation = y < 0 ? Quaternion.Euler(0, 0, y * 180f) : Quaternion.Euler(0, 0, 0);

        //Vector2 endPos, movement;

        for (var movementProgress = 0f; movementProgress < Mathf.Abs(y);)
        {
            movementProgress += DeltaTimeSpeed;
            movementProgress = Mathf.Clamp(movementProgress, minClamp, maxClamp);

            movement = new Vector2(0f, DeltaTimeSpeed * y);
            endPos = rigidBody.position + movement;

            if (Math.Abs(movementProgress - 1) < TOLERANCE) endPos = new Vector2(endPos.x, Mathf.RoundToInt(endPos.y));
            rigidBody.MovePosition(endPos);
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
    }

    [Obsolete] protected IEnumerator Move(float x, float y, Rigidbody2D rigidBody)
    {
        isMoving = true;

        var position = transform.position;
        position = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        transform.position = position;
        if (x == 0)
            transform.rotation = Quaternion.Euler(0, 0, -x * 90f);
        else
            transform.rotation = y < 0 ? Quaternion.Euler(0, 0, y * 180f) : Quaternion.Euler(0, 0, 0);


        Vector2 endPos, movement;

        for (var movementProgress = 0f; movementProgress < Mathf.Abs(y);)
        {
            movementProgress += DeltaTimeSpeed;
            movementProgress = Mathf.Clamp(movementProgress, minClamp, maxClamp);

            if (x == 0) 
                movement = new Vector2(DeltaTimeSpeed * x, 0f);
            else
                movement = new Vector2(0f, DeltaTimeSpeed * y);

            endPos = rigidBody.position + movement;

            if (x == 0)
                if (Math.Abs(movementProgress - 1) < TOLERANCE) endPos = new Vector2(endPos.x, Mathf.Round(endPos.y));
            else
                if (Math.Abs(movementProgress - 1) < TOLERANCE) endPos = new Vector2(Mathf.Round(endPos.x), endPos.y);

            rigidBody.MovePosition(endPos);
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
    }
}
