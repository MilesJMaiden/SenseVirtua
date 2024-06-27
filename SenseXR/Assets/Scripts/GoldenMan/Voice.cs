using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Voice : MonoBehaviour
{
    public AudioSource voiceAudio;

    public int currentIdx = 0;
    public string[] voiceTexts;
    public BubbleCanvas bubbleCanvas;

    public TMP_Text nextText;

    private void Awake()
    {
        voiceAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (onSite)
        {
            if (timer >= 3 && currentIdx == 0)
            {
                PlayVoice();
            }
            timer += Time.deltaTime;
        }
    }

    public void OnClickedNextBtn()
    {
        PlayVoice();
    }

    public bool playing = false;

    public void PlayVoice() // Changed to public
    {
        if (playing == true)
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

    void EndVoice()
    {
        if (currentIdx >= voiceTexts.Length)
        {
            bubbleCanvas.gameObject.SetActive(false);
            return;
        }

        playing = false;

        nextText.gameObject.SetActive(true);

        OnDialogueEnd?.Invoke();
    }

    public void StopVoice()
    {
        if (playing)
        {
            voiceAudio.Stop();
            CancelInvoke("EndVoice");
            EndVoice();
        }
    }

    public bool onSite;

    public float timer = 0;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = true;
            timer = 0;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = false;
        }
    }

    public event System.Action OnDialogueEnd;
}
