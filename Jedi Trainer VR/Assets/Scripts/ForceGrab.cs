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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // select state
        // Find game object in front of hand
        // Add random shake to it
        // move to selected state and save refrence position if grab button pressed
    // selected state
        // Disable gravity on gameobject selected
        // Add force to gameobject in relation to hand movement up/down left/right and foward/back from refrence point
        // When thumbstick is pulled back move the gameobject straight to users hand

}
