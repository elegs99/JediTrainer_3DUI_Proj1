using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightSaberController : MonoBehaviour
{
    public GameObject lightBlade;
    public GameObject saberHitEffect;
    public Transform saberBase;
    bool isExtended = false;
    bool isEffectSpawned = false;
    GameObject hitEffect;
    void Update() {
        if (isExtended) {
            if (isEffectSpawned) {
                if (Physics.Raycast(saberBase.position, -saberBase.transform.up, out RaycastHit hit, .9f)) {
                    hitEffect.transform.position = hit.point;
                } else {
                    Destroy(hitEffect);
                    isEffectSpawned = false;
                }
            } else {
                if (Physics.Raycast(saberBase.position, -saberBase.transform.up, out RaycastHit hit, .9f)) {
                    hitEffect = Instantiate(saberHitEffect, hit.point, hit.transform.rotation);
                    isEffectSpawned = true;
                }
            }
        } else {
            Destroy(hitEffect);
            isEffectSpawned = false;
        }
    }
    public void ExtendBlade() {
        isExtended = true;
        lightBlade.SetActive(true);
    }
    public void RetractBlade() {
        isExtended = false;
        lightBlade.SetActive(false);
    }
}
