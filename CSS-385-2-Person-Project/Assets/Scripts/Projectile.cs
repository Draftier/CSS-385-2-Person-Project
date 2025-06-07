using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Not sure if projectiles bound by screenbounds
    // For now just destroy if out of screen bounds
    public float speed;
    public float maxDistance;
    public float damage;
    private Vector3 screenBounds;
    Vector3 lastPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosition = transform.position;
        // screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        // lastPosition = transform.position;

        float traveled = Vector3.Distance(lastPosition, transform.position);
        if (traveled >= maxDistance)
        {
            Destroy(gameObject);
        }

        // if (transform.position.x < -screenBounds.x ||
        //    transform.position.x > screenBounds.x ||
        //    transform.position.y < -screenBounds.y ||
        //    transform.position.y > screenBounds.y)
        // {
        //     Destroy(gameObject);
        // }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Handle collision with enemy
            Destroy(gameObject); // Destroy the projectile on collision
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Projectile1"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider);
        }
    }

}
