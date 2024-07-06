using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Zone : MonoBehaviour
{
    public GameObject zoneObject; // The zone GameObject
    public Light pointLight; // The light component for the zone
    public Transform lanternStartPosition; // Start position for the lantern
    public Transform lanternEndPosition; // End position for the lantern
    public Transform finalPoint; // Final point for the lantern to move to
    public Collider podiumCollider; // Reference to the podium collider
    public float startingIlluminationRange = 0f; // Initial range of the light
    public float targetIlluminationRange = 10f; // Target range of the light
    public float tweenDuration = 1.0f; // Duration for tweens
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine; // Type of tween for animations

    public IlluminationGame illuminationGame; // Reference to the IlluminationGame
    public AudioSource audioSource; // Public reference to be set in the inspector
    public XRGrabInteractable artifactInteractable; // XRGrabInteractable component for the artifact

    private bool isIlluminated = false; // Flag to check if the zone is illuminated
    private bool artifactInteracted = false; // Flag to check if the artifact is interacted with
    private Component[] lanternComponents;

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

    public void PlaceLanternAtStartPosition(GameObject lantern)
    {
        lanternComponents = lantern.GetComponentsInChildren<Component>();
        EnableDisableLanternComponents(false);

        LTSeq seq = LeanTween.sequence();
        seq.append(LeanTween.move(lantern, lanternStartPosition.position, tweenDuration).setEase(tweenType));
        seq.append(LeanTween.rotate(lantern, lanternStartPosition.rotation.eulerAngles, tweenDuration).setEase(tweenType));
        seq.append(() =>
        {
            PlaceLanternAtEndPosition(lantern);
        });
    }

    private void PlaceLanternAtEndPosition(GameObject lantern)
    {
        LTSeq seq = LeanTween.sequence();
        seq.append(LeanTween.move(lantern, lanternEndPosition.position, tweenDuration).setEase(tweenType));
        seq.append(LeanTween.rotate(lantern, lanternEndPosition.rotation.eulerAngles, tweenDuration).setEase(tweenType));
        seq.append(() =>
        {
            EnableDisableLanternComponents(false); // Disable components after reaching end position
            podiumCollider.enabled = false;
            Illuminate();
        });
    }

    private void EnableDisableLanternComponents(bool enable)
    {
        foreach (var component in lanternComponents)
        {
            if (component is MeshRenderer)
                continue;

            if (component is Behaviour behaviour)
            {
                behaviour.enabled = enable;
            }
            else if (component is Collider collider)
            {
                collider.enabled = enable;
            }
        }
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
            GameObject lantern = args.interactableObject.transform.gameObject;

            LTSeq seq = LeanTween.sequence();
            seq.append(LeanTween.move(lantern, finalPoint.position, tweenDuration).setEase(tweenType));
            seq.append(LeanTween.rotate(lantern, finalPoint.rotation.eulerAngles, tweenDuration).setEase(tweenType));
            seq.append(() =>
            {
                EnableDisableLanternComponents(true); // Re-enable components after returning to start position
                args.interactableObject.transform.GetComponent<XRGrabInteractable>().enabled = true;
                illuminationGame.OnArtifactInteracted();
            });
        }
    }
}
