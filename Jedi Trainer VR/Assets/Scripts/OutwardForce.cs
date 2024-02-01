using UnityEngine;

public class OutwardForce : MonoBehaviour
{
    public float pushForce = 10f;
    public float viewRadius = 10f; // Radius within which to check for enemies
    public float viewAngle = 45f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) // Replace 'Y' with your desired button
        {
            Debug.Log("Pushing");
            PushEnemies();
        }
    }

    void PushEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, viewRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy");
                Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToEnemy);

                //if (angle < viewAngle / 2)
                //{
                    Debug.Log("Checking enemy for rigidbody");
                    Rigidbody enemyRigidbody = hitCollider.GetComponent<Rigidbody>();
                    if (enemyRigidbody != null)
                    {
                        Debug.Log("Pushing enemy");
                        Vector3 backwardForce = -hitCollider.transform.forward * pushForce;
                        enemyRigidbody.AddForce(backwardForce, ForceMode.Impulse);
                    }
                //}
            }
        }
    }
}