using System;
using System.Collections;
using UnityEngine;

public class Player_Controls : MonoBehaviour
{
    private float horizontalInput;
    private float rotationSpeed = 360.0f;
    private float speed = 5.0f;
    private bool isKnockedBack = false;
    private float knockBackTimer = 0.0f;
    private Rigidbody2D rb;
    public Gun[] guns;
    public float knockBackDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < guns.Length; i++)
            {
                if (guns[i] != null)
                {
                    guns[i].StartShooting();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < guns.Length; i++)
            {
                if (guns[i] != null)
                {
                    // guns[i].isShooting = false;
                    guns[i].StopShoot();
                }
            }
        }

        if (isKnockedBack)
        {
            knockBackTimer -= Time.fixedDeltaTime;
            if (knockBackTimer <= 0.0f)
            {
                isKnockedBack = false;
            }
            return;
        }

        FollowMouse();
        // Vector3 forward = transform.up;



        // if (Input.GetKey(KeyCode.W))
        // {
        //     speed += 2 * Time.deltaTime;
        //     speed = Mathf.Clamp(speed, 0, 3f); // Limit speed to a maximum of 100
        // }
        // else if (Input.GetKey(KeyCode.S))
        // {
        //     speed -= 3 * Time.deltaTime;
        //     speed = Mathf.Clamp(speed, -80f, 3f); // Limit speed to a maximum of 100
        // }
        // transform.Translate(forward * speed * Time.deltaTime, Space.World);
    }

    private void FixedUpdate()
    {

        if (isKnockedBack)
        {
            return; // Let physics handle movement during knockback
        }

        Vector2 moveDirection = transform.up;

        if (Input.GetKey(KeyCode.W))
        {
            speed += 2f * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speed -= 3f * Time.fixedDeltaTime;
        }
        else
        {
            if (speed > 0)
            {
                speed = Mathf.Max(speed - 1f * Time.fixedDeltaTime, 0.5f);
            }
            else
            {
                speed = Mathf.Min(speed + 1f * Time.fixedDeltaTime, -0.5f);
            }
        }

        // Clamp speed in one place for the full range
        speed = Mathf.Clamp(speed, -3f, 6f);

        rb.linearVelocity = moveDirection * speed;
    }

    private void FollowMouse()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = (mouseWorldPos - transform.position);

        // If mouse is very close to the player, do not rotate
        if (direction.magnitude < 0.5f) // Increase threshold as needed
            return;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void ApplyKnockback(Vector2 force, float duration)
    {
        isKnockedBack = true;
        knockBackTimer = duration;

        // Cancel existing velocity so knockback force is consistent
        rb.linearVelocity = Vector2.zero;

        // Apply the impulse force once
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    // public void ProjectileKnockback(Vector2 bulletVel)
    // {
    //     Debug.Log("Poop");
    //     Rigidbody2D rb = GetComponent<Rigidbody2D>();
    //     rb.AddForce(bulletVel.normalized * 1000000.0f, ForceMode2D.Impulse);
    // }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FrenchFryProjectile"))
        {
            Vector2 bulletDirection = other.GetComponent<Rigidbody2D>().linearVelocity;
            ApplyKnockback(bulletDirection.normalized * 20f, knockBackDuration);
            Destroy(other.gameObject);
        }
    }
}
