using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damageToDeal = 1;

    void Start()
    {
        // Destroy the bullet after 2 seconds if it hits nothing.
        Destroy(gameObject, 2f);
    }

    /// <summary>
    /// Called when the bullet's trigger enters another collider.
    /// This function now checks for different types of enemies.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // --- UPDATED ENEMY DETECTION LOGIC ---

        // 1. Check if the object we hit has a SlimeAI component.
        SlimeAI slime = otherCollider.GetComponent<SlimeAI>();
        if (slime != null)
        {
            // It's a slime! Deal damage and destroy the bullet.
            slime.TakeDamage(damageToDeal);
            Destroy(gameObject);
            return; // Exit the function immediately.
        }

        // 2. If it wasn't a slime, check if it has a PatrolEnemy component.
        PatrolEnemy patrolEnemy = otherCollider.GetComponent<PatrolEnemy>();
        if (patrolEnemy != null)
        {
            // It's a patrol enemy! Deal damage and destroy the bullet.
            patrolEnemy.TakeDamage(damageToDeal);
            Destroy(gameObject);
            return; // Exit the function immediately.
        }

        // 3. (Optional) If it wasn't any kind of enemy, check if it was a wall.
        // This makes your bullets disappear on walls instead of flying through them.
        if (otherCollider.CompareTag("Wall") || otherCollider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}