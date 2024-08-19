using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ArmSwingLocomotion : MonoBehaviour
{
    public Transform controller1;
    public Transform controller2;
    public Transform objectToMove;
    public float moveSpeed = 1.0f;
    public XRController inputController; // Reference to the XR controller for input
    public InputHelpers.Button activationButton = InputHelpers.Button.Trigger; // Button to activate movement
    public float activationThreshold = 0.1f; // Threshold for button press

    private float previousDistance = 0f;

    void Update()
    {
        if (controller1 != null && controller2 != null && objectToMove != null)
        {
            // Check if the activation button is pressed
            if (IsButtonPressed(inputController, activationButton))
            {
                float currentDistance = Vector3.Distance(controller1.position, controller2.position);

                if (currentDistance > previousDistance)
                {
                    // Move the object forward
                    objectToMove.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                }

                previousDistance = currentDistance;

                Debug.Log("Distance between controllers: " + currentDistance);
            }
        }
        else
        {
            Debug.LogWarning("Controllers or object to move not assigned.");
        }
    }

    private bool IsButtonPressed(XRController controller, InputHelpers.Button button)
    {
        if (controller.inputDevice.IsPressed(button, out bool isPressed, activationThreshold))
        {
            return isPressed;
        }
        return false;
    }
}
