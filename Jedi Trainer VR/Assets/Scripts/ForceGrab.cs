using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class ForceGrab : MonoBehaviour
{
    public InputActionReference gripButton;
    public Transform palmCenterRight;
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
            Vector3 forceDirection = palmCenterRight.transform.position - mainCamera.transform.position;
            // Should have a 3x3 grid of ray cast so corners of the hands detect stuff too
            // Write external raycast function if time
            if (Physics.Raycast(palmCenterRight.position, forceDirection * 10, out RaycastHit hit, 10)) { // Adjust the distance as needed
                if (hit.transform.CompareTag("ForceGrabbable")) {
                    selectedObject = hit.collider.gameObject;
                    // Add a slight random shake to the object to indicate it can be interacted with
                    selectedObject.transform.position += UnityEngine.Random.insideUnitSphere * 0.005f; // Adjust the shake intensity as needed
                }
            }
        }
    }
    private void SelectedState() {
        // Add force to gameobject in relation to hand movement up/down left/right and forward/back from reference point
        Vector3 forceDirection = palmCenterRight.position - referencePoint.position;

        // Calculate additional force direction
        Vector3 pullInForce = palmCenterRight.position - selectedObject.transform.position;
        float distance2Hand = pullInForce.magnitude;

        // Scale additional force so it's near 0 when distance2Hand is above 4 and close to 1 as distance2Hand approaches 0
        float scaledPullInForceMagnitude = Mathf.InverseLerp(4, 0, distance2Hand);
        Vector3 scaledPullInForce = pullInForce.normalized * scaledPullInForceMagnitude;

        rbTarget.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
        rbTarget.AddForce(scaledPullInForce * 1, ForceMode.Acceleration);

        // Cap the velocity at a maximum of 1 if it exceeds that
        if (rbTarget.velocity.magnitude > 1) {
            rbTarget.velocity = rbTarget.velocity.normalized;
        }
        selectedObject.transform.position += UnityEngine.Random.insideUnitSphere * 0.002f;
    }
    private void OnGripButtonPressed(InputAction.CallbackContext context) {
        if (selectedObject != null) {
            float distance2Hand = (palmCenterRight.position - selectedObject.transform.position).magnitude;
            if (distance2Hand > .2f) {
                referencePoint.position = palmCenterRight.transform.position; // Save the initial grab point
                rbTarget = selectedObject.GetComponent<Rigidbody>();
                if (rbTarget != null) {
                    rbTarget.useGravity = false;
                    rbTarget.drag = 5;
                }
                gripPressed = true;
            }
        }
    }
    private void OnGripButtonReleased(InputAction.CallbackContext context) {
        if (gripPressed && rbTarget != null) {
            rbTarget.useGravity = true;
            rbTarget.drag = 0;
            selectedObject = null;
            rbTarget = null;
        }
        gripPressed = false;
    }
}