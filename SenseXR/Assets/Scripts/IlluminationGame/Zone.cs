using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Zone : MonoBehaviour
{
    public GameObject zoneObject; // The zone GameObject
    public Light pointLight; // The light component for the zone
    public Transform lanternStartPosition; // Start position for the lantern
    public Transform lanternEndPosition; // End position for the lantern
    public Collider podiumCollider; // Reference to the podium collider
    public float startingIlluminationRange = 0f; // Initial range of the light
    public float targetIlluminationRange = 10f; // Target range of the light
    public float tweenDuration = 1.0f; // Duration for tweens
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine; // Type of tween for animations

    public IlluminationGame illuminationGame; // Reference to the IlluminationGame
    public AudioSource audioSource; // Public reference to be set in the inspector
    public XRGrabInteractable artifactInteractable; // XRGrabInteractable component for the artifact

    private XRGrabInteractable lanternInteractable; // XRGrabInteractable component for the lantern
    private bool isIlluminated = false; // Flag to check if the zone is illuminated
    private bool artifactInteracted = false; // Flag to check if the artifact is interacted with

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lantern") && other == podiumCollider)
        {
            PlaceLantern(other.gameObject);
        }
    }

    public void PlaceLantern(GameObject lantern)
    {
        lanternInteractable = lantern.GetComponent<XRGrabInteractable>();
        if (lanternInteractable != null)
        {
            lanternInteractable.enabled = false;
            LeanTween.move(lantern, lanternEndPosition.position, tweenDuration).setEase(tweenType).setOnComplete(() =>
            {
                Illuminate();
            });
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

        if (!artifactInteracted)
        {
            artifactInteracted = true;
            LeanTween.move(lanternInteractable.gameObject, lanternStartPosition.position, tweenDuration).setEase(tweenType).setOnComplete(() =>
            {
                lanternInteractable.enabled = true;
                illuminationGame.OnArtifactInteracted();
            });
        }
    }
}