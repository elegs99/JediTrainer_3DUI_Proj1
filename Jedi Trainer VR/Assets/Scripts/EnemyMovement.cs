using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 1;
    public float rotateSpeed = 15; // Speed of rotation around the camera
    public GameObject laserbeam;
    public Transform laserLaunchPoint;

    private GameObject player;
    private float currentRotateSpeed;
    private Coroutine rotateDirectionCoroutine;
    private Coroutine shootLaserCoroutine;
    private float orbitRadius;
    private bool shootLaser = false;

    void Start()
    {
        player = GameObject.FindWithTag("MainCamera");
        currentRotateSpeed = rotateSpeed;
        rotateDirectionCoroutine = StartCoroutine(ChangeRotateDirectionRoutine());
        orbitRadius = Random.Range(2, 6);
        shootLaserCoroutine = StartCoroutine(Wait2ShootLaser());
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > orbitRadius)
            {
                MoveTowardsCamera();
            }
            else
            {
                
                RotateAroundCamera();
                ShootPlayer();
            }
        }
    }
    // this part of the code isn't working right now will fix tmrw
    // it's because the grab interactable diasables the collider on the saber when you are holding it
    // Should put seperate collider on the blade and handle and update blade tag to saber
    // Also remove collider scaling from the lightsaber controller script
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Saber") {
            Destroy(gameObject);
        }
    }
    private void MoveTowardsCamera()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        transform.position += directionToPlayer * speed * Time.deltaTime;
        FaceTowardsCamera();
    }

    private void RotateAroundCamera()
    {
        Vector3 relativePosition = transform.position - player.transform.position;
        relativePosition = Quaternion.Euler(0, currentRotateSpeed * Time.deltaTime, 0) * relativePosition;
        transform.position = player.transform.position + relativePosition;
        FaceTowardsCamera();
    }

    private void FaceTowardsCamera()
    {
        transform.LookAt(player.transform);
    }
    private IEnumerator ChangeRotateDirectionRoutine()
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
            yield return new WaitForSeconds(Random.Range(2f, 5f)); // Random wait time between 2 and 5 seconds
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
        if (rotateDirectionCoroutine != null)
        {
            StopCoroutine(rotateDirectionCoroutine);
        }
    }
}