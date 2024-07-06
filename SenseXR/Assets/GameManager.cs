using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public Voice goldenManVoice;
    public GameObject changeFromMicObject;
    public AudioSource chantAudioSource;
    public GameObject hammer;
    public GameObject bell;
    public GameObject charm;
    public FadeScreen fadeScreen; // Reference to the FadeScreen component
    public float tweenDuration = 1.0f;
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine;

    private void Start()
    {
        // Disable hammer and bell at start
        hammer.SetActive(false);
        bell.SetActive(false);
        changeFromMicObject.SetActive(false); // Disable ChangeFromMic object at start
        chantAudioSource.enabled = false; // Disable Chant audio source at start
    }

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
        changeFromMicObject.SetActive(true);
        chantAudioSource.enabled = true;
    }

    public void OnPlayerVoiceInput()
    {
        Invoke("StartNextDialogue", 3f);
    }

    private void StartNextDialogue()
    {
        goldenManVoice.PlayVoice();
        changeFromMicObject.SetActive(false);
        chantAudioSource.enabled = false;
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
        {
            LeanTween.scale(charm, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType).setOnComplete(() =>
            {
                charm.GetComponent<XRGrabInteractable>().enabled = true;
            });
        };
    }

    public void OnCharmGrabbed()
    {
        fadeScreen.FadeOut();
    }

    public void OnBellHit()
    {
        goldenManVoice.PlayVoice();
    }
}
