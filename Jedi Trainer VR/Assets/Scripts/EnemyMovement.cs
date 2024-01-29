using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    public float maxDirectionChangeAngle = 30.0f;
    public float directionChangeInterval = 2.0f;

    private GameObject player;
    private Vector3 targetDirection;
    private float timer = 0.0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        CalculateNewDirection();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (player != null && timer >= directionChangeInterval)
        {
            timer = 0.0f;
            CalculateNewDirection();
        }
        transform.position += targetDirection * speed * Time.deltaTime;
    }

    private void CalculateNewDirection()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float angle = Random.Range(-maxDirectionChangeAngle, maxDirectionChangeAngle);
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            targetDirection = rotation * directionToPlayer;
        }
    }
}
