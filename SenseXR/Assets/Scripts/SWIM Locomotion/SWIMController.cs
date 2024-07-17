using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWIMController : MonoBehaviour
{
    // References to the full-size and miniature environments
    public GameObject fullSizeEnvironment;
    public GameObject miniatureEnvironment;

    // References to the left and right VR controllers
    public Transform leftController;
    public Transform rightController;

    // Variables to control the scaling factor
    private float scaleFactor = 1.0f;
    private Vector3 initialScale;
    private bool isScalingActive = false;

    void Start()
    {
        // Store the initial scale of the miniature environment
        initialScale = miniatureEnvironment.transform.localScale;
        
        // Initially hide the miniature environment
        miniatureEnvironment.SetActive(false);
    }

    void Update()
    {
        // Check if both triggers are pressed simultaneously
        if (Input.GetAxis("BButton") > 0.1f && Input.GetAxis("YButton") > 0.1f)
        {
            // Activate the miniature environment if it's not already active
            if (!isScalingActive)
            {
                isScalingActive = true;
                miniatureEnvironment.SetActive(true);
            }

            // Update the scaling of the miniature environment based on controller distance
            UpdateScaling();

            // Handle focus point change
            if (Input.GetButtonDown("Fire1")) // Assuming Fire1 is the secondary button
            {
                UpdateFocusPoint();
            }
        }
        else
        {
            // Deactivate the miniature environment if the triggers are released
            if (isScalingActive)
            {
                isScalingActive = false;
                miniatureEnvironment.SetActive(false);
            }
        }

        // Check if the A button on the right controller is pressed for teleportation
        if (Input.GetButtonDown("AButton")) // Assuming AButton is mapped to the A button
        {
            HandleTeleportation();
        }
    }

    void UpdateScaling()
    {
        // Calculate the distance between the two controllers
        float distance = Vector3.Distance(leftController.position, rightController.position);

        // Update the scale factor based on the distance
        scaleFactor = Mathf.Pow(distance, 2);

        // Apply the new scale to the miniature environment
        miniatureEnvironment.transform.localScale = initialScale * scaleFactor;
    }

    void UpdateFocusPoint()
    {
        // Shoot a ray from the left controller to determine the new focus point
        Ray ray = new Ray(leftController.position, leftController.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Update the position of the miniature environment to the hit point
            miniatureEnvironment.transform.position = hit.point;
        }
    }

    void HandleTeleportation()
    {
        // Shoot a ray from the right controller to determine the teleportation point
        Ray ray = new Ray(rightController.position, rightController.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Teleport the player to the hit point
            TeleportPlayer(hit.point);
        }
    }

    void TeleportPlayer(Vector3 targetPosition)
    {
        // Move the player's position to the target position
        // Assuming the player's position is managed by the Transform component of this GameObject
        transform.position = targetPosition;
    }
}
