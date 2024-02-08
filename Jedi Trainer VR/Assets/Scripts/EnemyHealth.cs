using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 1;
    public GameObject droidExplosion;
    void Update() {
        if (health <= 0) {
            Instantiate(droidExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    public void AlterEnemyHealth(float healthChange) {
        health += healthChange;
    }
}
