using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player Stats")]
    [Range(0, 100)]
    public int playerHealth = 50;
    [Range(0, 10)]
    public int playerForce = 10;
    public GameObject bodyCenterPoint;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AlterHealth(-15);
            Destroy(collision.gameObject);
        }
    }

    public float IsHandExtended(Transform handTransform) {
        Vector3 distance = handTransform.position - bodyCenterPoint.transform.position;
        return distance.magnitude;
    }

    public void AlterHealth(int health)
    {
        playerHealth += health;
        if(playerHealth >= 100)
        {
            playerHealth = 100;
        }
        else if (playerHealth <= 0)
        {
            playerHealth = 0;
            Die();
        }
        //Debug.Log(playerHealth);
    }

    public void AlterForce(int force)
    {
        playerForce += force;
        if (playerForce >= 10)
        {
            playerForce = 10;
        }
        else if(playerForce <= 0)
        {
            playerForce = 0;
        }
        //Debug.Log(playerForce);
    }

    private void Die()
    {
        Debug.Log("Player died");
    }
}
