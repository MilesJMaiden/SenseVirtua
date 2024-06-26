using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Voice : MonoBehaviour
{
    [Tooltip("AudioSource component for playing voice clips.")]
    public AudioSource voiceAudio;

    [Tooltip("Current index of the voice text.")]
    public int currentIdx = 0;

    [Tooltip("Array of keys to fetch voice clips and texts.")]
    public string[] voiceKeys;

    [Tooltip("Bubble canvas for displaying dialogues.")]
    public BubbleCanvas bubbleCanvas;

    [Tooltip("Text component for the next button.")]
    public TMP_Text nextText;

    public bool playing = false;
    public bool onSite = false;
    private float timer = 0;

    public event Action OnDialogueEnd;

    private void Awake()
    {
        voiceAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (onSite)
        {
            timer += Time.deltaTime;
            if (timer >= 3 && currentIdx == 0)
            {
                PlayVoice();
            }
        }
    }

    public void OnClickedNextBtn()
    {
        PlayVoice();
    }

    public void PlayVoice()
    {
        if (playing || currentIdx >= voiceKeys.Length)
            return;

        playing = true;
        string key = voiceKeys[currentIdx];
        Debug.Log("Playing voice: " + key);
        voiceAudio.clip = TranslationMgr.instance.GetTranslationVoice(key);
        voiceAudio.Play();

        string txt = TranslationMgr.instance.GetTranslationText(key);
        bubbleCanvas.ShowDialogue(txt, voiceAudio.clip.length);
        nextText.gameObject.SetActive(false);

        currentIdx++;
        Invoke("EndVoice", voiceAudio.clip.length);
    }

    private void EndVoice()
    {
        if (currentIdx >= voiceKeys.Length)
        {
            bubbleCanvas.gameObject.SetActive(false);
            return;
        }

        playing = false;
        nextText.gameObject.SetActive(true);
        Debug.Log("Dialogue ended.");
        OnDialogueEnd?.Invoke(); // Trigger the event
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = true;
            timer = 0;
            PlayVoice();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = false;
        }
    }
}
