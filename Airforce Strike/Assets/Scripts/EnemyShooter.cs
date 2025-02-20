using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform target;
    [SerializeField] private float minFireRate = 2f;
    [SerializeField] private float maxFireRate = 5f;
    [SerializeField] private int shotsPerBurst = 3;
    [SerializeField] private float burstInterval = 0.3f;
    [SerializeField] private float projectileSpeed = 5f;

    private void Start()
    {
        StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        while (true)
        {
            float fireRate = Random.Range(minFireRate, maxFireRate);
            yield return new WaitForSeconds(fireRate);
            StartCoroutine(FireBurst());
        }
    }

    private IEnumerator FireBurst()
    {
        for (int i = 0; i < shotsPerBurst; i++)
        {
            FireProjectile();
            yield return new WaitForSeconds(burstInterval);
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab != null && target != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (target.position - firePoint.position).normalized;
                rb.linearVelocity = direction * projectileSpeed;
            }
        }
    }
}