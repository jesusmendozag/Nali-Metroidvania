using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float runSpeed = 15f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float deathKick = 5f;
    [SerializeField] float deathJump = 5f;
    [SerializeField] float loadDelay = 2f;

    [Header("Ataque")]
    [SerializeField] Transform attackPoint;          
    [SerializeField] float attackRadius = 0.8f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float attackHitDelay = 0.15f;   
    [SerializeField] float attackDuration = 0.4f;    
    [SerializeField] float attackMoveMultiplier = 0.3f; 

    [Header("Daño")]
    [SerializeField] int contactDamage = 10;
    [SerializeField] float hurtDuration = 0.3f;      
    [SerializeField] float invulnerabilityTime = 1f;
    [SerializeField] Vector2 knockback = new Vector2(6f, 4f);

    //Cache
    Rigidbody2D myRB;
    PolygonCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    Animator myAnimator;

    //Strings
    private string GROUND_GROUND = "Ground";
    private string ENEMY_TAG = "Enemy";

    //Animator parameters
    private string PLAYER_JUMPING = "Jumping";
    private string PLAYER_RUNNING = "Running";
    private string PLAYER_DYING = "Dying";
    private string PLAYER_ATTACK = "Attack";   
    private string PLAYER_HURT = "Hurt";       

    //Checkers
    Vector2 moveInput;
    bool isAlive = true;
    bool isAttacking;
    bool isHurt;
    bool isInvulnerable;
    public bool abilityOcelote = false;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<PolygonCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        if (!isHurt) { Run(); }          
        FlipSprite();
        AnimationJump();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        float speedFactor = isAttacking ? attackMoveMultiplier : 1f;
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed * speedFactor, myRB.linearVelocityY);
        myRB.linearVelocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRB.linearVelocityX) > Mathf.Epsilon;
        myAnimator.SetBool(PLAYER_RUNNING, playerHasHorizontalSpeed);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive || isHurt) { return; }
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask(GROUND_GROUND))) { return; }

        if (value.isPressed)
        {
            myRB.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnAttack(InputValue value)
    {
        if (!isAlive || isAttacking || isHurt) { return; }
        if (value.isPressed)
        {
            StartCoroutine(AttackProcess());
        }
    }

    IEnumerator AttackProcess()
    {
        isAttacking = true;
        myAnimator.SetTrigger(PLAYER_ATTACK);

        yield return new WaitForSeconds(attackHitDelay);

        // Si nos golpearon durante el wind-up, se cancela el golpe
        if (!isHurt && isAlive)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
            foreach (Collider2D hit in hits)
            {
                
                Destroy(hit.gameObject);
            }
        }

        yield return new WaitForSeconds(Mathf.Max(0f, attackDuration - attackHitDelay));
        isAttacking = false;
    }

    
    public void TakeDamage(int amount, Vector2 sourcePosition)
    {
        if (!isAlive || isInvulnerable) { return; }

        FindAnyObjectByType<Gamesession>().RemoveHealth(amount);

        
        if (!isAlive) { return; }

        StartCoroutine(HurtProcess(sourcePosition));
    }

    IEnumerator HurtProcess(Vector2 sourcePosition)
    {
        isHurt = true;
        isInvulnerable = true;
        isAttacking = false;   
        myAnimator.SetTrigger(PLAYER_HURT);

        float direction = Mathf.Sign(transform.position.x - sourcePosition.x);
        myRB.linearVelocity = new Vector2(direction * knockback.x, knockback.y);

        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;

        yield return new WaitForSeconds(Mathf.Max(0f, invulnerabilityTime - hurtDuration));
        isInvulnerable = false;
    }

    private void AnimationJump()
    {
        myAnimator.SetBool(PLAYER_JUMPING, !myFeet.IsTouchingLayers(LayerMask.GetMask(GROUND_GROUND)));
    }

    public void Die()
    {
        if (!isAlive) { return; } 

        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    void FlipSprite()
    {
        if (isHurt) { return; }   // no voltear al ser empujado hacia atrás
        bool playerHasHorizontalSpeed = Mathf.Abs(myRB.linearVelocityX) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRB.linearVelocityX), 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != ENEMY_TAG) { return; }

        if (myFeet.IsTouchingLayers(LayerMask.GetMask(ENEMY_TAG)))
        {
            Destroy(collision.gameObject);
            myRB.linearVelocity = new Vector2(0f, deathJump);
        }
        else if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask(ENEMY_TAG)))
        {
            TakeDamage(contactDamage, collision.transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    IEnumerator DieProcess()
    {
        isAlive = false;

        myAnimator.SetBool(PLAYER_JUMPING, false);
        myAnimator.SetBool(PLAYER_RUNNING, false);

        myAnimator.SetTrigger(PLAYER_DYING);
        myRB.linearVelocity = new Vector2(0f, deathKick);
        
        // ¡Apagar ambos colliders para que caiga por el vacío!
        myFeet.enabled = false;
        myBodyCollider.enabled = false; // <--- Agrega esta línea

        yield return new WaitForSeconds(loadDelay);
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
    }
}