using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Voice goldenManVoice;
    public GameObject changeFromMicObject;
    public AudioSource chantAudioSource;
    public GameObject hammer;
    public GameObject bell;
    public GameObject charm;
    public FadeScreen fadeScreen;
    public float tweenDuration = 1.0f;
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine;
    public float chantVolumeIncreaseDuration = 15.0f;
    public float waitTimerDuration = 1.0f;

    private void Start()
    {
        hammer.SetActive(false);
        bell.SetActive(false);
        charm.SetActive(false);
        changeFromMicObject.SetActive(false);

        chantAudioSource.enabled = true; // Enable chantAudioSource on start
        chantAudioSource.volume = 0; // Start with volume at 0
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
        Debug.Log($"OnGoldenManDialogueEnd called. Current index: {goldenManVoice.currentIdx}");
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
            default:
                Debug.Log("All dialogues completed.");
                break;
        }
    }

    private void EnableVoiceInteractor()
    {
        Debug.Log("EnableVoiceInteractor called.");
        StartCoroutine(IncreaseChantVolume());
    }

    private IEnumerator IncreaseChantVolume()
    {
        Debug.Log("Increasing chant volume.");
        float elapsedTime = 0;
        float initialVolume = chantAudioSource.volume;

        while (elapsedTime < chantVolumeIncreaseDuration)
        {
            chantAudioSource.volume = Mathf.Lerp(initialVolume, 1.0f, elapsedTime / chantVolumeIncreaseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chantAudioSource.volume = 1.0f;
        yield return new WaitForSeconds(waitTimerDuration);
        changeFromMicObject.SetActive(true);
        Debug.Log("Voice interactor enabled.");
    }

    public void OnPlayerVoiceInput()
    {
        Debug.Log("Player voice input detected.");
        Invoke("StartNextDialogue", 3f);
    }

    private void StartNextDialogue()
    {
        Debug.Log("Starting next dialogue.");
        goldenManVoice.PlayVoice();
        changeFromMicObject.SetActive(false);
        chantAudioSource.enabled = false;
    }

    private void EnableHammerAndBell()
    {
        Debug.Log("Enabling hammer and bell.");
        hammer.SetActive(true);
        bell.SetActive(true);
        LeanTween.scale(hammer, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
        LeanTween.scale(bell, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
    }

    private void EnableCharm()
    {
        Debug.Log("Enabling charm.");
        charm.SetActive(true);
        LeanTween.scale(charm, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
    }

    public void OnCharmGrabbed()
    {
        Debug.Log("Charm grabbed, fading out.");
        fadeScreen.FadeOut();
    }

    public void OnBellHit()
    {
        Debug.Log("Bell hit, scaling down hammer and bell.");
        StartCoroutine(ScaleDownHammerAndBell());
        goldenManVoice.PlayVoice();
    }

    private IEnumerator ScaleDownHammerAndBell()
    {
        LeanTween.scale(hammer, Vector3.zero, tweenDuration).setEase(tweenType).setOnComplete(() => hammer.SetActive(false));
        LeanTween.scale(bell, Vector3.zero, tweenDuration).setEase(tweenType).setOnComplete(() => bell.SetActive(false));
        yield return new WaitForSeconds(tweenDuration);
    }
}
