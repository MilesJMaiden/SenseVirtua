using UnityEngine;
using System.Collections.Generic;

public class SymbolColliderGroup : MonoBehaviour
{
    [Tooltip("List of colliders that need to be triggered.")]
    public List<ColliderStatus> colliders = new List<ColliderStatus>();

    void Start()
    {
        // Initialize the colliders list with all children ColliderStatus components
        colliders.AddRange(GetComponentsInChildren<ColliderStatus>());
    }

    public bool IsCompletionThresholdReached(float threshold)
    {
        // Count the number of triggered colliders
        int triggeredCount = 0;
        foreach (ColliderStatus collider in colliders)
        {
            if (collider.isTriggered)
            {
                triggeredCount++;
            }
        }

        // Calculate the triggered percentage
        float triggeredPercentage = (float)triggeredCount / colliders.Count;
        return triggeredPercentage >= threshold;
    }
}
