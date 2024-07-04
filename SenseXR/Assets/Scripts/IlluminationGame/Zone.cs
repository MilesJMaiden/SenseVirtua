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
    public float tweenDuration = 0.1f; // Duration for tweens, set to a lower value for faster transitions
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine; // Type of tween for animations

    public IlluminationGame illuminationGame; // Reference to the IlluminationGame
    public AudioSource audioSource; // Public reference to be set in the inspector
    public XRGrabInteractable artifactInteractable; // XRGrabInteractable component for the artifact

    private XRGrabInteractable lanternInteractable; // Reference to the lantern's XRGrabInteractable component
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
        Debug.Log("OnTriggerEnter called with collider: " + other.name);
        if (other.CompareTag("Lantern"))
        {
            Debug.Log("Lantern entered podium collider");
            PlaceLanternAtStartPosition(other.gameObject);
        }
    }

    public void PlaceLanternAtStartPosition(GameObject lantern)
    {
        Debug.Log("PlaceLanternAtStartPosition called with lantern: " + lantern.name);
        lanternInteractable = lantern.GetComponent<XRGrabInteractable>();
        if (lanternInteractable != null)
        {
            Debug.Log("Disabling XRGrabInteractable on lantern");
            lanternInteractable.enabled = false;

            // Create a sequence for smooth position and rotation tweens
            LTSeq seq = LeanTween.sequence();
            seq.append(LeanTween.move(lantern, lanternStartPosition.position, tweenDuration).setEase(tweenType));
            seq.append(LeanTween.rotate(lantern, lanternStartPosition.rotation.eulerAngles, tweenDuration).setEase(tweenType));
            seq.append(() =>
            {
                PlaceLanternAtEndPosition(lantern);
            });
        }
        else
        {
            Debug.LogError("Lantern does not have an XRGrabInteractable component");
        }
    }

    public void PlaceLanternAtEndPosition(GameObject lantern)
    {
        Debug.Log("PlaceLanternAtEndPosition called with lantern: " + lantern.name);

        // Create a sequence for smooth position and rotation tweens
        LTSeq seq = LeanTween.sequence();
        seq.append(LeanTween.move(lantern, lanternEndPosition.position, tweenDuration).setEase(tweenType));
        seq.append(LeanTween.rotate(lantern, lanternEndPosition.rotation.eulerAngles, tweenDuration).setEase(tweenType));
        seq.append(() =>
        {
            Debug.Log("Lantern moved to end position, calling Illuminate");
            Illuminate();
        });
    }

    public void Illuminate()
    {
        if (!isIlluminated)
        {
            Debug.Log("Illuminating zone");
            isIlluminated = true;
            LeanTween.value(pointLight.gameObject, UpdateLightRange, pointLight.range, targetIlluminationRange, tweenDuration).setEase(tweenType);
            illuminationGame.OnZoneIlluminated();
        }
    }

    public void ResetIllumination()
    {
        if (isIlluminated)
        {
            Debug.Log("Resetting illumination");
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
        Debug.Log("Artifact picked up");
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        if (!artifactInteracted)
        {
            artifactInteracted = true;

            // Create a sequence for smooth position and rotation tweens
            LTSeq seq = LeanTween.sequence();
            seq.append(LeanTween.move(args.interactableObject.transform.gameObject, lanternStartPosition.position, tweenDuration).setEase(tweenType));
            seq.append(LeanTween.rotate(args.interactableObject.transform.gameObject, lanternStartPosition.rotation.eulerAngles, tweenDuration).setEase(tweenType));
            seq.append(() =>
            {
                args.interactableObject.transform.GetComponent<XRGrabInteractable>().enabled = true;
                illuminationGame.OnArtifactInteracted();
            });
        }
    }
}
