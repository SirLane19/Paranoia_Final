using UnityEngine;
using UnityEngine.UI;

public class SimpleFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    public float minInterval = 0.02f;   // jeda kedip tercepat
    public float maxInterval = 0.1f;    // jeda kedip terlama
    public float flickerIntensity = 0.3f; // seberapa redup pas flick
    public bool randomizePattern = true;  // biar gak ritmis

    private Light lightSource;
    private Image uiLight;
    private float baseIntensity;
    private Color baseColor;
    private float timer;

    void Start()
    {
        lightSource = GetComponent<Light>();
        uiLight = GetComponent<Image>();

        if (lightSource)
            baseIntensity = lightSource.intensity;

        if (uiLight)
            baseColor = uiLight.color;

        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            bool flick = Random.value > 0.5f;

            if (lightSource)
                lightSource.intensity = flick ? baseIntensity : baseIntensity * flickerIntensity;

            if (uiLight)
            {
                Color c = baseColor;
                c.a = flick ? baseColor.a : baseColor.a * flickerIntensity;
                uiLight.color = c;
            }

            ResetTimer();
        }
    }

    void ResetTimer()
    {
        if (randomizePattern)
            timer = Random.Range(minInterval, maxInterval);
        else
            timer = (minInterval + maxInterval) / 2f;
    }
}