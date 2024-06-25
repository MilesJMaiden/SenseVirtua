using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;

public class CalligraphyGame : MonoBehaviour
{
    [Tooltip("The parent GameObject containing all symbol tasks.")]
    public GameObject drawTasks;

    [Tooltip("The GameObject representing GoldenMan.")]
    public GameObject goldenMan;

    [Tooltip("The PlayableDirector for controlling the timeline.")]
    public PlayableDirector playableDirector;

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

        // Start the initial dialogue
        StartCoroutine(PlayInitialDialogue());
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
            currentSymbolIndex++;
            if (currentSymbolIndex < symbols.Count)
            {
                StartCoroutine(PlayNextDialogue());
            }
            else
            {
                calligraphyGameCompleted = true;
            }
            UpdateSymbolVisibility();
        }
    }

    IEnumerator PlayInitialDialogue()
    {
        if (goldenManVoice != null)
        {
            goldenManVoice.PlayVoice();
            yield return new WaitUntil(() => !goldenManVoice.playing);

            // Enable the first task with a scale tween
            if (symbols.Count > 0)
            {
                EnableTaskWithTween(symbols[0]);
            }
        }
    }

    IEnumerator PlayNextDialogue()
    {
        if (goldenManVoice != null)
        {
            goldenManVoice.PlayVoice();
            yield return new WaitUntil(() => !goldenManVoice.playing);

            // Enable the next task with a scale tween
            if (currentSymbolIndex < symbols.Count)
            {
                EnableTaskWithTween(symbols[currentSymbolIndex]);
            }
        }
    }

    void EnableTaskWithTween(GameObject task)
    {
        task.SetActive(true);
        task.transform.localScale = Vector3.zero;
        LeanTween.scale(task, Vector3.one, taskScaleDuration).setEase(LeanTweenType.easeInOutSine);
    }

    void UpdateSymbolVisibility()
    {
        for (int i = 0; i < symbols.Count; i++)
        {
            symbols[i].SetActive(i == currentSymbolIndex);
        }
    }

    // Methods triggered by the Timeline signals
    public void StartSymbolTask()
    {
        if (currentSymbolIndex < symbols.Count)
        {
            symbols[currentSymbolIndex].SetActive(true);
        }
    }

    public void WaitForDialogue()
    {
        StartCoroutine(WaitForDialogueCoroutine());
    }

    private IEnumerator WaitForDialogueCoroutine()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
            yield return new WaitUntil(() => playableDirector.state != PlayState.Playing);
        }
    }
}
