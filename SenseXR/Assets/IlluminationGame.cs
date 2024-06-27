using System.Collections.Generic;
using UnityEngine;

public class IlluminationGame : MonoBehaviour
{
    [Tooltip("List of zones in the scene.")]
    public List<Zone> zones;

    [Tooltip("Lantern object the player will carry.")]
    public GameObject lantern;

    [Tooltip("Game object to enable once a set number of zones are illuminated.")]
    public GameObject completionPointLight;

    [Tooltip("Number of zones that need to be illuminated to complete the game.")]
    public int zonesToIlluminate = 3;

    [Tooltip("Tween duration for illuminating the zones.")]
    public float tweenDuration = 1.0f;

    [Tooltip("Type of tween for illuminating the zones.")]
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine;

    private int illuminatedZonesCount = 0;

    void Start()
    {
        // Ensure all zones are initialized correctly
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

        // Ensure the completion point light is disabled initially
        if (completionPointLight != null)
        {
            completionPointLight.SetActive(false);
        }
    }

    // Method to be called when a zone is illuminated
    public void OnZoneIlluminated()
    {
        illuminatedZonesCount++;
        if (illuminatedZonesCount >= zonesToIlluminate)
        {
            EnableCompletionPointLight();
        }
    }

    // Method to enable and tween the completion point light
    private void EnableCompletionPointLight()
    {
        if (completionPointLight != null)
        {
            completionPointLight.SetActive(true);
            LeanTween.scale(completionPointLight, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
        }
    }

    // Context menu methods for toggling zone completion in the editor
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

    // Method to toggle the completion status of a zone
    public void ToggleZoneCompletion(int zoneIndex)
    {
        if (zoneIndex >= 0 && zoneIndex < zones.Count)
        {
            zones[zoneIndex].ToggleIllumination();
        }
    }
}
