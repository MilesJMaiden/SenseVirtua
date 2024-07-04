using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SlowBlinkEffect : MonoBehaviour
{
    public Volume globalVolume;
    private Vignette vignette;

    private void Start()
    {
        
        if (globalVolume.profile.TryGet(out vignette))
        {
            StartCoroutine(BlinkSequence());
        }
    }

    IEnumerator BlinkSequence()
    {
        // first blink
        yield return OpenEyesVerySlowly();
        yield return new WaitForSeconds(0.5f);

        // second blink
        yield return SlowBlink();
        yield return new WaitForSeconds(0.2f);

        // third blink
        yield return SlowBlink();
        yield return new WaitForSeconds(0.2f);

        // Set Vignette intensity to 0
        vignette.intensity.value = 0f;


    }

    IEnumerator SlowBlink()
    {

        float duration = 0.1f;
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            vignette.intensity.value = Mathf.Lerp(0, 1f, t / duration);
            yield return null;
        }
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            vignette.intensity.value = Mathf.Lerp(1f, 0, t / duration);
            yield return null;
        }
    }

    IEnumerator OpenEyesVerySlowly()
    {

        float duration = 2.0f;

        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            vignette.intensity.value = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            vignette.intensity.value = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }
    }
}