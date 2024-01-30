using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDroidController : MonoBehaviour
{
    public float speed = 1;

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

    private void FaceTowardsCamera()
    {
        transform.LookAt(player.transform);
    }

    private void OnDestroy()
    {
        playerController.AlterForce(2);
        if (rotateDirectionCoroutine != null)
        {
            StopCoroutine(rotateDirectionCoroutine);
        }
    }
}