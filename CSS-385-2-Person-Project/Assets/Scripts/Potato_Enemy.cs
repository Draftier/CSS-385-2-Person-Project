using System.Collections;
using UnityEngine;

public class Potato_Enemy : Enemy
{
    public bool isRotating = false;
    public bool spinningFromHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlanet();
    }

    private IEnumerator SpinFromHit()
    {
        spinningFromHit = true;
        isRotating = true;

        while (spinningFromHit)
        {
            transform.Rotate(0, 0, 360f * Time.deltaTime);
            yield return null;
        }

        isRotating = false;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("POop");
        if (other.CompareTag("Planet"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Projectile"))
        {
            Debug.Log("Hit by projectile");
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(FlashWhite());
            Projectile projectile = other.GetComponent<Projectile>();
        }
    }

    public override void TakeDamage()
    {
    }
}
