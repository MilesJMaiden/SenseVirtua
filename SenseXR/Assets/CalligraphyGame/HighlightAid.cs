using UnityEngine;
using UnityEditor;

public class HighlightAid : MonoBehaviour
{
    // Public reference to the SpriteRenderer and colors
    public SpriteRenderer targetSpriteRenderer;
    public Color colorOne = new Color(1, 1, 1, 0); // White with 0 alpha
    public Color colorTwo = new Color(1, 1, 1, 1); // White with full alpha

    // Duration of the lerp
    public float lerpDuration = 1.0f;

    // Easing type for the lerp
    public LeanTweenType lerpEaseType = LeanTweenType.easeInOutSine;

    // Public toggle for enabling/disabling the lerp
    public bool enableLerp = true;

    private bool isLerping = false;
    private LTDescr currentTween;

    void Start()
    {
        // Ensure the sprite renderer is set
        if (targetSpriteRenderer == null)
        {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Start the lerping process if enabled
        if (enableLerp)
        {
            StartLerping();
        }
    }

    void Update()
    {
        // Check if the lerp toggle has changed
        if (enableLerp && !isLerping)
        {
            // Start lerping if it's not already active
            StartLerping();
        }
        else if (!enableLerp && isLerping)
        {
            // Stop current tween and lerp to colorOne
            StopLerping();
        }
    }

    [ContextMenu("Start Lerping")]
    public void StartLerping()
    {
        if (targetSpriteRenderer != null)
        {
            enableLerp = true;
            isLerping = true;
            currentTween = LeanTween.value(gameObject, 0, 1, lerpDuration).setEase(lerpEaseType).setLoopPingPong().setOnUpdate(UpdateColor);
        }
    }

    [ContextMenu("Stop Lerping")]
    public void StopLerping()
    {
        if (targetSpriteRenderer != null)
        {
            if (currentTween != null)
            {
                LeanTween.cancel(currentTween.id);
            }
            isLerping = false;
            enableLerp = false;
            LeanTween.value(gameObject, targetSpriteRenderer.color, colorOne, lerpDuration)
                .setEase(lerpEaseType)
                .setOnUpdate((Color color) => targetSpriteRenderer.color = color);
        }
    }

    void UpdateColor(float t)
    {
        if (targetSpriteRenderer != null)
        {
            targetSpriteRenderer.color = Color.Lerp(colorOne, colorTwo, t);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(HighlightAid))]
public class HighlightAidEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HighlightAid script = (HighlightAid)target;
        if (GUILayout.Button("Start Lerping"))
        {
            script.StartLerping();
        }
        if (GUILayout.Button("Stop Lerping"))
        {
            script.StopLerping();
        }
    }
}
#endif
