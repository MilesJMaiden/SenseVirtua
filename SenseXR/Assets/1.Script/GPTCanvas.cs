using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPTCanvas : MonoBehaviour
{
    //ΩÃ±€≈Ê √ﬂ∞°
    private static GPTCanvas instance;
    public static GPTCanvas Instance
    { get { 
            if(instance == null)
                instance = FindObjectOfType<GPTCanvas>(true);
            return instance; } }


    Action endCallback;

   
    public void StartGPT(Action endCallback)
    {
        gameObject.SetActive(true);
        this.endCallback = endCallback;
    }
    public void OnClickedClose()
    {
        endCallback?.Invoke();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnClickedClose();
        }
    }
}
