using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class XR_TeleportationSystem : MonoBehaviour
{
    public GameObject[] teleportPoints; // Assign teleport points in the inspector
    public InputActionReference teleportAction; // Reference to the teleport action

    private XRRayInteractor rayInteractor;
    private TeleportPoint currentHoveredPoint = null;
    private bool isTeleportationActive = false;

    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        teleportAction.action.started += OnTeleportationButtonPressed;
        teleportAction.action.canceled += OnTeleportationButtonReleased;
    }

    void Update()
    {
        if (isTeleportationActive)
        {
            RaycastHit hit;
            if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
            {
                TeleportPoint hitPoint = hit.transform.GetComponent<TeleportPoint>();
                if (hitPoint != null)
                {
                    if (currentHoveredPoint != null && currentHoveredPoint != hitPoint)
                    {
                        currentHoveredPoint.SetNormalState();
                    }
                    currentHoveredPoint = hitPoint;
                    currentHoveredPoint.SetHoveredState();
                }
                else if (currentHoveredPoint != null)
                {
                    currentHoveredPoint.SetNormalState();
                    currentHoveredPoint = null;
                }
            }
        }
    }

    private void OnTeleportationButtonPressed(InputAction.CallbackContext context)
    {
        ShowTeleportPoints(true);
        isTeleportationActive = true;
    }

    private void OnTeleportationButtonReleased(InputAction.CallbackContext context)
    {
        if (currentHoveredPoint != null)
        {
            Transform playerTransform = Camera.main.transform;
            playerTransform.position = currentHoveredPoint.transform.position;
        }
        ShowTeleportPoints(false);
        isTeleportationActive = false;
    }

    private void ShowTeleportPoints(bool show)
    {
        foreach (var point in teleportPoints)
        {
            point.SetActive(show);
        }
    }

    private void OnDestroy()
    {
        teleportAction.action.started -= OnTeleportationButtonPressed;
        teleportAction.action.canceled -= OnTeleportationButtonReleased;
    }
}
