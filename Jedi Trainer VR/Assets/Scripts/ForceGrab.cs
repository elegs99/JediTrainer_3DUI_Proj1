using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForceGrab : MonoBehaviour
{
    public InputActionReference gripButton;

    private Transform referencePoint;
    private GameObject leftController;
    private GameObject rightController;
    private PlayerController player;
    // Start is called before the first frame update
    void Awake()
    {
        player = gameObject.GetComponent<PlayerController>();
        rightController = GameObject.Find("Right Controller");
        leftController = GameObject.Find("Left Controller");
        gripButton.action.started += OnGripButtonPressed;
    }

    private void OnEnable()
    {
        gripButton.action.Enable();
    }
    private void OnDisable()
    {
        gripButton.action.started -= OnGripButtonPressed;
        gripButton.action.Disable();
    }
    private void Update() {
        // selecting state
            // When Hand is outstretched look for closest game object in front of hand
            // Add random shake to it
    }
    private void OnGripButtonPressed(InputAction.CallbackContext context) {   
        // selected state
            // save reference position
            // Disable gravity on gameobject selected
            // Add force to gameobject in relation to hand movement up/down left/right and foward/back from refrence point
            // When thumbstick is pulled back move the gameobject straight to users hand
            // On grip button release enable gravity on object, set selected object to nothing
    }
}
