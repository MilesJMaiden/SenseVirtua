using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitGuideUntilCollider : MonoBehaviour
{
    public bool active = false;
    public Transform dialoguePoint;
    public Voice voice;
    private void Start()
    {
        //voice = GetComponent<Voice>();
        voice.OnLastDialogueEnd += EndDialogue;
        voice.enabled = false;
    }

    void EndDialogue()
    {
        active = false;
    }
    public void ActiveArea()
    {
        voice.enabled = true;
        active = true;
        Guide.Instance.LookAt(transform.rotation.eulerAngles);
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!active)
            return;
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entering dntr1");
            //Voice voice = GetComponent<Voice>();
            voice.PlayVoice();
            BubbleCanvas.Instance.transform.position = dialoguePoint.position;
            BubbleCanvas.Instance.transform.forward = dialoguePoint.forward;
        }
    }
}
