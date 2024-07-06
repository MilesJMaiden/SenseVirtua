using UnityEngine;

public class BellHit : MonoBehaviour
{
    public AudioClip bellSound;
    public GameObject bell;
    public GameObject hammer;
    public float tweenDuration = 1.0f;
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            PlayBellSound(collision.contacts[0].point);

            //Wait time??
            LeanTween.scale(hammer, Vector3.zero, tweenDuration).setEase(tweenType).setOnComplete(() =>
            {
                hammer.SetActive(false);
            });
            LeanTween.scale(bell, Vector3.zero, tweenDuration).setEase(tweenType).setOnComplete(() =>
            {
                bell.SetActive(false);
            });
        }
    }

    private void PlayBellSound(Vector3 hitPosition)
    {
        GameObject audioObject = new GameObject("BellSound");
        audioObject.transform.position = hitPosition;
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = bellSound;
        audioSource.Play();
        Destroy(audioObject, bellSound.length);
    }
}
