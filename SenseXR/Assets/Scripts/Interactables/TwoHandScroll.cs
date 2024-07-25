using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class TwoHandScroll : MonoBehaviour
{
    public float min_UnfoldingValue ;
    public float max_UnfoldingValue ;
    public float unfoldingValue;

    public GameObject cover;
    public GameObject slideScroll;
    
    [SerializeField]
    XRSlider xrSlider;

    [SerializeField]
    Material coverMat;

    protected void OnEnable()
    {
        if (cover != null)
        {
            coverMat = cover.GetComponent<MeshRenderer>().material;
            Debug.Log("Cover material assigned.");
        }
        else
        {
            Debug.LogError("Cover GameObject is not assigned.");
        }

        // Check if slideScroll is assigned and get XRSlider component
        if (slideScroll != null)
        {
            xrSlider = slideScroll.GetComponent<XRSlider>();
            if (xrSlider != null)
            {
                Debug.Log("XRSlider component assigned.");
            }
            else
            {
                Debug.LogError("XRSlider component not found on slideScroll.");
            }
        }
        else
        {
            Debug.LogError("SlideScroll GameObject is not assigned.");
        }

        Debug.Log("The scroll is awaken");
        
        ConnectControlEvents();
        InitialControls();
    }

    protected void OnDisable()
    {
        DisconnectControlEvent();
    }

    void ConnectControlEvents()
    {
        if (xrSlider != null)
        {
            xrSlider.onValueChange.AddListener(UpdateUnfoldingValue);
            Debug.Log("Listener added to XRSlider.");
        }
        else
        {
            Debug.LogError("XRSlider is null. Listener not added.");
        }
    }
    private void Update()
    {

            UpdateUnfoldingValue(xrSlider.value);

    }
    void DisconnectControlEvent()
    {
        if (xrSlider != null)
        {
            xrSlider.onValueChange.RemoveListener(UpdateUnfoldingValue);
            Debug.Log("Listener removed from XRSlider.");
        }
        else
        {
            Debug.LogError("XRSlider is null. Listener not removed.");
        }
    }

    void InitialControls()
    {
        unfoldingValue = Mathf.Clamp(unfoldingValue, min_UnfoldingValue, max_UnfoldingValue);
    }


    public void UpdateUnfoldingValue(float value)
    {
        Debug.Log("the XRSlider Value is" + xrSlider.value);
        Debug.Log("Updating unfolding value!!!");
        value = xrSlider.value;
        unfoldingValue =  value * (max_UnfoldingValue - min_UnfoldingValue) + min_UnfoldingValue;

        if (coverMat.HasProperty("_Unfolding_Value"))
        {
            coverMat.SetFloat("_Unfolding_Value", unfoldingValue);
        }
        else
        {
            Debug.LogError("The material does not have the property '_Unfolding_Value'.");
        }
        //coverMat.SetFloat("_Unfolding_Value", unfoldingValue);
        
    }

    


}
