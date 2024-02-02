using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5f;
    public float randomness = 1.05f;
    private Rigidbody rb;
    public bool isPaused = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        StartCoroutine(ApplyRandomForcesTowardsPlayer());
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
                Vector3 randomDirection = CalculateRandomDirection();
                rb.AddForce(randomDirection * moveSpeed, ForceMode.VelocityChange);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
            }
        }
    }

    Vector3 CalculateRandomDirection()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector3 randomOffset = new (
            Random.Range(-randomness, randomness),
            0,
            Random.Range(-randomness, randomness)
        );
        Vector3 slightRandomDirection = directionToPlayer + randomOffset * 0.1f;
        return slightRandomDirection.normalized;
    }


    public void PauseMovement()
    {
        isPaused = true;
        if(rb == null)
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
}
