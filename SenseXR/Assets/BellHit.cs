using UnityEngine;

public class BellHitDetection : MonoBehaviour
{
    public GameObject hitAudioPrefab;
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hammer"))
        {
            // Instantiate the hit audio source at the hit position
            GameObject hitAudio = Instantiate(hitAudioPrefab, other.transform.position, Quaternion.identity);
            AudioSource audioSource = hitAudio.GetComponent<AudioSource>();
            audioSource.Play();

            // Destroy the audio source after it has played
            Destroy(hitAudio, audioSource.clip.length);

            // Inform the GameManager that the bell was hit
            gameManager.OnBellHit();
        }
    }
}
