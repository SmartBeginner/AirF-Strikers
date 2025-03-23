using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private Transform player; // Referência ao jogador

    private void Update()
    {
        if (player != null)
        {
            // Alinha a posição do fundo à posição do jogador
            transform.position = new Vector2(player.position.x, player.position.y);
        }
    }
}
