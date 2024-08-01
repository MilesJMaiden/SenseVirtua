using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideVoice : MonoBehaviour
{
    public static GuideVoice Instance;
    public AudioSource audioSource;
    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
