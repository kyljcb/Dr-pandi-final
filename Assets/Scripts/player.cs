using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class player : MonoBehaviour
{
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private LayerMask jumpableGround;

    private bool dead = false;
    private float dirx = 0f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jump, land }

    // Start is called before the first frame update
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("death", false);
    }

    // Update is called once per frame
    private void Update()
    {
        dirx = CrossPlatformInputManager.GetAxis("Horizontal");
        rb.velocity = new Vector2(dirx * moveSpeed, rb.velocity.y);
        
        Jump();
        
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && dead == false)
        {
            Attack();
        }


        AnimationpUpdate();
    }
    public void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump") && IsGround())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        }
    }
    private void AnimationpUpdate()
    {
        MovementState state;
        if (dirx > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirx < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        
        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
        }

        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.land;
        }

        animator.SetInteger("state", (int)state);
    }

    private bool IsGround()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);

    }

    private void Attack()
    {
        animator.SetTrigger("attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<ZombieBehavior>().TakeDamage(2f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("void"))
        {
            Die();
        }
    }
    public void Die()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        sprite.flipX = false;
        animator.SetBool("death", true);
        dead = true;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
