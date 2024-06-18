using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HideHandModelOnGrab : MonoBehaviour
{
    public GameObject rightHandModel;
    public GameObject leftHandModel;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        // We will call the method in the parentheses when the object is grabbed
        grabInteractable.selectEntered.AddListener(HideGrabHand);
        // We will call the method in the parentheses when the object is released
        grabInteractable.selectExited.AddListener(ShowGrabHand);
    }

    public void HideGrabHand(SelectEnterEventArgs args)
    {
        if(args.interactorObject.transform.tag == "Right Hand")
        {
            leftHandModel.SetActive(false);
        } 
        else if (args.interactorObject.transform.tag == "Left Hand")
        {
            rightHandModel.SetActive(false);
        }
    }

    public void ShowGrabHand(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.tag == "Right Hand")
        {
            leftHandModel.SetActive(true);
        }
        else if (args.interactorObject.transform.tag == "Left Hand")
        {
            rightHandModel.SetActive(true);
        }
    }

}
