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

    [Tooltip("Percentage of colliders that need to be triggered to consider a task complete.")]
    [Range(0, 1)]
    public float completionThreshold = 0.8f;

    [Tooltip("Duration for scaling the task objects.")]
    public float taskScaleDuration = 1.0f;

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
        goldenManVoice.OnDialogueEnd += EnableFirstTask; // Subscribe to the event

        Debug.Log("Subscribed to OnDialogueEnd event.");

        // Disable the completion object initially
        if (completionObject != null)
        {
            completionObject.SetActive(false);
        }
    }

    void EnableFirstTask()
    {
        Debug.Log("EnableFirstTask called.");
        // Enable the first task with a scale tween after the dialogue finishes
        if (drawTasks.Length > 0)
        {
            Debug.Log("Enabling Symbol1: " + drawTasks[0].name);
            EnableTaskWithTween(drawTasks[0]);
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
            EndSymbolTask();
            currentSymbolIndex++;
            if (currentSymbolIndex < drawTasks.Length)
            {
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
            UpdateSymbolVisibility();
        }
    }

    IEnumerator PlayInitialDialogue()
    {
        Debug.Log("PlayInitialDialogue started.");
        if (goldenManVoice != null)
        {
            goldenManVoice.PlayVoice();
            yield return new WaitUntil(() => !goldenManVoice.playing);

            Debug.Log("Initial dialogue finished.");
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
            // Enable the next task with a scale tween
            if (currentSymbolIndex < drawTasks.Length)
            {
                EnableTaskWithTween(drawTasks[currentSymbolIndex]);
            }
        }
    }

    void EnableTaskWithTween(GameObject task)
    {
        Debug.Log("EnableTaskWithTween: " + task.name);
        task.SetActive(true);
        task.transform.localScale = Vector3.zero;
        LeanTween.scale(task, Vector3.one, taskScaleDuration).setEase(LeanTweenType.easeInOutSine);

        // Start the HighlightAid lerping
        HighlightAid highlightAid = task.GetComponent<HighlightAid>();
        if (highlightAid != null)
        {
            highlightAid.StartLerping();
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
