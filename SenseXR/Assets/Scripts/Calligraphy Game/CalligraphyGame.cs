using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    [Tooltip("VFX prefab to instantiate when a task is completed.")]
    public GameObject vfxPrefab;

    [Tooltip("Transform where the VFX prefab will be instantiated.")]
    public Transform vfxSpawnTransform;

    [Tooltip("Game object to enable and tween after the final draw task is completed.")]
    public GameObject finalCompletionObject;

    private int currentSymbolIndex = 0;
    public bool calligraphyGameCompleted = false;
    private Voice goldenManVoice;
    private bool isProcessingTask = false;
    private Whiteboard whiteboard;

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

        // Disable the completion objects initially
        if (completionObject != null)
        {
            completionObject.SetActive(false);
        }

        if (finalCompletionObject != null)
        {
            finalCompletionObject.SetActive(false);
        }

        // Get the Whiteboard component
        whiteboard = FindObjectOfType<Whiteboard>();

        if (whiteboard == null)
        {
            Debug.LogError("Whiteboard component not found in the scene.");
        }
    }

    void OnDialogueEnd()
    {
        Debug.Log($"OnDialogueEnd called. currentSymbolIndex: {currentSymbolIndex}");
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
        if (currentSymbolIndex < drawTasks.Length && !isProcessingTask)
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
            Debug.Log($"CheckColliders: Threshold reached for {currentSymbol.name}");
            isProcessingTask = true;
            StartCoroutine(CompleteCurrentTask());
        }
    }

    IEnumerator CompleteCurrentTask()
    {
        Debug.Log($"CompleteCurrentTask started for {drawTasks[currentSymbolIndex].name}");
        EndSymbolTask();
        ExecuteSuccessVisuals(currentSymbolIndex);
        InstantiateVFX();

        yield return StartCoroutine(FadeOutAndDisable(drawTasks[currentSymbolIndex]));

        currentSymbolIndex++;
        Debug.Log($"currentSymbolIndex incremented to {currentSymbolIndex}");
        if (currentSymbolIndex < drawTasks.Length)
        {
            Debug.Log("CompleteCurrentTask: Starting next dialogue.");
            StartCoroutine(PlayNextDialogue());
        }
        else
        {
            Debug.Log("All tasks completed.");
            calligraphyGameCompleted = true;
            if (completionObject != null)
            {
                completionObject.SetActive(true);
                TweenCompletionObject();
            }
            if (finalCompletionObject != null)
            {
                EnableAndTweenFinalCompletionObject();
            }
        }
        isProcessingTask = false;

        // Reset the whiteboard texture after completing each task
        if (whiteboard != null)
        {
            whiteboard.ResetTexture();
        }
    }

    IEnumerator FadeOutAndDisable(GameObject task)
    {
        Debug.Log($"Fading out and disabling: {task.name}");

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
        Debug.Log($"Fading in and enabling: {task.name}");

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
            Debug.Log($"Executing success visuals for task index: {index}");
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

    private void InstantiateVFX()
    {
        if (vfxPrefab != null && vfxSpawnTransform != null)
        {
            GameObject vfxInstance = Instantiate(vfxPrefab, vfxSpawnTransform.position, vfxSpawnTransform.rotation);
            Destroy(vfxInstance, 1f);
        }
    }

    private void TweenCompletionObject()
    {
        if (completionObject != null)
        {
            completionObject.transform.localScale = Vector3.zero;
            LeanTween.scale(completionObject, Vector3.one, taskFadeDuration).setEase(LeanTweenType.easeInOutSine);
        }
    }

    private void EnableAndTweenFinalCompletionObject()
    {
        if (finalCompletionObject != null)
        {
            StartCoroutine(PlayNextDialogue());
            finalCompletionObject.SetActive(true);
            finalCompletionObject.transform.localScale = Vector3.zero;
            LeanTween.scale(finalCompletionObject, Vector3.one, taskFadeDuration).setEase(LeanTweenType.easeInOutSine);
        }
    }

    // Methods for editor buttons
    public void SkipCurrentDrawTask()
    {
        Debug.Log("Skipping current draw task.");
        if (currentSymbolIndex < drawTasks.Length)
        {
            // Simulate completing the current task
            StartCoroutine(CompleteCurrentTask());
        }
    }

    public void SkipCurrentDialogue()
    {
        Debug.Log("Skipping current dialogue.");
        if (goldenManVoice != null)
        {
            goldenManVoice.StopVoice();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CalligraphyGame))]
public class CalligraphyGameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CalligraphyGame script = (CalligraphyGame)target;
        if (GUILayout.Button("Skip Current Draw Task"))
        {
            script.SkipCurrentDrawTask();
        }
        if (GUILayout.Button("Skip Current Dialogue"))
        {
            script.SkipCurrentDialogue();
        }
    }
}
#endif