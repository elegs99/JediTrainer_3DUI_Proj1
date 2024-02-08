using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public GameObject[] dronePrefabs = new GameObject[3];
    public TextMeshProUGUI roundText;
    public float spawnInterval = 1.0f;
    public bool isPaused = false;
    public GameObject textBubble;
    public TextMeshProUGUI dialogText;
    public float typingSpeed = 0.05f;

    private bool inTutorial = true;
    private float timer = 0.0f;
    private int currentWave = 1;
    private int waveIndex = 0;
    private int enemiesToSpawn;
    private int enemiesSpawned = 0;
    private int spawnDifficultyIncrease = 0;
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
        if(isPaused)
        {
            return;
        }

        if (timer >= spawnInterval && enemiesSpawned < enemiesToSpawn)
        {
            timer = 0.0f;
            GameObject drone = Instantiate(dronePrefabs[waveIndex], transform.position, transform.rotation);
            if(currentWave == 2)
            {
                drone.name = "Attack Droid " + enemiesSpawned;
            }
            else if(currentWave == 1)
            {
                drone.name = "Training Droid " + enemiesSpawned;
            }
            drone.tag = "Enemy";
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

    private IEnumerator StartTutorial()
    {
        inTutorial = true;
        yield return StartCoroutine(TypeText("Welcome to Tatooine!"));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(TypeText("Before we start on your Jedi training, you will need to know how to use a lightsaber."));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(TypeText("I have placed a lightsaber to your left, pick it up now."));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(TypeText("Good, now let's practice your swings. I have summoned an attack droid with the force, these" +
            " enemies will explode when they make contact with you, so be careful!"));
        yield return new WaitForSeconds(2.0f);
        // GameObject drone = Instantiate(dronePrefabs[waveIndex], transform.position, transform.rotation);

        textBubble.SetActive(!textBubble.activeSelf);
        currentWave++;
        inTutorial = false;
    }

    IEnumerator TypeText(string text)
    {
        dialogText.text = ""; // Start with an empty text
        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter; // Add each letter one by one
            yield return new WaitForSeconds(typingSpeed); // Wait a bit between each character
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
        switch(((waveNumber-1)%3)+1)
        { 
            case 1:
                spawnAreaSize = new Vector2(5f, 5f);
                enemiesToSpawn = 5 + spawnDifficultyIncrease;
                waveIndex = 0;
                break;
            case 2:
                spawnAreaSize = new Vector2(2.5f, 2.5f);
                enemiesToSpawn = 8 + spawnDifficultyIncrease;
                waveIndex = 1;
                break;
            case 3:
                spawnAreaSize = new Vector2(1f, 1f);
                enemiesToSpawn = 1;
                waveIndex = 2;
                spawnDifficultyIncrease += 2;
                break;
            default:
                spawnAreaSize = new Vector2(1f, 1f);
                enemiesToSpawn = 1;
                waveIndex = 1;
                break;
        }
        roundText.text = "Round: " + currentWave.ToString();
        enemiesSpawned = 0;
    }

    public void PauseSpawning()
    {
        isPaused = true;
    }

    public void ResumeSpawning()
    {
        isPaused = false;
    }
}