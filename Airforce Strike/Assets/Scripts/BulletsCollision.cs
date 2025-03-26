using UnityEngine;
using System;

public class DroneAI : MonoBehaviour
{
    public event Action<GameObject> OnDroneDeath;
    public bool isSpawnerDrone = false;
    [SerializeField] private int enemyLife = 3;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 3f;
    [SerializeField] private float evadeDistance = 1.5f;
    [SerializeField] private float separationDistance = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float tiltAngle = 10f;
    [SerializeField] private float tiltSpeed = 5f;
    private Transform player;
    private bool isInvisible = false;
    private float searchInterval = 0f;
    private float searchTimer = 0f;

    void Start()
    {
        if (isSpawnerDrone)
        {
            MakeInvisible();
        }
        else
        {
            MakeVisible();
        }
        FindClosestPlayer();
    }

    void Update()
    {
        searchTimer += Time.deltaTime;
        if (searchTimer >= searchInterval)
        {
            FindClosestPlayer();
            searchTimer = 0f;
        }

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

    void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject p in players)
        {
            float distance = Vector3.Distance(transform.position, p.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = p.transform;
            }
        }
        player = closestPlayer;
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
        if (isSpawnerDrone) return;

        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            enemyLife -= 1;
            if (enemyLife <= 0) Die();
        }
        else if (other.CompareTag("Missile"))
        {
            Destroy(other.gameObject);
            enemyLife -= 3;
            if (enemyLife <= 0) Die();
        }
    }

    void Die()
    {
        if (!isSpawnerDrone)
        {
            OnDroneDeath?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    void MakeInvisible()
    {
        isInvisible = true;
        foreach (Renderer rend in GetComponentsInChildren<Renderer>()) rend.enabled = false;
        if (TryGetComponent(out Collider2D col2D)) col2D.enabled = false;
        if (TryGetComponent(out Collider col)) col.enabled = false;
        if (TryGetComponent(out DroneAI ai)) ai.enabled = false;
        if (TryGetComponent(out EnemyShooter shooter)) shooter.enabled = false;
    }

    public void MakeVisible()
    {
        isInvisible = false;
        foreach (Renderer rend in GetComponentsInChildren<Renderer>()) rend.enabled = true;
        if (TryGetComponent(out Collider2D col2D)) col2D.enabled = true;
        if (TryGetComponent(out Collider col)) col.enabled = true;
        if (TryGetComponent(out DroneAI ai)) ai.enabled = true;
    }
}
