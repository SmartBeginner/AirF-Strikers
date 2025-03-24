using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 3f; // Tempo de vida da bala em segundos
    
    [SerializeField]
    private float directionChangeTime = 0.0001f; // Tempo antes de mudar de direção aleatoriamente

    private float speed; // Velocidade da bala
    private float timeElapsed = 0f;
    private bool directionChanged = false;
    private Vector2 direction;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        direction = transform.right; // Define a direção inicial da bala
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        
        if (!directionChanged)
        {
            directionChanged = true;
            float angleOffset = Random.Range(-2f, 2f); // Escolhe um ângulo aleatório entre -15 e 15 graus
            direction = Quaternion.Euler(0, 0, angleOffset) * direction; // Aplica a rotação na direção
        }
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        transform.localScale += Vector3.one * 3 * Time.deltaTime; // Aumenta o tamanho da bala com o tempo
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
