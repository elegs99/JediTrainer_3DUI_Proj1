using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDroidController : MonoBehaviour
{
    public float speed = 1;
    public float hitRadius = .1f;

    private bool hitPlayer = false; 
    private GameObject player;
    private PlayerController playerController;

    private float currentRotateSpeed;
    private Coroutine rotateDirectionCoroutine;
    void Start()
    {
        player = GameObject.Find("Player Target");
        playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > hitRadius)
            {
                MoveTowardsTarget();
            } else {
                hitPlayer = true;
                playerController.AlterHealth(-5);
                Destroy(gameObject);
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
    private void MoveTowardsTarget()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        transform.position += directionToPlayer * speed * Time.deltaTime;
        FaceTowardsTarget();
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