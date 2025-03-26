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
    private float deceleration = 1f; // Reduzi para desacelerar suavemente
    [SerializeField]
    private float rotationSpeed = 100f;
    [SerializeField]
    private float rotationAcceleration = 50f;
    [SerializeField]
    private float rotationDeceleration = 50f;
    [SerializeField]
    private float gravity = 2f;
    [SerializeField]
    private float gravityReductionRate = 0.5f;

    public Rigidbody2D rb;
    private Vector2 currentVelocity = Vector2.zero;
    private Vector2 gravityVelocity;
    private float lastRotation;
    private float currentRotationSpeed = 100f;

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
        }
    }

    public void FixedUpdate()
    {
        bool isAccelerating = Input.GetKey(KeyCode.W);
        Vector2 desiredVelocity = currentVelocity;

        if (isAccelerating)
        {
            Vector2 targetDirection = transform.right;
            float targetSpeed = maxSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                targetSpeed += 30;
            }

            desiredVelocity = Vector2.Lerp(currentVelocity, targetDirection * targetSpeed, acceleration * Time.fixedDeltaTime);
            gravityVelocity = Vector2.MoveTowards(gravityVelocity, Vector2.zero, gravityReductionRate * Time.fixedDeltaTime);
        }
        else
        {
            desiredVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            if (gravityVelocity.y > -10)
            {
                gravityVelocity += Vector2.down * gravity * Time.fixedDeltaTime;
            }
        }

        currentVelocity = desiredVelocity;
        Vector2 movement = currentVelocity * Time.fixedDeltaTime + gravityVelocity * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public Vector2 getRBPos()
    {
        return rb.position;
    }
}
