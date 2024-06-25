using System.Collections;
using UnityEngine;
using TMPro;

public class Voice : MonoBehaviour
{
    [Tooltip("AudioSource component for playing voice clips.")]
    public AudioSource voiceAudio;

    [Tooltip("Current index of the voice text.")]
    public int currentIdx = 0;

    [Tooltip("Array of voice texts to be played.")]
    public string[] voiceTexts;

    [Tooltip("Bubble canvas for displaying dialogues.")]
    public BubbleCanvas bubbleCanvas;

    [Tooltip("Text component for the next button.")]
    public TMP_Text nextText;

    public bool playing = false;
    public bool onSite = false;
    private float timer = 0;

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
        if (playing || currentIdx >= voiceTexts.Length)
            return;

        playing = true;
        voiceAudio.clip = TranslationMgr.instance.GetTranslationVoice(voiceTexts[currentIdx]);
        voiceAudio.Play();

        string txt = TranslationMgr.instance.GetTranslationText(voiceTexts[currentIdx]);
        bubbleCanvas.ShowDialogue(txt, voiceAudio.clip.length);
        nextText.gameObject.SetActive(false);

        currentIdx++;
        Invoke("EndVoice", voiceAudio.clip.length);
    }

    private void EndVoice()
    {
        if (currentIdx >= voiceTexts.Length)
        {
            bubbleCanvas.gameObject.SetActive(false);
            return;
        }

        playing = false;
        nextText.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = true;
            timer = 0;
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
