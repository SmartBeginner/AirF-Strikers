using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dronePrefab; // Prefab do drone
    [SerializeField] private Transform player; // Referência ao jogador
    [SerializeField] private float spawnRadius = 5f; // Distância mínima do jogador
    [SerializeField] private float timeBetweenWaves = 5f; // Tempo entre as ondas
    [SerializeField] private int initialDroneCount = 3; // Número inicial de drones por onda
    [SerializeField] private float difficultyMultiplier = 1.5f; // Multiplicador de dificuldade por onda

    private int currentWave = 1; // Número da onda atual
    private int dronesToSpawn; // Quantidade de drones dessa onda
    private List<GameObject> activeDrones = new List<GameObject>(); // Lista dos drones vivos

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves); // Espera antes da próxima onda

        dronesToSpawn = Mathf.RoundToInt(initialDroneCount * Mathf.Pow(difficultyMultiplier, currentWave - 1)); // Aumenta a dificuldade
        Debug.Log($"Iniciando Onda {currentWave} com {dronesToSpawn} drones!");

        SpawnDrones(dronesToSpawn);
    }

    void SpawnDrones(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle.normalized * spawnRadius; // Posição aleatória
            Vector3 spawnPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            GameObject drone = Instantiate(dronePrefab, spawnPosition, Quaternion.identity);
            activeDrones.Add(drone); // Adiciona à lista de drones vivos

            // Adiciona um listener de morte ao drone (assumindo que ele tenha um EnemyHealth ou similar)
            drone.GetComponent<DroneAI>().OnDroneDeath += HandleDroneDeath;
        }
    }

    void HandleDroneDeath(GameObject drone)
    {
        activeDrones.Remove(drone); // Remove da lista quando o drone morre

        if (activeDrones.Count == 0)
        {
            currentWave++; // Próxima onda
            StartCoroutine(StartNextWave()); // Chama nova onda
        }
    }
}
