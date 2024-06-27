using UnityEngine;

[System.Serializable]
public class ObjectiveZone
{
    public GameObject zoneObject;
    public Light pointLight;
    public float startingIlluminationRange = 0f;
    public float targetIlluminationRange = 10f;

    public IlluminationGame illuminationGame;
    private bool isIlluminated = false;

    public void Initialize(IlluminationGame game)
    {
        illuminationGame = game;
        pointLight.range = startingIlluminationRange;
    }

    public void Illuminate()
    {
        if (!isIlluminated)
        {
            isIlluminated = true;
            LeanTween.value(pointLight.gameObject, UpdateLightRange, pointLight.range, targetIlluminationRange, 1.0f).setEase(LeanTweenType.easeInOutSine);
            illuminationGame.OnZoneIlluminated();
        }
    }

    public void ToggleIllumination()
    {
        if (isIlluminated)
        {
            isIlluminated = false;
            LeanTween.value(pointLight.gameObject, UpdateLightRange, pointLight.range, startingIlluminationRange, 1.0f).setEase(LeanTweenType.easeInOutSine);
        }
        else
        {
            Illuminate();
        }
    }

    private void UpdateLightRange(float value)
    {
        pointLight.range = value;
    }
}
