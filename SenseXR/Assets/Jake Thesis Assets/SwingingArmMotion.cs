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
    [SerializeField] private float contributionMultiplier = 2.0f; // Hard-coded multiplier to amplify hand contributions

    #endregion

    #region Unity Methods

    void Start()
    {
        // Set Game Object to Unhide as InActive
        objectToUnhide.SetActive(false);

        if (LeftHand == null || RightHand == null || XRRig == null || PlayerCamera == null)
        {
            Debug.LogError("One or more required GameObjects are not assigned in the Inspector.");
            enabled = false; // Disable the script to prevent further errors
            return;
        }

        PlayerPositionPreviousFrame = transform.position; // Set current positions
        PositionPreviousFrameLeftHand = LeftHand.transform.position; // Set previous positions
        PositionPreviousFrameRightHand = RightHand.transform.position;
    }

    void Update()
    {
        // Calculate the current hand positions and the player position
        PositionCurrentFrameLeftHand = LeftHand.transform.position;
        PositionCurrentFrameRightHand = RightHand.transform.position;
        PlayerPositionCurrentFrame = transform.position;

        // Calculate distances moved
        var playerDistanceMoved = Vector3.Distance(PlayerPositionCurrentFrame, PlayerPositionPreviousFrame);
        var leftHandDistanceMoved = Vector3.Distance(PositionPreviousFrameLeftHand, PositionCurrentFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(PositionPreviousFrameRightHand, PositionCurrentFrameRightHand);

        // Calculate hand contribution to movement
        float leftHandContribution = (leftHandDistanceMoved - playerDistanceMoved) * contributionMultiplier;
        float rightHandContribution = (rightHandDistanceMoved - playerDistanceMoved) * contributionMultiplier;

        // Calculate the speed based on hand contributions
        HandSpeed = (leftHandContribution + rightHandContribution);
        HandSpeed = Mathf.Clamp(HandSpeed, -0.5f, 5f);

        // Check if hands are moving enough to warrant movement
        if (Mathf.Abs(HandSpeed) > 0.01f) // 0.01f is a small threshold to avoid jittering
        {
            Debug.Log("Moving player based on hand motion at " + HandSpeed);
            Vector3 forwardDirection = PlayerCamera.transform.forward.normalized;
            MovePlayer(forwardDirection);
        }

        // Update previous positions for the next frame
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

        Debug.Log($"Forward Direction: {forwardDirection}");
        Debug.Log($"Movement Vector: {movement}");
        Debug.Log($"Calculated New Position: {newPosition}");

        if (IsValidVector3(newPosition))
        {
            Debug.Log($"Moving player to new position: {newPosition} at " + Speed);
            transform.position = newPosition;
        }
        else
        {
            Debug.LogError($"Invalid player position calculated: {newPosition}");
        }
    }

    private bool IsValidVector3(Vector3 vector)
    {
        return !float.IsInfinity(vector.x) && !float.IsInfinity(vector.y) && !float.IsInfinity(vector.z) &&
               !float.IsNaN(vector.x) && !float.IsNaN(vector.y) && !float.IsNaN(vector.z);
    }

    #endregion
}
