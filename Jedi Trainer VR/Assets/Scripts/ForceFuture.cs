using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForceFuture : MonoBehaviour
{
    public InputActionReference futureButton;
    public float futureTimeLength = 5f;
    
    private PlayerController player;
    private bool isSimulating = false;
    private List<GameObject> originalEnemies = new List<GameObject>();
    private Dictionary<GameObject, List<Vector3>> recordedForces = new Dictionary<GameObject, List<Vector3>>();

    private void Awake()
    {
        player = gameObject.GetComponent<PlayerController>(); ;
        futureButton.action.started += OnFutureButtonPressed;
    }

    void OnFutureButtonPressed(InputAction.CallbackContext context)
    {
        if(isSimulating)
        {
            return;
        }
        StartCoroutine(SimulateFutureMovement());
    }

    private void OnEnable()
    {
        futureButton.action.Enable();
    }

    private void OnDisable()
    {
        futureButton.action.started -= OnFutureButtonPressed;
        futureButton.action.Disable();
    }

    IEnumerator SimulateFutureMovement()
    {
        isSimulating = true;
        foreach (GameObject originalEnemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            GameObject clone = Instantiate(originalEnemy, originalEnemy.transform.position, originalEnemy.transform.rotation);
            foreach (Collider col in  originalEnemy.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
            foreach (Collider col in clone.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
            if(originalEnemy.name.Contains("Attack"))
            {
                AttackDroidController cloneController = clone.GetComponent<AttackDroidController>();

                originalEnemy.GetComponent<AttackDroidController>().PauseMovement();
                cloneController.isPaused = true;
            }
            else if(originalEnemy.name.Contains("Training"))
            {
                TrainingDroidController cloneController = clone.GetComponent<TrainingDroidController>();

                originalEnemy.GetComponent<TrainingDroidController>().PauseMovement();
                cloneController.isPaused = true;
            }

            originalEnemies.Add(originalEnemy);

            recordedForces.Add(clone, new List<Vector3>());
        }

        yield return StartCoroutine(SimulateClonesMovement());

        foreach (KeyValuePair<GameObject, List<Vector3>> entry in recordedForces)
        {
            GameObject originalEnemy = originalEnemies.Find(c => c.name == entry.Key.name.Replace("(Clone)", ""));
            StartCoroutine(ApplyForcesSequentially(originalEnemy, entry.Value));
            Destroy(entry.Key);
        }
        originalEnemies.Clear();
        recordedForces.Clear();
        isSimulating = false;
    }

    IEnumerator SimulateClonesMovement()
    {
        float startTime = Time.time;
        while (Time.time - startTime < futureTimeLength)
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

    IEnumerator ApplyForcesSequentially(GameObject enemy, List<Vector3> forces)
    {
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        foreach (Vector3 force in forces)
        {
            rb.AddForce(force * 5.0f, ForceMode.VelocityChange);
            yield return new WaitForSeconds(2f);
        }
        foreach (Collider col in enemy.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        if (enemy.name.Contains("Attack"))
        {
             enemy.GetComponent<AttackDroidController>().ResumeMovement();
        }
        else if (enemy.name.Contains("Training"))
        {
            enemy.GetComponent<TrainingDroidController>().ResumeMovement();
        }

    }

    Vector3 CalculateRandomDirection(GameObject enemy)
    {
        Vector3 directionToPlayer = (player.transform.position - enemy.transform.position).normalized;
        Vector3 randomDirection = new(
            directionToPlayer.x + Random.Range(-2.0f, 2.0f),
            0,
            directionToPlayer.z + Random.Range(-2.0f, 2.0f)
        );

        Vector3 adjustedDirection = (randomDirection.normalized + directionToPlayer).normalized;
        return adjustedDirection;
    }
}
