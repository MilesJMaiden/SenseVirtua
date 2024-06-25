using UnityEngine;
using UnityEditor;

public class HighlightAid : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer;
    public Color colorOne = new Color(1, 1, 1, 0); // White with 0 alpha
    public Color colorTwo = new Color(1, 1, 1, 1); // White with full alpha

    public float lerpDuration = 1.0f;
    public LeanTweenType lerpEaseType = LeanTweenType.easeInOutSine;

    public bool enableLerp = true;

    private bool isLerping = false;
    private LTDescr currentTween;

    void Start()
    {
        if (targetSpriteRenderer == null)
        {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (enableLerp && !isLerping)
        {
            StartLerping();
        }
        else if (!enableLerp && isLerping)
        {
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

    public void FadeIn()
    {
        if (targetSpriteRenderer != null)
        {
            LeanTween.value(gameObject, colorOne, colorTwo, lerpDuration)
                .setEase(lerpEaseType)
                .setOnUpdate((Color color) => targetSpriteRenderer.color = color);
        }
    }

    public void FadeOut()
    {
        if (targetSpriteRenderer != null)
        {
            LeanTween.value(gameObject, colorTwo, colorOne, lerpDuration)
                .setEase(lerpEaseType)
                .setOnUpdate((Color color) => targetSpriteRenderer.color = color)
                .setOnComplete(() => gameObject.SetActive(false));
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
        if (GUILayout.Button("Fade In"))
        {
            script.FadeIn();
        }
        if (GUILayout.Button("Fade Out"))
        {
            script.FadeOut();
        }
    }
}
#endif
