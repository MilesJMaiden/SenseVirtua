using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ArmSwingMovementWithRigidbody : MonoBehaviour
{
    #region Fields

    [Header("Movement Settings")]
    [Tooltip("Multiplier for the movement speed based on arm swinging.")]
    public float speed = 7.5f; // Speed multiplier for movement

    [Header("Controller Settings")]
    [Tooltip("XR Node representing the left hand.")]
    public XRNode leftHandNode = XRNode.LeftHand;

    [Tooltip("XR Node representing the right hand.")]
    public XRNode rightHandNode = XRNode.RightHand;

    // Previous positions of the controllers
    private Vector3 lastLeftPosition;
    private Vector3 lastRightPosition;

    // Rigidbody component reference
    private Rigidbody rb;

    [Header("Input Actions")]
    [Tooltip("Input action for detecting when the left trigger is pressed.")]
    private InputAction leftTriggerAction;

    [Tooltip("Input action for detecting when the right trigger is pressed.")]
    private InputAction rightTriggerAction;

    [Header("Interaction Settings")]
    [Tooltip("Flag to disable movement when the player is interacting with an object.")]
    private bool isInteracting = false;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Sets up the input actions for the left and right triggers.
    /// </summary>
    void Awake()
    {
        // Set up the Input Actions
        leftTriggerAction = new InputAction(type: InputActionType.Value, binding: "<XRController>{LeftHand}/trigger");
        rightTriggerAction = new InputAction(type: InputActionType.Value, binding: "<XRController>{RightHand}/trigger");
        leftTriggerAction.Enable();
        rightTriggerAction.Enable();
    }

    /// <summary>
    /// Initializes the Rigidbody component and the previous controller positions.
    /// </summary>
    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Initialize the previous positions
        lastLeftPosition = InputTracking.GetLocalPosition(leftHandNode);
        lastRightPosition = InputTracking.GetLocalPosition(rightHandNode);
    }

    /// <summary>
    /// Calculates and applies force based on the velocities of the left and right hands.
    /// Stops the movement when the player is interacting with an object.
    /// </summary>
    void FixedUpdate()
    {
        // Activate or deactivate Rigidbody based on interaction status
        rb.isKinematic = isInteracting;

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

        // Apply force based on left hand movement if left trigger is pressed
        if (leftTriggerAction.ReadValue<float>() > 0.1f)
        {
            float leftMagnitude = Vector3.Dot(leftMovement, forwardDirection);
            force += forwardDirection * leftMagnitude * speed;
        }

        // Apply force based on right hand movement if right trigger is pressed
        if (rightTriggerAction.ReadValue<float>() > 0.1f)
        {
            float rightMagnitude = Vector3.Dot(rightMovement, forwardDirection);
            force += forwardDirection * rightMagnitude * speed;
        }

        // Prevent backward movement when interacting with objects close to the body
        if (force.magnitude > 0 && Vector3.Dot(force, forwardDirection) < 0)
        {
            force = Vector3.zero;
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

    /// <summary>
    /// Disables the input actions when the object is disabled.
    /// </summary>
    void OnDisable()
    {
        // Disable the input actions when the object is disabled
        leftTriggerAction.Disable();
        rightTriggerAction.Disable();
    }

    #endregion

    #region Custom Methods

    /// <summary>
    /// Disables movement and Rigidbody physics while the player is interacting with an object.
    /// </summary>
    public void StartInteraction()
    {
        Debug.Log("Start Interaction: Disabling movement and Rigidbody");
        isInteracting = true;
        rb.isKinematic = true; // Disable physics interactions
    }

    /// <summary>
    /// Enables movement and Rigidbody physics after the player has stopped interacting with an object.
    /// </summary>
    public void StopInteraction()
    {
        Debug.Log("Stop Interaction: Enabling movement and Rigidbody");
        isInteracting = false;
        rb.isKinematic = false; // Enable physics interactions
    }

    /// <summary>
    /// Sets the rigidbody drag and angular drag variables to the default values to stop unintentional changes
    /// on the collision with an object.
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionExit(Collision collision)
    {
        // Reset drag after collision
        GetComponent<Rigidbody>().drag = 1f;
        GetComponent<Rigidbody>().angularDrag = 5f; 
    }

    #endregion
}
