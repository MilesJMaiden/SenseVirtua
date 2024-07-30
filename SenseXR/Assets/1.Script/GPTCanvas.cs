using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPTCanvas : MonoBehaviour
{
    //ΩÃ±€≈Ê √ﬂ∞°
    public static GPTCanvas instance;

    Action endCallback;

    private void Awake()
    {
        instance = this;
    }
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
}
