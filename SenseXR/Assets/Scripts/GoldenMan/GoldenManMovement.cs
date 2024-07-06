using UnityEngine;

public class GoldenManMovement : MonoBehaviour
{
    // Public variables for start and end positions
    public Transform startPoint;
    public Transform endPoint;

    // Duration of the float up and down animation
    public float floatDuration = 2.0f;

    // Private variable to store the initial position
    private Vector3 initialPosition;

    void Start()
    {
        // Set the initial position
        initialPosition = transform.position;

        // Start the floating animation
        StartFloating();
    }

    void StartFloating()
    {
        // Move to the end point
        LeanTween.move(gameObject, endPoint.position, floatDuration).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
        {
            // Move back to the start point
            LeanTween.move(gameObject, startPoint.position, floatDuration).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                // Loop the floating animation
                StartFloating();

            });
        });
    }

    void OnDisable()
    {
        // Cancel all tweens when the object is disabled
        LeanTween.cancel(gameObject);
    }
}
