using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health = 100f; // Health of the enemy
    public float speed = 2f; // Movement speed of the enemy
    public GameObject enemyProjectile; // Projectile prefab for the enemy
    public GameObject deathEffect; // Effect to play on death
    public float flashDuration;
    public Transform target;
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public Color originalColor;
    private int hitCount;
    private int enemyCount;
    public Coroutine flashCoroutine;
    public abstract void TakeDamage();
    public abstract void OnTriggerEnter2D(Collider2D other);

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }

    public void MoveTowardsPlanet()
    {
        // // Get position of player and direction to player
        // Vector2 targetPosition = target.position;
        // Vector2 direction = (targetPosition - (Vector2)transform.position);

        // // Normalize the direction vector
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        // Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // // Rotate towards the target rotation
        // float rotationSpeed = 120f;
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // // Move towards the target position based upon the speed and direction
        // float speed = 10f;
        // transform.position += transform.up * speed * Time.deltaTime;
        if (target == null) return;

        // 1. Get direction to target
        Vector2 direction = (target.position - transform.position).normalized;

        // 2. Move towards the target
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // 3. Rotate to face target (optional, visual only)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
