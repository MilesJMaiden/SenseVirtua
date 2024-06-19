using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class OutlineOnHover : MonoBehaviour
{
    private Outline outline;
    private XRBaseInteractor hoverInteractor;

    void Start()
    {
        outline = GetComponent<Outline>();
        var interactable = GetComponent<XRGrabInteractable>();

        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    void OnDestroy()
    {
        var interactable = GetComponent<XRGrabInteractable>();

        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        hoverInteractor = args.interactorObject;
        outline.enabled = true;
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (args.interactorObject == hoverInteractor)
        {
            outline.enabled = false;
            hoverInteractor = null;
        }
    }
}
