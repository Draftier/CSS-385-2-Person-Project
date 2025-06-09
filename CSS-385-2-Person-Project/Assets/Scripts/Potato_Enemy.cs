using UnityEngine;

public class Potato_Enemy : Enemy
{
    public bool isRotating = false;
    public bool spinningFromHit = false;
    private Coroutine spinCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        // MoveTowardsPlanet();
    }

    private void StartSpin()
    {
        if (!spinningFromHit)
        {
            spinningFromHit = true;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // Freeze position but allow rotation
            rb.constraints = RigidbodyConstraints2D.FreezePosition;

            // Set spin speed in degrees/sec
            rb.angularVelocity = 360f;
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
            StartSpin();
        }
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
}
