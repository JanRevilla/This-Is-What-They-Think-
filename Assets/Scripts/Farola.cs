using UnityEngine;

public class Farola : MonoBehaviour
{
    [Header("Elements")]
    public Light[] Lights;
    private BoxCollider Trigger;
    public Farola NextFarola;

    [Header("Atributos")]
    private bool ActivatingLights = false;
    private bool DesactivatingLights = false;
    public bool LightsOn;

    public float MaxLight = 1.327805f;
    public float MaxSpotLight = 104.6128f;
    public float MaxLowLight = 152.5728f;
    public float MaxUpperLight = 417.042f;

    [Header("Timing")]
    public float FadeDuration = 3f;
    private float fadeTimer = 0f;

    void Start()
    {
        Trigger = GetComponent<BoxCollider>();

        if (!LightsOn)
        {
            foreach (var b in Lights)
            {
                b.intensity = 0;
            }
        }
    }

    void Update()
    {
        if (ActivatingLights)
        {
            fadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeTimer / FadeDuration);

            foreach (var b in Lights)
            {
                if (b.name == "SpotLight")
                {
                    b.intensity = Mathf.Lerp(0, MaxSpotLight, t);
                }
                else if (b.name == "Upper")
                {
                    b.intensity = Mathf.Lerp(0, MaxUpperLight, t);
                }
                else if (b.name == "Low")
                {
                    b.intensity = Mathf.Lerp(0, MaxLowLight, t);
                }
                else
                {
                    b.intensity = Mathf.Lerp(0, MaxLight, t);
                }    
            }

            if (fadeTimer >= FadeDuration)
            {
                ActivatingLights = false;
                LightsOn = true;
                fadeTimer = 0f;
            }
        }

        if (DesactivatingLights)
        {
            fadeTimer += Time.deltaTime;
            float t = fadeTimer / FadeDuration;

            foreach (var b in Lights)
            {
                if (b.name == "SpotLight")
                {
                    b.intensity = Mathf.Lerp(MaxSpotLight, 0, t);
                }
                else if (b.name == "Low")
                {
                    b.intensity = Mathf.Lerp(MaxLowLight, 0, t);
                }
                else if (b.name == "Upper")
                {
                    b.intensity = Mathf.Lerp(MaxUpperLight, 0, t);
                }
                else
                {
                    b.intensity = Mathf.Lerp(MaxLight, 0, t);
                }
            }

            if (fadeTimer >= FadeDuration)
            {
                DesactivatingLights = false;
                LightsOn = false;
                fadeTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (NextFarola != null)
        {
            NextFarola.ActivateLights();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (NextFarola != null)
        {
            NextFarola.NoLights();
        }
    }

    public void ActivateLights()
    {
        if (LightsOn) return;

        DesactivatingLights = false;
        fadeTimer = 0f;
        ActivatingLights = true;
    }

    public void NoLights()
    {
        if (!LightsOn) return;

        ActivatingLights = false;
        fadeTimer = 0f;
        DesactivatingLights = true;
    }
}
