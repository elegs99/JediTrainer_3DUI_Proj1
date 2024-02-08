using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class ForceFuture : MonoBehaviour
{
    public InputActionReference futureButton;
    public float futureTimeLength = 5f;
    public EnemyController enemySpawnController;
    public Material futureMaterial;

    private PlayerController player;
    private bool isSimulating = false;
    private List<GameObject> originalEnemies = new List<GameObject>();
    private Dictionary<GameObject, List<Vector3>> recordedForces = new Dictionary<GameObject, List<Vector3>>();

    private void Update()
    {
        if(isSimulating)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(SimulateFutureMovement());
        }
    }

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
        enemySpawnController.PauseSpawning();
        foreach (GameObject originalEnemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            GameObject clone = Instantiate(originalEnemy, originalEnemy.transform.position, originalEnemy.transform.rotation);
            ReplaceAllMaterials(clone);
            clone.layer = 6;
            clone.tag = "Untagged";
            foreach (Collider col in  originalEnemy.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
            /*foreach (Collider col in clone.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }*/
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
        enemySpawnController.ResumeSpawning();
    }

    IEnumerator SimulateClonesMovement()
    {
        float startTime = Time.time;
        string enemyType = "";
        while (Time.time - startTime < futureTimeLength)
        {

            foreach (GameObject clone in recordedForces.Keys)
            {
                if (clone.name.Contains("Attack"))
                {
                    enemyType = "Attack";
                    Rigidbody rb = clone.GetComponent<Rigidbody>();
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    Vector3 force = CalculateRandomDirection(clone);
                    rb.AddForce(force * 5f, ForceMode.VelocityChange);
                    recordedForces[clone].Add(force);
                }
                else if (clone.name.Contains("Training"))
                {
                    enemyType = "Training";
                    TrainingDroidController cloneController = clone.GetComponent<TrainingDroidController>();
                    GameObject originalTrainingDroid = originalEnemies.Find(c => c.name == clone.name.Replace("(Clone)", ""));
                    TrainingDroidController originalController = originalTrainingDroid.GetComponent<TrainingDroidController>();
                    cloneController.orbitRadius = originalController.orbitRadius;
                    Rigidbody rb = clone.GetComponent<Rigidbody>();
                    float distanceToPlayer = Vector3.Distance(rb.position, player.transform.position);
                    Vector3 force;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    if (Mathf.Floor(distanceToPlayer) > originalController.orbitRadius && !originalController.switchToOrbit)
                    {
                        force = cloneController.MoveTowardsTarget();
                    }
                    else
                    {
                        force = cloneController.RotateAroundTarget();
                    }
                    recordedForces[clone].Add(force);
                }
            }
            if(enemyType.Contains("Attack"))
            {                 
                yield return new WaitForSeconds(2f);
            }
            else if(enemyType.Contains("Training"))
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator ApplyForcesSequentially(GameObject enemy, List<Vector3> forces)
    {
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        if (enemy.name.Contains("Attack"))
        {
            foreach (Vector3 force in forces)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(force * 5.0f, ForceMode.VelocityChange);
                yield return new WaitForSeconds(2f);
            }
        }
        else if (enemy.name.Contains("Training"))
        {
            foreach (Vector3 force in forces)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(force /  1.35f, ForceMode.VelocityChange);
                yield return new WaitForSeconds(0.5f);
            }
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
        Vector3 randomOffset = new(
            Random.Range(-2.0f, 2.0f),
            Random.Range(0, 3.0f),
            Random.Range(-2.0f, 2.0f)
        );
        Vector3 slightRandomDirection = directionToPlayer + randomOffset * 0.1f;
        return slightRandomDirection.normalized;
    }

    private void ReplaceAllMaterials(GameObject enemy)
    {
        Renderer[] renderers = enemy.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = new Material[renderer.sharedMaterials.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = futureMaterial;
            }

            renderer.sharedMaterials = materials;
        }
    }
}
