using System.Collections;
using UnityEngine;

// This defines the possible states for our slime.
public enum SlimeState
{
    Patrolling,
    Chasing
}

// This is your SINGLE, CORRECT SlimeAI script.
public class SlimeAI : MonoBehaviour
{
    [Header("AI State")]
    [SerializeField] private SlimeState currentState = SlimeState.Patrolling;
    public Transform playerTransform;
    public float detectionRadius = 8f;
    public float attackHopForce = 7f;

    [Header("Movement")]
    [SerializeField] private float hopForce = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float hopDuration = 0.3f;

    [Header("Timing")]
    [SerializeField] private float minWaitTime = 1f;
    [SerializeField] private float maxWaitTime = 3f;

    // --- HEALTH SECTION ---
    [Header("Health")]
    // This is the health that your bullet will reduce.
    public int health = 4;

    // --- INTERNAL VARIABLES ---
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
        }
        StartCoroutine(HoppingRoutine());
    }

    // --- COMBAT FUNCTIONS ---
    // This is the public function that our bullet will call.
    public void TakeDamage(int damage)
    {
        // Subtract the damage from our health.
        health -= damage;

        // Check if the enemy has run out of health.
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy was defeated!");
        Destroy(gameObject);
    }

    // --- AI LOGIC (No changes needed here) ---
    private IEnumerator HoppingRoutine()
    {
        while (true)
        {
            if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= detectionRadius) { currentState = SlimeState.Chasing; } else { currentState = SlimeState.Patrolling; }
            switch (currentState) { case SlimeState.Patrolling: { float waitTime = Random.Range(minWaitTime, maxWaitTime); yield return new WaitForSeconds(waitTime); float randomDirection = Random.Range(0, 2) == 0 ? -1f : 1f; Flip(randomDirection); rb.linearVelocity = new Vector2(randomDirection * hopForce, jumpForce); break; } case SlimeState.Chasing: { yield return new WaitForSeconds(minWaitTime); float directionToPlayer = Mathf.Sign(playerTransform.position.x - transform.position.x); Flip(directionToPlayer); rb.linearVelocity = new Vector2(directionToPlayer * attackHopForce, jumpForce); break; } }
            yield return new WaitForSeconds(hopDuration);
            rb.linearVelocity = Vector2.zero;
        }
    }

    // --- HELPER FUNCTIONS (No changes needed here) ---
    private void OnCollisionEnter2D(Collision2D collision) { if (collision.gameObject.CompareTag("Player")) { /* Player damage logic */ } }
    private void Flip(float direction) { if ((isFacingRight && direction < 0f) || (!isFacingRight && direction > 0f)) { isFacingRight = !isFacingRight; Vector3 localScale = transform.localScale; localScale.x *= -1f; transform.localScale = localScale; } }
    private void OnDrawGizmosSelected() { Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, detectionRadius); }
}