using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPlaceInfo : MonoBehaviour
{
    public Transform placeTr; // EndDialogue() �Լ� ȣ�� �� ���̵尡 �̵��ϴ� ��ġ
    public void Awake()
    {
        //Voice voice = GetComponent<Voice>();
        //voice.OnLastDialogueEnd += EndDialogue;
    }
    public void StartNext()
    {
        // ���̵� �̵�
        Guide.Instance.MoveTo(placeTr.position, () => {
            Debug.Log("���� �嵵 ������");
            placeTr.GetComponent<WaitGuideUntilCollider>().ActiveArea();
        });
    }
}
