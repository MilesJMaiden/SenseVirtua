using UnityEngine;

public class CloudSeparator : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Transform setPoint; // The set location transform
    public Transform object1; // The first object to move
    public Transform object2; // The second object to move
    public Transform object1StartPos; // The start position of the first object
    public Transform object1EndPos; // The end position of the first object
    public Transform object2StartPos; // The start position of the second object
    public Transform object2EndPos; // The end position of the second object

    private bool movementEnabled = true; // Boolean to enable/disable movement

    void Update()
    {
        if (movementEnabled)
        {
            float distance = Vector3.Distance(player.position, setPoint.position);
            float maxDistance = Vector3.Distance(object1EndPos.position, object1StartPos.position); // Max distance for normalization
            float t = Mathf.Clamp01(distance / maxDistance);

            object1.position = Vector3.Lerp(object1EndPos.position, object1StartPos.position, t);
            object2.position = Vector3.Lerp(object2EndPos.position, object2StartPos.position, t);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (movementEnabled && other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger collider. Disabling movement.");
            movementEnabled = false;
            DisableObjects();
        }
    }

    private void DisableObjects()
    {
        object1.gameObject.SetActive(false);
        object2.gameObject.SetActive(false);
        Debug.Log("Objects disabled. Movement is now permanently disabled.");
    }
}
