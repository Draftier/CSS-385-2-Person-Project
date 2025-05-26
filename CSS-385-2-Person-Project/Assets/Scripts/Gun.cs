using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public float projectileSpeed;
    public bool isShooting;
    public GameObject projectilePrefab;
    public float fireRate;
    public abstract IEnumerator shootingCoroutine();
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
            StartCoroutine(shootingCoroutine());
        }
    }
}
