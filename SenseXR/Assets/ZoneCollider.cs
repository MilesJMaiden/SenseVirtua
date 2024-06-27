using UnityEngine;

public class ZoneCollider : MonoBehaviour
{
    private ObjectiveZone objectiveZone;

    void Start()
    {
        objectiveZone = GetComponentInParent<ObjectiveZone>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == objectiveZone.illuminationGame.lantern)
        {
            objectiveZone.Illuminate();
        }
    }
}
