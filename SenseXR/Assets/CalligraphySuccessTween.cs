using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class CalligraphySuccessTween : MonoBehaviour
{
    public Transform targetTransform; // Reference to the target transform for final position and rotation
    public float tweenDuration = 2.0f;
    public AudioClip successSound; // Audio clip to play on success

    // Public reference to the SpriteRenderer and colors
    public SpriteRenderer targetSpriteRenderer;
    public Color startColor = Color.white;
    public Color endColor = Color.red;

    // Expose easing types for each step
    public LeanTweenType scaleUpEaseType = LeanTweenType.easeInOutSine;
    public LeanTweenType colorEaseType = LeanTweenType.easeInOutSine;
    public LeanTweenType moveEaseType = LeanTweenType.easeInOutSine;
    public LeanTweenType rotateEaseType = LeanTweenType.easeInOutSine;
    public LeanTweenType finalScaleEaseType = LeanTweenType.easeInOutSine;
    public LeanTweenType hdrIntensityEaseType = LeanTweenType.easeInOutSine;

    private Vector3 initialScale;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial values
        initialScale = transform.localScale;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Set initial scale to 0
        transform.localScale = Vector3.zero;

        // Ensure the sprite renderer is set
        if (targetSpriteRenderer == null)
        {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    [ContextMenu("Execute Tween Sequence")]
    public void ExecuteTweenSequence()
    {
        // Ensure the object is reset before starting the sequence
        ResetObject();
        StepOne();
    }

    [ContextMenu("Reset Object")]
    public void ResetObject()
    {
        // Cancel all tweens and reset the object to initial values
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.zero;
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reset the color
        if (targetSpriteRenderer != null)
        {
            targetSpriteRenderer.color = startColor;
        }
    }

    void StepOne()
    {
        LeanTween.scale(gameObject, initialScale, tweenDuration).setEase(scaleUpEaseType).setOnComplete(StepTwo);
    }

    void StepTwo()
    {
        LeanTween.value(gameObject, 0, 1, tweenDuration).setOnUpdate(UpdateColor).setEase(colorEaseType).setOnComplete(StepThree);
    }

    void StepThree()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = successSound;
        audioSource.PlayOneShot(successSound);

        // Start the color tweening
        LeanTween.value(gameObject, 0, 1, tweenDuration).setLoopPingPong().setEase(hdrIntensityEaseType).setOnUpdate(UpdateColor);

        StepFour();
    }

    void StepFour()
    {
        LeanTween.scale(gameObject, Vector3.one * 2.35f, tweenDuration).setEase(finalScaleEaseType);
        LeanTween.move(gameObject, targetTransform.position, tweenDuration).setEase(moveEaseType);
        LeanTween.rotate(gameObject, targetTransform.rotation.eulerAngles, tweenDuration).setEase(rotateEaseType);
    }

    void UpdateColor(float t)
    {
        if (targetSpriteRenderer != null)
        {
            targetSpriteRenderer.color = Color.Lerp(startColor, endColor, t);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CalligraphySuccessTween))]
public class CalligraphySuccessTweenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CalligraphySuccessTween script = (CalligraphySuccessTween)target;
        if (GUILayout.Button("Execute Tween Sequence"))
        {
            script.ExecuteTweenSequence();
        }
        if (GUILayout.Button("Reset Object"))
        {
            script.ResetObject();
        }
    }
}
#endif
