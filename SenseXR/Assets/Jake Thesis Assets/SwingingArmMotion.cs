using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SwingingArmMotion : MonoBehaviour
{
    #region Fields

    // Game Objects
    [Header("Game Objects")]
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject XRRig;
    [SerializeField] private Camera PlayerCamera;
    public GameObject objectToUnhide;

    // Vector3 Positions
    private Vector3 PositionPreviousFrameLeftHand;
    private Vector3 PositionPreviousFrameRightHand;
    private Vector3 PlayerPositionPreviousFrame;
    private Vector3 PlayerPositionCurrentFrame;
    private Vector3 PositionCurrentFrameLeftHand;
    private Vector3 PositionCurrentFrameRightHand;

    // Speed
    [Header("Speed Control")]
    [SerializeField] private float Speed = 10;
    private float HandSpeed;

    // Inputs
    [Header("Input References")]
    [SerializeField] private XRIDefaultInputActions1 inputActions;

    // Bools
    [SerializeField] private bool swingMovement;


    // Multipliers
    [SerializeField]private float contributionMultiplier = 2.0f; // Hard-coded multiplier to amplify hand contributions

    #endregion

    #region Unity Methods

    void OnEnable()
    {
        Debug.Log("OnEnable called.");
        inputActions = new XRIDefaultInputActions1();
        inputActions.XRILeftHandLocomotion.DoubleTriggerMovement.performed += OnDoubleTriggerMovementPressed;
        inputActions.XRILeftHandLocomotion.DoubleTriggerMovement.canceled += OnDoubleTriggerMovementReleased;

        //inputActions.XRIRightHandInteraction.Select.performed += OnRightTriggerPressed;
        //inputActions.XRIRightHandInteraction.Select.canceled += OnRightTriggerReleased;

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.XRILeftHandLocomotion.DoubleTriggerMovement.performed += OnDoubleTriggerMovementPressed;
        inputActions.XRILeftHandLocomotion.DoubleTriggerMovement.canceled += OnDoubleTriggerMovementReleased;
        //inputActions.XRIRightHandInteraction.Select.performed -= OnRightTriggerPressed;
        //inputActions.XRIRightHandInteraction.Select.canceled -= OnRightTriggerReleased;

        inputActions.Disable();
    }

    void Start()
    {
        //Set Game Object to Unhide as InActive
        objectToUnhide.SetActive(false);

        if (LeftHand == null || RightHand == null || XRRig == null || PlayerCamera == null)
        {
            //Debug.LogError("One or more required GameObjects are not assigned in the Inspector.");
            enabled = false; // Disable the script to prevent further errors
            return;
        }

        PlayerPositionPreviousFrame = transform.position; // Set current positions
        PositionPreviousFrameLeftHand = LeftHand.transform.position; // Set previous positions
        PositionPreviousFrameRightHand = RightHand.transform.position;

        swingMovement = false;
    }

    void Update()
    {
        //Debug.Log("Update called.");
        //Debug.Log($"Player position: {transform.position}");
        //Debug.Log($"Left hand position: {LeftHand.transform.position}");
        //Debug.Log($"Right hand position: {RightHand.transform.position}");

        Vector3 forwardDirection = PlayerCamera.transform.forward;

        PositionCurrentFrameLeftHand = LeftHand.transform.position;
        PositionCurrentFrameRightHand = RightHand.transform.position;

        PlayerPositionCurrentFrame = transform.position;

        var playerDistanceMoved = Vector3.Distance(PlayerPositionCurrentFrame, PlayerPositionPreviousFrame);
        var leftHandDistanceMoved = Vector3.Distance(PositionPreviousFrameLeftHand, PositionCurrentFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(PositionPreviousFrameRightHand, PositionCurrentFrameRightHand);

        //Debug.Log($"Player Distance Moved: {playerDistanceMoved}");
        //Debug.Log($"Left Hand Distance Moved: {leftHandDistanceMoved}");
        //Debug.Log($"Right Hand Distance Moved: {rightHandDistanceMoved}");

        float leftHandContribution = (leftHandDistanceMoved - playerDistanceMoved) * contributionMultiplier;
        float rightHandContribution = (rightHandDistanceMoved - playerDistanceMoved) * contributionMultiplier;

        //Debug.Log($"Left Hand Contribution: {leftHandContribution}");
        //Debug.Log($"Right Hand Contribution: {rightHandContribution}");

        HandSpeed = (leftHandContribution + rightHandContribution);

        HandSpeed = Mathf.Clamp(HandSpeed, -1f, 1f);

        //Debug.Log($"Hand Speed: {HandSpeed}");

        if (swingMovement && Time.timeSinceLevelLoad > 1f)
        {
            //Debug.Log("Both triggers pressed and valid for movement");
            MovePlayer(forwardDirection);
        }

        PositionPreviousFrameLeftHand = PositionCurrentFrameLeftHand;
        PositionPreviousFrameRightHand = PositionCurrentFrameRightHand;

        PlayerPositionPreviousFrame = PlayerPositionCurrentFrame;
    }

    #endregion

    #region Custom Methods

    public void MovePlayer(Vector3 forwardDirection)
    {
        Vector3 movement = forwardDirection * HandSpeed * Speed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        //Debug.Log($"Forward Direction: {forwardDirection}");
        //Debug.Log($"Movement Vector: {movement}");
        //Debug.Log($"Calculated New Position: {newPosition}");

        if (IsValidVector3(newPosition))
        {
           // Debug.Log($"Moving player to new position: {newPosition}");
            transform.position = newPosition;
        }
        else
        {
            //Debug.LogError($"Invalid player position calculated: {newPosition}");
        }
    }

    /*public void OnLeftTriggerPressed(InputAction.CallbackContext context)
    {
        swingMovementLeft = true;
        Debug.Log("Left trigger pressed");
    }*/

    public void OnDoubleTriggerMovementPressed(InputAction.CallbackContext context)
    {
        //Set Game Object to Unhide as Active
        objectToUnhide.SetActive(true);
        swingMovement = true;
        Debug.Log("Triggers pressed");

    }

    public void OnDoubleTriggerMovementReleased(InputAction.CallbackContext context)
    {
        //Set Game Object to Unhide as InActive
        objectToUnhide.SetActive(false);
        swingMovement = false;
        Debug.Log("Triggers released");
    }

    /*public void OnLeftTriggerReleased(InputAction.CallbackContext context)
    {
        swingMovementLeft = false;
        Debug.Log("Left trigger released");
    }*/

    /*public void OnRightTriggerReleased(InputAction.CallbackContext context)
    {
        swingMovementRight = false;
        Debug.Log("Right trigger released");
    }*/

    private bool IsValidVector3(Vector3 vector)
    {
        return !float.IsInfinity(vector.x) && !float.IsInfinity(vector.y) && !float.IsInfinity(vector.z) &&
               !float.IsNaN(vector.x) && !float.IsNaN(vector.y) && !float.IsNaN(vector.z);
    }

    #endregion
}
