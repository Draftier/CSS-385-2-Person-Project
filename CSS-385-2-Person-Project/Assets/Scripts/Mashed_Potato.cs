using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Mashed_Potato : Enemy
{
    public float shockwaveRadius = 1.0f;
    public float projectileKnockBackForce = 5.0f;
    public float playerKnockBackForce = 2.0f;
    public float cooldown = 3f;
    private float lastTime;
    private Projectile lastHitProjectile = null;
    public LayerMask shockwaveLayerMask;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public override void TakeDamage(Projectile projectile)
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashWhite());
        health -= projectile.damage;

        if (Time.time >= lastTime + cooldown)
        {
            CreateShockWave();
            lastTime = Time.time;
        }


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
            lastHitProjectile = other.GetComponent<Projectile>();
            if (lastHitProjectile != null)
            {
                Destroy(other.gameObject);
                TakeDamage(lastHitProjectile);
            }
        }
    }

    private void CreateShockWave()
    {
        Debug.Log("Shockwave triggered");
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, shockwaveRadius, shockwaveLayerMask);

        foreach (Collider2D col in hitObjects)
        {
            Debug.Log("Hit: " + col.name);
            Rigidbody2D rb = col.attachedRigidbody;
            if (rb != null)
            {
                Vector2 dir = (col.transform.position - transform.position).normalized;
                if (col.CompareTag("Projectile"))
                {
                    Debug.Log("rb detected");
                    Projectile projectile = col.GetComponent<Projectile>();
                    if (projectile != null && projectile == lastHitProjectile)
                    { 
                        continue;
                    }

                    rb.AddForce(dir * projectileKnockBackForce, ForceMode2D.Impulse);
                }
                else if (col.CompareTag("Player"))
                {
                    rb.AddForce(dir * playerKnockBackForce, ForceMode2D.Impulse);
                    var player = col.GetComponentInParent<Player_Controls>();
                    if (player != null)
                    {
                        player.ApplyKnockback(dir * 7f, 3f);
                    }
                }

            }
        }
    }
   


    private void OnDrawGizmosSelected()
    {
        // Draw shockwave radius in editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, shockwaveRadius);
    }
}
