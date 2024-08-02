using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TMP_Text contentsText;
    void OnEnable()
    {
        contentsText.text = null;
    }

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
