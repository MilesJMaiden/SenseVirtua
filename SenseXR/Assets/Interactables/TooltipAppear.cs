using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipAppear : MonoBehaviour
{
    public float duration = 2f;
    public Material coverMat;

    public void Start()
    {
        
    }
    public void CallCoverDissolve()
    {
        StartCoroutine(CoverDissolve());
    }
    public IEnumerator CoverDissolve()
    {
        coverMat.SetFloat("_AppearStrength", 0f);
        float timeElapse = 0f;

        while (timeElapse < duration)
        {
            float newValue = Mathf.Lerp(0f, 0.75f, timeElapse / duration);
            coverMat.SetFloat("_AppearStrength", newValue);
            timeElapse += Time.deltaTime;
            yield return null;
        }

        coverMat.SetFloat("_AppearStrength", 0.75f);
    }
}
