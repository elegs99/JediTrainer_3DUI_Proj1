using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public enum OculusButton
    {
        PrimaryButton, // X or A button
        SecondaryButton // Y or B button
    }

    [Header("Player Stats")]
    [Range(0, 100)]
    public int playerHealth = 50;
    [Range(0, 10)]
    public int playerForce = 10;

    [Header("Input Actions")]
    public InputActionReference primaryButtonAction;
    public InputActionReference secondaryButtonAction;

    private Coroutine primaryButtonHoldCoroutine;
    private const float REQUIRED_HOLD_DURATION = 5.0f;

    private void Awake()
    {
        primaryButtonAction.action.started += OnPrimaryButtonPress;
        primaryButtonAction.action.canceled += OnPrimaryButtonRelease;
        secondaryButtonAction.action.started += OnSecondaryButtonPress;
    }

    private void OnEnable()
    {
        primaryButtonAction.action.Enable();
        secondaryButtonAction.action.Enable();
    }

    private void OnDisable()
    {
        Debug.Log("Disabling player controller.");
        primaryButtonAction.action.started -= OnPrimaryButtonPress;
        secondaryButtonAction.action.started -= OnSecondaryButtonPress;
        primaryButtonAction.action.Disable();
        secondaryButtonAction.action.Disable();
    }

    private void OnPrimaryButtonPress(InputAction.CallbackContext context)
    {
        Debug.Log("Primary button pressed.");
        primaryButtonHoldCoroutine = StartCoroutine(PrimaryButtonHoldCheck());
    }

    private void OnPrimaryButtonRelease(InputAction.CallbackContext context)
    {
        if (primaryButtonHoldCoroutine != null)
        {
            StopCoroutine(primaryButtonHoldCoroutine);
        }
    }
    private IEnumerator PrimaryButtonHoldCheck()
    {
        yield return new WaitForSeconds(REQUIRED_HOLD_DURATION);
        HandleButtonHeld(OculusButton.PrimaryButton);
    }

    private void HandleButtonHeld(OculusButton button)
    {
        if (button == OculusButton.PrimaryButton)
        {
            AlterForce(-3);
            AlterHealth(25);
        }
    }

    private void OnSecondaryButtonPress(InputAction.CallbackContext context)
    {
    }

    public void AlterHealth(int health)
    {
        playerHealth += health;
        if (playerHealth <= 0)
        {
            Die();
        }
    }


    public void AlterForce(int force)
    {
        playerForce += force;
    }


    private void Die()
    {
        Debug.Log("Player died");
    }


}
