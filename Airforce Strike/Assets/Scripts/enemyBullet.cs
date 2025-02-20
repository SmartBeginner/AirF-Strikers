using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 1f;            // Tempo de vida da bala em segundos

    private float speed;                    // Velocidade da bala

    private void Start()
    {
        // Destroi a bala ap�s o tempo de vida especifico
        Destroy(gameObject, lifetime);
    }

    // Configura a velocidade da bala (chamado pelo script do jogador)
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        // Move a bala na dire��o em que est� apontando
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se colidir com qualquer coisa que n�o seja o jogador, destr�i a bala
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}