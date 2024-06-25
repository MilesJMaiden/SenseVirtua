using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CalligraphyGame : MonoBehaviour
{
    [Tooltip("The GameObject representing GoldenMan.")]
    public GameObject goldenMan;

    [Tooltip("Game object to enable when the calligraphy game is completed.")]
    public GameObject completionObject;

    [Tooltip("Draw tasks in the order they should be completed.")]
    public GameObject[] drawTasks;

    [Tooltip("Success visuals for each draw task.")]
    public CalligraphySuccessTween[] successVisuals;

    [Tooltip("Percentage of colliders that need to be triggered to consider a task complete.")]
    [Range(0, 1)]
    public float completionThreshold = 0.8f;

    [Tooltip("Duration for fading in/out the task objects.")]
    public float taskFadeDuration = 1.0f;

    private int currentSymbolIndex = 0;
    public bool calligraphyGameCompleted = false;
    private Voice goldenManVoice;

    void Start()
    {
        // Ensure all symbols are inactive initially
        foreach (GameObject task in drawTasks)
        {
            task.SetActive(false);
        }

        // Get the Voice component from GoldenMan
        goldenManVoice = goldenMan.GetComponent<Voice>();
        goldenManVoice.OnDialogueEnd += OnDialogueEnd; // Subscribe to the event

        Debug.Log("Subscribed to OnDialogueEnd event.");

        // Disable the completion object initially
        if (completionObject != null)
        {
            completionObject.SetActive(false);
        }
    }

    void OnDialogueEnd()
    {
        Debug.Log("OnDialogueEnd called. Enabling next task.");
        // Enable the next task with a fade-in effect after the dialogue finishes
        if (currentSymbolIndex < drawTasks.Length)
        {
            StartCoroutine(FadeInTask(drawTasks[currentSymbolIndex]));
        }
        else
        {
            Debug.Log("No more tasks to enable.");
        }
    }

    void Update()
    {
        if (currentSymbolIndex < drawTasks.Length)
        {
            CheckColliders();
        }
    }

    void CheckColliders()
    {
        GameObject currentSymbol = drawTasks[currentSymbolIndex];
        SymbolColliderGroup colliderGroup = currentSymbol.GetComponentInChildren<SymbolColliderGroup>();

        if (colliderGroup != null && colliderGroup.IsCompletionThresholdReached(completionThreshold))
        {
            Debug.Log("CheckColliders: Threshold reached for " + currentSymbol.name);
            StartCoroutine(CompleteCurrentTask());
        }
    }

    IEnumerator CompleteCurrentTask()
    {
        Debug.Log("CompleteCurrentTask started for " + drawTasks[currentSymbolIndex].name);
        EndSymbolTask();
        ExecuteSuccessVisuals(currentSymbolIndex);

        yield return StartCoroutine(FadeOutAndDisable(drawTasks[currentSymbolIndex]));

        currentSymbolIndex++;
        if (currentSymbolIndex < drawTasks.Length)
        {
            Debug.Log("CompleteCurrentTask: Starting next dialogue.");
            StartCoroutine(PlayNextDialogue());
        }
        else
        {
            calligraphyGameCompleted = true;
            if (completionObject != null)
            {
                completionObject.SetActive(true);
            }
        }
    }

    IEnumerator FadeOutAndDisable(GameObject task)
    {
        Debug.Log("Fading out and disabling: " + task.name);

        HighlightAid highlightAid = task.GetComponent<HighlightAid>();
        if (highlightAid != null)
        {
            highlightAid.FadeOut();
        }

        yield return new WaitForSeconds(taskFadeDuration);
        task.SetActive(false); // Ensure the task is disabled after fading out
    }

    IEnumerator FadeInTask(GameObject task)
    {
        Debug.Log("Fading in and enabling: " + task.name);

        task.SetActive(true); // Ensure the task is set active before fading in
        HighlightAid highlightAid = task.GetComponent<HighlightAid>();
        if (highlightAid != null)
        {
            highlightAid.FadeIn();
        }

        yield return new WaitForSeconds(taskFadeDuration);
    }

    void ExecuteSuccessVisuals(int index)
    {
        if (successVisuals != null && index < successVisuals.Length)
        {
            Debug.Log("Executing success visuals for task index: " + index);
            successVisuals[index].ExecuteTweenSequence();
        }
    }

    IEnumerator PlayInitialDialogue()
    {
        Debug.Log("PlayInitialDialogue started.");
        if (goldenManVoice != null)
        {
            goldenManVoice.PlayVoice();
            yield return new WaitUntil(() => !goldenManVoice.playing);

            Debug.Log("Initial dialogue finished. Enabling first task.");
            OnDialogueEnd();
        }
    }

    IEnumerator PlayNextDialogue()
    {
        Debug.Log("PlayNextDialogue started.");
        if (goldenManVoice != null)
        {
            goldenManVoice.PlayVoice();
            yield return new WaitUntil(() => !goldenManVoice.playing);

            Debug.Log("Next dialogue finished. Enabling next task.");
            OnDialogueEnd();
        }
    }

    void DisableCurrentTask()
    {
        if (currentSymbolIndex - 1 >= 0 && currentSymbolIndex - 1 < drawTasks.Length)
        {
            drawTasks[currentSymbolIndex - 1].SetActive(false);
        }
    }

    void UpdateSymbolVisibility()
    {
        for (int i = 0; i < drawTasks.Length; i++)
        {
            drawTasks[i].SetActive(i == currentSymbolIndex);
        }
    }

    public void StartSymbolTask()
    {
        // This method is intentionally left empty to avoid automatic task enabling
    }

    public void EndSymbolTask()
    {
        DisableCurrentTask();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !calligraphyGameCompleted)
        {
            Debug.Log("Player entered collider. Starting initial dialogue.");
            StartCoroutine(PlayInitialDialogue());
        }
    }
}
