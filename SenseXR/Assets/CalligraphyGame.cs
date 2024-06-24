using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;

public class CalligraphyGame : MonoBehaviour
{
    public GameObject drawTasks;
    public GameObject goldenMan; // NPC GameObject for dialogues
    public PlayableDirector playableDirector; // Timeline PlayableDirector
    public float completionThreshold = 0.8f; // Percentage of colliders that need to be triggered

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
