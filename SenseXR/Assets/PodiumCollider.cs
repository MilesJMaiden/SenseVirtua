using UnityEngine;

public class PodiumCollider : MonoBehaviour
{
    public Zone parentZone; // Reference to the parent Zone script

    private void Start()
    {
        if (parentZone == null)
        {
            Debug.LogError("PodiumCollider: Parent Zone reference is not set.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PodiumCollider OnTriggerEnter called with collider: " + other.name);
        if (other.CompareTag("Lantern"))
        {
            Debug.Log("Lantern entered podium collider");
            parentZone.PlaceLantern(other.gameObject);
        }
    }
}
