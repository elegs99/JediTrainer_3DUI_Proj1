using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class ForceGrab : MonoBehaviour
{
    public InputActionReference gripButton;
    public Transform PalmCenterRight;
    public GameObject mainCamera;
    public float forceMultiplier = .5f;

    private Transform referencePoint;
    private GameObject selectedObject;
    private GameObject leftController;
    private GameObject rightController;
    private PlayerController player;
    private Rigidbody rbTarget;
    private bool gripPressed = false;
    void Awake()
    {
        player = gameObject.GetComponent<PlayerController>();
        rightController = GameObject.Find("Right Controller");
        leftController = GameObject.Find("Left Controller");
        gripButton.action.started += OnGripButtonPressed;
        gripButton.action.canceled += OnGripButtonReleased;
        referencePoint = new GameObject("ReferencePoint").transform;
    }

    private void OnEnable()
    {
        gripButton.action.Enable();
    }
    
    private void OnDisable()
    {
        gripButton.action.started -= OnGripButtonPressed;
        gripButton.action.canceled -= OnGripButtonReleased;
        gripButton.action.Disable();
    }

    void Update() {
        if (gripPressed && selectedObject != null) {
            SelectedState();
        } else {
            SelectingState();
        }
    }

    private void SelectingState() {
        if (player.IsHandExtended(rightController.transform) > .5f) {
            Vector3 forceDirection = PalmCenterRight.transform.position - mainCamera.transform.position;
            if (Physics.Raycast(PalmCenterRight.position, forceDirection * 10, out RaycastHit hit, 10)) { // Adjust the distance as needed
                if (hit.transform.CompareTag("ForceGrabbable")) {
                    selectedObject = hit.collider.gameObject;
                    // Add a slight random shake to the object to indicate it can be interacted with
                    selectedObject.transform.position += Random.insideUnitSphere * 0.005f; // Adjust the shake intensity as needed
                }
            } else {
                selectedObject = null;
            }
        }
    }
    private void SelectedState() {
        // Add force to gameobject in relation to hand movement up/down left/right and foward/back from refrence point
        // Calculate the new position for the object based on the hand's current position
        Vector3 forceDirection = PalmCenterRight.position - referencePoint.position;
        if (forceDirection.magnitude < .1) {
            rbTarget.drag = 5;
        } else {
            rbTarget.drag = 0;
        }
        if (rbTarget.velocity.magnitude > 1) rbTarget.velocity = Vector3.Scale(rbTarget.velocity.normalized, Vector3.one);
        rbTarget.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
    }
    private void OnGripButtonPressed(InputAction.CallbackContext context) {
        if (selectedObject != null) {
            if (!gripPressed) referencePoint.position = PalmCenterRight.transform.position; // Save the initial grab point
            rbTarget = selectedObject.GetComponent<Rigidbody>();
            if (rbTarget != null) {
                rbTarget.useGravity = false;
            }
            gripPressed = true;
        }
    }
    private void OnGripButtonReleased(InputAction.CallbackContext context) {
        if (gripPressed && rbTarget != null) {
            rbTarget.useGravity = true;
            selectedObject = null;
        }
        gripPressed = false;
    }
}