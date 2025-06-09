using UnityEngine;

public class FrenchFryEnemy : Enemy
{
    // The target the enemy moves toward (not the player)
    public Transform targetPosition;

    // The player the enemy aims at
    public Transform player;

    // Projectile
    public float fireForce = 10f;
    public float fireRate = 1f;
    public float detectionRadius = 5f;

    public float desiredDistance = 3f;
    public float moveSpeed = 2f;

    private float fireCooldown = 0f;

    void Update()
    {
        Vector2 moveDir = (targetPosition.position - transform.position).normalized;
        transform.position += (Vector3)moveDir * moveSpeed * Time.deltaTime;

        float playerDistance = Vector2.Distance(transform.position, player.position);

        if (playerDistance <= detectionRadius)
        {
            // Aim at the player
            Vector2 shootDir = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Fire at the player if cooldown passed
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Fire(shootDir);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    void Fire(Vector2 direction)
    {
        float spawnDistance = 0.6f;
        Vector2 spawnPos = (Vector2)transform.position + direction * spawnDistance;

        GameObject proj = Instantiate(enemyProjectile, spawnPos, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * fireForce;
    }

    public override void TakeDamage(Projectile projectile)
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashWhite());
        health -= projectile.damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            TakeDamage(projectile);
        }
    }
}
