using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 🔹 Necessário para manipular UI

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject misslePrefab;
    [SerializeField]
    private float shootDelay = 0.5f;
    [SerializeField]
    private float missleDelay = 10.0f; // Delay do míssil (10 segundos)
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private float missleSpeed = 15f;
    [SerializeField] private int maxHp = 3;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Image missileFill; // 🔹 Barra de recarga UI

    private bool isShooting = false;
    private float shootTimer = 0f;
    private float lastMissleTime = -10f;
    private int hp;

    private bool isReloading = false;
    private float cooldownTimer = 0f;

    private void Start()
    {
        hp = maxHp;
        healthBar.UpdateHealthBar(hp, maxHp);
        ResetBar(); // Reseta a barra de recarga
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

        if (Input.GetKeyDown(KeyCode.K) && Time.time - lastMissleTime >= missleDelay && !isReloading)
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

        UpdateReloadingBar(); // Atualiza a barra de recarga a cada frame
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

        StartReloading(); // Inicia a recarga visual

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

    // Lógica da Barra de Recarga UI
    private void StartReloading()
    {
        isReloading = true;
        cooldownTimer = 0f; // Reinicia o timer ao começar a recarga
        ResetBar();
    }

    private void UpdateReloadingBar()
    {
        if (isReloading && missileFill != null)
        {
            cooldownTimer += Time.deltaTime; // Incrementa o tempo de recarga
            float progress = cooldownTimer / missleDelay; // Sincroniza com o tempo de missleDelay (10 segundos)

            // Garante que o valor do progresso não ultrapasse 1
            progress = Mathf.Clamp01(progress);

            missileFill.fillAmount = progress; // Atualiza a barra de UI com o progresso

            missileFill.color = Color.gray; // Sempre cinza enquanto recarrega

            if (cooldownTimer >= missleDelay)
            {
                isReloading = false;
                missileFill.color = new Color32(34, 185, 34, 255); // Verde escuro (Forest Green)
            }
        }
    }

    private void ResetBar()
    {
        if (missileFill != null)
        {
            missileFill.fillAmount = 1; // Começa cheia
            missileFill.color = new Color32(34, 185, 34, 255); // Começa verde
        }
    }
}