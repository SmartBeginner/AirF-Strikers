using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 10f;             // Velocidade máxima do jogador
    [SerializeField]
    private float acceleration = 5f;          // Aceleração ao apertar "W"
    [SerializeField]
    private float deceleration = 5f;          // Desaceleração quando "W" não está pressionado
    [SerializeField]
    private float rotationSpeed = 100f;       // Velocidade de rotação do jogador
    [SerializeField]
    private float gravity = 2f;               // Intensidade da gravidade personalizada
    [SerializeField]
    private float customDecelerationSpeed = 2f;  // Velocidade de desaceleração personalizada

    public Rigidbody2D rb;
    private float currentSpeed = 0f;          // Velocidade atual do jogador
    private bool isGrounded = false;          // Checa se o jogador está em contato com o solo
    private Vector2 gravityVelocity;          // Armazena a velocidade da gravidade aplicada
    private float lastRotation;               // Armazena o último ângulo de rotação
    private bool isDecelerating;              // Indica se o jogador está desacelerando

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Controla a rotação do jogador com as teclas "A" e "D"
        float rotationInput = 0;
        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = -1;
        }

        // Aplica a rotação com base no input, independente de "W" estar ou não pressionado
        rb.rotation += rotationInput * rotationSpeed * Time.deltaTime;

        // Registra o ângulo atual como lastRotation se "W" está pressionado
        if (Input.GetKey(KeyCode.W))
        {
            lastRotation = rb.rotation;
            isDecelerating = false;  // Desativa desaceleração enquanto está acelerando
        }
        else
        {
            // Ativa o modo de desaceleração se "W" não está pressionado
            isDecelerating = true;
        }
    }

    public void FixedUpdate()
    {
        // Verifica se a tecla "W" está pressionada para definir o movimento
        bool isAccelerating = Input.GetKey(KeyCode.W);

        // Ajusta a velocidade atual com base na aceleração e desaceleração
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

        // Determina a direção de movimento: na direção da rotação atual ou da lastRotation
        Vector2 movement = Vector2.zero;
        if (isAccelerating)
        {
            Vector2 rightDirection = transform.right;
            movement = rightDirection * currentSpeed * Time.fixedDeltaTime;
        }
        else if (isDecelerating)
        {
            // Calcula a direção baseada no último ângulo registrado (lastRotation)
            float radians = lastRotation * Mathf.Deg2Rad;
            Vector2 lastDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
            movement = lastDirection * currentSpeed * Time.fixedDeltaTime;
        }

        // Adiciona a gravidade ao movimento para que o jogador se mova para baixo quando não estiver acelerando
        movement += gravityVelocity * Time.fixedDeltaTime;

        // Aplica o movimento ao jogador
        rb.MovePosition(rb.position + movement);
    }

    public Vector2 getRBPos()
    {
        return rb.position;
    }
}
