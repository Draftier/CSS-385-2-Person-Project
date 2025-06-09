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
    public static int enemyCount;
    public Coroutine flashCoroutine;
    public abstract void TakeDamage(Projectile projectile);
    public abstract void OnTriggerEnter2D(Collider2D other);
    private bool isCounted;
    public Rigidbody2D rb;
    private float knockbackDuration = 0.3f;
    private float knockbackTimer = 0f;

    public bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        if (isCounted)
        {
            isCounted = false;
            enemyCount--;

            if (Enemy_Manager.Instance != null)
            {
                Enemy_Manager.Instance.OnEnemyDestroyed();
            }
        }
    }

    private void OnEnable()
    {
        if (!isCounted)
        {
            isCounted = true;
            enemyCount++;
        }
    }

    public IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }
    // Move toward the target if not knocked back
    private void FixedUpdate()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
        }
        else
        {
            MoveTowardsPlanet();
        }

        ClampPositionWithinScreen(0.5f);
    }

    public void MoveTowardsPlanet()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    public void ApplyKnockback(Vector2 force)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
        knockbackTimer = knockbackDuration;
    }
    // public void MoveTowardsPlanet()
    // {
    //     // // Get position of player and direction to player
    //     // Vector2 targetPosition = target.position;
    //     // Vector2 direction = (targetPosition - (Vector2)transform.position);

    //     // // Normalize the direction vector
    //     // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    //     // Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

    //     // // Rotate towards the target rotation
    //     // float rotationSpeed = 120f;
    //     // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    //     // // Move towards the target position based upon the speed and direction
    //     // float speed = 10f;
    //     // transform.position += transform.up * speed * Time.deltaTime;
    //     if (target == null) return;

    //     // 1. Get direction to target
    //     Vector2 direction = (target.position - transform.position).normalized;

    //     // 2. Move towards the target
    //     transform.position += (Vector3)direction * speed * Time.deltaTime;

    //     // 3. Rotate to face target (optional, visual only)
    //     // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    //     // transform.rotation = Quaternion.Euler(0, 0, angle);

    //     ClampPositionWithinScreen(-5.0f); // keep enemies inside with a small buffer
    // }
    void ClampPositionWithinScreen(float buffer = 0.5f)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Calculate the screen bounds based on camera size and aspect
        float screenHalfWidth = cam.orthographicSize * cam.aspect;
        float screenHalfHeight = cam.orthographicSize;

        Vector2 minBounds = (Vector2)cam.transform.position - new Vector2(screenHalfWidth, screenHalfHeight) + Vector2.one * buffer;
        Vector2 maxBounds = (Vector2)cam.transform.position + new Vector2(screenHalfWidth, screenHalfHeight) - Vector2.one * buffer;

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minBounds.x, maxBounds.x);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minBounds.y, maxBounds.y);

        transform.position = clampedPos;
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         rb.linearVelocity = Vector2.zero;
    //         rb.angularVelocity = 0.0f;
    //     }        
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 knockDir = (transform.position - collision.transform.position).normalized;
            ApplyKnockback(knockDir * 5f); // Tune force as needed
        }
    }

}
