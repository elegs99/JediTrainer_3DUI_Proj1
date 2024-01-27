using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightSaberController : MonoBehaviour
{
    public GameObject lightBlade;
    public CapsuleCollider lightsaberCollider;
    public GameObject saberHitEffect;
    public Transform saberBase;
    bool isExtended = false;
    bool isEffectSpawned = false;
    GameObject hitEffect;
    void Update() {
        if (isExtended) {
            if (isEffectSpawned) {
                if (Physics.Raycast(saberBase.position, -saberBase.transform.up, out RaycastHit hit, .9f)) {
                    Debug.Log("moving");
                    hitEffect.transform.position = hit.point;
                } else {
                    Debug.Log("Destroying");
                    Destroy(hitEffect);
                    isEffectSpawned = false;
                }
            } else {
                if (Physics.Raycast(saberBase.position, -saberBase.transform.up, out RaycastHit hit, .9f)) {
                    Debug.Log("spawning");
                    hitEffect = Instantiate(saberHitEffect, hit.point, hit.transform.rotation);
                    isEffectSpawned = true;
                }
            }
        } else {
            Debug.Log("Destroying");
            Destroy(hitEffect);
            isEffectSpawned = false;
        }
    }
    public void ExtendBlade() {
        isExtended = true;
        lightBlade.SetActive(true);
        lightsaberCollider.height = 24;
        lightsaberCollider.center = new Vector3(0,0,8.6f);
    }
    public void RetractBlade() {
        isExtended = false;
        lightBlade.SetActive(false);
        lightsaberCollider.height = 8;
        lightsaberCollider.center = new Vector3(0,0,0.5f);
    }
}
