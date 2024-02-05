using System.Collections;
using UnityEngine;

public class VultureBehaviorScenario_1 : MonoBehaviour {

    enum StateMovement
    {
        Idle,
        Move,
        Fly
    }

    const float TIME_FOR_SMOOTH_TURN = 15f;
    const float DISTANCE_OF_CHECKING_TO_GROUND = 1f;

    [SerializeField] private float speedWalk;               // Walking speed
    [SerializeField] private float speedFly;                // Flight speed
    [SerializeField] private LayerMask layerMaskGround;     // Layers which is considered under the object
    [SerializeField] private AnimationCurve flyCurve;       // Curve for smooth rotation of the object

    private StateMovement stateMovementVulture;             // Object state {Idle, Move, Fly}
    private float speedRotationFly;                         // Turning speed
    private float diractionFlyX = 0;                        // X rotation in flight

    private Animator animator;                              // reference Animator
    private Rigidbody rigidbodyComponent;                   // reference Rigidbody

    private void Awake()
    {
        animator = GetComponent<Animator>();                // assignment Animator
        rigidbodyComponent = GetComponent<Rigidbody>();     // assignment Rigidbody
    }

    private void Start()
    {
        StartCoroutine(BehaviorScenario());                 // Launch behavior scenario
    }

    private void Update()
    {
        RotateObject();                  // Rotate object

        // Movement of an object if its state is not Idle
        if (stateMovementVulture != StateMovement.Idle)
            Move();
    }

    // Rotate the object relative to the ground below
    private void RotateObject()
    {
        Quaternion rotation = transform.rotation;
        RaycastHit hit;

        if (stateMovementVulture == StateMovement.Fly)
        {
            Quaternion targetRotation = Quaternion.Euler(diractionFlyX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            rotation = Quaternion.Slerp( transform.rotation, targetRotation, speedRotationFly * Time.deltaTime);
        }
        else if (Physics.Raycast(transform.position, -Vector3.up, out hit, DISTANCE_OF_CHECKING_TO_GROUND, layerMaskGround))
        {
            Quaternion rotationFromObjectBelow = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            rotation = Quaternion.Slerp(transform.rotation, rotationFromObjectBelow, Time.deltaTime * TIME_FOR_SMOOTH_TURN);
        }

        rigidbodyComponent.MoveRotation(rotation);
    }

    // Object movement
    private void Move()
    {
        Vector3 movement = rigidbodyComponent.position;

        if (stateMovementVulture == StateMovement.Move)
        {
            movement += transform.forward * speedWalk * Time.deltaTime;
        }
        else
        {
            movement += transform.forward * speedFly * Time.deltaTime;
        }

        rigidbodyComponent.MovePosition(movement);
    }

    IEnumerator ChageRotationLerp()
    {
        float timeElapsed = 0;
        while (timeElapsed < 1)
        {
            yield return null;
            timeElapsed += Time.deltaTime;
            speedRotationFly = flyCurve.Evaluate(timeElapsed);
        }
    }

    // Behavior scenario Object
    IEnumerator BehaviorScenario()
    {
        // Start Moving
        yield return new WaitForSeconds(2f);
        stateMovementVulture = StateMovement.Move;
        animator.SetBool("IsMove", true);

        // End Moving
        yield return new WaitForSeconds(10f);
        stateMovementVulture = StateMovement.Idle;
        animator.SetBool("IsMove", false);

        // Play an animation Eat
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsEat", true);
        animator.SetTrigger("Other");

        // Play an animation Head Rotate
        yield return new WaitForSeconds(2f);
        animator.SetBool("IsEat", false);
        animator.SetTrigger("Other");
        
        // Start fly
        yield return new WaitForSeconds(2f);
        stateMovementVulture = StateMovement.Fly;
        animator.SetBool("IsFly", true);
        rigidbodyComponent.useGravity = false;
        diractionFlyX = -25f;
        StartCoroutine(ChageRotationLerp());
        StartCoroutine(FlapWings());

        // Change flight directions
        yield return new WaitForSeconds(10f);
        diractionFlyX = 0f;
        StartCoroutine(ChageRotationLerp());
    }

    IEnumerator FlapWings()
    {
        // Flap wings
        while (true)
        {
            yield return new WaitForSeconds(1f);
            animator.SetTrigger("Wings Flap");
            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("Wings Flap");

            if (stateMovementVulture != StateMovement.Fly)
                break;
        }
    }
}
