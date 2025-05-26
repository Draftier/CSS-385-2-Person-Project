using System;
using UnityEngine;

public class Player_Controls : MonoBehaviour
{
    private float horizontalInput;
    private float rotationSpeed = 360.0f;
    private float speed = 5.0f;
    public Gun[] guns;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
        Vector3 forward = transform.up;

        if (Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < guns.Length; i++)
            {
                if (guns[i] != null)
                {
                    guns[i].StartShooting();
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            for(int i = 0; i < guns.Length; i++)
            {
                if (guns[i] != null)
                {
                    guns[i].isShooting = false;
                }
            }
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            speed += 10 * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, 8f); // Limit speed to a maximum of 100
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speed -= 5 * Time.deltaTime;
            speed = Mathf.Clamp(speed, -80f, 8f); // Limit speed to a maximum of 100
        }
        transform.Translate(forward * speed * Time.deltaTime, Space.World);
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
}
