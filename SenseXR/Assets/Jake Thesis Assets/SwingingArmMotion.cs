using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingArmMotion : MonoBehaviour
{
    public Transform leftController;
    public Transform rightController;
    public Transform targetObject; // The game object in front of the player
    public float speedThreshold = 0.1f; // Speed threshold to start moving the character
    public float maxMoveSpeed = 5.0f; // Maximum speed at which the character moves

    private Vector3 leftControllerPreviousPosition;
    private Vector3 rightControllerPreviousPosition;

    void Start()
    {
        leftControllerPreviousPosition = leftController.position;
        rightControllerPreviousPosition = rightController.position;
    }

    void Update()
    {
        // Calculate the velocity of the controllers
        Vector3 leftControllerVelocity = (leftController.position - leftControllerPreviousPosition) / Time.deltaTime;
        Vector3 rightControllerVelocity = (rightController.position - rightControllerPreviousPosition) / Time.deltaTime;

        // Calculate the speed of the controllers
        float leftControllerSpeed = leftControllerVelocity.magnitude;
        float rightControllerSpeed = rightControllerVelocity.magnitude;

        // Update previous positions
        leftControllerPreviousPosition = leftController.position;
        rightControllerPreviousPosition = rightController.position;

        // Debugging velocities and speeds
        Debug.Log($"Left Controller Velocity: {leftControllerVelocity}, Speed: {leftControllerSpeed}");
        Debug.Log($"Right Controller Velocity: {rightControllerVelocity}, Speed: {rightControllerSpeed}");

        // Check if the speed of either controller exceeds the threshold
        if (leftControllerSpeed > speedThreshold || rightControllerSpeed > speedThreshold)
        {
            // Calculate the move direction towards the target object
            Vector3 moveDirection = (targetObject.position - transform.position).normalized;

            // Calculate the average speed of the controllers
            float averageSpeed = (leftControllerSpeed + rightControllerSpeed) / 2.0f;

            // Clamp the movement speed to the maxMoveSpeed
            float movementSpeed = Mathf.Clamp(averageSpeed, 0, maxMoveSpeed);

            // Move the character towards the target object
            transform.position += moveDirection * movementSpeed * Time.deltaTime;

            // Debug the move direction and character's new position
            Debug.Log($"Move Direction: {moveDirection}");
            Debug.Log($"Character Position: {transform.position}");
        }
    }
}
