using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForceLightning : MonoBehaviour
{
    public InputActionReference lightningButton;
    public ParticleSystem lightningEffect;
    public Transform referencePoint;

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
        if (CheckIfExtended(rightController.transform, referencePoint))
        {
            ShootLightning();
        }
    }

    private bool CheckIfExtended(Transform controllerTransform, Transform referencePoint)
    {
        Vector3 armVector = controllerTransform.position - referencePoint.position;
        Vector3 torsoDirection = referencePoint.forward;
        float armTorsoAngle = Vector3.Angle(armVector, torsoDirection);
        float armLength = armVector.magnitude;
        return armTorsoAngle >= angleThreshold && armLength >= extensionThreshold;
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
