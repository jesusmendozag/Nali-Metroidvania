using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] private int damage = 10;

    Rigidbody2D myRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public int SendDamage()
    {
        return damage;
    }

    void Movement()
    {
        myRB.linearVelocity = new Vector2(moveSpeed, myRB.linearVelocityY);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemySprite();
    }

    void FlipEnemySprite()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRB.linearVelocityX)), 1f);
    }
}
