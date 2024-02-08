using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDroidController : MonoBehaviour
{
    public float speed = 1;
    public float hitRadius = .1f;
    public float randomness = 1.05f;
    public float moveSpeed = 5f;
    public bool isPaused = false;

    private bool hitPlayer = false; 
    private GameObject player;
    private PlayerController playerController;

    private Rigidbody rb;
    private float currentRotateSpeed;
    private Coroutine rotateDirectionCoroutine;
    private EnemyHealth enemyHealth;
    void Start()
    {
        player = GameObject.Find("Player Target");
        enemyHealth = GetComponent<EnemyHealth>();
        playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(ApplyRandomForcesTowardsPlayer());
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Saber") {
            enemyHealth.AlterEnemyHealth(-1);
        }
        //Debug.Log("Triggered by: " + collider.gameObject.name);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided with: " + collision.gameObject.name);
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
    }

    private void OnDestroy()
    {
        if (!hitPlayer) {
            playerController.AlterForce(2);
        }
    }
}