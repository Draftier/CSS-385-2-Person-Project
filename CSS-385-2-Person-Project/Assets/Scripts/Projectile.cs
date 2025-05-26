using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Not sure if projectiles bound by screenbounds
    // For now just destroy if out of screen bounds
    public float speed;
    private Vector3 screenBounds;
    Vector3 lastPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosition = transform.position;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        lastPosition = transform.position;

        if (transform.position.x < -screenBounds.x ||
           transform.position.x > screenBounds.x ||
           transform.position.y < -screenBounds.y ||
           transform.position.y > screenBounds.y)
        {
            Destroy(gameObject);
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Handle collision with enemy
            Destroy(gameObject); // Destroy the projectile on collision
        }
    }
}
