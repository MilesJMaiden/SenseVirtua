using UnityEngine;

public class PodiumCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with collider: " + other.name);
        if (other.CompareTag("Lantern"))
        {
            Zone zone = GetComponentInParent<Zone>();
            if (zone != null)
            {
                Debug.Log("Lantern entered podium collider");
                zone.PlaceLanternAtStartPosition(other.gameObject);
            }
            else
            {
                Debug.LogError("PodiumCollider: Zone not found in parent.");
            }
        }
    }
}