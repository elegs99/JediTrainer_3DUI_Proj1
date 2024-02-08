using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 1;
    void Update() {
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
    public void AlterEnemyHealth(float healthChange) {
        health += healthChange;
    }
}
