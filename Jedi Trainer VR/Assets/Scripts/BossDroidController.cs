using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDroidController : MonoBehaviour
{
    public float speed = 1;
    public float hitRadius = .1f;
    public float randomness = 1.05f;
    public float moveSpeed = 5f;
    public bool isPaused = false;
    public GameObject trainingPrefab;
    public GameObject attackPrefab;
    public GameObject enemySpawner;

    private bool hitPlayer = false;
    private GameObject player;
    private PlayerController playerController;

    private Rigidbody rb;

    private int enemiesSpawned = 0;

    void Start()
    {
        player = GameObject.Find("Player Target");
        playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        Random.InitState((int)System.DateTime.Now.Ticks);
        StartCoroutine(ApplyRandomForcesTowardsPlayer());
        StartCoroutine(BossSpawner());
    }

    void Update()
    {
        if(player == null)
        {
            player = GameObject.Find("Player Target");
            playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody>();
            Random.InitState((int)System.DateTime.Now.Ticks);
            StartCoroutine(ApplyRandomForcesTowardsPlayer());
            StartCoroutine(BossSpawner());
        }
    }

    IEnumerator BossSpawner()
    {

        while(true)
        {
            if (isPaused)
            {
                yield return null;
            }
            int flipResult = Random.Range(0, 2);
            if (flipResult == 0)
            {
                flipResult = Random.Range(0, 2);
                enemiesSpawned++;
                if (flipResult == 0)
                {
                    SpawnTrainingDroid();
                }
                else
                {
                    SpawnAttackDroid();
                }
            }
            yield return new WaitForSeconds(5f);

        }
    }

    // this part of the code isn't working right now will fix tmrw
    // it's because the grab interactable diasables the collider on the saber when you are holding it
    // Should put seperate collider on the blade and handle and update blade tag to saber
    // Also remove collider scaling from the lightsaber controller script
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Saber")
        {
            Destroy(gameObject);

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Vector3 collisionDirection = transform.position - collision.contacts[0].point;
            collisionDirection.Normalize();
            rb.AddForce(collisionDirection * moveSpeed, ForceMode.VelocityChange);
            FaceTowardsTarget();
        }
    }

    void SpawnTrainingDroid()
    {
        GameObject trainingDroid = Instantiate(trainingPrefab, enemySpawner.transform.position, enemySpawner.transform.rotation);
        trainingDroid.name = "Training Droid " + enemiesSpawned;
    }

    void SpawnAttackDroid()
    {
        GameObject attackDroid = Instantiate(attackPrefab, enemySpawner.transform.position, enemySpawner.transform.rotation);
        attackDroid.name = "Attack Droid " + enemiesSpawned;
    }

    IEnumerator ApplyRandomForcesTowardsPlayer()
    {
        while (true)
        {
            if (isPaused)
            {
                yield return null;
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                Vector3 randomDirection = CalculateRandomDirection();
                rb.AddForce(randomDirection * moveSpeed, ForceMode.VelocityChange);
                FaceTowardsTarget();
                yield return new WaitForSeconds(Random.Range(1f, 3f));
            }
        }
    }

    Vector3 CalculateRandomDirection()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector3 randomOffset = new(
            Random.Range(-randomness, randomness),
            Random.Range(-randomness, randomness),
            Random.Range(-randomness, randomness)
        );
        Vector3 slightRandomDirection = directionToPlayer + randomOffset * 0.1f;
        return slightRandomDirection.normalized;
    }

    public void PauseMovement()
    {
        isPaused = true;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void ResumeMovement()
    {
        isPaused = false;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    private void FaceTowardsTarget()
    {
        transform.LookAt(player.transform);
        transform.Rotate(0, 90, 0);
    }

    private void OnDestroy()
    {
        if (!hitPlayer)
        {
            playerController.AlterForce(2);
        }
    }
}