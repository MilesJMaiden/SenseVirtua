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

    private int currentSymbolIndex = 0;
    private List<GameObject> symbols = new List<GameObject>();
    public bool calligraphyGameCompleted = false;

    void Start()
    {
        // Initialize the symbols list
        foreach (Transform child in drawTasks.transform)
        {
            symbols.Add(child.gameObject);
        }

        // Ensure only the first symbol is active initially
        UpdateSymbolVisibility();
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

    IEnumerator PlayNextDialogue()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
            yield return new WaitUntil(() => playableDirector.state != PlayState.Playing);
        }
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
