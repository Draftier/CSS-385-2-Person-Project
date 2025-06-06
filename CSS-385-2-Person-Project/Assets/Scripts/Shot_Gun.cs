using System.Collections;
using UnityEngine;

public class Shotgun : Gun
{
    [Header("Shotgun Settings")]
    public int pelletCount = 8;
    public float spreadAngle = 30f;
    public Transform firePoint; // Assign this in the Inspector

    public override IEnumerator shootingCoroutine()
    {
        isShooting = true;

        while (isShooting)
        {
            ShootPellets();
            yield return new WaitForSeconds(fireRate);
        }

        isShooting = false;
    }

    // private void ShootPellets()
    // {
    //     Vector3 shootDirection = firePoint != null ? firePoint.up : transform.up;
    //     Vector3 shootOrigin = firePoint != null ? firePoint.position : transform.position;
    //     float halfSpread = spreadAngle / 2f;

    //     for (int i = 0; i < pelletCount; i++)
    //     {
    //         float t = (pelletCount == 1) ? 0.5f : i / (float)(pelletCount - 1);
    //         float angleOffset = Mathf.Lerp(-halfSpread, halfSpread, t);

    //         Vector3 pelletDirection = Quaternion.Euler(0, 0, angleOffset) * shootDirection;

    //         GameObject pellet = Instantiate(projectilePrefab, shootOrigin, Quaternion.identity);

    //         // Ignore collision with the shooter
    //         Collider2D pelletCol = pellet.GetComponent<Collider2D>();
    //         Collider2D gunCol = GetComponent<Collider2D>();
    //         if (pelletCol != null && gunCol != null)
    //         {
    //             Physics2D.IgnoreCollision(pelletCol, gunCol);
    //         }

    //         // Set projectile rotation to match direction
    //         pellet.transform.up = pelletDirection;

    //         // Set velocity
    //         Rigidbody2D rb = pellet.GetComponent<Rigidbody2D>();
    //         if (rb != null)
    //         {
    //             rb.linearVelocity = pelletDirection * projectileSpeed;
    //         }

    //         // Optionally set speed value on projectile script
    //         Projectile proj = pellet.GetComponent<Projectile>();
    //         if (proj != null)
    //         {
    //             proj.speed = projectileSpeed;
    //         }
    //     }
    // }

    private void ShootPellets()
    {
        // Ensure firePoint is valid
        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint not assigned.");
            return;
        }

        Vector3 spawnPosition = firePoint.position;
        Vector3 baseDirection = firePoint.up;

        // Spread logic
        float angleStep = pelletCount > 1 ? spreadAngle / (pelletCount - 1) : 0f;
        float halfSpread = spreadAngle / 2f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angleOffset = -halfSpread + (i * angleStep);
            // Rotate around Z-axis (2D space)
            Quaternion spreadRotation = Quaternion.Euler(0, 0, angleOffset);
            Vector3 pelletDirection = spreadRotation * baseDirection;

            // Spawn pellet
            GameObject pellet = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            pellet.transform.up = pelletDirection;

            // Set velocity
            Rigidbody2D rb = pellet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = pelletDirection * projectileSpeed;
                rb.angularVelocity = 0f;
                rb.freezeRotation = true; // Optional
            }

            // Optional: assign speed to Projectile script
            Projectile proj = pellet.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.speed = projectileSpeed;
            }

            // Ignore collision with shooter
            Collider2D pelletCol = pellet.GetComponent<Collider2D>();
            Collider2D shooterCol = GetComponentInParent<Collider2D>(); // <- Important: parent may hold the collider
            if (pelletCol != null && shooterCol != null)
            {
                Physics2D.IgnoreCollision(pelletCol, shooterCol);
            }
        }
    }



}
