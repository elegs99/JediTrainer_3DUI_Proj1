using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureMovement : MonoBehaviour
{
    public GameObject player;
    public float simulationTime = 5f;
    private List<GameObject> originalCubes = new List<GameObject>();
    private Dictionary<GameObject, List<Vector3>> recordedForces = new Dictionary<GameObject, List<Vector3>>();


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(SimulateFutureMovement());
        }
    }

    IEnumerator SimulateFutureMovement()
    {
        foreach (GameObject originalCube in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            GameObject clone = Instantiate(originalCube, originalCube.transform.position, originalCube.transform.rotation);
            foreach (Collider col in  originalCube.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
            foreach (Collider col in clone.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
            CubeMovement cloneController = clone.GetComponent<CubeMovement>();
      
            cloneController.player = this.player;

            originalCube.GetComponent<CubeMovement>().PauseMovement();
            cloneController.isPaused = true;
            originalCubes.Add(originalCube);

            recordedForces.Add(clone, new List<Vector3>());
        }

        yield return StartCoroutine(SimulateClonesMovement());

        foreach (KeyValuePair<GameObject, List<Vector3>> entry in recordedForces)
        {
            GameObject originalCube = originalCubes.Find(c => c.name == entry.Key.name.Replace("(Clone)", ""));
            StartCoroutine(ApplyForcesSequentially(originalCube, entry.Value));
            Destroy(entry.Key);
        }
        originalCubes.Clear();
        recordedForces.Clear();
    }

    IEnumerator SimulateClonesMovement()
    {
        float startTime = Time.time;
        while (Time.time - startTime < simulationTime)
        {
            foreach (GameObject clone in recordedForces.Keys)
            {
                Vector3 force = CalculateRandomDirection(clone);
                clone.GetComponent<Rigidbody>().AddForce(force * 5f, ForceMode.VelocityChange);
                recordedForces[clone].Add(force);
            }
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator ApplyForcesSequentially(GameObject cube, List<Vector3> forces)
    {
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        foreach (Vector3 force in forces)
        {
            rb.AddForce(force * 5.0f, ForceMode.VelocityChange);
            yield return new WaitForSeconds(2f);
        }
        foreach (Collider col in cube.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        cube.GetComponent<CubeMovement>().ResumeMovement();
    }


    Vector3 CalculateRandomDirection(GameObject cube)
    {
        Vector3 directionToPlayer = (player.transform.position - cube.transform.position).normalized;
        Vector3 randomDirection = new(
            directionToPlayer.x + Random.Range(-2.0f, 2.0f),
            0,
            directionToPlayer.z + Random.Range(-2.0f, 2.0f)
        );

        Vector3 adjustedDirection = (randomDirection.normalized + directionToPlayer).normalized;
        return adjustedDirection;
    }
}
