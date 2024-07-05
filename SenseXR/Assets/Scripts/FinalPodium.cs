using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FinalPodium : MonoBehaviour
{
    public Transform lanternStartPosition;
    public Transform lanternEndPosition;
    public Collider podiumCollider;
    public float tweenDuration = 1.0f;
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine;
    public GameObject renderPlane;
    public Light completionLight;
    public float completionLightEndRange = 10f; // End range for the completion light
    private Component[] lanternComponents;

    private void Awake()
    {
        if (podiumCollider == null)
        {
            podiumCollider = GetComponent<Collider>();
        }
    }

    public void EnableFinalPodium()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lantern"))
        {
            lanternComponents = other.GetComponentsInChildren<Component>();
            PlaceLanternAtStartPosition(other.gameObject);
        }
    }

    private void PlaceLanternAtStartPosition(GameObject lantern)
    {
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
            ExecuteFinalRenderSceneTween();
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

    private void ExecuteFinalRenderSceneTween()
    {
        if (renderPlane != null)
        {
            renderPlane.SetActive(true); // Enable the render plane
            LeanTween.scale(renderPlane, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
        }

        if (completionLight != null)
        {
            LeanTween.value(completionLight.gameObject, completionLight.range, completionLightEndRange, tweenDuration)
                .setEase(tweenType)
                .setOnUpdate((float value) =>
                {
                    completionLight.range = value;
                });
        }
    }
}
