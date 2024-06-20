using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Voice : MonoBehaviour
{
    public AudioSource voiceAudio;

    public int currentIdx = 0;
    //public AudioClip[] voiceClips;
    public string[] voiceTexts;
    //public GameObject nextBtnCanvas;
    public BubbleCanvas bubbleCanvas;

    public TMP_Text nextText;

    private void Awake()
    {
        voiceAudio = GetComponent<AudioSource>();
        //voiceAudio.clip = voiceClips[currentIdx];
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
    void PlayVoice()
    {
        if (playing == true)
            return;


        playing = true;
        voiceAudio.clip = TranslationMgr.instance.GetTranslationVoice(voiceTexts[currentIdx]);
        voiceAudio.Play();

        string txt = TranslationMgr.instance.GetTranslationText(voiceTexts[currentIdx]);

        //bubbleCanvas.gameObject.SetActive(true);
        bubbleCanvas.ShowDialogue(txt, voiceAudio.clip.length);
        nextText.gameObject.SetActive(false);

        //voiceAudio.clip.length;
        //onSite = false;
        currentIdx++;

        Invoke("EndVoice", voiceAudio.clip.length);
    }

    void EndVoice()
    {


        if (currentIdx >= voiceTexts.Length)
        {
            bubbleCanvas.gameObject.SetActive(false);
            //nextBtnCanvas.SetActive(false);
            return;
        }


        playing = false;

        Debug.Log("EndVoice");

        //nextBtnCanvas.SetActive(true);
        nextText.gameObject.SetActive(true);
        
    }



    // If you are in the space for 3 seconds, voiceAudio.Play()
    public bool onSite;

    public float timer = 0;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = true;
            timer = 0;

            //voiceAudio.Play();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = false;

            //voiceAudio.Play();
        }
    }
}
