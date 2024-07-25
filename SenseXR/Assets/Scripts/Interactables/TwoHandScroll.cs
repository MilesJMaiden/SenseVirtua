using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class TwoHandScroll : MonoBehaviour
{
    public float min_UnfoldingValue = -1f;
    public float max_UnfoldingValue = 0.72f;
    public float unfoldingValue = 0.72f;

    public GameObject cover;
    public XRSlider slider;


    private Material coverMat;

    private void OnEnable()
    {
        Material coverMat = cover.GetComponent<Material>();
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
        slider.onValueChange.AddListener(UpdateUnfoldingValue);
    }

    void DisconnectControlEvent()
    {
        slider.onValueChange.RemoveListener(UpdateUnfoldingValue);
    }

    void InitialControls()
    {
        unfoldingValue = Mathf.Clamp(unfoldingValue, min_UnfoldingValue, max_UnfoldingValue);
    }


    public void UpdateUnfoldingValue(float value)
    {
        Debug.Log("Updating unfolding value!!!");
        value = slider.value;
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
