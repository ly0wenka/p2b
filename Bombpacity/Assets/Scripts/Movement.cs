using System.Collections;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public int speed = 5;
    public Vector2 movement, endPos;

    protected bool isMoving = false;
    private float minClamp = 0f;
    private float maxClamp = 1f;
    private float deltaTimeSpeed { get { return speed * Time.deltaTime; } }

    protected IEnumerator MoveHorizontal(float x, Rigidbody2D rigidBody)
    {
        isMoving = true;

        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        transform.rotation = Quaternion.Euler(0, 0, -x * 90f);


        for (var movementProgress = 0f; movementProgress < Mathf.Abs(x);)
        {
            movementProgress += deltaTimeSpeed;
            movementProgress = Mathf.Clamp(movementProgress, minClamp, maxClamp);
            movement = new Vector2(deltaTimeSpeed * x, 0f);
            endPos = rigidBody.position + movement;

            if (movementProgress == 1) endPos = new Vector2(Mathf.Round(endPos.x), endPos.y);
            rigidBody.MovePosition(endPos);
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
    }

    protected IEnumerator MoveVertical(float y, Rigidbody2D rigidBody)
    {
        isMoving = true;

        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        transform.rotation = y < 0 ? Quaternion.Euler(0, 0, y * 180f) : Quaternion.Euler(0, 0, 0);

        //Vector2 endPos, movement;

        for (var movementProgress = 0f; movementProgress < Mathf.Abs(y);)
        {
            movementProgress += deltaTimeSpeed;
            movementProgress = Mathf.Clamp(movementProgress, minClamp, maxClamp);

            movement = new Vector2(0f, deltaTimeSpeed * y);
            endPos = rigidBody.position + movement;

            if (movementProgress == 1) endPos = new Vector2(endPos.x, Mathf.Round(endPos.y));
            rigidBody.MovePosition(endPos);
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
    }

    protected IEnumerator Move(float x, float y, Rigidbody2D rigidBody)
    {
        isMoving = true;

        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        if (x == 0)
            transform.rotation = Quaternion.Euler(0, 0, -x * 90f);
        else
            transform.rotation = y < 0 ? Quaternion.Euler(0, 0, y * 180f) : Quaternion.Euler(0, 0, 0);


        Vector2 endPos, movement;

        for (var movementProgress = 0f; movementProgress < Mathf.Abs(y);)
        {
            movementProgress += deltaTimeSpeed;
            movementProgress = Mathf.Clamp(movementProgress, minClamp, maxClamp);

            if (x == 0) 
                movement = new Vector2(deltaTimeSpeed * x, 0f);
            else
                movement = new Vector2(0f, deltaTimeSpeed * y);

            endPos = rigidBody.position + movement;

            if (x == 0)
                if (movementProgress == 1) endPos = new Vector2(endPos.x, Mathf.Round(endPos.y));
            else
                if (movementProgress == 1) endPos = new Vector2(Mathf.Round(endPos.x), endPos.y);

            rigidBody.MovePosition(endPos);
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
    }
}
