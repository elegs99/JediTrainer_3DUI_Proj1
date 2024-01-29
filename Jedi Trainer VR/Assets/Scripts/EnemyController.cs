using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject dronePrefab;

    public float spawnInterval = 5.0f;
    private float timer = 0.0f;

    public Vector2 spawnAreaSize = new Vector2(10f, 10f);

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0.0f;
            GameObject drone = Instantiate(dronePrefab, transform.position, transform.rotation);
            drone.AddComponent<EnemyMovement>();
            MoveSpawner();
        }
    }

    private void MoveSpawner()
    {
        float newX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float newZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        transform.position = new Vector3(newX, transform.position.y, newZ);
    }
}
