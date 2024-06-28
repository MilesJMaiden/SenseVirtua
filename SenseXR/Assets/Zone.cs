using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Zone : MonoBehaviour
{
    public GameObject zoneObject;
    public Light pointLight;
    public float startingIlluminationRange = 0f;
    public float targetIlluminationRange = 10f;

    public IlluminationGame illuminationGame;  // Public to allow access from ZoneCollider

    private bool isIlluminated = false;
    private float tweenDuration;
    private LeanTweenType tweenType;

    public AudioSource audioSource;  // Public reference to be set in the inspector
    private XRGrabInteractable artifactInteractable;

    private void Awake()
    {
        artifactInteractable = GetComponentInChildren<XRGrabInteractable>();

        if (artifactInteractable != null)
        {
            artifactInteractable.selectEntered.AddListener(OnArtifactPickedUp);
        }
    }

    public void Initialize(IlluminationGame game, float duration, LeanTweenType type)
    {
        illuminationGame = game;
        tweenDuration = duration;
        tweenType = type;
        pointLight.range = startingIlluminationRange;

        if (illuminationGame == null)
        {
            Debug.LogError("Zone: IlluminationGame reference is not set.");
        }
    }

    public void Illuminate()
    {
        if (!isIlluminated)
        {
            isIlluminated = true;
            LeanTween.value(pointLight.gameObject, UpdateLightRange, pointLight.range, targetIlluminationRange, tweenDuration).setEase(tweenType);
            illuminationGame.OnZoneIlluminated();
        }
    }

    public void ResetIllumination()
    {
        if (isIlluminated)
        {
            isIlluminated = false;
            LeanTween.value(pointLight.gameObject, UpdateLightRange, pointLight.range, startingIlluminationRange, tweenDuration).setEase(tweenType);
            illuminationGame.OnZoneReset();
        }
    }

    public bool IsIlluminated()
    {
        return isIlluminated;
    }

    private void UpdateLightRange(float value)
    {
        pointLight.range = value;
    }

    private void OnArtifactPickedUp(SelectEnterEventArgs args)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
