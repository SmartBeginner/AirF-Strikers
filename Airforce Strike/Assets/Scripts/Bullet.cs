using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 1f;            // Tempo de vida da bala em segundos

    private float speed;                    // Velocidade da bala

    private void Start()
    {
        // Destroi a bala após o tempo de vida especifico
        Destroy(gameObject, lifetime);
    }

    // Configura a velocidade da bala (chamado pelo script do jogador)
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        // Move a bala na direção em que está apontando
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se colidir com qualquer coisa que não seja o jogador, destrói a bala
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
