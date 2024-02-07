using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForceHeal : MonoBehaviour
{
    public InputActionReference healButton;
    public ParticleSystem healthRestoreEffectLeft;
    public ParticleSystem healthRestoreEffectRight;
    
    [Tooltip("Required amount of time the heal button needs to be pressed")]
    public float requiredHoldDuration = 5.0f;

    private bool isHealing;
    private Coroutine healingCoroutine;
    private PlayerController player;

    private void Awake()
    {
        player = gameObject.GetComponent<PlayerController>();
        healButton.action.performed += OnHealButtonPressed;
        healButton.action.canceled += OnHealButtonReleased;
    }

    private void OnEnable()
    {
        healButton.action.Enable();
    }

    private void OnDisable()
    {
        healButton.action.performed -= OnHealButtonPressed;
        healButton.action.canceled -= OnHealButtonReleased;
        healButton.action.Disable();
    }

    private void OnHealButtonPressed(InputAction.CallbackContext context)
    {
        //Debug.Log("Heal button pressed");
        healingCoroutine = StartCoroutine(HealButtonHoldCheck());
    }

    private void OnHealButtonReleased(InputAction.CallbackContext context)
    {
        if (isHealing)
        {
            healthRestoreEffectLeft.Stop();
            healthRestoreEffectRight.Stop();
            StopCoroutine(healingCoroutine);
            isHealing = false;
        }
    }

    private IEnumerator HealButtonHoldCheck()
    {
        //Debug.Log("Heal button hold check started");
        isHealing = true;
        healthRestoreEffectLeft.Play();
        healthRestoreEffectRight.Play();
        yield return new WaitForSeconds(requiredHoldDuration);
        if (player.playerForce > 1)
        {
            ApplyHealEffect();
        }
    }

    private void ApplyHealEffect()
    {
        player.AlterForce(-1);
        player.AlterHealth(30);
        healthRestoreEffectLeft.Stop();
        healthRestoreEffectRight.Stop();
    }
}
