using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject misslePrefab;
    [SerializeField]
    private float shootDelay = 0.5f;
    [SerializeField]
    private float missleDelay = 10.0f;
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private float missleSpeed = 15f;
    [SerializeField] private int maxHp = 3;
    [SerializeField] private HealthBar healthBar;

    private bool isShooting = false;
    private float shootTimer = 0f;

    private float lastMissleTime = -10f;
    private int hp;

    private void Start()
    {
        hp = maxHp;
        healthBar.UpdateHealthBar(hp, maxHp);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShootBullet();
            isShooting = true;
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            isShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.K) && Time.time - lastMissleTime >= missleDelay)
        {
            ShootMissle();
            lastMissleTime = Time.time;
        }

        if (isShooting)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootDelay)
            {
                ShootBullet();
                shootTimer = 0f;
            }
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetSpeed(bulletSpeed);
        }
    }

    private void ShootMissle()
    {
        GameObject missle = Instantiate(misslePrefab, transform.position, transform.rotation);
        Missile missleScript = missle.GetComponent<Missile>();
        if (missleScript != null)
        {
            missleScript.SetSpeed(missleSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enmy Bullet"))
        {
            hp--;
            healthBar.UpdateHealthBar(hp, maxHp);
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
            Destroy(other.gameObject);
        }
    }
}
