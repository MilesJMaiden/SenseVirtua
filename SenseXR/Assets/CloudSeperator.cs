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
    public float movementSpeed = 1.0f; // Speed of the movement

    private bool movementEnabled = true; // Boolean to enable/disable movement

    void Update()
    {
        if (movementEnabled)
        {
            float distance = Vector3.Distance(player.position, setPoint.position);
            float t = Mathf.Clamp01(distance / Vector3.Distance(object1StartPos.position, object1EndPos.position));

            object1.position = Vector3.Lerp(object1StartPos.position, object1EndPos.position, t * movementSpeed * Time.deltaTime);
            object2.position = Vector3.Lerp(object2StartPos.position, object2EndPos.position, t * movementSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movementEnabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movementEnabled = true;
        }
    }
}
