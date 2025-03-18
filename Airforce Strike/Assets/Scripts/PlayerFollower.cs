using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 10f;             // Velocidade m�xima do jogador
    [SerializeField]
    private float acceleration = 5f;          // Acelera��o ao apertar "W"
    [SerializeField]
    private float deceleration = 5f;          // Desacelera��o quando "W" n�o est� pressionado
    [SerializeField]
    private float rotationSpeed = 100f;       // Velocidade de rota��o do jogador
    [SerializeField]
    private float gravity = 2f;               // Intensidade da gravidade personalizada
    [SerializeField]
    private float customDecelerationSpeed = 2f;  // Velocidade de desacelera��o personalizada

    public Rigidbody2D rb;
    private float currentSpeed = 0f;          // Velocidade atual do jogador
    private bool isGrounded = false;          // Checa se o jogador est� em contato com o solo
    private Vector2 gravityVelocity;          // Armazena a velocidade da gravidade aplicada
    private float lastRotation;               // Armazena o �ltimo �ngulo de rota��o
    private bool isDecelerating;              // Indica se o jogador est� desacelerando

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Controla a rota��o do jogador com as teclas "A" e "D"
        float rotationInput = 0;
        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = -1;
        }

        // Aplica a rota��o com base no input, independente de "W" estar ou n�o pressionado
        rb.rotation += rotationInput * rotationSpeed * Time.deltaTime;

        // Registra o �ngulo atual como lastRotation se "W" est� pressionado
        if (Input.GetKey(KeyCode.W))
        {
            lastRotation = rb.rotation;
            isDecelerating = false;  // Desativa desacelera��o enquanto est� acelerando
        }
        else
        {
            // Ativa o modo de desacelera��o se "W" n�o est� pressionado
            isDecelerating = true;
        }
    }

    public void FixedUpdate()
    {
        // Verifica se a tecla "W" est� pressionada para definir o movimento
        bool isAccelerating = Input.GetKey(KeyCode.W);

        // Ajusta a velocidade atual com base na acelera��o e desacelera��o
        if (isAccelerating)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
            gravityVelocity = Vector2.zero; // Zera a gravidade quando o jogador acelera
        }
        else if (isDecelerating)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, customDecelerationSpeed * Time.fixedDeltaTime);
            gravityVelocity += Vector2.down * gravity * Time.fixedDeltaTime;
        }

        // Determina a dire��o de movimento: na dire��o da rota��o atual ou da lastRotation
        Vector2 movement = Vector2.zero;
        if (isAccelerating)
        {
            Vector2 rightDirection = transform.right;
            movement = rightDirection * currentSpeed * Time.fixedDeltaTime;
        }
        else if (isDecelerating)
        {
            // Calcula a dire��o baseada no �ltimo �ngulo registrado (lastRotation)
            float radians = lastRotation * Mathf.Deg2Rad;
            Vector2 lastDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
            movement = lastDirection * currentSpeed * Time.fixedDeltaTime;
        }

        // Adiciona a gravidade ao movimento para que o jogador se mova para baixo quando n�o estiver acelerando
        movement += gravityVelocity * Time.fixedDeltaTime;

        // Aplica o movimento ao jogador
        rb.MovePosition(rb.position + movement);
    }

    public Vector2 getRBPos()
    {
        return rb.position;
    }
}
