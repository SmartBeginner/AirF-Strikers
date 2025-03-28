using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DroneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dronePrefab; // Prefab do drone
    [SerializeField] private GameObject droneTankPrefab;
    [SerializeField]private Transform playerRef; // Referência ao jogador
    [SerializeField] private GameObject playerPrefab; // Prefab do jogador
    [SerializeField] private float spawnRadius = 5f; // Distância mínima do jogador
    [SerializeField] private float timeBetweenWaves = 5f; // Tempo entre as ondas
    [SerializeField] private int initialDroneCount = 3; // Número inicial de drones por onda
    [SerializeField] private float difficultyMultiplier = 1.5f; // Multiplicador de dificuldade por onda
    private Text scoreText;
    private int currentWave = 1; // Número da onda atual
    private int dronesToSpawn; // Quantidade de drones dessa onda
    private List<GameObject> activeDrones = new List<GameObject>(); // Lista dos drones vivos

    
    void Start()
    {
        
        SpawnDrones(dronesToSpawn);
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
    for (int i = 1; i <= count; i++)
    {
        Vector3 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
        float positionX = randomOffset.x;
        float positionY = randomOffset.y;
        if(positionX >-30 && positionX <30){
            if(positionX % 2 == 0){
                positionX += 30;
            }
            else{
                positionX -= 30;
            }
        }
        if(positionY >-30 && positionY <30){
            if(positionY % 2 == 0){
                positionY += 30;
            }
            else{
                positionY -= 30;
            }
        }
        Vector3 spawnPosition = playerRef.position + new Vector3(positionX, positionY, 0);

        GameObject drone = Instantiate(dronePrefab, spawnPosition, Quaternion.identity);
        int j = i % 15;
        if(j == 0){
            Debug.Log("Numero de drones tank: "+ j);
            GameObject droneTank = Instantiate(droneTankPrefab, spawnPosition, Quaternion.identity);
            DroneAI dtAI = droneTank.GetComponent<DroneAI>();

            if (dtAI != null)
            {
                dtAI.isSpawnerDrone = false; // ✅ Garante que os clones não sejam spawner
                dtAI.MakeVisible();

                EnemyShooter shooter = drone.GetComponent<EnemyShooter>();
                if (shooter != null) shooter.enabled = true;
            }

            activeDrones.Add(droneTank);
            dtAI.OnDroneDeath += HandleDroneDeath;
        }

        DroneAI dAI = drone.GetComponent<DroneAI>();
        

        if (dAI != null)
        {
            dAI.isSpawnerDrone = false; // ✅ Garante que os clones não sejam spawner
            dAI.MakeVisible();

            EnemyShooter shooter = drone.GetComponent<EnemyShooter>();
            if (shooter != null) shooter.enabled = true;
        }

        activeDrones.Add(drone);
        dAI.OnDroneDeath += HandleDroneDeath;
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
