using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CalligraphyGame : MonoBehaviour
{
    [Tooltip("The parent GameObject containing all symbol tasks.")]
    public GameObject drawTasks;

    [Tooltip("The GameObject representing GoldenMan.")]
    public GameObject goldenMan;

    [Tooltip("Game object to enable when the calligraphy game is completed.")]
    public GameObject completionObject;

    [Tooltip("Percentage of colliders that need to be triggered to consider a task complete.")]
    [Range(0, 1)]
    public float completionThreshold = 0.8f;

    [Tooltip("Duration for scaling the task objects.")]
    public float taskScaleDuration = 1.0f;

    private int currentSymbolIndex = 0;
    private List<GameObject> symbols = new List<GameObject>();
    public bool calligraphyGameCompleted = false;
    private Voice goldenManVoice;

    void Start()
    {
        // Initialize the symbols list
        foreach (Transform child in drawTasks.transform)
        {
            symbols.Add(child.gameObject);
        }

        // Ensure only the first symbol is active initially
        UpdateSymbolVisibility();

        // Get the Voice component from GoldenMan
        goldenManVoice = goldenMan.GetComponent<Voice>();

        // Disable the completion object initially
        if (completionObject != null)
        {
            completionObject.SetActive(false);
        }
    }

    void Update()
    {
        if (currentSymbolIndex < symbols.Count)
        {
            CheckColliders();
        }
    }

    void CheckColliders()
    {
        GameObject currentSymbol = symbols[currentSymbolIndex];
        SymbolColliderGroup colliderGroup = currentSymbol.GetComponentInChildren<SymbolColliderGroup>();

        if (colliderGroup != null && colliderGroup.IsCompletionThresholdReached(completionThreshold))
        {
            EndSymbolTask();
            currentSymbolIndex++;
            if (currentSymbolIndex < symbols.Count)
            {
                PlayNextDialogue();
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

    void PlayNextDialogue()
    {
        if (goldenManVoice != null)
        {
            goldenManVoice.PlayVoice();
            StartSymbolTask();
        }
    }

    void EnableTaskWithTween(GameObject task)
    {
        task.SetActive(true);
        task.transform.localScale = Vector3.zero;
        LeanTween.scale(task, Vector3.one, taskScaleDuration).setEase(LeanTweenType.easeInOutSine);
    }

    void DisableCurrentTask()
    {
        if (currentSymbolIndex - 1 >= 0 && currentSymbolIndex - 1 < symbols.Count)
        {
            symbols[currentSymbolIndex - 1].SetActive(false);
        }
    }

    void UpdateSymbolVisibility()
    {
        for (int i = 0; i < symbols.Count; i++)
        {
            symbols[i].SetActive(i == currentSymbolIndex);
        }
    }

    public void StartSymbolTask()
    {
        if (currentSymbolIndex < symbols.Count)
        {
            EnableTaskWithTween(symbols[currentSymbolIndex]);
        }
    }

    public void EndSymbolTask()
    {
        DisableCurrentTask();
    }
}
