using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private Transform player; // Referência ao jogador
    [SerializeField] private float parallaxFactor = 0.5f; // Fator de paralaxe para suavizar o movimento

    private Vector2 startPosition;
    private Vector2 playerStartPosition;

    private void Start()
    {
        if (player != null)
        {
            startPosition = transform.position;
            playerStartPosition = player.position;
        }
    }

    private void Update()
    {   
        if(parallaxFactor != 0){
            if (player != null)
            {
                // Calcula o deslocamento do jogador e aplica o fator de paralaxe
                Vector2 deltaMovement = (Vector2)player.position - playerStartPosition;
                transform.position = startPosition + deltaMovement * parallaxFactor;
            }
        }
        else{
            transform.position = (Vector2)player.position;
        }
    }
}
