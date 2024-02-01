using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float directionChangeInterval = 1f;
    private bool isMoving = true;
    private Transform playerTransform;
    private Vector3 randomDirectionOffset;
    private float directionChangeTimer;
    private bool wasMovingBeforeFreeze;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {

        if (isMoving && playerTransform != null)
        {
            MoveTowardsPlayer();
        }
    }

    public void FreezeMovement()
    {
        wasMovingBeforeFreeze = isMoving;
        StopMovement();
    }

    public void UnfreezeMovement()
    {
        if (wasMovingBeforeFreeze)
        {
            StartMovement();
        }
    }

    void MoveTowardsPlayer()
    {
        if (playerTransform != null)
        {
            directionChangeTimer -= Time.deltaTime;
            if (directionChangeTimer <= 0)
            {
                ChangeDirection();
                directionChangeTimer = directionChangeInterval;
            }

            Vector3 targetDirection = (playerTransform.position - transform.position).normalized;
            Vector3 moveDirection = (targetDirection + randomDirectionOffset).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    void ChangeDirection()
    {
        randomDirectionOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    public void StartMovement()
    {
        isMoving = true;
    }
}
