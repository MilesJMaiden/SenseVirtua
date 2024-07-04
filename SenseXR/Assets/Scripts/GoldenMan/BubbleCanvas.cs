using System.Collections;
using TMPro;
using UnityEngine;

public class BubbleCanvas : MonoBehaviour
{
    [Tooltip("Text component for displaying dialogues.")]
    public TMP_Text dialogueText;

    private Coroutine showTextCoroutine;
    private bool showing = false;

    public void ShowDialogue(string message, float voiceTime)
    {
        gameObject.SetActive(true);

        if (showing)
            return;

        showing = true;
        showTextCoroutine = StartCoroutine(ShowText(message, voiceTime));
        //StartCoroutine(ShowText(message, voiceTime));
    }

    public void ShowFullDialogue(string text)
    {
        if (showTextCoroutine != null)
        {
            StopCoroutine(showTextCoroutine);
            showTextCoroutine = null;
        }
        dialogueText.text = text;
        showing = false;
    }

    private IEnumerator ShowText(string message, float voiceTime)
    {
        char[] messageChars = message.ToCharArray();
        float wordWaitTime = voiceTime / messageChars.Length;

        dialogueText.text = string.Empty;
        foreach (char c in messageChars)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(wordWaitTime);
        }

        showing = false;
    }
}
