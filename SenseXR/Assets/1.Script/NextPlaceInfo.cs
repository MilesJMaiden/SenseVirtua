using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPlaceInfo : MonoBehaviour
{
    public Transform placeTr; // EndDialogue() 함수 호출 시 가이드가 이동하는 위치
    public void Awake()
    {
        //Voice voice = GetComponent<Voice>();
        //voice.OnLastDialogueEnd += EndDialogue;
    }
    public void StartNext()
    {
        // 가이드 이동
        Guide.Instance.MoveTo(placeTr.position, () => {
            Debug.Log("다음 장도 도착함");
            placeTr.GetComponent<WaitGuideUntilCollider>().ActiveArea();
        });
    }
}
