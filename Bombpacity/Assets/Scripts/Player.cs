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