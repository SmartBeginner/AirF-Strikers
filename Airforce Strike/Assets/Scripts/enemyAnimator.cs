using UnityEngine;

public class EnemyPositionComparer : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private float searchInterval = 1f;
    private float searchTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
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

        if (player != null && animator != null)
        {
            float relatX = transform.position.x - player.position.x;
            animator.SetFloat("relatX", relatX);
        }
    }

    private void FindClosestPlayer()
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
}
