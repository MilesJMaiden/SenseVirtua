using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Voice : MonoBehaviour
{
    public int currentIdx = 0;
    public string[] voiceTexts;

    public GameObject bubbleCanvas;
    private BubbleCanvas bubbleCanvasScript;

    public TMP_Text nextText;
    public TMP_Text endText;

    public GameObject buddhaStatue;
    public Animator animator; // 캐릭터의 Animator 컴포넌트

    public event System.Action OnDialogueEnd;
    public event System.Action OnLastDialogueEnd;

    public bool onSite;
    public float timer = 0;
    public bool playing = false;

    public void Start()
    {
        animator = Guide.Instance.animator;
        bubbleCanvasScript = bubbleCanvas.GetComponent<BubbleCanvas>();
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

    public void PlayVoice() // Changed to public
    {
        if (playing == true)
            return;

        playing = true;

        // 순차적으로 애니메이션 선택
        string animationName;
        switch (currentIdx % 3)
        {
            case 0:
                animationName = "talking2";
                break;
            case 1:
                animationName = "talking1";
                break;
            case 2:
                animationName = "Idle"; // Idle 애니메이션
                break;
            default:
                animationName = "Idle";
                break;
        }

        // 지정한 이름의 애니메이션을 즉시 재생
        animator.Play(animationName);
        Debug.Log($"Playing {animationName}");
        //animator.Play("talking1");

        Debug.Log($"======={voiceTexts[currentIdx]}=====");
        AudioClip clip = TranslationMgr.instance.GetTranslationVoice(voiceTexts[currentIdx]);
        GuideVoice.Instance.Play(clip);
        //voiceAudio.Play();

        string txt = TranslationMgr.instance.GetTranslationText(voiceTexts[currentIdx]);

        if (clip == null)
        {
            Debug.Log("empty empty");
        }
        bubbleCanvasScript.nextButton.onClick.RemoveAllListeners();
        bubbleCanvasScript.skipButton.onClick.RemoveAllListeners();
        bubbleCanvasScript.skipButton.onClick.AddListener(SkipVoice);
        bubbleCanvasScript.nextButton.onClick.AddListener(OnClickedNextBtn);

        bubbleCanvasScript.ShowDialogue(txt, clip.length);
        nextText.gameObject.SetActive(false);
        endText.gameObject.SetActive(false);

        

        Invoke("EndVoice", clip.length);
    }

    void EndVoice()
    {
        Debug.Log($"EndVoice {currentIdx}");
        if (currentIdx >= voiceTexts.Length)
        {
            OnLastDialogueEnd?.Invoke();
            bubbleCanvas.SetActive(false);

            if (SceneManager.GetActiveScene().name == "Beginning")
            {
                Invoke("HandleSpecialActions", 10f);
            }

            Debug.Log("Final dialogue ended, triggering OnDialogueEnd event.");
            OnDialogueEnd?.Invoke();

            enabled = false;
            return;
        }

        currentIdx++;

        playing = false;
        if (currentIdx >= voiceTexts.Length)
        {
            endText.gameObject.SetActive(true);
        }
        else
        {
            nextText.gameObject.SetActive(true);
        }
            

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

            string txt = TranslationMgr.instance.GetTranslationText(voiceTexts[currentIdx]);
            bubbleCanvasScript.ShowFullDialogue(txt);

            playing = false;

            currentIdx++;

            if (currentIdx >= voiceTexts.Length)
            {
                endText.gameObject.SetActive(true);
            }
            else
            {
                nextText.gameObject.SetActive(true);
            }

            OnDialogueEnd?.Invoke();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onSite = true;
            bubbleCanvas.SetActive(true);
            timer = 0;

            GameObject playerObject = GameObject.Find("Player");
            Vector3 direction = playerObject.transform.position - Guide.Instance.transform.position;
            direction.y = 0;
            Guide.Instance.LookAt(direction);
        }
    }

    public void UpdateDialogueAndVoice()
    {
        AudioClip clip = TranslationMgr.instance.GetTranslationVoice(voiceTexts[currentIdx > 0 ? currentIdx - 1 : 0]);
        string txt = TranslationMgr.instance.GetTranslationText(voiceTexts[currentIdx > 0 ? currentIdx - 1 : 0]);

        if (playing || currentIdx == 0)
        {
            StopVoice();
            GuideVoice.Instance.Play(clip);
            bubbleCanvasScript.ShowFullDialogue(txt);

            Invoke("EndVoice", clip.length);

            playing = true;
        }
        else
        {
            bubbleCanvasScript.ShowFullDialogue(txt);
        }
    }
}
