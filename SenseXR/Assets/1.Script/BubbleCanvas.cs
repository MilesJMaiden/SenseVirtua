using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubbleCanvas : MonoBehaviour
{

    public TMP_Text dialogueText; // refer Component
    //List<string> dialogues = new List<string>();
    bool showing; // Prevent other letters from appearing when a letter is appearing

    //bool dialogueStarted = false;
    public void ShowDialogue(string d, float voiceTime)
    {
        gameObject.SetActive(true);
        //dialogues.Add(d);

        if (showing)
        {
            return;
        }

        showing = true;
        StartCoroutine(ShowText(d,voiceTime));
    }

    IEnumerator ShowText(string message, float voiceTime)
    {
        char[] messageChars = message.ToCharArray();
        float wordWaitTime = voiceTime / messageChars.Length;

        string curMessage = "";
        dialogueText.text = curMessage;
        for (int i = 0; i < messageChars.Length; i++)
        {
            curMessage += messageChars[i];
            dialogueText.text = curMessage;
            yield return new WaitForSeconds(wordWaitTime);
        }
        //yield return new WaitForSeconds(1f);
        showing = false;

        //Delete first data of List
        //dialogues.RemoveAt(0);
        //if (dialogues.Count > 0)
        //{
        //    StartCoroutine(ShowText(dialogues[0]));
        //}
        //else
        //    gameObject.SetActive(false);
    }
}
