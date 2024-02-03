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

    private void OnCollisionEnter(Collision collision) // XR rig has no collider so this won't be triggered
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AlterHealth(-15);
            Destroy(collision.gameObject);
        }
    }

    public float IsHandExtended(Transform handTransform) {
        Vector2 bodyVec2 = new Vector2(bodyCenterPoint.transform.position.x, bodyCenterPoint.transform.position.z);
        Vector2 handVec2 = new Vector2(handTransform.position.x, handTransform.position.z);
        Vector2 flatDistance = handVec2 - bodyVec2;
        return flatDistance.magnitude;
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
            Die();
        }
        Debug.Log(playerHealth);
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
