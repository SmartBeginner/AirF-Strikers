using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private float acceleration = 5f;
    [SerializeField]
    private float deceleration = 5f;
    [SerializeField]
    private float rotationSpeed = 100f;
    [SerializeField]
    private float rotationAcceleration = 50f; // Aceleração do giro
    [SerializeField]
    private float rotationDeceleration = 50f; // Desaceleração do giro
    [SerializeField]
    private float gravity = 2f;
    [SerializeField]
    private float customDecelerationSpeed = 2f;

    public Rigidbody2D rb;
    private float currentSpeed = 0f;

    private Vector2 gravityVelocity;
    private float lastRotation;
    private bool isDecelerating;
    private float currentRotationSpeed = 100f; // Velocidade atual da rotação

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float rotationInput = 0;
        if (Input.GetKey(KeyCode.A))
        {
            rotationInput = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationInput = -1;
        }

        if (rotationInput != 0)
        {
            currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, rotationSpeed, rotationAcceleration * Time.deltaTime);
        }
        else
        {
            currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, 0, rotationDeceleration * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.rotation += rotationInput * (currentRotationSpeed + 100) * Time.deltaTime;
        }
        else
        {
            rb.rotation += rotationInput * currentRotationSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            lastRotation = rb.rotation;
            isDecelerating = false;
        }
        else
        {
            isDecelerating = true;
        }
    }

    public void FixedUpdate()
    {
        bool isAccelerating = Input.GetKey(KeyCode.W);

        if (isAccelerating)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed + 30, (acceleration + 20) * Time.fixedDeltaTime);
                gravityVelocity = Vector2.zero;
            }
            else
            {
                if (currentSpeed > 20)
                {
                    currentSpeed = 18;
                }
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.fixedDeltaTime);
                gravityVelocity = Vector2.zero;
            }
        }
        else if (isDecelerating)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, customDecelerationSpeed * Time.fixedDeltaTime);
            gravityVelocity += Vector2.down * gravity * Time.fixedDeltaTime;
        }

        Vector2 movement = Vector2.zero;
        if (isAccelerating)
        {
            Vector2 rightDirection = transform.right;
            movement = rightDirection * currentSpeed * Time.fixedDeltaTime;
        }
        else if (isDecelerating)
        {
            float radians = lastRotation * Mathf.Deg2Rad;
            Vector2 lastDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
            movement = lastDirection * currentSpeed * Time.fixedDeltaTime;
        }

        movement += gravityVelocity * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }

    public Vector2 getRBPos()
    {
        return rb.position;
    }
}
