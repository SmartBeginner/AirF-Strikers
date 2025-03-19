using UnityEngine;
using System;


public class DroneAI : MonoBehaviour
{
    public event Action<GameObject> OnDroneDeath;
    public bool isSpawnerDrone = false; // ✅ Definir no inspetor para o drone inicial
    [SerializeField] private Transform player;
    [SerializeField] private int enemyLife = 3; //vida dos drones
    [SerializeField] private float moveSpeed = 2f; // Velocidade dos drones
    [SerializeField] private float stopDistance = 3f; // Distância mínima para parar
    [SerializeField] private float evadeDistance = 1.5f; // Distância para tentar desviar
    [SerializeField] private float separationDistance = 1f; // Distância mínima entre drones
    [SerializeField] private LayerMask enemyLayer; // Camada dos inimigos


    void Start(){
        gameObject.SetActive(false);
        if (isSpawnerDrone)
        {
            // Invisível e intangível
            Renderer rend = GetComponent<Renderer>();
            if (rend != null) rend.enabled = false;

            Collider2D col2D = GetComponent<Collider2D>();
            if (col2D != null) col2D.enabled = false;

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }
    public void SetTarget(Transform playerTarget)
    {
        player = playerTarget;
    }

    void Die()
    {
        OnDroneDeath?.Invoke(gameObject); // Notifica o Spawner
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else if (distanceToPlayer < evadeDistance)
        {
            transform.position -= direction * moveSpeed * Time.deltaTime;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        AvoidOtherDrones();
    }

    void AvoidOtherDrones()
    {
        Collider2D[] nearbyDrones = Physics2D.OverlapCircleAll(transform.position, separationDistance, enemyLayer);
        foreach (Collider2D drone in nearbyDrones)
        {
            if (drone.transform != transform)
            {
                Vector3 avoidDirection = (transform.position - drone.transform.position).normalized;
                transform.position += avoidDirection * moveSpeed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("Missile"))
        {
            Destroy(other.gameObject);
            enemyLife -= 1;
            if(enemyLife <= 0){
                Die();
            }
        }
    }
}
