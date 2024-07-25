using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class TwoHandScroll : MonoBehaviour
{
    public float min_UnfoldingValue = -0.4f;
    public float max_UnfoldingValue = 0.28f;
    public float unfoldingValue;

    private Material coverMat;

    private void Start()
    {
        coverMat = GetComponent<MeshRenderer>().material;
    }
    public void updateunfoldingvalue(XRSlider slider)
    {
        Debug.Log("Updating unfolding value!!!");
        float value = slider.value;
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

    public void Test(XRSlider slider)
    {
        float value = slider.value;
        Debug.Log("I see you are playing slider and the value is" + value);
        
    }
    public void Test2()
    {
        //float value = slider.value;
        Debug.Log("I see you are playing slider and I have no value");

    }


}
