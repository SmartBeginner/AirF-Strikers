using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;         // Prefab do projétil a ser disparado
    [SerializeField]
    private float shootDelay = 0.5f;         // Delay entre cada disparo
    [SerializeField]
    private float bulletSpeed = 10f;         // Velocidade do projétil

    private bool isShooting = false;         // Indica se o jogador está disparando
    private float shootTimer = 0f;           // Temporizador para controlar o delay entre disparos

    private void Update()
    {
        // Verifica se a tecla "L" está pressionada para iniciar ou parar o disparo
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShootBullet();
            isShooting = true;
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            isShooting = false;
        }

        // Controla o temporizador e dispara projéteis enquanto a tecla "L" está pressionada
        if (isShooting)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootDelay)
            {
                ShootBullet();
                shootTimer = 0f; // Reseta o temporizador após cada disparo
            }
        }
    }

    // Função responsável por instanciar o projétil e aplicá-lo na direção desejada
    private void ShootBullet()
    {
        // Instancia o projétil na posição do jogador e com sua rotação atual
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        // Define a velocidade da bala ao instanciar
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetSpeed(bulletSpeed);  // Configura a velocidade da bala
        }
    }
}
