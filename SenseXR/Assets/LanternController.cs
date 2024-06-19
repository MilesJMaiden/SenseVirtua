using UnityEngine;

public class LanternController : MonoBehaviour
{
    [Header("Light Settings")]
    public Light lanternLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float intensityFlickerSpeed = 0.1f;

    public float minRange = 5f;
    public float maxRange = 7f;
    public float rangeFlickerSpeed = 0.1f;

    [Header("Particle System Settings")]
    //public ParticleSystem flameParticleSystem;

    private float targetIntensity;
    private float targetRange;
    private float intensityFlickerTimer;
    private float rangeFlickerTimer;

    void Start()
    {
        if (lanternLight == null)
        {
            Debug.LogError("Lantern Light not assigned.");
            return;
        }

        //if (flameParticleSystem == null)
        //{
        //    Debug.LogError("Flame Particle System not assigned.");
        //    return;
        //}

        targetIntensity = lanternLight.intensity;
        targetRange = lanternLight.range;
        intensityFlickerTimer = intensityFlickerSpeed;
        rangeFlickerTimer = rangeFlickerSpeed;
    }

    void Update()
    {
        FlickerLight();
    }

    void FlickerLight()
    {
        if (intensityFlickerTimer <= 0)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            intensityFlickerTimer = intensityFlickerSpeed;
        }

        if (rangeFlickerTimer <= 0)
        {
            targetRange = Random.Range(minRange, maxRange);
            rangeFlickerTimer = rangeFlickerSpeed;
        }

        lanternLight.intensity = Mathf.Lerp(lanternLight.intensity, targetIntensity, Time.deltaTime * intensityFlickerSpeed);
        lanternLight.range = Mathf.Lerp(lanternLight.range, targetRange, Time.deltaTime * rangeFlickerSpeed);

        intensityFlickerTimer -= Time.deltaTime;
        rangeFlickerTimer -= Time.deltaTime;
    }
}
