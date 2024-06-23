using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFromAudioPlay : MonoBehaviour
{
    public AudioSource source;
    public AudioLoudnessDetection detector;
    public Vector3 minScale;
    public Vector3 maxScale;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip) * loudnessSensibility;

        if (loudness < threshold)
            loudness = 0;
        // lerp value from minscale to maxscale
        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
        
    }
}
