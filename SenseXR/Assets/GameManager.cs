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
    public float chantVolumeIncreaseDuration = 3.0f;
    public float waitTimerDuration = 5.0f;

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
        StartCoroutine(IncreaseChantVolume());
    }

    private IEnumerator IncreaseChantVolume()
    {
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
        LeanTween.scale(charm, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType).setOnComplete(() =>
        {
            charm.GetComponent<XRGrabInteractable>().enabled = true;
        });
    }

    public void OnCharmGrabbed()
    {
        fadeScreen.FadeOut();
    }

    public void OnBellHit()
    {
        StartCoroutine(ScaleDownHammerAndBell());
        goldenManVoice.PlayVoice();

        EnableCharm();
    }

    private IEnumerator ScaleDownHammerAndBell()
    {
        LeanTween.scale(hammer, Vector3.zero, tweenDuration).setEase(tweenType).setOnComplete(() => hammer.SetActive(false));
        LeanTween.scale(bell, Vector3.zero, tweenDuration).setEase(tweenType).setOnComplete(() => bell.SetActive(false));
        yield return new WaitForSeconds(tweenDuration);
    }
}
