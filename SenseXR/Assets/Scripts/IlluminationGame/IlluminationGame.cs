using System.Collections.Generic;
using UnityEngine;

public class IlluminationGame : MonoBehaviour
{
    public List<Zone> zones; // List of zones in the game
    public GameObject lantern; // The lantern GameObject
    public GameObject portal; // The portal GameObject
    public Transform portalStartTransform; // Start transform for the portal
    public Transform portalEndTransform; // End transform for the portal
    public GameObject renderPlane; // The render plane GameObject (formerly specialGameObject)
    public GameObject goldenMan; // The Golden Man GameObject
    public GameObject particleEffect; // Particle effect GameObject
    public AudioSource loopAudioSource; // Looping audio source
    public Transform lanternFinalStartPosition; // Starting position for the lantern on the final completion podium
    public Transform lanternFinalEndPosition; // Final position for the lantern
    public int zonesToIlluminate; // Number of zones to illuminate
    public int artifactsToInteract; // Number of artifacts to interact with in each zone
    public float lanternTweenDuration = 1.0f; // Duration for lantern tweens
    public float portalTweenDuration = 2.0f; // Duration for portal tween
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine; // Type of tween for animations
    public FinalPodium finalPodium; // Reference to the final podium
    public Light completionLight; // Reference to the completion light
    public float completionLightStartRange = 0f; // Start range for the completion light
    public float completionLightEndRange = 10f; // End range for the completion light

    private int illuminatedZonesCount = 0; // Counter for illuminated zones
    private int interactedArtifactsCount = 0; // Counter for interacted artifacts
    private Voice goldenManVoice; // Reference to the Golden Man's Voice component
    private bool allZonesCompleted = false; // Flag to track if all zones are completed

    public AudioSource endingVoice;

    void Start()
    {
        // Initialize the Golden Man's voice component
        //goldenManVoice = goldenMan.GetComponent<Voice>();

        // Initialize each zone
        foreach (Zone zone in zones)
        {
            if (zone != null)
            {
                zone.Initialize(this, lanternTweenDuration, tweenType);
            }
            else
            {
                Debug.LogError("IlluminationGame: Zone is null in the zones list.");
            }
        }

        // Set initial state for portal and render plane
        if (portal != null)
        {
            portal.transform.position = portalStartTransform.position;
            portal.SetActive(false);
        }

        if (renderPlane != null)
        {
            renderPlane.SetActive(false);
        }

        // Deactivate particle effect and loop audio source
        if (particleEffect != null)
        {
            particleEffect.SetActive(false);
        }

        if (loopAudioSource != null)
        {
            loopAudioSource.enabled = false;
        }

        // Set the initial range for the completion light
        if (completionLight != null)
        {
            completionLight.range = completionLightStartRange;
        }

        // Subscribe to the Golden Man's dialogue end event
        //goldenManVoice.OnDialogueEnd += OnGoldenManDialogueEnd;

        // Disable the final podium at the start
        if (finalPodium != null)
        {
            finalPodium.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the Golden Man's dialogue end event
        //goldenManVoice.OnDialogueEnd -= OnGoldenManDialogueEnd;
    }

    public void OnZoneIlluminated()
    {
        // Increment the illuminated zones counter
        illuminatedZonesCount++;
        Debug.Log("IlluminationGame: Zone illuminated. Total illuminated zones: " + illuminatedZonesCount);
        // Check if all zones are illuminated
        if (illuminatedZonesCount >= zonesToIlluminate && !allZonesCompleted)
        {
            allZonesCompleted = true;
            ReturnToGoldenMan();
        }
    }

    public void OnZoneReset()
    {
        // Decrement the illuminated zones counter
        illuminatedZonesCount--;
        Debug.Log("IlluminationGame: Zone reset. Total illuminated zones: " + illuminatedZonesCount);
    }

    public void OnArtifactInteracted()
    {
        // Increment the interacted artifacts counter
        interactedArtifactsCount++;
        Debug.Log("IlluminationGame: Artifact interacted. Total interacted artifacts: " + interactedArtifactsCount);
        // Check if all artifacts are interacted with
        if (interactedArtifactsCount >= zones.Count * artifactsToInteract)
        {
            MovePortal();
        }
    }

    private void MovePortal()
    {
        // Move the portal to the target position and activate effects
        if (portal != null && portalEndTransform != null)
        {
            portal.SetActive(true);
            if (particleEffect != null)
            {
                particleEffect.SetActive(true);
            }

            if (loopAudioSource != null)
            {
                loopAudioSource.enabled = true;
                loopAudioSource.Play();
            }

            LeanTween.move(portal, portalEndTransform.position, portalTweenDuration).setEase(tweenType).setOnComplete(() =>
            {
                if (particleEffect != null)
                {
                    particleEffect.SetActive(false);
                }

                if (loopAudioSource != null)
                {
                    loopAudioSource.Stop();
                    loopAudioSource.enabled = false;
                }

                // Enable the final podium script
                if (finalPodium != null)
                {
                    finalPodium.EnableFinalPodium();
                }
            });
        }
    }

    private void ReturnToGoldenMan()
    {
        // Trigger the Golden Man's voice
        //goldenManVoice.PlayVoice();
        if (allZonesCompleted)
        {
            MovePortal();
            Invoke("EndingVoice", 10f);
            //CompleteGame();
        }
    }

    private void EndingVoice()
    {
        endingVoice.Play();
    }

    private void OnGoldenManDialogueEnd()
    {
        // Check if all zones are completed and finalize the game
        //if (allZonesCompleted)
        //{
        //    CompleteGame();
        //}
    }

    private void CompleteGame()
    {
        // Activate and scale the render plane
        if (renderPlane != null)
        {
            renderPlane.SetActive(true);
            LeanTween.scale(renderPlane, Vector3.one, lanternTweenDuration).setFrom(Vector3.zero).setEase(tweenType);
        }
    }

    // Inspector buttons for debugging
    [ContextMenu("Complete All Zones")]
    public void CompleteAllZones()
    {
        while (illuminatedZonesCount < zonesToIlluminate)
        {
            OnZoneIlluminated();
        }
    }

    [ContextMenu("Trigger Completion Light")]
    public void TriggerCompletionLight()
    {
        CompleteGame();
    }

    [ContextMenu("Trigger Special Game Object")]
    public void TriggerSpecialGameObject()
    {
        if (renderPlane != null)
        {
            renderPlane.SetActive(true);
            LeanTween.scale(renderPlane, Vector3.one, lanternTweenDuration).setFrom(Vector3.zero).setEase(tweenType);
        }
    }

    public void TweenCompletionLight()
    {
        if (completionLight != null)
        {
            LeanTween.value(completionLight.gameObject, completionLightStartRange, completionLightEndRange, lanternTweenDuration)
                .setEase(tweenType)
                .setOnUpdate((float value) =>
                {
                    completionLight.range = value;
                });
        }
    }
}
