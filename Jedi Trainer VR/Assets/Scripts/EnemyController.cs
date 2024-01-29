using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class EnemyController : MonoBehaviour
{
    public GameObject dronePrefab;

    public float spawnInterval = 1.0f;
    private float timer = 0.0f;
    private int currentWave = 1;
    private int enemiesToSpawn;
    private int enemiesSpawned = 0;

    public Vector2 spawnAreaSize = new Vector2(20f, 20f);

    public TextMeshProUGUI roundText;

    private void Start()
    {
        StartWave(currentWave);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && enemiesSpawned < enemiesToSpawn)
        {
            timer = 0.0f;
            GameObject drone = Instantiate(dronePrefab, transform.position, transform.rotation);
            drone.AddComponent<EnemyMovement>();
            enemiesSpawned++;
            MoveSpawner();
        }
        else if(enemiesSpawned >= enemiesToSpawn)
        {
            currentWave++;
            StartWave(currentWave);
        }
    }

    private void MoveSpawner()
    {
        float newX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float newZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        transform.position = new Vector3(newX, transform.position.y, newZ);
    }

    void StartWave(int waveNumber)
    {
        switch(waveNumber)
        { 
            case 1:
                enemiesToSpawn = 10;
                break;
            case 2:
                enemiesToSpawn = 20;
                break;
            case 3:
                StartBossFight();
                break;
            default:
                enemiesToSpawn = 10;
            break;
        }
        roundText.text = "Round: " + currentWave.ToString();
        enemiesSpawned = 0;
    }

    void StartBossFight()
    {

    }
}
