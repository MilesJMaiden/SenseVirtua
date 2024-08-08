using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SwingingArmMotion : MonoBehaviour
{
    // Game Objects
    [Header("Game Objects")]
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject XRRig;
    [SerializeField] private GameObject ForwardDirection;

    // Vector3 Positions
    private Vector3 PositionPreviousFrameLeftHand;
    private Vector3 PositionPreviousFrameRightHand;
    private Vector3 PlayerPositionPreviousFrame;
    private Vector3 PlayerPositionCurrentFrame;
    private Vector3 PositionCurrentFrameLeftHand;
    private Vector3 PositionCurrentFrameRightHand;

    // Speed
    [Header("Speed Control")]
    [SerializeField] private float Speed = 5;
    private float HandSpeed;

    // Inputs
    [Header("Input References")]
    public InputActionAsset actionAssets;
    public InputActionProperty selectRight;
    public InputActionProperty selectLeft;

    // Bools
    [SerializeField] private bool swingMovementRight;
    [SerializeField] private bool swingMovementLeft;

    void OnEnable()
    {
        selectLeft.action.performed += OnLeftSelectPressed;
        selectRight.action.performed += OnRightSelectPressed;

        selectLeft.action.canceled += OnLeftSelectReleased;
        selectRight.action.canceled += OnRightSelectReleased;
    }

    void OnDisable()
    {
        selectLeft.action.performed -= OnLeftSelectPressed;
        selectRight.action.performed -= OnRightSelectPressed;

        selectLeft.action.canceled -= OnLeftSelectReleased;
        selectRight.action.canceled -= OnRightSelectReleased;
    }

    void Start()
    {
        // Ensure all references are assigned
        if (LeftHand == null || RightHand == null || XRRig == null || ForwardDirection == null)
        {
            Debug.LogError("One or more required GameObjects are not assigned in the Inspector.");
            enabled = false; // Disable the script to prevent further errors
            return;
        }

        PlayerPositionPreviousFrame = transform.position; // Set current positions
        PositionPreviousFrameLeftHand = LeftHand.transform.position; // Set previous positions
        PositionPreviousFrameRightHand = RightHand.transform.position;

        swingMovementRight = false;
        swingMovementLeft = false;
    }

    void Update()
    {
        // Get forward direction from the center eye camera and set it to the forward direction object
        float yRotation = XRRig.transform.eulerAngles.y;
        ForwardDirection.transform.eulerAngles = new Vector3(0, yRotation, 0);

        // Get positions of hands
        PositionCurrentFrameLeftHand = LeftHand.transform.position;
        PositionCurrentFrameRightHand = RightHand.transform.position;

        // Position of player
        PlayerPositionCurrentFrame = transform.position;

        // Get distance the hands and player has moved from last frame
        var playerDistanceMoved = Vector3.Distance(PlayerPositionCurrentFrame, PlayerPositionPreviousFrame);
        var leftHandDistanceMoved = Vector3.Distance(PositionPreviousFrameLeftHand, PositionCurrentFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(PositionPreviousFrameRightHand, PositionCurrentFrameRightHand);

        // Log distances moved
        Debug.Log($"Player Distance Moved: {playerDistanceMoved}");
        Debug.Log($"Left Hand Distance Moved: {leftHandDistanceMoved}");
        Debug.Log($"Right Hand Distance Moved: {rightHandDistanceMoved}");

        // Aggregate to get hand speed
        float leftHandContribution = leftHandDistanceMoved - playerDistanceMoved;
        float rightHandContribution = rightHandDistanceMoved - playerDistanceMoved;

        Debug.Log($"Left Hand Contribution: {leftHandContribution}");
        Debug.Log($"Right Hand Contribution: {rightHandContribution}");

        HandSpeed = (leftHandContribution + rightHandContribution);

        // Cap HandSpeed to prevent excessively large values
        HandSpeed = Mathf.Clamp(HandSpeed, -1f, 1f);

        // Log hand speed
        Debug.Log($"Hand Speed: {HandSpeed}");

        // Get the button press event
        if (swingMovementRight && swingMovementLeft)
        {
            Debug.Log("Both triggers pressed and valid for movement");
            MovePlayer();
        }

        // Set previous position of hands for next frame
        PositionPreviousFrameLeftHand = PositionCurrentFrameLeftHand;
        PositionPreviousFrameRightHand = PositionCurrentFrameRightHand;

        // Set player position previous frame
        PlayerPositionPreviousFrame = PlayerPositionCurrentFrame;
    }

    public void MovePlayer()
    {
        Vector3 movement = ForwardDirection.transform.forward * HandSpeed * Speed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        Debug.Log($"Forward Direction: {ForwardDirection.transform.forward}");
        Debug.Log($"Movement Vector: {movement}");
        Debug.Log($"Calculated New Position: {newPosition}");

        if (IsValidVector3(newPosition))
        {
            Debug.Log($"Moving player to new position: {newPosition}");
            transform.position = newPosition;
        }
        else
        {
            Debug.LogError($"Invalid player position calculated: {newPosition}");
        }
    }

    public void OnLeftSelectPressed(InputAction.CallbackContext context)
    {
        swingMovementLeft = true;
        Debug.Log("Left select pressed");
    }

    public void OnRightSelectPressed(InputAction.CallbackContext context)
    {
        swingMovementRight = true;
        Debug.Log("Right select pressed");
    }

    public void OnLeftSelectReleased(InputAction.CallbackContext context)
    {
        swingMovementLeft = false;
        Debug.Log("Left select released");
    }

    public void OnRightSelectReleased(InputAction.CallbackContext context)
    {
        swingMovementRight = false;
        Debug.Log("Right select released");
    }

    private bool IsValidVector3(Vector3 vector)
    {
        return !float.IsInfinity(vector.x) && !float.IsInfinity(vector.y) && !float.IsInfinity(vector.z) &&
               !float.IsNaN(vector.x) && !float.IsNaN(vector.y) && !float.IsNaN(vector.z);
    }
}
