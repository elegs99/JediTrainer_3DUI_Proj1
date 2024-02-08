using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDroidController : MonoBehaviour
{
    public float speed = 1;
    public float rotateSpeed = 15;
    public GameObject laserbeam;
    public Transform laserLaunchPoint;
    public Vector2Int orbitRadiusRange;
    public bool isPaused = false;
    private GameObject player;
    private PlayerController playerController;

    private float currentRotateSpeed;
    private Coroutine rotateDirectionCoroutine;
    private Coroutine shootLaserCoroutine;
    public float orbitRadius;
    private bool shootLaser = false;
    public bool switchToOrbit = false;
    private Rigidbody rb;
    private EnemyHealth enemyHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyHealth = GetComponent<EnemyHealth>();

        player = GameObject.FindWithTag("MainCamera");
        playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();

        currentRotateSpeed = rotateSpeed;
        rotateDirectionCoroutine = StartCoroutine(ChangeRotateDirection());
        shootLaserCoroutine = StartCoroutine(Wait2ShootLaser());

        orbitRadius = Random.Range(orbitRadiusRange.x, orbitRadiusRange.y);
    }

    void FixedUpdate()
    {
        if (player != null && !isPaused)
        {
            float distanceToPlayer = Vector3.Distance(rb.position, player.transform.position);
            if (Mathf.Floor(distanceToPlayer) > orbitRadius)
            {
                MoveTowardsTarget();
            }
            else if(Mathf.Floor(distanceToPlayer) == orbitRadius && !switchToOrbit)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                switchToOrbit = true;
                shootLaser = true;
            }
            else
            {
                RotateAroundTarget();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Saber")
        {
            enemyHealth.AlterEnemyHealth(-1);
        }
    }

    public Vector3 MoveTowardsTarget()
    {
        if(player == null)
        {
            rb = GetComponent<Rigidbody>();

            player = GameObject.FindWithTag("MainCamera");
            playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();
        }
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 directionToPlayer = (player.transform.position - rb.position).normalized;
        float distanceToPlayer = Vector3.Distance(rb.position, player.transform.position);
        float speedAdjustmentFactor = Mathf.Clamp01((distanceToPlayer - orbitRadius) / speed);
        float adjustedSpeed = speed * speedAdjustmentFactor;
        if (distanceToPlayer - adjustedSpeed * Time.fixedDeltaTime < orbitRadius)
        {
            adjustedSpeed = (distanceToPlayer - orbitRadius) / Time.fixedDeltaTime;
        }

        rb.velocity = directionToPlayer * adjustedSpeed;
        FaceTowardsTarget();
        return directionToPlayer * speed;
    }

    public Vector3 RotateAroundTarget()
    {
        if (player == null)
        {
            rb = GetComponent<Rigidbody>();

            player = GameObject.FindWithTag("MainCamera");
            playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();

            currentRotateSpeed = rotateSpeed;
            rotateDirectionCoroutine = StartCoroutine(ChangeRotateDirection());
            shootLaserCoroutine = StartCoroutine(Wait2ShootLaser());
        }
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 offset = rb.position - player.transform.position;
        Quaternion rotation = Quaternion.Euler(0, currentRotateSpeed * Time.fixedDeltaTime, 0);
        Vector3 rotatedOffset = rotation * offset;
        Vector3 targetPosition = player.transform.position + rotatedOffset;
        Vector3 direction = (targetPosition - rb.position).normalized;
        rb.velocity = direction * rotateSpeed;

        FaceTowardsTarget();
        return direction * rotateSpeed;
    }

    private void FaceTowardsTarget()
    {
        Vector3 direction = player.transform.position - rb.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, 10 * Time.fixedDeltaTime);
    }

    private IEnumerator ChangeRotateDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            currentRotateSpeed = -currentRotateSpeed; // Reverse rotation direction
        }
    }

    private IEnumerator Wait2ShootLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            if (!isPaused)
            {
                ShootPlayer();
            }
        }
    }

    private void ShootPlayer()
    {
        if (shootLaser && laserbeam != null && laserLaunchPoint != null)
        {
            GameObject laser = Instantiate(laserbeam, laserLaunchPoint.position, laserLaunchPoint.rotation);
        }
    }

    private void OnDestroy()
    {
        playerController.AlterForce(2);
        if (rotateDirectionCoroutine != null)
        {
            StopCoroutine(rotateDirectionCoroutine);
        }
        if (shootLaserCoroutine != null)
        {
            StopCoroutine(shootLaserCoroutine);
        }
    }
    public void ResumeMovement()
    {
        isPaused = false;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
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
}