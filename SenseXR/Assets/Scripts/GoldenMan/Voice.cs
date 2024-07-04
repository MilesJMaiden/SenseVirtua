using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Voice : MonoBehaviour
{
    public AudioSource voiceAudio;

    public AudioSource specialAudioSource; 

    public int currentIdx = 0;
    public string[] voiceTexts;
    public BubbleCanvas bubbleCanvas;

    public TMP_Text nextText;

    public GameObject buddhaStatue;

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

    public void OnClickedSkipBtn()
    {
        SkipVoice();
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

            // Work only in the Beginning Scene
            if (SceneManager.GetActiveScene().name == "Beginning")
            {
                Invoke("HandleSpecialActions", 10f); 
            }

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

    public void SkipVoice()
    {
        if (playing)
        {
            voiceAudio.Stop();
            CancelInvoke("EndVoice");

            // Show the full text immediately
            string txt = TranslationMgr.instance.GetTranslationText(voiceTexts[currentIdx - 1]);
            bubbleCanvas.ShowFullDialogue(txt);

            playing = false;
            // Check if it's the last dialogue
            if (currentIdx >= voiceTexts.Length)
            {
                bubbleCanvas.gameObject.SetActive(false);

                
                if (SceneManager.GetActiveScene().name == "Beginning")
                {
                    Invoke("HandleSpecialActions", 10f); 
                }
            }
            else
            {
                nextText.gameObject.SetActive(true);
            }

            OnDialogueEnd?.Invoke();
        }
    }

    public bool IsPlaying()
    {
        return voiceAudio.isPlaying;
    }

    public bool onSite;

    public float timer = 0;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = true;
            bubbleCanvas.gameObject.SetActive(true);
            timer = 0;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = false;
            bubbleCanvas.gameObject.SetActive(false);
        }
    }

    public event System.Action OnDialogueEnd;

    private void HandleSpecialActions()
    {
        // Buddha disappear
        if (buddhaStatue != null)
        {
            buddhaStatue.SetActive(false);
        }

        // extra sound
        if (specialAudioSource != null)
        {
            specialAudioSource.Play();
        }
    }

}
