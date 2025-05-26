using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f; // Health of the enemy
    public float speed = 2f; // Movement speed of the enemy
    public GameObject enemyProjectile; // Projectile prefab for the enemy
    public GameObject deathEffect; // Effect to play on death
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private int hitCount;
    private int enemyCount;
    public Transform target;

    private void Awake()
    {
        hitCount = 0;
        enemyCount++;
    }

    private void OnDestroy()
    {
        enemyCount--;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            // Check type of bullet

        }
    }

    private void TakeDamage(Vector2 bulletVel, float damage)
    {
        health -= damage;
        hitCount++;
        // Reduce health by damage amount
        // Check hitcount and apply damage
        // Change sprite
    }
}
