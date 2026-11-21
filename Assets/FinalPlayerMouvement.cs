using System.Collections;
using UnityEngine;

public class FinalPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;

    [Header("Jumping")]
    public float jumpForce = 15f;
    [SerializeField] private int extraJumpsValue = 1;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    [Header("Effects")]
    public GameObject doubleJumpEffectPrefab;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float moveInput;
    private bool isGrounded;
    private int extraJumps;
    private bool isFacingRight = true;
    private bool canShoot = false;
    private bool isInvincible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        extraJumps = extraJumpsValue;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        HandleJumping();
        HandleShooting();
        Flip();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;

                if (doubleJumpEffectPrefab != null)
                {
                    Instantiate(doubleJumpEffectPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void HandleShooting()
    {
        if (canShoot && Input.GetKeyDown(KeyCode.O))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            float forceDirection = isFacingRight ? 1f : -1f;
            rbBullet.AddForce(Vector2.right * forceDirection * bulletSpeed, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            extraJumps = extraJumpsValue;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isInvincible)
        {
            GameManager.instance.LoseHeart();
            StartCoroutine(InvincibilityFrames());
        }
    }

    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        // Flash the player to show invincibility
        for (int i = 0; i < 6; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        isInvincible = false;
    }

    private void Flip()
    {
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void ChangeSprite(Sprite newSprite)
    {
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }

    public void EnableShooting()
    {
        canShoot = true;
    }
}