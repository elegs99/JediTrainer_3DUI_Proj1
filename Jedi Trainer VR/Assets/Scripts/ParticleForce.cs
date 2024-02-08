using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleForce : MonoBehaviour
{
    private ParticleSystem particleLauncher;
    List<ParticleCollisionEvent> collisionEvents;
    private EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        particleLauncher = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void OnParticleCollision(GameObject target) {
        if (target.gameObject.tag == "Enemy") {
            enemyHealth = target.GetComponent<EnemyHealth>();
            enemyHealth.AlterEnemyHealth(-1);
        }
    }
}
