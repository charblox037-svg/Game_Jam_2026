using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

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

        private GameObject attackArea = default; // Reference to the attack area GameObject
        private bool attacking = false; // Flag to indicate if the player is currently attacking
        
        private float timeToAttack = 0.25f; // Time duration for the attack
        private float timer = 0f; // Timer to track the attack duration

        private Rigidbody2D rb;
        private bool isGrounded;
        private Animator animator;
        private SpriteRenderer spriteRenderer;


        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            attackArea = transform.GetChild(0).gameObject; // Assuming the attack area is the first child of the player
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
            if(Input.GetKeyDown(KeyCode.F))
            {
                Attack();
            }
            if(attacking)
            {                 
                timer += Time.deltaTime;
                if(timer >= timeToAttack)
                {
                    attacking = false;
                    attackArea.SetActive(attacking); // Deactivate the attack area after the attack duration
                    timer = 0f; // Reset the timer
                }
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
                else
                {
                    animator.Play("Player_Run");
                    spriteRenderer.flipX = moveInput < 0; // Flip the sprite based on movement direction
                }
            }
            else
            {
                if (rb.linearVelocityY > 0)
                {
                    animator.Play("Player_Jump");
                }
                else
                {
                    animator.Play("Player_Fall");
                }
            }

        }

        private void Attack()
        {            
            StopAllCoroutines(); // Stop any ongoing coroutines to prevent overlapping attacks
            animator.Play("Player_Attack");
            attacking = true;
            attackArea.SetActive(true); // Activate the attack area

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