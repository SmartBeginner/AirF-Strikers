using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] 
    private Transform playerTransform;
    [SerializeField]
    private float followDistance = 3f;            // Distância fixa à frente do jogador ao acelerar
    [SerializeField]
    private float maxDistanceFromPlayer = 2f;     // Distância máxima permitida ao jogador
    [SerializeField]
    private float initialCameraSmoothSpeed = 1f;  // Velocidade de ajuste inicial da câmera
    [SerializeField]
    private float maxCameraSmoothSpeed = 5f;      // Velocidade máxima de ajuste gradual
    [SerializeField]
    private float instantCameraSpeed = 50f;       // Velocidade "absurda" para reposicionamento instantâneo
    [SerializeField]
    private float cameraSmoothAcceleration = 0.5f; // Aceleração do ajuste da câmera
    [SerializeField]
    private float zoomSpeed = 2f;                 // Velocidade de transição do zoom
    [SerializeField]
    private float minZoomZ = -15f;                // Valor inicial do eixo Z
    [SerializeField]
    private float maxZoomZ = -25f;                // Valor final do eixo Z durante o zoom

    public PlayerFollower playerFollower;
    private Vector2 targetPosition;               // Posição alvo da câmera
    private bool isAccelerating;
    private float currentSmoothSpeed;             // Velocidade de ajuste atual da câmera

    private void Start()
    {
            // playerTransform = player.transform;
         //playerFollower = player.GetComponent<PlayerFollower>();
        if (playerFollower == null)
        {
            Debug.LogError("PlayerFollower não encontrado no Player!");
        }
        else
        {
            Debug.Log("PlayerFollower encontrado!");
        }
        Debug.Log(playerFollower);
        // Inicializa a posição alvo da câmera e o valor inicial do Smooth Speed
        targetPosition = transform.position;
        currentSmoothSpeed = initialCameraSmoothSpeed;
    }

    private void FixedUpdate()
    {
        // Verifica se a tecla "W" está pressionada para definir a posição alvo e iniciar o zoom
        bool wasAccelerating = isAccelerating;
        isAccelerating = Input.GetKey(KeyCode.W);

        if (isAccelerating)
        {
            // Define a posição da câmera a uma distância fixa à frente do jogador
            
            Vector2 playerPosition = playerFollower.getRBPos();
            //Debug.Log("Player Position: " + playerPosition);
            targetPosition = playerPosition - (Vector2)playerTransform.right * followDistance;

            // Inicia a aceleração gradual do Smooth Speed se "W" acabou de ser pressionado
            if (!wasAccelerating)
            {
                currentSmoothSpeed = initialCameraSmoothSpeed; // Reseta o Smooth Speed
            }
            currentSmoothSpeed = Mathf.MoveTowards(currentSmoothSpeed, maxCameraSmoothSpeed, cameraSmoothAcceleration * Time.fixedDeltaTime);

            // Aplica o zoom para a posição máxima no eixo Z gradualmente
            float currentZ = Mathf.Lerp(transform.position.z, maxZoomZ, zoomSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, currentZ);
        }
        else
        {
            // Calcula a distância entre a câmera e o jogador quando ele está parado
            Vector2 playerPosition = playerFollower.getRBPos();
            Vector2 cameraToPlayer = playerPosition - (Vector2)transform.position;
            float distanceToPlayer = cameraToPlayer.magnitude;

            // Se o jogador está muito distante, reposiciona a câmera instantaneamente
            if (distanceToPlayer > maxDistanceFromPlayer)
            {
                targetPosition = (Vector2)transform.position + cameraToPlayer.normalized * (distanceToPlayer - maxDistanceFromPlayer);
                currentSmoothSpeed = instantCameraSpeed; // Define o Smooth Speed como alto para reposicionamento instantâneo
            }
            else
            {
                targetPosition = transform.position;
                currentSmoothSpeed = initialCameraSmoothSpeed; // Reseta para o valor padrão de suavidade
            }

            // Retorna a câmera para o valor mínimo no eixo Z ao parar de acelerar
            float currentZ = Mathf.Lerp(transform.position.z, minZoomZ, zoomSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, currentZ);
        }

        // Move a câmera suavemente para a posição alvo calculada no eixo X e Y
        Vector2 smoothedPosition = Vector2.Lerp((Vector2)transform.position, targetPosition, currentSmoothSpeed * Time.fixedDeltaTime);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);

        //Debug.Log($"PlayerFollower: {playerFollower}, RB Pos: {playerFollower?.getRBPos()}");

    }
}
