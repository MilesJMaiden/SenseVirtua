using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminationGame : MonoBehaviour
{
    public List<Zone> zones;
    public GameObject lantern;
    public GameObject completionPointLightObject;
    public Light completionPointLight;
    public GameObject specialGameObject;
    public GameObject goldenMan;
    public int zonesToIlluminate = 3;
    public float tweenDuration = 1.0f;
    public LeanTweenType tweenType = LeanTweenType.easeInOutSine;
    public float completionLightStartRange = 0f;
    public float completionLightTargetRange = 10f;

    private int illuminatedZonesCount = 0;
    private Voice goldenManVoice;
    private bool allZonesCompleted = false;

    void Start()
    {
        goldenManVoice = goldenMan.GetComponent<Voice>();

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

        if (completionPointLightObject != null)
        {
            completionPointLightObject.SetActive(false);
        }

        if (specialGameObject != null)
        {
            specialGameObject.SetActive(false);
        }

        if (completionPointLight != null)
        {
            completionPointLight.range = completionLightStartRange;
        }

        goldenManVoice.OnDialogueEnd += OnGoldenManDialogueEnd;
    }

    void OnDestroy()
    {
        goldenManVoice.OnDialogueEnd -= OnGoldenManDialogueEnd;
    }

    public void OnZoneIlluminated()
    {
        illuminatedZonesCount++;
        if (illuminatedZonesCount >= zonesToIlluminate && !allZonesCompleted)
        {
            allZonesCompleted = true;
            ReturnToGoldenMan();
        }
    }

    public void OnZoneReset()
    {
        illuminatedZonesCount--;
    }

    private void ReturnToGoldenMan()
    {
        goldenManVoice.PlayVoice();
    }

    private void OnGoldenManDialogueEnd()
    {
        if (allZonesCompleted)
        {
            CompleteGame();
        }
    }

    private void CompleteGame()
    {
        if (completionPointLightObject != null)
        {
            completionPointLightObject.SetActive(true);
            LeanTween.scale(completionPointLightObject, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
        }

        if (completionPointLight != null)
        {
            LeanTween.value(completionPointLight.gameObject, completionLightStartRange, completionLightTargetRange, tweenDuration)
                     .setEase(tweenType)
                     .setOnUpdate((float value) => { completionPointLight.range = value; });
        }

        if (specialGameObject != null)
        {
            specialGameObject.SetActive(true);
            LeanTween.scale(specialGameObject, Vector3.one, tweenDuration).setFrom(Vector3.zero).setEase(tweenType);
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
}
