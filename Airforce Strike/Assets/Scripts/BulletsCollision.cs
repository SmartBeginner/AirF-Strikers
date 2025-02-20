using UnityEngine;

public class DroneAI : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float moveSpeed = 2f; // Velocidade dos drones

    public void SetTarget(Transform playerTarget)
    {
        player = playerTarget;
    }

    void Update()
    {
        if (player == null) return;

        // Movimenta e rotaciona o drone em direção ao jogador
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Se colidir com um objeto da tag "Bala", destrói o drone
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject); // Destroi o drone
            Destroy(other.gameObject); // Destroi a bala também
        }
    }
}