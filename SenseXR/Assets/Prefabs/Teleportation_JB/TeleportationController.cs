using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationController : MonoBehaviour
{
    static private bool _teleportIsActive = false;

    public enum ControllerType
    {
        RightHand,
        LeftHand
    }

    public ControllerType targetController;
    public InputActionAsset inputAction;
    public XRRayInteractor rayInteractor;
    public TeleportationProvider teleportationProvider;

    private InputAction _thumbstickInputAction;
    private InputAction _teleportActivate;
    private InputAction _teleportCancel;

    void Start()
    {
        rayInteractor.enabled = false;

        Debug.Log("XRI " + targetController.ToString());
        _teleportActivate = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Teleport Mode Activate");
        _teleportActivate.Enable();
        _teleportActivate.performed += OnTeleportActivate;

        _teleportCancel = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Teleport Mode Cancel");
        _teleportCancel.Enable();
        _teleportCancel.performed += OnTeleportCancel;

        _thumbstickInputAction = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Move");
        _thumbstickInputAction.Enable();
    }


    void Update()
    {
        if (!_teleportIsActive)
        {
            return;
        }
        if (!rayInteractor)
        {
            return;
        }

        Vector2 thumbstickValue = _thumbstickInputAction.ReadValue<Vector2>();
        if (thumbstickValue.y >= 0.75f) // Adjust threshold as needed
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
            {
                Debug.Log("Teleport hit: " + raycastHit.point);
                TeleportRequest teleportRequest = new TeleportRequest()
                {
                    destinationPosition = raycastHit.point,
                };

                teleportationProvider.QueueTeleportRequest(teleportRequest);

                rayInteractor.enabled = false;
                _teleportIsActive = false;
            }
            else
            {
                Debug.Log("Raycast hit failed");
                rayInteractor.enabled = false;
                _teleportIsActive = false;
            }
        }
    }


    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        Debug.Log("Teleport Activate triggered");
        if (!_teleportIsActive)
        {
            rayInteractor.enabled = true;
            _teleportIsActive = true;
        }
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        Debug.Log("Teleport Cancel triggered");
        if (_teleportIsActive && rayInteractor.enabled == true)
        {
            rayInteractor.enabled = false;
            _teleportIsActive = false;
        }
    }


}
