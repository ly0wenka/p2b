using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkyBehaviourScript : MonoBehaviour
{
    public CircleCollider2D boxCollider2D;
    public float MaxSpeed = 10f;
    public float jumpForce = 700f;
    bool IsFacingRight = true;

    bool IsGrounded { get; set; } = false;
    
    public Transform Transform;
    public float GroundRadius = 0.2f;
    [SerializeField] private LayerMask WhatIsGround;
    public float PositionX;
    public float PositionY;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = Transform.GetComponent<CircleCollider2D>();
    }

    void FixedUpdate()
    {
        //IsGrounded = Physics2D.OverlapCircle(Transform.position, GroundRadius, WhatIsGround);
        PositionX = Input.GetAxis("Horizontal");
        PositionY = Transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        var body2D = GetComponent<Rigidbody2D>();
        //if (IsGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            body2D.AddForce(new Vector2(0f, jumpForce));
        }
        body2D.velocity = new Vector2(PositionX * MaxSpeed, body2D.velocity.y);

        if (PositionX > 0 && !IsFacingRight)
            Flip();
        else if (PositionX < 0 && IsFacingRight)
            Flip();

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Platforms") 
        {
            IsGrounded = true;
        }
    }

    // public bool IsGrounded()
    // {
    //     var extraHeightText = 500f;
    //     RaycastHit2D raycastHit2d = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + extraHeightText);
    //     Color rayColor = Color.green;
    //     Debug.DrawRay(boxCollider2D.bounds.center, Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
    //     Debug.Log(string.Format("RaycastHit2d: {0}", raycastHit2d.collider != null));
    //     return raycastHit2d.collider != null;
    // }
}
