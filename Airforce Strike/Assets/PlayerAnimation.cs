using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
         float rotationZ = transform.rotation.eulerAngles.z;
        animator.SetFloat("RotationZ", NormalizeRotation(rotationZ));
    }
    float NormalizeRotation(float angle)
    {
        // Converte ângulos para valores entre -180° e 180°
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
