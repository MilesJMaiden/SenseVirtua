using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using GLTFast.Schema;
//using UnityEngine.ProBuilder.MeshOperations;

public class Voice : MonoBehaviour
{
    //public AudioSource voiceAudio;
    //public GameObject voiceObject;

    //public AudioSource specialAudioSource; 

    public int currentIdx = 0;
    public string[] voiceTexts;
    public BubbleCanvas bubbleCanvas;

    public TMP_Text nextText;
    public TMP_Text endText;

    public GameObject buddhaStatue;

    public Animator animator; // 캐릭터의 Animator 컴포넌트

    public event System.Action OnDialogueEnd;
    public event System.Action OnLastDialogueEnd;

    public void Start()
    {
        animator = Guide.Instance.animator;
    }
    private void Awake()
    {
        //voiceAudio = GetComponent<AudioSource>();
        //if (voiceObject != null)
        //{
        //    voiceAudio = voiceObject.GetComponent<AudioSource>();
        //    if (voiceAudio == null)
        //    {
        //      Debug.LogError("AudioSource component not found on the assigned Voice object.");
        //    }
        //}
        //else
        //{
        //    Debug.LogError("Voice object is not assigned.");
        //}
       
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

        if (Input.GetKeyDown(KeyCode.N))
        {
            OnClickedNextBtn();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnClickedSkipBtn();
        }
    }

    public void OnClickedNextBtn()
    {
        if (currentIdx >= voiceTexts.Length)
        {
            EndVoice();
            return;
        }
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

        // 순차적으로 애니메이션 선택
        animator.Play("talking1");

        Debug.Log($"======={voiceTexts[currentIdx]}=====");
        AudioClip clip = TranslationMgr.instance.GetTranslationVoice(voiceTexts[currentIdx]);
        GuideVoice.Instance.Play(clip);
        //voiceAudio.Play();

        string txt = TranslationMgr.instance.GetTranslationText(voiceTexts[currentIdx]);

        if (clip == null)
        {
            Debug.Log("empty empty");
        }
        bubbleCanvas.ShowDialogue(txt, clip.length);
        nextText.gameObject.SetActive(false);
        endText.gameObject.SetActive(false);

        currentIdx++;

        Invoke("EndVoice", clip.length);
    }

    void EndVoice()
    {
        Debug.Log($"EndVoice {currentIdx}");
        if (currentIdx >= voiceTexts.Length)
        {
            OnLastDialogueEnd?.Invoke();
            bubbleCanvas.gameObject.SetActive(false);
            

            if (SceneManager.GetActiveScene().name == "Beginning")
            {
                Invoke("HandleSpecialActions", 10f);
            }

            Debug.Log("Final dialogue ended, triggering OnDialogueEnd event.");
            OnDialogueEnd?.Invoke();

           

            enabled = false;
            return;
        }

        playing = false;
        nextText.gameObject.SetActive(true);

        Debug.Log("Dialogue ended, triggering OnDialogueEnd event.");
        OnDialogueEnd?.Invoke(); 
    }

    public void StopVoice()
    {
        if (playing)
        {
            GuideVoice.Instance.Stop();
            CancelInvoke("EndVoice");
            EndVoice();
        }
    }

    public void SkipVoice()
    {
        if (playing)
        {
            GuideVoice.Instance.Stop();
            CancelInvoke("EndVoice");

            // Show the full text immediately
            string txt = TranslationMgr.instance.GetTranslationText(voiceTexts[currentIdx - 1]);
            bubbleCanvas.ShowFullDialogue(txt);

            playing = false;
            // Check if it's the last dialogue
            if (currentIdx >= voiceTexts.Length)
            {
                //bubbleCanvas.gameObject.SetActive(false);
                endText.gameObject.SetActive(true);

                //if (SceneManager.GetActiveScene().name == "Beginning")
                //{
                //    Invoke("HandleSpecialActions", 10f); 
                //}
            }
            else
            {
                nextText.gameObject.SetActive(true);
            }

            OnDialogueEnd?.Invoke();
        }
    }

    //public bool IsPlaying()
    //{
    //    return voiceAudio.isPlaying;
    //}

    public bool onSite;

    public float timer = 0;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = true;
            bubbleCanvas.gameObject.SetActive(true);
            timer = 0;

            GameObject playerObject = GameObject.Find("Player");
            Vector3 direction = playerObject.transform.position - Guide.Instance.transform.position;
            direction.y = 0;
            Guide.Instance.LookAt(direction);
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

    // Update dialogue and voice based on the current language
    public void UpdateDialogueAndVoice()
    {
        // Update the voice audio clip and text to the current language
        AudioClip clip = TranslationMgr.instance.GetTranslationVoice(voiceTexts[currentIdx > 0 ? currentIdx - 1 : 0]);
        string txt = TranslationMgr.instance.GetTranslationText(voiceTexts[currentIdx > 0 ? currentIdx - 1 : 0]);

        // Play the updated voice and show the updated text if playing or if at the start
        if (playing || currentIdx == 0)
        {
            StopVoice();

            GuideVoice.Instance.Play(clip);
            bubbleCanvas.ShowFullDialogue(txt);

            // Schedule the end of the current voice
            Invoke("EndVoice", clip.length);

            playing = true;
        }
        else
        {
            //voiceAudio.clip = TranslationMgr.instance.GetTranslationVoice(voiceTexts[currentIdx > 0 ? currentIdx - 1 : 0]);
            bubbleCanvas.ShowFullDialogue(txt);
        }
    }




    private void HandleSpecialActions()
    {
        // Buddha disappear
        if (buddhaStatue != null)
        {
            buddhaStatue.SetActive(false);
        }

        // extra sound
        //if (specialAudioSource != null)
        //{
        //    specialAudioSource.Play();
        //}
    }

}
