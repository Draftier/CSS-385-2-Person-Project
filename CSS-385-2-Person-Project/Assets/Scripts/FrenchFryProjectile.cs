using UnityEngine;

public class FrenchFryProjectile : MonoBehaviour
{
    [Header("Settings")]
    public float pushForce = 12f;
    public float lifetime = 2.5f;
    public GameObject impactEffect;

    private Vector2 direction;
    public float speed;
    private Rigidbody2D rb;

    public void Initialize(Vector2 fireDirection, float projectileSpeed)
    {
        direction = fireDirection;
        speed = projectileSpeed;
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);

        // Rotate projectile to face direction
        if (rb.linearVelocity != Vector2.zero) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other) return;

        // Player impact
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb)
            {
                direction = other.GetComponent<Rigidbody2D>().linearVelocity;
                Initialize(direction, speed);
                playerRb.AddForce(direction * pushForce, ForceMode2D.Impulse);
                SpawnImpactEffect();
            }
            Destroy(gameObject);
        }
        // Environment impact
        else if (other.CompareTag("Planet") || other.CompareTag("Obstacle"))
        {
            SpawnImpactEffect();
            Destroy(gameObject);
        }
    }

    void SpawnImpactEffect()
    {
        if (impactEffect)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
    }
}