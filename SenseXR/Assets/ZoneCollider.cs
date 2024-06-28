using UnityEngine;

public class ZoneCollider : MonoBehaviour
{
    private Zone zone;

    void Start()
    {
        zone = GetComponentInParent<Zone>();
        if (zone == null)
        {
            Debug.LogError("ZoneCollider: Zone component not found in parent.");
        }
        if (zone != null && zone.illuminationGame == null)
        {
            Debug.LogError("ZoneCollider: IlluminationGame reference not set in Zone component.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (zone != null && zone.illuminationGame != null && other.gameObject == zone.illuminationGame.lantern)
        {
            zone.Illuminate();
        }
    }
}
