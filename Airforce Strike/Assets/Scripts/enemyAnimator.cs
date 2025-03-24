using UnityEngine;

public class EnemyPositionComparer : MonoBehaviour
{
    [SerializeField] private Transform player; // Referência ao jogador
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null && animator != null)
        {
            float relatX = transform.position.x - player.position.x;
            animator.SetFloat("relatX", relatX);
        }
    }
}
