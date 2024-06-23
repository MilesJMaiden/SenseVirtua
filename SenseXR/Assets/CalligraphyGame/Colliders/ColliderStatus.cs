using UnityEngine;

public class ColliderStatus : MonoBehaviour
{
    [Tooltip("Indicates if the collider has been triggered.")]
    public bool isTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is triggered by an object with the "Paintbrush" tag
        if (other.CompareTag("Paintbrush"))
        {
            isTriggered = true;
        }
    }
}
