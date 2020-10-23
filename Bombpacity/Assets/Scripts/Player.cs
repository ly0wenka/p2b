//using Assets.Scripts.Direction;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Player : MonoBehaviour
//{
//    private Vector3 movement;

//    public int speed;

//    // Start is called before the first frame update
//    void Start()
//    {
//        speed = 10;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
//        //if (Input.GetKeyDown(KeyCode.UpArrow))
//        //{
//        //    transform.Translate(Vector3.up);
//        //}
//        //else if (Input.GetKeyDown(KeyCode.DownArrow))
//        //{
//        //    transform.Translate(Vector3.down);
//        //}
//        //else if (Input.GetKeyDown(KeyCode.RightArrow))
//        //{
//        //    transform.Translate(Vector3.right);
//        //}
//        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
//        //{
//        //    transform.Translate(Vector3.left);
//        //}
//    }

//    void FixedUpdate()
//    {
//        MoveCharacter(movement);
//    }

//    private void MoveCharacter(Vector3 movement)
//    {
//        GetComponent<Rigidbody2D>().velocity = movement*speed;
//    }
//}

using System.Linq;
using UnityEngine;
public class Player : Movement
{
    private float horizontal, vertical;
    private new Rigidbody2D rigidbody2D;
    public int lifeTotal;
    public int starTotal;
    public Field field;
    public PlayerInventoryDisplay playerInventoryDisplay;
    void Start()
    {
        starTotal = 0;
        lifeTotal = 3;
        playerInventoryDisplay.OnChangeLifeTotal(lifeTotal);
        playerInventoryDisplay.OnChangeStarTotal(starTotal);
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        //if (h != 0 && !isMoving) StartCoroutine(Move(h, v, rigidBody));
        //else if (v != 0 && !isMoving) StartCoroutine(Move(h, v, rigidBody));
        if (horizontal != 0 && !isMoving) StartCoroutine(MoveHorizontal(horizontal, rigidbody2D));
        else if (vertical != 0 && !isMoving) StartCoroutine(MoveVertical(vertical, rigidbody2D));
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    public void MoveUp() => StartCoroutine(MoveVertical(1, rigidbody2D));
    public void MoveRight() => StartCoroutine(MoveHorizontal(1, rigidbody2D));
    public void MoveDown() => StartCoroutine(MoveVertical(-1, rigidbody2D));
    public void MoveLeft() => StartCoroutine(MoveHorizontal(-1, rigidbody2D));

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.name.StartsWith("powerUp"))
        {
            lifeTotal++;
            playerInventoryDisplay.OnChangeLifeTotal(lifeTotal);
            Destroy(collider2D.gameObject);
        }
        else if (collider2D.name.StartsWith("star"))
        {
            starTotal++;
            playerInventoryDisplay.OnChangeStarTotal(starTotal);
            field.stars.Remove(field.stars.First(s => s.position == collider2D.transform.position));
            if (!field.stars.Any())
                print("win");
            collider2D.gameObject.name = $"{collider2D.gameObject.name}-grab";
            Destroy(collider2D.gameObject);
        }
        else if (collider2D.name.StartsWith("enemy"))
        {
            lifeTotal--;
            transform.position = new Vector3(0, 0);
            
            playerInventoryDisplay.OnChangeLifeTotal(lifeTotal);
        }
    }
}