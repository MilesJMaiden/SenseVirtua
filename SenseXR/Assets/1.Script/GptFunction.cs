using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GptFunction : MonoBehaviour
{
    public void Awake()
    {
        Voice voice = GetComponent<Voice>();
        voice.OnLastDialogueEnd += EndDialogue;
    }

    //�� �Լ��� ȣ�� �� ���� ��ҷ� ���̵带 �̵���Ű��!

    void EndDialogue()
    {
        // GPT ��� ����
        // GPT �� UI ���� �ű⿡ Text ������
        //������� ��ġ�� �������� + WaitGuide~~ ���� dialoguePoint �����.
        GPTCanvas.instance.StartGPT(EndGPT);
    }

    public void EndGPT() // Gpt ����� ������ �� ȣ��
    {
        GetComponent<NextPlaceInfo>().StartNext();
    }
}
