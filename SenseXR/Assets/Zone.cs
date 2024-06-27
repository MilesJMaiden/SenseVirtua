using UnityEngine;

public class Zone : MonoBehaviour
{
    public GameObject zoneObject;
    public Light pointLight;
    public float startingIlluminationRange = 0f;
    public float targetIlluminationRange = 10f;

    public IlluminationGame illuminationGame;  // Public to allow access from ZoneCollider

    private bool isIlluminated = false;
    private float tweenDuration;
    private LeanTweenType tweenType;

    // Initialize the zone with reference to the illumination game, tween duration, and tween type
    public void Initialize(IlluminationGame game, float duration, LeanTweenType type)
    {
        illuminationGame = game;
        tweenDuration = duration;
        tweenType = type;
        pointLight.range = startingIlluminationRange;

        if (illuminationGame == null)
        {
            Debug.LogError("Zone: IlluminationGame reference is not set.");
        }
    }

    // Illuminate the zone if it's not already illuminated
    public void Illuminate()
    {
        if (!isIlluminated)
        {
            isIlluminated = true;
            LeanTween.value(pointLight.gameObject, UpdateLightRange, pointLight.range, targetIlluminationRange, tweenDuration).setEase(tweenType);
            illuminationGame.OnZoneIlluminated();
        }
    }

    // Toggle the illumination of the zone
    public void ToggleIllumination()
    {
        if (isIlluminated)
        {
            isIlluminated = false;
            LeanTween.value(pointLight.gameObject, UpdateLightRange, pointLight.range, startingIlluminationRange, tweenDuration).setEase(tweenType);
        }
        else
        {
            Illuminate();
        }
    }

    // Update the light range of the point light
    private void UpdateLightRange(float value)
    {
        pointLight.range = value;
    }
}
