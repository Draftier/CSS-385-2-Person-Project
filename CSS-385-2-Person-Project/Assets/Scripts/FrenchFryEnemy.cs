using UnityEngine;

public class FrenchFryEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform planet;
    public Transform player;
    public GameObject projectilePrefab;
    
    [Header("Settings")]
    public float moveSpeed = 1.5f;
    public float rotationSpeed = 5f;
    public float detectionRange = 7f;
    public float fireRate = 2.5f;
    public float projectileSpeed = 8f;
    
    private float fireTimer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (!planet) planet = GameObject.FindGameObjectWithTag("Planet").transform;
        if (!player) player = GameObject.FindGameObjectWithTag("Player").transform;
        
        fireTimer = Random.Range(0, fireRate);
    }

    void Update()
    {
        MoveTowardPlanet();
        HandleShooting();
    }

    void MoveTowardPlanet()
    {
        Vector2 direction = (planet.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        
        if (rb.linearVelocity != Vector2.zero) {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void HandleShooting()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer > 0 || !player) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            FireProjectile();
            fireTimer = fireRate;
        }
    }

    void FireProjectile()
    {
        Vector2 fireDirection = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(
            projectilePrefab, 
            transform.position, 
            Quaternion.identity
        );
        
        FrenchFryProjectile projectileScript = projectile.GetComponent<FrenchFryProjectile>();
        if (projectileScript)
        {
            projectileScript.Initialize(fireDirection, projectileSpeed);
        }
    }
}