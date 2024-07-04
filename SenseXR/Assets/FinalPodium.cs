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
    private BoxCollider[] lanternColliders;

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
            lanternColliders = other.GetComponentsInChildren<BoxCollider>();
            PlaceLanternAtStartPosition(other.gameObject);
        }
    }

    private void PlaceLanternAtStartPosition(GameObject lantern)
    {
        foreach (var collider in lanternColliders)
        {
            collider.enabled = false;
        }

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
            foreach (var collider in lanternColliders)
            {
                collider.enabled = true;
            }

            podiumCollider.enabled = false;
            ExecuteFinalRenderSceneTween();
        });
    }

    private void ExecuteFinalRenderSceneTween()
    {
        if (renderPlane != null)
        {
            renderPlane.SetActive(true); // Enable the render plane
            LeanTween.scale(renderPlane, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
        }
    }
}
