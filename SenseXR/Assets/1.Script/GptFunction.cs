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

    //�� �Լ��� ȣ�� �� ���� ��ҷ� ���̵带 �̵���Ű��!

    void EndDialogue()
    {
        Debug.Log("GptFunction + EndDialogue");
        // GPT ��� ����
        // GPT �� UI ���� �ű⿡ Text ������
        //������� ��ġ�� �������� + WaitGuide~~ ���� dialoguePoint �����.

       
        GPTCanvas.Instance.StartGPT(EndGPT);
        Debug.Log($"placeTr.postion {placeTr.position}");
        GPTCanvas.Instance.transform.position = BubbleCanvas.Instance.transform.position;
        GPTCanvas.Instance.transform.forward = BubbleCanvas.Instance.transform.forward;
    }

    public void EndGPT() // Gpt ����� ������ �� ȣ��
    {
        GetComponent<NextPlaceInfo>().StartNext();
    }
}
