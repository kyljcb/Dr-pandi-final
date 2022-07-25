using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform player;
    [SerializeField] private float agroRange;
    [SerializeField] private float attackRange;

    private float maxHealth = 2f;
    private float currentHealth;

    Animator animator;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if(distToPlayer < agroRange)
        {
            ChasePlayer();
            animator.SetBool("agro", true);

        }
        
        else
        {
            rb.velocity = new Vector2(0, 0);
            animator.SetBool("agro", false);
        }
        
        if(distToPlayer < attackRange)
        {
            animator.SetBool("attack", true);
        }
        else
        {
            animator.SetBool("attack", false);
        }
    }

    private void ChasePlayer()
    {
        if(transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            transform.localScale = new Vector2(0.3f, 0.2f);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            transform.localScale = new Vector2(-0.3f, 0.2f);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        animator.SetBool("death", true);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        
    }
    private void Stop()
    {
        animator.SetBool("death", false);
    }
}
