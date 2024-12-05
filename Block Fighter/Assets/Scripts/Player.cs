using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D rb;
    public Animator animator;
    public GameManager gm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Get the player's screen position
            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);

            // Compare the mouse position with the player's screen position
            if (Input.mousePosition.x < playerScreenPos.x)
            {
                rb.AddForce(Vector2.left * moveSpeed);
                animator.SetBool("isRunning", true);

                // Make player face left by flipping the sprite
                transform.localScale = new Vector3(-3f, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rb.AddForce(Vector2.right * moveSpeed);
                animator.SetBool("isRunning", true);

                // Make player face right by resetting the flip
                transform.localScale = new Vector3(3f, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            // Stop horizontal movement but preserve any vertical velocity
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("isRunning", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            CoinManager.coinAll += gm.coinLvl;
            SceneManager.LoadScene("MainMenu");
        }

        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            gm.coinLvl++;
        }

    }
}
