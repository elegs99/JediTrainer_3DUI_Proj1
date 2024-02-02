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
    private GameObject player;
    private PlayerController playerController;

    private float currentRotateSpeed;
    private Coroutine rotateDirectionCoroutine;
    private Coroutine shootLaserCoroutine;
    private float orbitRadius;
    private bool shootLaser = false;

    void Start()
    {
        player = GameObject.FindWithTag("MainCamera");
        playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();

        currentRotateSpeed = rotateSpeed;
        rotateDirectionCoroutine = StartCoroutine(ChangeRotateDirection());
        shootLaserCoroutine = StartCoroutine(Wait2ShootLaser());

        orbitRadius = Random.Range(orbitRadiusRange.x, orbitRadiusRange.y);
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > orbitRadius)
            {
                MoveTowardsTarget();
            }
            else
            {
                
                RotateAroundTarget();
                ShootPlayer();
            }
        }
    }
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Saber") {
            Destroy(gameObject);
        }
    }
    private void MoveTowardsTarget()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        transform.position += directionToPlayer * speed * Time.deltaTime;
        FaceTowardsTarget();
    }

    private void RotateAroundTarget()
    {
        Vector3 relativePosition = transform.position - player.transform.position;
        relativePosition = Quaternion.Euler(0, currentRotateSpeed * Time.deltaTime, 0) * relativePosition;
        transform.position = player.transform.position + relativePosition;
        FaceTowardsTarget();
    }

    private void FaceTowardsTarget()
    {
        transform.LookAt(player.transform);
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
            shootLaser = true;
        }
    }
    private void ShootPlayer()
    {
        if (shootLaser && laserbeam != null && laserLaunchPoint != null)
        {
            Instantiate(laserbeam, laserLaunchPoint.position, laserLaunchPoint.rotation);
            shootLaser = false;
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
}