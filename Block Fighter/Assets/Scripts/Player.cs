using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D rb;
    public Animator animator;
    public GameManager gm;
    public AudioManager am;

    private bool canDash = true;
    private bool isDashing = false;
    private float dashingPower = 4f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 2f;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Image imageCooldown;
    private float cooldownTimer = 0f;

    //double tap
    private const float double_click_time = 0.2f;
    private float lastClickTime;


    private void Awake()
    {
        am = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        imageCooldown.fillAmount = 0f;
    }

    void Update()
    {
        if (isDashing) { return; }

        if (Input.GetMouseButton(0))
        {
            // Get the player's screen position
            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);

            float timeSinceLastTimeClick = Time.time - lastClickTime;

            Debug.Log( timeSinceLastTimeClick);

            if (timeSinceLastTimeClick <= double_click_time && timeSinceLastTimeClick > 0.05 && canDash)
            {
                //Dash
                StartCoroutine(Dash());
                am.PlaySFX(am.dash);

                cooldownTimer = dashingCooldown;
                ApplyCooldown();
            }
            else
            {

                // Compare the mouse position with the player's screen position
                if (Input.mousePosition.x < playerScreenPos.x)
                {
                    rb.AddForce(Vector2.left * moveSpeed);
                    animator.SetBool("isRunning", true);
                    //am.PlaySFX(am.steps);

                    // Make player face left by flipping the sprite
                    transform.localScale = new Vector3(-3f, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    rb.AddForce(Vector2.right * moveSpeed);
                    animator.SetBool("isRunning", true);
                    //am.PlaySFX(am.steps);

                    // Make player face right by resetting the flip
                    transform.localScale = new Vector3(3f, transform.localScale.y, transform.localScale.z);
                }
            }

            lastClickTime = Time.time;
        }
        else
        {
            // Stop horizontal movement but preserve any vertical velocity
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("isRunning", false);
        }
        ApplyCooldown();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            CoinManager.coinAll += gm.coinLvl;
            PlayerPrefs.SetInt("allCoins", CoinManager.coinAll);
            am.PlaySFX(am.death);
            SceneManager.LoadScene("MainMenu");
            
        }

        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            gm.coinLvl++;
            am.PlaySFX(am.coin);
        }

    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void ApplyCooldown() 
    { 
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            imageCooldown.fillAmount = 0f;
        }
        else
        {
            imageCooldown.fillAmount = cooldownTimer / dashingCooldown;
        }
    }    
}
