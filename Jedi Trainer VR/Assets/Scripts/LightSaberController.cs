using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSaberController : MonoBehaviour
{
    public GameObject lightBlade;
    public Light pointLight;
    public void ExtendBlade() {
        lightBlade.SetActive(true);
        pointLight.enabled = true;
    }
    public void RetractBlade() {
        lightBlade.SetActive(false);
        pointLight.enabled = false;
    }
}
