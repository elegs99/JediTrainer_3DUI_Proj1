using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForceMaxPower : MonoBehaviour
{
    public InputActionReference maxPowerTrigger;
    public GameObject[] LightsaberPrefabs;
    public float knockBackForce;
    public float knockBackRadius;
    private PlayerController player; 
    private Transform leftHand;
    private Transform rightHand;
    // Start is called before the first frame update
    void Awake()
    {
        leftHand = GameObject.Find("Left Controller").transform;
        rightHand = GameObject.Find("Right Controller").transform;
        maxPowerTrigger.action.performed += OnMaxPowerPressed;
        maxPowerTrigger.action.canceled += OnMaxPowerReleased;
        player = GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        maxPowerTrigger.action.Enable();
    }

    private void OnDisable()
    {
        maxPowerTrigger.action.performed -= OnMaxPowerPressed;
        maxPowerTrigger.action.canceled -= OnMaxPowerReleased;
        maxPowerTrigger.action.Disable();
    }

    // Update is called once per frame
    private void OnMaxPowerPressed(InputAction.CallbackContext context)
    {
        if (player.playerForce > 8) {
            player.AlterForce(-8);
            // pick two lightsabers at random from list of lightsaberPrefabs
            // Instantiate both and set one position to rightHand.transform.position and the other to leftHand.transform.position

            // Find all enemy tag in radius
            // for each enemy
                // Calc vector from player to it
                // Apply force vector * knockBackForce
                // Apply 5 damage // need to write script that goes on all enemies to handle health droids set to 1 health boss set to 10

        }
        Debug.Log("Max power pressed");

    }
    private void OnMaxPowerReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Max power released");
    }
}
