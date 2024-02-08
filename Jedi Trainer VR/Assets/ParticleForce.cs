using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleForce : MonoBehaviour
{
    private ParticleSystem particleLauncher;
    List<ParticleCollisionEvent> collisionEvents;
    // Start is called before the first frame update
    void Start()
    {
        particleLauncher = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void OnParticleCollision(GameObject other) {
        Debug.Log("Enemy hit by: " + other.gameObject.name);
        if (other.gameObject.tag == "Enemy") {
            Debug.Log("Enemy hit by particle");
            GameObject.Destroy(other);
        }
    }
}
