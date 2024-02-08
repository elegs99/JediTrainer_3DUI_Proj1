using System.Collections;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class LaserbeamController : MonoBehaviour
{
    public float speed = 5.0f;
    private GameObject target;
    private PlayerController playerController;
    private bool switchedTarget = false;
    private EnemyHealth enemyHealth;
    void Start()
    {
        playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();
        target = GameObject.Find("Player Target");
    }

    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget < .2f) {
                if (!switchedTarget && playerController != null) {
                    playerController.AlterHealth(-5);
                    Destroy(gameObject);
                } else if (playerController != null) {
                    enemyHealth = target.GetComponent<EnemyHealth>();
                    enemyHealth.AlterEnemyHealth(-1);
                    Destroy(gameObject);
                }
            }
            // Move the laser towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.LookAt(target.transform);
        } else {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("Saber"))
        {
            // Change target to GameObject with enemy tag
            target = GameObject.FindWithTag("Enemy");
            if (target != null) 
            {
                switchedTarget = true;
                transform.LookAt(target.transform);
                speed = 15;
            } else {
                Destroy(gameObject);
            }
        }
    }
}