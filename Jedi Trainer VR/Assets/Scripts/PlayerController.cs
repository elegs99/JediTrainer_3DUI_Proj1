using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player Stats")]
    [Range(0, 100)]
    public int playerHealth = 50;
    [Range(0, 10)]
    public int playerForce = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AlterHealth(-15);
            Destroy(collision.gameObject);
        }
    }

    public void AlterHealth(int health)
    {
        playerForce += health;
        if (playerHealth <= 0)
        {
            Die();
        }
    }

    public void AlterForce(int force)
    {
        playerForce += force;
        if (playerForce <= 10)
        {
            playerForce = 10;
        }
        else if(playerForce <= 0)
        {
            playerForce = 0;
        }
    }

    private void Die()
    {
        Debug.Log("Player died");
    }
}
