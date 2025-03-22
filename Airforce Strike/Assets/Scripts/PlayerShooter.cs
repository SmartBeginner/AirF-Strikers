using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject misslePrefab;
    [SerializeField] 
    private GameObject flamePrefab;
    [SerializeField]
    private float shootDelay = 0.5f;
    [SerializeField]
    private float missleDelay = 10.0f;
    [SerializeField]
    private float flameDelay = 10.0f;
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private float missleSpeed = 15f;
    [SerializeField]
    private float flameSpeed = 10f;
    [SerializeField] private int maxHp = 3;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Image missileFill;
    [SerializeField] private int flameCount = 5;
    [SerializeField] private float flameSpread = 0.5f;
    [SerializeField] private float flameLifetime = 5f;
    [SerializeField] private float flameDeceleration = 2f; // 🔥 Taxa de desaceleração
    [SerializeField] private Image flameFill;
    [SerializeField] private float flameSpawnDelay = 0.1f;
    private bool isShooting = false;
    private float shootTimer = 0f;
    private float lastMissleTime = -10f;
    private float lastFlameTime = -10f;
    private int hp;
    private bool isReloading = false;
    private float cooldownTimer = 0f;
    private bool isReloadingFlame = false;
    private float cooldownTimerFlame = 0f;

    private void Start()
    {
        hp = maxHp;
        healthBar.UpdateHealthBar(hp, maxHp);
        ResetBar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !Input.GetKey(KeyCode.LeftShift))
        {
            ShootBullet();
            isShooting = true;
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            isShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.K) && Time.time - lastMissleTime >= missleDelay && !isReloading && !Input.GetKey(KeyCode.LeftShift))
        {
            ShootMissle();
            lastMissleTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.J) && Time.time - lastFlameTime >= flameDelay && !isReloadingFlame && !Input.GetKey(KeyCode.LeftShift))
        {
            StartCoroutine(ShootFlames());
            lastFlameTime = Time.time;
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
        UpdateReloadingBar();
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
        StartReloading();
        if (missleScript != null)
        {
            missleScript.SetSpeed(missleSpeed);
        }
    }

    private IEnumerator ShootFlames()
    {
        StartReloadingFlame();
        for (int i = 0; i < flameCount; i++)
        {
            Vector3 spawnPosition = transform.position;
            float randomAngle = Random.Range(-75f, 75f);
            Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 180 + randomAngle);
            GameObject flame = Instantiate(flamePrefab, spawnPosition, rotation);
            Rigidbody2D rb = flame.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = rotation * Vector2.up * flameSpeed;
                StartCoroutine(SlowDownFlame(rb)); // 🔥 Inicia a desaceleração da chama
            }
            Destroy(flame, flameLifetime);
            yield return new WaitForSeconds(flameSpawnDelay);
        }
    }

    private IEnumerator SlowDownFlame(Rigidbody2D rb)
    {
        while (rb.linearVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime * flameDeceleration);
            yield return null;
        }
        rb.linearVelocity = Vector2.zero;
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

    private void StartReloading()
    {
        isReloading = true;
        cooldownTimer = 0f;
        ResetBar();
    }
    private void StartReloadingFlame()
    {
        isReloadingFlame = true;
        cooldownTimerFlame = 0f;
        ResetBarFlame();
    }

    private void UpdateReloadingBar()
    {
        if (isReloading && missileFill != null)
        {
            cooldownTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(cooldownTimer / missleDelay);
            missileFill.fillAmount = progress;
            missileFill.color = Color.gray;
            if (cooldownTimer >= missleDelay)
            {
                isReloading = false;
                missileFill.color = new Color32(34, 185, 34, 255);
            }
        }
        if (isReloadingFlame && flameFill != null)
        {
            cooldownTimerFlame += Time.deltaTime;
            float progress = Mathf.Clamp01(cooldownTimerFlame / flameDelay);
            flameFill.fillAmount = progress;
            flameFill.color = Color.gray;
            if (cooldownTimerFlame >= flameDelay)
            {
                isReloadingFlame = false;
                flameFill.color = new Color32(224, 140, 16, 255);
            }
        }
    }

    private void ResetBar()
    {
        if (missileFill != null)
        {
            missileFill.fillAmount = 1;
            missileFill.color = new Color32(34, 185, 34, 255);
        }
    }
    private void ResetBarFlame()
    {
        if (flameFill != null)
        {
            flameFill.fillAmount = 1;
            flameFill.color = new Color32(224, 140, 16, 255);
        }
    }
}
