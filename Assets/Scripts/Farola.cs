using UnityEngine;
using System.Collections.Generic;

public class Farola : MonoBehaviour
{
    [Header("Configuración")]
    public bool lightsOn = false;
    public Farola[] nextFarola;
    public float fadeDuration = 2f;
    public bool TurnOff = false;

    [Header("Estado Interno")]
    private Light[] lights;
    private Dictionary<Light, float> baseIntensities = new Dictionary<Light, float>();
    private float fadeTimer = 0f;
    private bool isFading = false;
    private float targetWeight = 0f;
    private float startWeight = 0f;

    void Awake()
    {
        lights = GetComponentsInChildren<Light>();

        foreach (Light l in lights)
        {
            baseIntensities[l] = l.intensity;

            if (!lightsOn)
                l.intensity = 0;
            else
                l.intensity = baseIntensities[l];
        }
    }

    void Update()
    {
        if (!isFading) return;

        fadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(fadeTimer / fadeDuration);

        float lerpVal = Mathf.SmoothStep(startWeight, targetWeight, t);

        foreach (Light l in lights)
        {
            l.intensity = baseIntensities[l] * lerpVal;
        }

        if (fadeTimer >= fadeDuration)
        {
            isFading = false;
            lightsOn = (targetWeight > 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && nextFarola[0] != null)
        {
            foreach (var b in nextFarola)
            {
                b.SetLights(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && nextFarola[0] != null && TurnOff)
        {
            foreach (var b in nextFarola)
            {
                b.SetLights(false);
            }
        }
    }

    public void SetLights(bool turnOn)
    {
        if (turnOn == lightsOn && !isFading) return;

        isFading = true;
        fadeTimer = 0f;

        if (lights.Length > 0 && baseIntensities[lights[0]] > 0)
            startWeight = lights[0].intensity / baseIntensities[lights[0]];
        else
            startWeight = turnOn ? 0f : 1f;

        targetWeight = turnOn ? 1f : 0f;
    }
}
