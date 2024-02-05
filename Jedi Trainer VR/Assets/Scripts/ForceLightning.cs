using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class ForceLightning : MonoBehaviour
{
    public InputActionReference triggerInputLeft;
    public InputActionReference triggerInputRight;
    public ParticleSystem lightningEffectLeft;
    public ParticleSystem lightningEffectRight;
    public float extensionThreshold = .75f;

    private GameObject leftController;
    private GameObject rightController;
    private PlayerController player;
    private ParticleSystem lightningEffect;


    private void Awake()
    {
        player = GetComponent<PlayerController>();
        rightController = GameObject.Find("Right Controller");
        leftController = GameObject.Find("Left Controller");
        triggerInputLeft.action.started += context => OnTriggerPressed(isRightHand: false);
        triggerInputLeft.action.canceled += context => OnTriggerReleased(isRightHand: false);
        triggerInputRight.action.started += context => OnTriggerPressed(isRightHand: true);
        triggerInputRight.action.canceled += context => OnTriggerReleased(isRightHand: true);
    }

    private void OnEnable()
    {
        triggerInputLeft.action.Enable();
        triggerInputRight.action.Enable();
    }
    private void OnDisable()
    {
        triggerInputLeft.action.Disable();
        triggerInputRight.action.Disable();
    }

    private void OnTriggerPressed(bool isRightHand)
    {
        lightningEffect = isRightHand ? lightningEffectRight : lightningEffectLeft;
        if (isRightHand) {
            if (player.IsHandExtended(rightController.transform) > extensionThreshold){
                ShootLightning();
            }
        }
        else {
            if (player.IsHandExtended(leftController.transform) > extensionThreshold){
                ShootLightning();
            }
        }
    }
    private void OnTriggerReleased(bool isRightHand)
    {
        if (isRightHand) {
            lightningEffectRight.Stop();
        }
        else {
            lightningEffectLeft.Stop();
        }
    }
    private void ShootLightning()
    {
        if (player.playerForce > 0)
        {
            player.AlterForce(-1);
            StartCoroutine(ShootLightningEnumerator());
        }
    }

    private IEnumerator ShootLightningEnumerator()
    {
        lightningEffect.Play();
        yield return new WaitForSeconds(1f);
        lightningEffect.Stop();
    }
}
