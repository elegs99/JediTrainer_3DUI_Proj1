using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class ForceLightning : MonoBehaviour
{
    public InputActionReference lightningButton;
    public ParticleSystem lightningEffect;
    public Transform referencePoint;
    public float extendHandThreshold = .8f;

    private GameObject leftController;
    private GameObject rightController;
    private PlayerController player;

    public float extensionThreshold = 0.4f;
    public float angleThreshold = 35f;


    private void Awake()
    {
        player = gameObject.GetComponent<PlayerController>();
        rightController = GameObject.Find("Right Controller");
        leftController = GameObject.Find("Left Controller");
        lightningButton.action.started += OnLightningButtonPressed;
    }

    private void OnEnable()
    {
        lightningButton.action.Enable();
    }
    private void OnDisable()
    {
        lightningButton.action.started -= OnLightningButtonPressed;
        lightningButton.action.Disable();
    }

    private void OnLightningButtonPressed(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        if (device.usages.Contains(CommonUsages.LeftHand)) {
            if (player.IsHandExtended(leftController.transform) > extendHandThreshold){
                ShootLightning();
            }
        }
        else {
            if (player.IsHandExtended(rightController.transform) > extendHandThreshold){
                ShootLightning();
            }
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
