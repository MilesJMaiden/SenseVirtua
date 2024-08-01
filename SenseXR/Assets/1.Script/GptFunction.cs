using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GptFunction : MonoBehaviour
{
    public Transform placeTr;
    public void Awake()
    {
        Voice voice = GetComponent<Voice>();
        voice.OnLastDialogueEnd += EndDialogue;
    }

    //이 함수가 호출 시 다음 장소로 가이드를 이동시키기!

    void EndDialogue()
    {
        Debug.Log("GptFunction + EndDialogue");
        // GPT 기능 시작
        // GPT 용 UI 띄우고 거기에 Text 나오게
        //띄어지는 위치도 잡아줘야함 + WaitGuide~~ 에서 dialoguePoint 쓰면됨.

       
        GPTCanvas.Instance.StartGPT(EndGPT);
        Debug.Log($"placeTr.postion {placeTr.position}");
        GPTCanvas.Instance.transform.position = BubbleCanvas.Instance.transform.position;
        GPTCanvas.Instance.transform.forward = BubbleCanvas.Instance.transform.forward;
    }

    public void EndGPT() // Gpt 기능이 끝났을 때 호출
    {
        GetComponent<NextPlaceInfo>().StartNext();
    }
}
