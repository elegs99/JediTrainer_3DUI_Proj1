using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public float spawnInterval = 2f;
    private float timer;
    public int maxCubes = 3;
    private int counter = 0;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && counter < maxCubes)
        {
            SpawnCube();
            timer = spawnInterval;
        }
    }
    void SpawnCube()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 1f, Random.Range(-10f, 10f));
        GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        newCube.name = "Cube " + counter;
        counter++;
    }
}
