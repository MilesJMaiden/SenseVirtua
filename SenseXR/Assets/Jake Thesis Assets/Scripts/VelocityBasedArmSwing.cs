using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VelocityBasedArmSwing : MonoBehaviour
{
    #region Fields

    [Header("Player Speed Settings")]
    [Tooltip("Multiplier for the movement speed based on hand swing velocity.")]
    public float speed = 0.2f; // Speed multiplier for movement

    [Tooltip("Maximum speed the player can reach while swinging.")]
    public float maxSpeed = 2.0f; // Max speed to clamp movement

    [Tooltip("Minimum hand movement threshold required to apply movement force.")]
    public float movementThreshold = 0.01f; // Minimum hand movement required to apply force

    private Vector3 combinedVelocity = Vector3.zero;

    [Header("Controller Input Settings")]
    [Tooltip("Reference to the left controller to track its movement.")]
    public XRBaseController leftController;

    [Tooltip("Reference to the right controller to track its movement.")]
    public XRBaseController rightController;

    private Rigidbody rb;

    // Vectors to store the previous positions of the controllers
    private Vector3 previousLeftPosition;
    private Vector3 previousRightPosition;

    [Header("Input Actions")]
    [Tooltip("Input action for detecting when the left trigger is pressed.")]
    private InputAction leftTriggerAction;

    [Tooltip("Input action for detecting when the right trigger is pressed.")]
    private InputAction rightTriggerAction;

    [Header("Interaction Settings")]
    [Tooltip("Flag to disable movement when the player is interacting with an object.")]
    private bool isInteracting = false;

    [Tooltip("Flag to enable or disable swing-based movement. When disabled, movement stops immediately.")]
    public bool swingMovement = true; // This should be controlled by your game logic

    #endregion

    #region Unity Methods

    /// <summary>
    /// Initializes the input actions for the left and right triggers.
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
    /// Retrieves the Rigidbody component and sets up initial states.
    /// Checks if the controllers are assigned.
    /// </summary>
    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Check if controllers are assigned
        if (leftController == null || rightController == null)
        {
            Debug.LogError("Controllers not assigned in the Inspector.");
            enabled = false; // Disable the script to prevent further errors
            return;
        }

        // Initialize previous positions
        previousLeftPosition = leftController.transform.position;
        previousRightPosition = rightController.transform.position;
    }

    /// <summary>
    /// Calculates and applies force based on the velocities of the left and right hands.
    /// Force is applied to the Rigidbody to move the player in the direction of the hand motion.
    /// Stops the movement when the hands stop moving or when swingMovement becomes false.
    /// </summary>
    void FixedUpdate()
    {
        // Stop movement if swingMovement is false
        if (!swingMovement)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // Check if the player is interacting with an object
        if (isInteracting)
        {
            Debug.Log("Skipping movement logic due to interaction");
            return; // Skip movement logic if interacting
        }

        // Ensure both triggers are pressed
        bool isLeftTriggerPressed = leftTriggerAction.ReadValue<float>() > 0.1f;
        bool isRightTriggerPressed = rightTriggerAction.ReadValue<float>() > 0.1f;

        if (!isLeftTriggerPressed || !isRightTriggerPressed)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // Calculate the current velocity based on position change
        Vector3 leftVelocity = (leftController.transform.position - previousLeftPosition) / Time.fixedDeltaTime;
        Vector3 rightVelocity = (rightController.transform.position - previousRightPosition) / Time.fixedDeltaTime;

        // Update previous positions
        previousLeftPosition = leftController.transform.position;
        previousRightPosition = rightController.transform.position;

        // Ensure both hands are moving above the threshold
        if (leftVelocity.magnitude < movementThreshold || rightVelocity.magnitude < movementThreshold)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // Calculate the forward direction based on the camera's forward direction
        Vector3 forwardDirection = Camera.main.transform.forward;
        forwardDirection.y = 0; // Keep movement horizontal
        forwardDirection.Normalize();

        // Calculate the combined movement velocity from both hands
        combinedVelocity = (leftVelocity + rightVelocity) * 0.5f; // Average velocity of both hands

        // Calculate the player's movement direction based on the forward direction and combined hand velocity
        float movementSpeed = Vector3.Dot(combinedVelocity, forwardDirection) * speed;

        // Clamp the movement speed to prevent excessively high speeds
        movementSpeed = Mathf.Clamp(movementSpeed, -maxSpeed, maxSpeed);

        // Apply the movement to the player's Rigidbody
        rb.velocity = forwardDirection * movementSpeed;

        Debug.Log($"Player velocity: {rb.velocity}");
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
    /// Disables movement while the player is interacting with an object.
    /// </summary>
    public void StartInteraction()
    {
        Debug.Log("Start Interaction: Disabling movement");
        isInteracting = true;
    }

    /// <summary>
    /// Enables movement after the player has stopped interacting with an object.
    /// </summary>
    public void StopInteraction()
    {
        Debug.Log("Stop Interaction: Enabling movement");
        isInteracting = false;
    }

    #endregion
}
