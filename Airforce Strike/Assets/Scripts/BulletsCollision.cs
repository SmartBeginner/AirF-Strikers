using UnityEngine;
using System;


public class DroneAI : MonoBehaviour
{
    public event Action<GameObject> OnDroneDeath;
    public bool isSpawnerDrone = false; // ✅ Definir no inspetor para o drone inicial
    [SerializeField] private Transform player;
    [SerializeField] private int enemyLife = 3; //vida dos drones
    private bool isInvisible = false;
    [SerializeField] private float moveSpeed = 2f; // Velocidade dos drones
    [SerializeField] private float stopDistance = 3f; // Distância mínima para parar
    [SerializeField] private float evadeDistance = 1.5f; // Distância para tentar desviar
    [SerializeField] private float separationDistance = 1f; // Distância mínima entre drones
    [SerializeField] private LayerMask enemyLayer; // Camada dos inimigos
        [SerializeField] private float tiltAngle = 10f; // Inclinação máxima
        [SerializeField] private float tiltSpeed = 5f; // Velocidade da rotação

    void Start(){
        if (isSpawnerDrone)
        {
            MakeInvisible();
        }
        else{
            MakeVisible();
        }
    }

    void MakeInvisible()
    {
        isInvisible = true; // Flag para evitar interações

        // ✅ Desativa renderização
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        // ✅ Desativa colisores
        Collider2D col2D = GetComponent<Collider2D>();
        if (col2D != null) col2D.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        

        // ✅ Se o drone tiver IA ou tiro, desativa o script dele
        DroneAI ai = GetComponent<DroneAI>(); // Substitua pelo nome correto do script de IA
        if (ai != null) ai.enabled = false;

        EnemyShooter shooter = GetComponent<EnemyShooter>(); // ✅ Desativa tiro
        if (shooter != null) shooter.enabled = false;
    }

    public void MakeVisible()
    {
        isInvisible = false; // Flag para evitar interações

        // ✅ Desativa renderização
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }

        // ✅ Desativa colisores
        Collider2D col2D = GetComponent<Collider2D>();
        if (col2D != null) col2D.enabled = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;

        // ✅ Se o drone tiver IA ou tiro, desativa o script dele
        DroneAI ai = GetComponent<DroneAI>(); // Substitua pelo nome correto do script de IA
        if (ai != null) ai.enabled = true;
    }
    public void SetTarget(Transform playerTarget)
    {
        player = playerTarget;
    }

    void Die()
    {
        if (!isSpawnerDrone) // ✅ Drone inicial não morre
        {
            OnDroneDeath?.Invoke(gameObject); // Notifica o Spawner
            Destroy(gameObject); // Destroi o drone
        }
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

        TiltDrone(direction.x);
        AvoidOtherDrones();
    }


    void TiltDrone(float horizontalMovement)
    {
        float targetTilt = -horizontalMovement * tiltAngle;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTilt);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
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
        if (isSpawnerDrone){
           return; // ✅ Drone inicial não toma dano
 
        }

        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            enemyLife -= 1;
            if(enemyLife <= 0){
                Die();
            }
        }
        else if(other.CompareTag("Missile")){
            Destroy(other.gameObject);
            enemyLife -= 3;
            if(enemyLife <= 0){
                Die();
            }
        }
    }
}
