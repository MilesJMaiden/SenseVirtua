using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFromMic : MonoBehaviour
{
    public AudioSource source;
    public AudioLoudnessDetection detector;
    public ParticleSystem audioFeedbackPS;
    public GameManager gameManager;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness < threshold && audioFeedbackPS.isPlaying)
            audioFeedbackPS.Stop();
        else if (loudness > threshold && audioFeedbackPS.isStopped)
            audioFeedbackPS.Play();

        if (loudness > threshold)
        {
            gameManager.OnPlayerVoiceInput();
        }
    }

    private void OnDisable()
    {
        audioFeedbackPS.Stop();
    }
}
