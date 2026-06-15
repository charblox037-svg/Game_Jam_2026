using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GlosCol
{
    public class Player : MonoBehaviour
    {
        public TextMeshProUGUI healthText; // Reference to the TextMeshProUGUI component for health display
        public Slider slider; // Reference to the UI Slider for health display
        public int health = 100;
        public float moveSpeed = 5f;
        public float jumpForce = 10f;
        public Transform groundCheck;
        public float groundCheckRadius = 0.2f;
        public LayerMask groundLayer;


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


            float moveInput = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);



            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                }
            }
            if (Input.GetMouseButtonDown(0)) // FIX: Use Input.GetMouseButtonDown for mouse button press
            {
                animator.Play("Player_Attack");
            }


            SetAnimation(moveInput);
            slider.value = health; // Update the slider value to reflect the current health
            healthText.text = "Health: " + health; // Update the health text display
        }



        private void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        private void SetAnimation(float moveInput)
        {
            if (isGrounded)
            {
                if (moveInput == 0)
                {
                    animator.Play("Player_Idle");
                }
                else if (moveInput > 0)
                {
                    animator.Play("Player_Run");
                    spriteRenderer.flipX = false; // Flip the sprite to face left
                }
                else
                {
                    animator.Play("Player_Run");
                    spriteRenderer.flipX = true; // Flip the sprite to face right
                }
            }
            else
            {
                if (rb.linearVelocity.y > 0)
                {
                    animator.Play("Player_Jump");
                }
                else
                {
                    animator.Play("Player_Fall");
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Damage")
            {
                health -= 25; // reduce health by 25 when colliding with an object tagged "Damage"
                Debug.Log("Player Health: " + health); // Log the current health to the console
                // Update the slider value to reflect the new health
                // Knockback effect (push player upwards)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                // Flash red to indicate damage
                StartCoroutine(BlinkRed());

                // Check if player is dead
                if (health <= 0)
                {
                    // Handle player death (e.g., reload scene, show game over screen)
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level_17"); // Reload the current scene (you can change this to a game over screen if you have one)
        }
    }
}