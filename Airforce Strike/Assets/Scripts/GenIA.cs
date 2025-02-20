using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dronePrefab; // Prefab do drone
    [SerializeField] private Transform player; // Referência ao jogador
    [SerializeField] private int droneCount = 3; // Número de drones a serem spawnados
    [SerializeField] private float spawnRadius = 5f; // Distância mínima do jogador

    void Start()
    {
        SpawnDrones();
    }

    void SpawnDrones()
    {
        for (int i = 0; i < droneCount; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle.normalized * spawnRadius; // Posição aleatória no círculo
            Vector3 spawnPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            Instantiate(dronePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
