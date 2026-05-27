using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private const float DoubleClickTime = 0.2f;
    private const float MinimumDoubleClickTime = 0.05f;
    private const string BlockTag = "Block";
    private const string CoinTag = "Coin";
    private const string MainMenuSceneName = "MainMenu";
    private const float FacingLeftX = -3f;
    private const float FacingRightX = 3f;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private GameManager gm;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Image imageCooldown;
    [SerializeField] private float dashingPower = 4f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 2f;

    private Rigidbody2D rb;
    private AudioManager audioManager;
    private bool canDash = true;
    private bool isDashing;
    private float cooldownTimer;
    private float lastClickTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gm ??= FindObjectOfType<GameManager>();

        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
        }
    }

    private void Start()
    {
        PlayerSkinApplier.ApplySelectedSkin(animator);
        SetCooldownFill(0f);
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            HandleMovementInput();
        }
        else
        {
            StopMoving();
        }

        ApplyCooldown();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(BlockTag))
        {
            audioManager?.PlayDeath();

            if (gm != null)
            {
                gm.PlayerDied();
            }
            else
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }

            return;
        }

        if (collision.gameObject.CompareTag(CoinTag))
        {
            Destroy(collision.gameObject);
            gm?.AddLevelCoin();
            audioManager?.PlayCoin();
        }
    }

    private void HandleMovementInput()
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= DoubleClickTime && timeSinceLastClick > MinimumDoubleClickTime && canDash)
        {
            StartCoroutine(Dash());
            audioManager?.PlayDash();
            cooldownTimer = dashingCooldown;
            return;
        }

        MoveTowardsPointer();
        lastClickTime = Time.time;
    }

    private void MoveTowardsPointer()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return;
        }

        Vector3 playerScreenPosition = mainCamera.WorldToScreenPoint(transform.position);
        bool shouldMoveLeft = Input.mousePosition.x < playerScreenPosition.x;
        float horizontalVelocity = shouldMoveLeft ? -moveSpeed : moveSpeed;

        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
        SetRunning(true);
        SetFacing(shouldMoveLeft ? FacingLeftX : FacingRightX);
    }

    private void StopMoving()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        SetRunning(false);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        SetTrailEmitting(true);

        yield return new WaitForSeconds(dashingTime);

        SetTrailEmitting(false);
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void ApplyCooldown()
    {
        cooldownTimer -= Time.deltaTime;
        SetCooldownFill(cooldownTimer <= 0f ? 0f : cooldownTimer / dashingCooldown);
    }

    private void SetCooldownFill(float fillAmount)
    {
        if (imageCooldown != null)
        {
            imageCooldown.fillAmount = fillAmount;
        }
    }

    private void SetRunning(bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
        }
    }

    private void SetFacing(float xScale)
    {
        Vector3 scale = transform.localScale;
        scale.x = xScale;
        transform.localScale = scale;
    }

    private void SetTrailEmitting(bool emitting)
    {
        if (tr != null)
        {
            tr.emitting = emitting;
        }
    }
}
