using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerRedAnt : MonoBehaviour
{
    public static int EnemyHealth = 100; // Health of the WalkerRedAnt
    public float jumpForce = 10f; // Force applied when the WalkerRedAnt takes damage

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }
    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0)
            {
                animator.Play("WalkerRedAnt_Idle");
            }
            else
            {
                animator.Play("WalkerRedAnt_Run");
                spriteRenderer.flipX = moveInput < 0; // Flip the sprite based on movement direction
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            EnemyHealth -= 25; // reduce health by 25 when colliding with an object tagged "Damage"
            Debug.Log("Enemy Health: " + EnemyHealth); // Log the current health to the console
                                                   // Update the slider value to reflect the new health
                                                   // Knockback effect (push player upwards)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // Flash red to indicate damage
            StartCoroutine(BlinkRed());

            // Check if Enemy is dead
            if (EnemyHealth <= 0)
            {
                // Handle enemy death (e.g., play death animation, drop loot)
                Die();
            }
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red; // Change color to red
        yield return new WaitForSeconds(0.1f); // Wait for a short time
        spriteRenderer.color = Color.white; // Change color back to normal
    }

    private void Die()
    {
        Destroy(gameObject); // Destroy the current game object
    }
}
