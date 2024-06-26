using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class FadeEffectCanvas : MonoBehaviour
{
    public static FadeEffectCanvas instance;
    public Image bgImage;
    public float fadeDuration = 1.0f;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        //bgImage.color = new Color(1,1,1,0); // transparent
        //bgImage.color = new Color(1, 1, 1, 1); 
    }

    public void StartEffect(Action completeFadeOutCallback)
    {
        StartCoroutine(CoEffect(completeFadeOutCallback));
    }

    IEnumerator CoEffect(Action completeFadeOutCallback)
    {
        // Make it opaque
        yield return StartCoroutine(FadeTo(1.0f, fadeDuration));

        completeFadeOutCallback.Invoke();
        // Make it transparent
        yield return StartCoroutine(FadeTo(0.0f, fadeDuration));
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = bgImage.color.a;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            bgImage.color = new Color(1, 1, 1, newAlpha);
            yield return null;
        }

        bgImage.color = new Color(1, 1, 1, targetAlpha);
    }

}

