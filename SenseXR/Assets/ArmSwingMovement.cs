using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ArmSwingMovementWithRigidbody : MonoBehaviour
{
    public float speed = 20.0f; // Speed multiplier for movement
    public XRNode leftHandNode = XRNode.LeftHand;
    public XRNode rightHandNode = XRNode.RightHand;

    private Vector3 lastLeftPosition;
    private Vector3 lastRightPosition;
    private Rigidbody rb;

    // Input Actions for triggers
    private InputAction leftTriggerAction;
    private InputAction rightTriggerAction;

    // Interaction flag
    private bool isInteracting = false;

    void Awake()
    {
        // Set up the Input Actions
        leftTriggerAction = new InputAction(type: InputActionType.Value, binding: "<XRController>{LeftHand}/trigger");
        rightTriggerAction = new InputAction(type: InputActionType.Value, binding: "<XRController>{RightHand}/trigger");
        leftTriggerAction.Enable();
        rightTriggerAction.Enable();
    }

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Initialize the previous positions
        lastLeftPosition = InputTracking.GetLocalPosition(leftHandNode);
        lastRightPosition = InputTracking.GetLocalPosition(rightHandNode);
    }

    void FixedUpdate()
    {
        // Check if the player is interacting with an object
        if (isInteracting)
        {
            Debug.Log("Skipping movement logic due to interaction");
            return; // Skip movement logic if interacting
        }

        // Get the current position of both controllers
        Vector3 currentLeftPosition = InputTracking.GetLocalPosition(leftHandNode);
        Vector3 currentRightPosition = InputTracking.GetLocalPosition(rightHandNode);

        // Calculate the movement vector based on the swing motion
        Vector3 leftMovement = currentLeftPosition - lastLeftPosition;
        Vector3 rightMovement = currentRightPosition - lastRightPosition;

        // Calculate the forward direction based on the camera's forward direction
        Vector3 forwardDirection = Camera.main.transform.forward;
        forwardDirection.y = 0; // Keep movement horizontal
        forwardDirection.Normalize();

        // Calculate forces for each hand movement
        Vector3 force = Vector3.zero;

        // Apply force based on left hand movement
        if (leftTriggerAction.ReadValue<float>() > 0.1f)
        {
            float leftMagnitude = Vector3.Dot(leftMovement, forwardDirection);
            force += forwardDirection * leftMagnitude * speed;
        }

        // Apply force based on right hand movement
        if (rightTriggerAction.ReadValue<float>() > 0.1f)
        {
            float rightMagnitude = Vector3.Dot(rightMovement, forwardDirection);
            force += forwardDirection * rightMagnitude * speed;
        }

        // Reverse the force if the movement is backward
        if (Vector3.Dot(force, forwardDirection) < 0)
        {
            force = -force;
        }

        // Apply the force to the Rigidbody
        if (force != Vector3.zero)
        {
            rb.AddForce(force, ForceMode.VelocityChange);
            Debug.Log($"Applying force: {force}");
        }

        // Update the last positions
        lastLeftPosition = currentLeftPosition;
        lastRightPosition = currentRightPosition;
    }

    void OnDisable()
    {
        // Disable the input actions when the object is disabled
        leftTriggerAction.Disable();
        rightTriggerAction.Disable();
    }

    // Methods to handle interaction state
    public void StartInteraction()
    {
        Debug.Log("Start Interaction: Disabling movement");
        isInteracting = true;
    }

    public void StopInteraction()
    {
        Debug.Log("Stop Interaction: Enabling movement");
        isInteracting = false;
    }
}
