using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D rb;

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
            }
            else
            {
                rb.AddForce(Vector2.right * moveSpeed);
            }
        }
        else
        {
            // Stop horizontal movement but preserve any vertical velocity
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Block")
        {
            //Later change to menu scene
            SceneManager.LoadScene(0);
        }
    }
}
