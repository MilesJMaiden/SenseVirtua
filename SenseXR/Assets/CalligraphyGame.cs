using UnityEngine;
using System.Collections.Generic;

public class CalligraphyGame : MonoBehaviour
{
    [Tooltip("The parent GameObject containing all symbol tasks.")]
    public GameObject drawTasks;

    [Tooltip("The percentage of colliders that need to be triggered to consider a task complete.")]
    [Range(0, 1)]
    public float completionThreshold = 0.8f;

    private int currentSymbolIndex = 0;
    private List<GameObject> symbols = new List<GameObject>();

    void Start()
    {
        // Initialize the symbols list by gathering all children of drawTasks
        foreach (Transform child in drawTasks.transform)
        {
            symbols.Add(child.gameObject);
        }

        // Ensure only the first symbol is active initially
        UpdateSymbolVisibility();
    }

    void Update()
    {
        // Check colliders only if there are more symbols to process
        if (currentSymbolIndex < symbols.Count)
        {
            CheckColliders();
        }
    }

    void CheckColliders()
    {
        // Get the current symbol and its collider group component
        GameObject currentSymbol = symbols[currentSymbolIndex];
        SymbolColliderGroup colliderGroup = currentSymbol.GetComponentInChildren<SymbolColliderGroup>();

        // Check if the completion threshold is reached
        if (colliderGroup != null && colliderGroup.IsCompletionThresholdReached(completionThreshold))
        {
            currentSymbolIndex++;
            UpdateSymbolVisibility();
        }
    }

    void UpdateSymbolVisibility()
    {
        // Enable the current symbol and disable all others
        for (int i = 0; i < symbols.Count; i++)
        {
            symbols[i].SetActive(i == currentSymbolIndex);
        }
    }
}
