using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForceGrab : MonoBehaviour
{
    public InputActionReference gripButton;
    public Transform palmCenterRight;
    public Transform palmCenterLeft; // Add reference to the left palm center as well
    public GameObject mainCamera;
    public float forceMultiplier = 0.5f;

    private Transform referencePoint;
    private GameObject selectedObject;
    private Rigidbody rbTarget;
    private bool isGripping = false;
    private PlayerController player;
    private bool isRightHand = true; // Default hand is right

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        gripButton.action.started += OnGripButtonPressed;
        gripButton.action.canceled += OnGripButtonReleased;
        referencePoint = new GameObject("ReferencePoint").transform;
    }

    private void OnEnable() => gripButton.action.Enable();

    private void OnDisable() => gripButton.action.Disable();

    private void Update()
    {
        if (isGripping && selectedObject != null) {
            ApplySelectedState();
        } else {
            AttemptSelectingState();
        }
    }

    private void AttemptSelectingState()
    {
        Transform currentPalm = isRightHand ? palmCenterRight : palmCenterLeft;
        if (player.IsHandExtended(currentPalm) > 0.5f) {
            GameObject potentialSelection = PerformRaycast(currentPalm);
            if (potentialSelection != null) {
                JiggleObject(potentialSelection, 0.005f);
            }
        }
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

    private void ApplySelectedState()
    {
        if (rbTarget != null) {
            Transform currentPalm = isRightHand ? palmCenterRight : palmCenterLeft;
            Vector3 forceDirection = currentPalm.position - referencePoint.position;
            Vector3 pullDirection = currentPalm.position - selectedObject.transform.position;
            ApplyForces(forceDirection, pullDirection);
            CapVelocity(rbTarget);
            JiggleObject(selectedObject, 0.002f);
        }
    }

    private void ApplyForces(Vector3 forceDirection, Vector3 pullDirection)
    {
        float distanceToHand = forceDirection.magnitude;
        Vector3 pullInForce = pullDirection.normalized * Mathf.InverseLerp(6, 0, distanceToHand);
        rbTarget.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
        rbTarget.AddForce(pullInForce * forceMultiplier, ForceMode.Acceleration);
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

    private void OnGripButtonPressed(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        isRightHand = device.usages.Contains(CommonUsages.RightHand); // Determine if the right hand is used

        Transform currentPalm = isRightHand ? palmCenterRight : palmCenterLeft;
        GameObject potentialSelection = PerformRaycast(currentPalm);
        if (potentialSelection != null) {
            selectedObject = potentialSelection;
            referencePoint.position = currentPalm.position; // Save the initial grab point
            rbTarget = selectedObject.GetComponent<Rigidbody>();
            if (rbTarget != null) {
                rbTarget.useGravity = false;
                rbTarget.drag = 3;
            }
            isGripping = true;
        }
    }

    private void OnGripButtonReleased(InputAction.CallbackContext context)
    {
        if (isGripping && rbTarget != null) {
            rbTarget.useGravity = true;
            rbTarget.drag = 0;
            selectedObject = null;
            rbTarget = null;
        }
        isGripping = false;
    }
    public void ReleaseSelectedObject()
    {
        selectedObject = null;
        isGripping = false;
    }
}