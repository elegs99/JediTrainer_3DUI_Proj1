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
    public InputActionReference triggerAction;
    public GameObject leftController;
    public GameObject rightController;

    [Header("Action Values")]
    public float extensionThreshold = 0.4f;
    public float angleThreshold = 35f;

    public Transform referencePoint;
    public ParticleSystem lightningEffect;
    public GameObject enemyPrefab;
    public float predictionTime = 2f;
    public float slowMotionScale = 0.5f;

    private Coroutine primaryButtonHoldCoroutine;
    private const float REQUIRED_HOLD_DURATION = 5.0f;

    private GameObject[] futureEnemies;
    private Vector3[] originalPositions;
    private bool isPredicting = false;

    private void Awake()
    {
        primaryButtonAction.action.started += OnPrimaryButtonPress;
        primaryButtonAction.action.canceled += OnPrimaryButtonRelease;
        secondaryButtonAction.action.started += OnSecondaryButtonPress;
        triggerAction.action.started += OnTriggerPressed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isPredicting)
            {
                FutureSight();
            }
            else
            {
                EndPrediction();
            }
        }
    }

    private void OnEnable()
    {
        primaryButtonAction.action.Enable();
        secondaryButtonAction.action.Enable();
        triggerAction.action.Enable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AlterHealth(-15);
            Destroy(collision.gameObject);
        }
    }

    private void OnDisable()
    {
        Debug.Log("Disabling player controller.");
        primaryButtonAction.action.started -= OnPrimaryButtonPress;
        secondaryButtonAction.action.started -= OnSecondaryButtonPress;
        primaryButtonAction.action.Disable();
        secondaryButtonAction.action.Disable();
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        if(CheckIfExtended(rightController.transform, referencePoint))
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
        if (playerForce > 0)
        {
            AlterForce(-1);
            StartCoroutine(ShootLightningEnumerator());
        }

    }
    private void OnPrimaryButtonPress(InputAction.CallbackContext context)
    {
        Debug.Log("Primary button pressed.");
        //AlterHealth(-5);
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

    private IEnumerator ShootLightningEnumerator()
    {
        lightningEffect.Play();
        yield return new WaitForSeconds(1f);
        lightningEffect.Stop();
    }

    private void HandleButtonHeld(OculusButton button)
    {
        //Debug.Log("Primary button Held.");
        if (button == OculusButton.PrimaryButton)
        {
            if (playerForce-3 >= 0) { // Check if player has enough force 
                AlterForce(-3);
                AlterHealth(25);
            }
        }
    }

    private void OnSecondaryButtonPress(InputAction.CallbackContext context)
    {
    }

    private void FutureSight()
    {
        isPredicting = true;
        Time.timeScale = slowMotionScale;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        futureEnemies = new GameObject[enemies.Length];
        originalPositions = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            originalPositions[i] = enemies[i].transform.position;

            futureEnemies[i] = Instantiate(enemyPrefab, PredictFuturePosition(enemies[i]), Quaternion.identity);
            Renderer renderer = futureEnemies[i].GetComponent<Renderer>();
            if(renderer != null)
            {
                Color c = renderer.material.color;
                c.a = 0.1f;
                renderer.material.color = c;
            }
            futureEnemies[i].tag = "Untagged";
            Rigidbody rbClone = enemies[i].GetComponent<Rigidbody>();
            if (rbClone != null)
            {
                rbClone.isKinematic = true;
            }
            Collider collider = enemies[i].GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

        }
    }

    private Vector3 PredictFuturePosition(GameObject enemy)
    {
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        if (rb != null)
        {
            return enemy.transform.position + rb.velocity * predictionTime;
        }

        return enemy.transform.position;

    }

    private void EndPrediction()
    {
        isPredicting = false;
        Time.timeScale = 1f;
        for (int i = 0; i < futureEnemies.Length; i++)
        {
            Destroy(futureEnemies[i]);
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].transform.position = originalPositions[i];
        }

    }

    public void AlterHealth(int health)
    {
        if ((playerHealth + health) < 0)
        {
            playerHealth += health;
            Die(); // Replace with end game function
        } else if (playerHealth <= (100-health)) { // Check if +health exceeds max
            playerHealth += health;
        }
    }


    public void AlterForce(int force)
    {
        if (playerForce <= (10-force)) { // Check if +force exceeds max
            playerForce += force;
        }
    }


    private void Die()
    {
        Debug.Log("Player died");
    }


}
