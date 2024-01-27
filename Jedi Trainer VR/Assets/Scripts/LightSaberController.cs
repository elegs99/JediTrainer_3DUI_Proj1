using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaberController : MonoBehaviour
{
    public GameObject lightBlade;
    public CapsuleCollider lightsaberCollider;

    public void ExtendBlade() {
        lightBlade.SetActive(true);
        lightsaberCollider.height = 24;
        lightsaberCollider.center = new Vector3(0,0,8.6f);
    }
    public void RetractBlade() {
        lightBlade.SetActive(false);
        lightsaberCollider.height = 8;
        lightsaberCollider.center = new Vector3(0,0,0.5f);
    }
}
