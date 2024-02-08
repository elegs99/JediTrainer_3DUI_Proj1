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
    if (player.playerForce > 8)
    {
        player.AlterForce(-8);

        // Picking two random lightsabers
        var lightsaber1 = Instantiate(LightsaberPrefabs[Random.Range(0, LightsaberPrefabs.Length)], rightHand.position, Quaternion.identity);
        var lightsaber2 = Instantiate(LightsaberPrefabs[Random.Range(0, LightsaberPrefabs.Length)], leftHand.position, Quaternion.identity);

        // Finding all enemies within knockBackRadius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, knockBackRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                direction.y = 0;
                if (hitCollider.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.AddForce(direction * knockBackForce, ForceMode.Impulse);
                }
                if (hitCollider.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
                {
                    enemy.AlterEnemyHealth(-5);
                }
            }
        }

        Debug.Log("Max power pressed");
    }
}
    private void OnMaxPowerReleased(InputAction.CallbackContext context)
    {
        // stop all effects here
        Debug.Log("Max power released");
    }
}
