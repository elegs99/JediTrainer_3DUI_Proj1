using UnityEngine;

public class OutwardForce : MonoBehaviour
{
    public float pushForce = 500f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ApplyOutwardForceToEnemies();
        }
    }

    void ApplyOutwardForceToEnemies()
    {
        Debug.Log("Applying force to enemies");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Debug.Log("Applying force to enemy: " + enemy.name);
            Rigidbody rb = enemy.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 forceDirection = (enemy.transform.position - transform.position).normalized;

                rb.AddForce(forceDirection * pushForce);
            }
        }
    }
}
