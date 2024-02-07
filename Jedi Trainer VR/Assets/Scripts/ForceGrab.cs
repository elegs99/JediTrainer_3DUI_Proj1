using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class ForceGrab : MonoBehaviour
{
    public InputActionReference gripButtonLeft;
    public InputActionReference gripButtonRight;
    public InputActionReference anyTriggerButton;
    public InputActionReference IncreaseForceAxisValue;
    public Transform palmCenterRight;
    public Transform palmCenterLeft;
    public GameObject mainCamera;
    public float forceMultiplier = 0.5f;

    private Transform referencePoint;
    private Transform currentPalm;
    private GameObject selectedObject;
    private GameObject potentialSelection;
    private Rigidbody rbTarget;
    private bool isGripping = false;
    private PlayerController player;
    private ForceLightning lightningController;
    private bool isRightHand = true;
    private bool isTriggerPressed = false;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        lightningController = GetComponent<ForceLightning>();
        gripButtonLeft.action.started += context => OnGripButtonPressed(context, _isRightHand: false);
        gripButtonLeft.action.canceled += context => OnGripButtonReleased(_isRightHand: false);
        gripButtonRight.action.started += context => OnGripButtonPressed(context, _isRightHand: true);
        gripButtonRight.action.canceled += context => OnGripButtonReleased(_isRightHand: true);
        anyTriggerButton.action.started += OnTriggerPressed;
        anyTriggerButton.action.canceled += OnTriggerReleased;
        IncreaseForceAxisValue.action.started += CheckAxisDirection;
        referencePoint = new GameObject("ReferencePoint").transform;
    }

    private void OnEnable()
    {
        gripButtonLeft.action.Enable();
        gripButtonRight.action.Enable();
        anyTriggerButton.action.Enable();
    }

    private void OnDisable()
    {
        gripButtonLeft.action.Disable();
        gripButtonRight.action.Disable();
        anyTriggerButton.action.Disable();
    }

    private void Update()
    {
        if (isGripping && selectedObject != null) {
            lightningController.enabled = false;// disable force lighting script
            ApplySelectedState();
        } else {
            lightningController.enabled = true;// enable force lighting script
            AttemptSelectingState();
        }
    }

    private void AttemptSelectingState()
    {
        currentPalm = isRightHand ? palmCenterRight : palmCenterLeft;
        if (player.IsHandExtended(currentPalm) > 0.5f) {
            potentialSelection = PerformRaycast(currentPalm);
            if (potentialSelection != null) {
                JiggleObject(potentialSelection, 0.005f);
            }
        }
    }

    private void ApplySelectedState()
    {
        if (rbTarget != null) {
            if (isTriggerPressed) {
                GameObject targetEnemy = GameObject.FindWithTag("Enemy");
                if (targetEnemy != null) {
                    StartCoroutine(ShootObjectAtEnemy(targetEnemy));
                }
            } else {
                currentPalm = isRightHand ? palmCenterRight : palmCenterLeft;
                Vector3 forceDirection = currentPalm.position - referencePoint.position; // Force based on hand movement
                Vector3 pullDirection = currentPalm.position - selectedObject.transform.position; // Force pulling towards hand
                ApplyForces(forceDirection, pullDirection);
                CapVelocity(rbTarget);
                JiggleObject(selectedObject, 0.002f);
            }
        }
    }
    
    // ADD MORE FORCES HERE 
    // Thumbstick to increase pull force
    private void ApplyForces(Vector3 forceDirection, Vector3 pullDirection)
    {
        float distanceToHand = forceDirection.magnitude;
        Vector3 pullInForce = pullDirection.normalized * Mathf.InverseLerp(1, 0, distanceToHand)*5; // closer to hand more force pulls
        rbTarget.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
        rbTarget.AddForce(pullInForce * forceMultiplier, ForceMode.Acceleration);
    }

    private GameObject PerformRaycast(Transform palm)
        {
        Vector3 direction = (palm.position - mainCamera.transform.position).normalized;
        float raycastDistance = 10f; // Maximum distance for the raycast
        float gridSpacing = 0.1f; // Spacing between the rays in the grid

        // Define offsets for a 3x3 grid
        Vector2[] offsets = new Vector2[]
        {
            new Vector2(-gridSpacing, gridSpacing), // Top-left
            new Vector2(0, gridSpacing), // Top-center
            new Vector2(gridSpacing, gridSpacing), // Top-right
            new Vector2(-gridSpacing, 0), // Middle-left
            new Vector2(0, 0), // Middle-center
            new Vector2(gridSpacing, 0), // Middle-right
            new Vector2(-gridSpacing, -gridSpacing), // Bottom-left
            new Vector2(0, -gridSpacing), // Bottom-center
            new Vector2(gridSpacing, -gridSpacing) // Bottom-right
        };

        foreach (Vector2 offset in offsets)
        {
            // Calculate the ray's origin for the current offset
            Vector3 rayOrigin = palm.position + palm.right * offset.x + palm.up * offset.y;
            // Perform the raycast
            if (Physics.Raycast(rayOrigin, direction*raycastDistance, out RaycastHit hit, raycastDistance))
            {
                if (hit.transform.CompareTag("ForceGrabbable"))
                {
                    // Return the first 'ForceGrabbable' object hit
                    return hit.collider.gameObject;
                }
            }
        }
        return null; // Return null if no 'ForceGrabbable' objects are hit
    }

    private void CapVelocity(Rigidbody rb)
    {
        if (rb.velocity.magnitude > 1) {
            rb.velocity = rb.velocity.normalized;
        }
    }

    private void JiggleObject(GameObject obj, float jiggleAmount)
    {
        obj.transform.position += UnityEngine.Random.insideUnitSphere * jiggleAmount;
    }

    private void OnGripButtonPressed(InputAction.CallbackContext context, bool _isRightHand)
    {
        isRightHand = _isRightHand;
        //Debug.Log($"{(isRightHand ? "Right" : "Left")} hand grip press");
        currentPalm = isRightHand ? palmCenterRight : palmCenterLeft;

        potentialSelection = PerformRaycast(currentPalm);
        if (potentialSelection != null)
        {
            selectedObject = potentialSelection;
            referencePoint.position = currentPalm.position;
            rbTarget = selectedObject.GetComponent<Rigidbody>();
            if (rbTarget != null)
            {
                rbTarget.useGravity = false;
                rbTarget.drag = 3;
            }
            isGripping = true;
        }
    }

    private void OnGripButtonReleased(bool _isRightHand)
    {
        if (isGripping && rbTarget != null)
        {
            rbTarget.useGravity = true;
            rbTarget.drag = 0;
            selectedObject = null;
            rbTarget = null;
        }
        isGripping = false;
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        isTriggerPressed = true;
    }
    private void OnTriggerReleased(InputAction.CallbackContext context)
    {
        isTriggerPressed = false;
        referencePoint.position = currentPalm.position;
    }
    private void CheckAxisDirection(InputAction.CallbackContext context)
    {
        isTriggerPressed = false;
        referencePoint.position = currentPalm.position;
    }
    private IEnumerator ShootObjectAtEnemy(GameObject targetEnemy)
    {
        Debug.Log("Shooting object");
        float speed = 1;
        Vector3 targetDirection = targetEnemy.transform.position - rbTarget.transform.position;
        rbTarget.AddForce(targetDirection * speed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(.2f);
        Destroy(targetEnemy);
        Destroy(selectedObject);
        ReleaseSelectedObject();
    }
    public void ReleaseSelectedObject()
    {
        potentialSelection = null;
        selectedObject = null;
        isGripping = false;
        isTriggerPressed = false;
    }
}