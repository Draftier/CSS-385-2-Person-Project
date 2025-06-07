using System.Collections;
using UnityEngine;

public class Short_Range : Gun
{
    public Transform firePoint; // Assign in Inspector this is where the projectile will be spawned

    public override IEnumerator shootingCoroutine()
    {
        // Boolean used to prevent two fireprojectile coroutines from running at the same time
        isShooting = true;

        // Instantiate a projectile and ignore collision with player when shooting is activated
        while (isShooting)
        {
            // Check if projectilePrefab and firePoint are assigned
            GameObject projectile = Instantiate(
                projectilePrefab,
                firePoint != null ? firePoint.position : transform.position,
                firePoint != null ? firePoint.rotation : transform.rotation
            );

            // Ignore collision between the projectile and the player
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            // Set the projectile's speed to be used to apply velocity to enemies
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Projectile proj = projectile.GetComponent<Projectile>();

            proj.speed = projectileSpeed;
            proj.maxDistance = maxDistance;

            proj.speed = 5;
            rb.linearVelocity = firePoint != null ? firePoint.up * projectileSpeed : transform.up * projectileSpeed;

            

            yield return new WaitForSeconds(fireRate); // Use fire rate for timing
        }
        // Boolean used to prevent two fireprojectile coroutines from running at the same time
        isShooting = false;
    }
}
