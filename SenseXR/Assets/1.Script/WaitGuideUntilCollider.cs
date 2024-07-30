using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitGuideUntilCollider : MonoBehaviour
{
    public bool active = false;
    public Transform dialoguePoint;

    private void Start()
    {
        Voice voice = GetComponent<Voice>();
        voice.OnLastDialogueEnd += EndDialogue;
    }

    void EndDialogue()
    {
        active = false;
    }
    public void ActiveArea()
    {
        active = true;
        Guide.Instance.LookAt(transform.rotation.eulerAngles);
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!active)
            return;
        if (other.CompareTag("Player"))
        {
            Voice voice = GetComponent<Voice>();
            voice.PlayVoice();
            BubbleCanvas.Instance.transform.position = dialoguePoint.position;
            BubbleCanvas.Instance.transform.forward = dialoguePoint.forward;
        }
    }
}
