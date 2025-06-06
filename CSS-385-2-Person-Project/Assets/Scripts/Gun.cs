using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public float projectileSpeed;
    public bool isShooting;
    public GameObject projectilePrefab;
    public float fireRate;
    public abstract IEnumerator shootingCoroutine();
    private Coroutine shootingCoroutineInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            shootingCoroutineInstance = StartCoroutine(shootingCoroutine());
        }
    }

    public virtual void StopShoot()
    {
        if (isShooting)
        {
            StopCoroutine(shootingCoroutineInstance);
            shootingCoroutineInstance = null;
            isShooting = false;
        }
    }
}
