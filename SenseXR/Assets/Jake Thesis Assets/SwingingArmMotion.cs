using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SwingingArmMotion : MonoBehaviour
{
    // Game Objects
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject ForwardDirection;

    //Vector3 Positions
    [SerializeField] private Vector3 PositionPreviousFrameLeftHand;
    [SerializeField] private Vector3 PositionPreviousFrameRightHand;
    [SerializeField] private Vector3 PlayerPositionPreviousFrame;
    [SerializeField] private Vector3 PlayerPositionCurrentFrame;
    [SerializeField] private Vector3 PositionCurrentFrameLeftHand;
    [SerializeField] private Vector3 PositionCurrentFrameRightHand;

    //Speed
    [SerializeField] private float Speed = 70;
    [SerializeField] private float HandSpeed;

    //Inputs
    public InputActionAsset inputAction;
    [SerializeField] private InputAction buttonPressRight;
    [SerializeField] private InputAction buttonPressLeft;
    private bool swingMovement;


    public XRController targetController;



    void Start()
    {
        PlayerPositionPreviousFrame = transform.position; //set current positions
        PositionPreviousFrameLeftHand = LeftHand.transform.position; //set previous positions
        PositionPreviousFrameRightHand = RightHand.transform.position;

        swingMovement = false;
        buttonPressRight = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Activate");
        buttonPressLeft = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Activate");
    }

    // Update is called once per frame
    void Update()
    {
        // get forward direction from the center eye camera and set it to the forward direction object
        float yRotation = MainCamera.transform.eulerAngles.y;
        ForwardDirection.transform.eulerAngles = new Vector3(0, yRotation, 0);

        // get positons of hands
        PositionCurrentFrameLeftHand = LeftHand.transform.position;
        PositionCurrentFrameRightHand = RightHand.transform.position;

        // position of player
        PlayerPositionCurrentFrame = transform.position;

        // get distance the hands and player has moved from last frame
        var playerDistanceMoved = Vector3.Distance(PlayerPositionCurrentFrame, PlayerPositionPreviousFrame);
        var leftHandDistanceMoved = Vector3.Distance(PositionPreviousFrameLeftHand, PositionCurrentFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(PositionPreviousFrameRightHand, PositionCurrentFrameRightHand);

        // aggregate to get hand speed
        HandSpeed = ((leftHandDistanceMoved - playerDistanceMoved) + (rightHandDistanceMoved - playerDistanceMoved));

        // get the button press event
        if (buttonPressLeft.triggered && buttonPressRight.triggered)
        {
            swingMovement = true;
        }
        else
        {
            swingMovement = false;
        }

        if (swingMovement && Time.timeSinceLevelLoad > 1f)
        {
            transform.position += ForwardDirection.transform.forward * HandSpeed * Speed * Time.deltaTime;
        }

        // set previous position of hands for next frame
        PositionPreviousFrameLeftHand = PositionCurrentFrameLeftHand;
        PositionPreviousFrameRightHand = PositionCurrentFrameRightHand;

        // set player position previous frame
        PlayerPositionPreviousFrame = PlayerPositionCurrentFrame;
    }
}






/*using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SwingArmMotion : MonoBehaviour
{
    public Transform leftController; // Reference to the left controller
    public Transform rightController; // Reference to the right controller
    public GameObject player; // Reference to the player GameObject
    public GameObject target; // Target GameObject to move towards
    public XRController xrControllerLeft;

    private float speed = 3f; // Maximum speed
    private float velocityThreshold = 0.1f; // Velocity threshold

    void Update()
    {
        Vector3 leftControllerVelocity = GetControllerVelocity(InputDeviceCharacteristics.Left);
        Vector3 rightControllerVelocity = GetControllerVelocity(InputDeviceCharacteristics.Right);

        Vector3 combinedVelocity = leftControllerVelocity + rightControllerVelocity;

        Debug.Log("Left Controller Velocity: " + leftControllerVelocity);
        Debug.Log("Right Controller Velocity: " + rightControllerVelocity);
        Debug.Log("Combined Velocity: " + combinedVelocity.magnitude);

        if (combinedVelocity.magnitude > velocityThreshold)
        {
            Debug.Log("Moving Player");
            MovePlayerTowardsTarget(combinedVelocity);
        }
        else
        {
            Debug.Log("Velocity below threshold");
        }
    }

    private Vector3 GetControllerVelocity(InputDeviceCharacteristics characteristics)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);

        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
            {
                return velocity;
            }
        }

        return Vector3.zero;
    }

    private void MovePlayerTowardsTarget(Vector3 velocity)
    {
        Vector3 direction = (target.transform.position - player.transform.position).normalized;
        Vector3 moveVelocity = direction * Mathf.Clamp(velocity.magnitude, 0, speed) * Time.deltaTime;
        player.transform.Translate(moveVelocity, Space.World);
    }
}*/

