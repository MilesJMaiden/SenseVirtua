using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public Voice goldenManVoice;
    public GameObject voiceInteractorPrefab;
    public GameObject hammer;
    public GameObject bell;
    public GameObject charm;
    public Transform charmStartPosition;
    public Transform charmEndPosition;
    public FadeScreen fadeScreen; // Reference to the FadeScreen component
    public float tweenDuration = 1.0f;
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine;

    private void OnEnable()
    {
        goldenManVoice.OnDialogueEnd += OnGoldenManDialogueEnd;
    }

    private void OnDisable()
    {
        goldenManVoice.OnDialogueEnd -= OnGoldenManDialogueEnd;
    }

    private void OnGoldenManDialogueEnd()
    {
        switch (goldenManVoice.currentIdx)
        {
            case 1:
                EnableVoiceInteractor();
                break;
            case 2:
                EnableHammerAndBell();
                break;
            case 3:
                EnableCharm();
                break;
        }
    }

    private void EnableVoiceInteractor()
    {
        voiceInteractorPrefab.SetActive(true);
    }

    public void OnPlayerVoiceInput()
    {
        goldenManVoice.PlayVoice();
        voiceInteractorPrefab.SetActive(false);
    }

    private void EnableHammerAndBell()
    {
        hammer.SetActive(true);
        bell.SetActive(true);
        LeanTween.scale(hammer, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
        LeanTween.scale(bell, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
    }

    private void EnableCharm()
    {
        charm.SetActive(true);
        LeanTween.move(charm, charmStartPosition.position, tweenDuration).setEase(tweenType).setOnComplete(() =>
        {
            LeanTween.scale(charm, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType).setOnComplete(() =>
            {
                charm.GetComponent<XRGrabInteractable>().enabled = true;
            });
        });
    }

    public void OnCharmGrabbed()
    {
        fadeScreen.FadeOut();
    }
}
