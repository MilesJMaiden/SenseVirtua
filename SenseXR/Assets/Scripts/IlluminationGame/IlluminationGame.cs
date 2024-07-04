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
    public int zonesToIlluminate = 3; // Number of zones to illuminate
    public int artifactsToInteract = 5; // Number of artifacts to interact with in each zone
    public float tweenDuration = 1.0f; // Duration for tweens
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine; // Type of tween for animations

    private int illuminatedZonesCount = 0; // Counter for illuminated zones
    private int interactedArtifactsCount = 0; // Counter for interacted artifacts
    private Voice goldenManVoice; // Reference to the Golden Man's Voice component
    private bool allZonesCompleted = false; // Flag to track if all zones are completed

    void Start()
    {
        // Initialize the Golden Man's voice component
        goldenManVoice = goldenMan.GetComponent<Voice>();

        // Initialize each zone
        foreach (Zone zone in zones)
        {
            if (zone != null)
            {
                zone.Initialize(this, tweenDuration, tweenType);
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

        // Subscribe to the Golden Man's dialogue end event
        goldenManVoice.OnDialogueEnd += OnGoldenManDialogueEnd;
    }

    void OnDestroy()
    {
        // Unsubscribe from the Golden Man's dialogue end event
        goldenManVoice.OnDialogueEnd -= OnGoldenManDialogueEnd;
    }

    public void OnZoneIlluminated()
    {
        // Increment the illuminated zones counter
        illuminatedZonesCount++;
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
    }

    public void OnArtifactInteracted()
    {
        // Increment the interacted artifacts counter
        interactedArtifactsCount++;
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

            LeanTween.move(portal, portalEndTransform.position, tweenDuration).setEase(tweenType).setOnComplete(() =>
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
            });
        }
    }

    private void ReturnToGoldenMan()
    {
        // Trigger the Golden Man's voice
        goldenManVoice.PlayVoice();
    }

    private void OnGoldenManDialogueEnd()
    {
        // Check if all zones are completed and finalize the game
        if (allZonesCompleted)
        {
            CompleteGame();
        }
    }

    private void CompleteGame()
    {
        // Activate and scale the render plane
        if (renderPlane != null)
        {
            renderPlane.SetActive(true);
            LeanTween.scale(renderPlane, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
        }
    }

    // Inspector buttons for debugging
    [ContextMenu("Toggle Zone 1 Completion")]
    public void ToggleZone1Completion()
    {
        ToggleZoneCompletion(0);
    }

    [ContextMenu("Toggle Zone 2 Completion")]
    public void ToggleZone2Completion()
    {
        ToggleZoneCompletion(1);
    }

    [ContextMenu("Toggle Zone 3 Completion")]
    public void ToggleZone3Completion()
    {
        ToggleZoneCompletion(2);
    }

    [ContextMenu("Toggle Zone 4 Completion")]
    public void ToggleZone4Completion()
    {
        ToggleZoneCompletion(3);
    }

    [ContextMenu("Toggle Zone 5 Completion")]
    public void ToggleZone5Completion()
    {
        ToggleZoneCompletion(4);
    }

    private void ToggleZoneCompletion(int zoneIndex)
    {
        if (zoneIndex >= 0 && zoneIndex < zones.Count)
        {
            Zone zone = zones[zoneIndex];
            if (zone.IsIlluminated())
            {
                zone.ResetIllumination();
                OnZoneReset();
            }
            else
            {
                zone.Illuminate();
                OnZoneIlluminated();
            }
        }
    }

    public void PlaceLanternInFinalTrigger(GameObject lantern)
    {
        if (allZonesCompleted)
        {
            LeanTween.move(lantern, lanternFinalStartPosition.position, tweenDuration).setEase(tweenType).setOnComplete(() =>
            {
                LeanTween.move(lantern, lanternFinalEndPosition.position, tweenDuration).setEase(tweenType).setOnComplete(() =>
                {
                    CompleteGame();
                    lantern.SetActive(false);
                });
            });
        }
    }
}