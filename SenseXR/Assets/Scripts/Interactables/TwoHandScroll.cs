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
    private XRSlider xrSlider;


    private Material coverMat;

    private void OnEnable()
    {
        Material coverMat = cover.GetComponent<Material>();
        xrSlider = slideScroll.GetComponent<XRSlider>();
        Debug.Log("the scroll is awaken");
        ConnectControlEvents();
        InitialControls();
    }

    private void OnDisable()
    {
        DisconnectControlEvent();
    }

    void ConnectControlEvents()
    {
        xrSlider.onValueChange.AddListener(UpdateUnfoldingValue);
        Debug.Log("Slider is changing Value");
    }

    void DisconnectControlEvent()
    {
        xrSlider.onValueChange.RemoveListener(UpdateUnfoldingValue);
        Debug.Log("Listener is removed from slider");
    }

    void InitialControls()
    {
        unfoldingValue = Mathf.Clamp(unfoldingValue, min_UnfoldingValue, max_UnfoldingValue);
    }


    public void UpdateUnfoldingValue(float value)
    {
        Debug.Log("Updating unfolding value!!!");
        value = xrSlider.value;
        unfoldingValue = max_UnfoldingValue - value * (max_UnfoldingValue - min_UnfoldingValue);

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
