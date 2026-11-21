using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    // Define the type of movement
    public enum MovementType
    {
        Horizontal,
        Vertical
    }

    [Header("Patrol Points")]
    public Transform pointA; // The first point of the patrol
    public Transform pointB; // The second point of the patrol

    [Header("Enemy Settings")]
    public float speed = 2f;
    public MovementType movementType;

    // --- HEALTH SETTING ---
    [Header("Combat")]
    // This is the only line that was changed.
    // The enemy now requires 2 hits to be defeated.
    public int health = 2; 

    // --- Internal Variables (not visible in Inspector) ---
    private Transform currentTarget;

    // --- SETUP ---
    void Start()
    {
        // Align patrol points to the enemy's starting axis to ensure straight movement.
        // This prevents diagonal drifting.
        if (movementType == MovementType.Horizontal)
        {
            pointA.position = new Vector3(pointA.position.x, transform.position.y, transform.position.z);
            pointB.position = new Vector3(pointB.position.x, transform.position.y, transform.position.z);
        }
        else // Vertical
        {
            pointA.position = new Vector3(transform.position.x, pointA.position.y, transform.position.z);
            pointB.position = new Vector3(transform.position.x, pointB.position.y, transform.position.z);
        }

        // The enemy will start by moving towards Point B.
        currentTarget = pointB;
    }

    // --- MOVEMENT LOGIC (per frame) ---
    void Update()
    {
        // Safety check to prevent errors if the points aren't assigned in the Inspector.
        if (pointA == null || pointB == null)
        {
            return;
        }

        // Move the enemy towards its current target position.
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Check if the enemy has reached its target.
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            // If it has reached the target, switch to the other target.
            // This is a short-hand "if-else" statement called a ternary operator.
            currentTarget = (currentTarget == pointB) ? pointA : pointB;
        }
    }

    // --- COMBAT FUNCTIONS ---

    /// <summary>
    /// This is a public function that other scripts (like a bullet) can call to deal damage.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        // Subtract the incoming damage from our health.
        health -= damageAmount;

        // Check if health has dropped to 0 or below.
        if (health <= 0)
        {
            // If so, call the Die function.
            Die();
        }
    }

    /// <summary>
    /// This function handles the enemy's death.
    /// </summary>
    private void Die()
    {
        // You could add a particle effect for an explosion here before destroying the object.
        // For example: Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        
        // Destroy this enemy's GameObject, removing it from the game.
        Destroy(gameObject);
    }

    // --- EDITOR VISUALS ---

    /// <summary>
    /// This special function draws helpful visuals in the Scene view so you can see the patrol path.
    /// </summary>
    private void OnDrawGizmos()
    {
        // If both points are assigned, draw a line and spheres to represent the path.
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.15f);
            Gizmos.DrawSphere(pointB.position, 0.15f);
        }
    }
}