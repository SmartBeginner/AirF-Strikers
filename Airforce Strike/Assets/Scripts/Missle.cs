using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 5f;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float turnSpeed = 200f;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private GameObject targetMarkerPrefab;

    private static Transform target;
    private static GameObject targetMarker;
    private static bool isMissileFired = false;

    private void Awake()
    {
        if (target == null)
        {
            FindNearestEnemy();
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
        StartCoroutine(TargetUpdateRoutine());
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector2 direction = (Vector2)(target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.deltaTime);
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private IEnumerator TargetUpdateRoutine()
    {
        while (true)
        {
            if (!isMissileFired)
            {
                FindNearestEnemy();
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private void FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 100f, enemyLayer);
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null && target != nearestEnemy)
        {
            target = nearestEnemy;
            UpdateTargetMarker();
        }
    }

    private void UpdateTargetMarker()
    {
        if (targetMarker != null)
        {
            Destroy(targetMarker);
        }

        if (target != null)
        {
            targetMarker = Instantiate(targetMarkerPrefab, target.position, Quaternion.identity);
            targetMarker.transform.SetParent(target);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            isMissileFired = false;
        }
    }
}