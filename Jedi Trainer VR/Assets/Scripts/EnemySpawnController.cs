using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public GameObject[] dronePrefabs = new GameObject[3];
    public TextMeshProUGUI roundText;
    public float spawnInterval = 1.0f;

    private float timer = 0.0f;
    private int currentWave = 1;
    private int waveIndex = 0;
    private int enemiesToSpawn;
    private int enemiesSpawned = 0;
    private Vector2 spawnAreaSize;
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // List to keep track of spawned enemies

    private void Start()
    {
        StartWave(currentWave);
        timer = spawnInterval;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && enemiesSpawned < enemiesToSpawn)
        {
            timer = 0.0f;
            GameObject drone = Instantiate(dronePrefabs[waveIndex], transform.position, transform.rotation);
            spawnedEnemies.Add(drone);
            enemiesSpawned++;
            MoveSpawner();
        }

        if (AllEnemiesDestroyed() && enemiesSpawned >= enemiesToSpawn)
        {
            currentWave++;
            StartWave(currentWave);
        }
    }

    private bool AllEnemiesDestroyed()
    {
        spawnedEnemies.RemoveAll(item => item == null); // Remove any null references (destroyed enemies)
        return spawnedEnemies.Count == 0;
    }

    private void MoveSpawner()
    {
        float newX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float newZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        transform.position = new Vector3(transform.position.x + newX, transform.position.y, transform.position.z + newZ);
    }

    void StartWave(int waveNumber)
    {
        switch(waveNumber)
        { 
            case 1:
                spawnAreaSize = new Vector2(2f, 2f);
                enemiesToSpawn = 5;
                waveIndex = 0;
                break;
            case 2:
                spawnAreaSize = new Vector2(10f, 10f);
                enemiesToSpawn = 8;
                waveIndex = 1;
                break;
            case 3:
                spawnAreaSize = new Vector2(5f, 5f);
                enemiesToSpawn = 1;
                waveIndex = 2;
                break;
            default:
                spawnAreaSize = new Vector2(1f, 1f);
                enemiesToSpawn = 1;
                waveIndex = 0;
                break;
        }
        roundText.text = "Round: " + currentWave.ToString();
        enemiesSpawned = 0;
    }
}