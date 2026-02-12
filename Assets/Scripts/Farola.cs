using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Farola : MonoBehaviour
{
    [Header ("Elements")]
    public Light[] Lights;
    BoxCollider Trigger;
    public Farola NextFarola;

    [Header("Atributos")]
    bool ActivatingLights = false;
    bool DesactivatingLights = false;
    public bool LightsOn;
    public float MaxLight = 1.327805f;
    public float MaxSpotLight = 104.6128f;
    public float Speed = 0.5f;

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
        if (!LightsOn)
        {
            if (ActivatingLights)
            {
                float LightCounter = 0;
                while (LightCounter < MaxLight)
                {
                    foreach (var b in Lights)
                    {
                        if (b.name == "SpotLight")
                        {

                            b.intensity += (MaxSpotLight / 3f) * Time.deltaTime;
                            

                        }
                        else
                        {
                            b.intensity += (MaxLight / 3f) * Time.deltaTime;
                            


                        }
                    }
                    LightCounter += (MaxLight / 3f) * Time.deltaTime;
                    
                }
                ActivatingLights = false;
                LightsOn = true;
            }
        }


        if (LightsOn)
        {
            if (DesactivatingLights)
            {
                float LightCounter = MaxLight;
                while (LightCounter > 0)
                {
                    foreach (var b in Lights)
                    {
                        if (b.name == "SpotLight")
                        {

                            b.intensity -= (MaxSpotLight / 3f) * Time.deltaTime;

                        }
                        else
                        {
                            b.intensity -= (MaxLight / 3f) * Time.deltaTime;


                        }
                    }
                    LightCounter -= (MaxLight / 3f) * Time.deltaTime;
                }
                DesactivatingLights = false;
                LightsOn = false;
            }
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        //ActivateLights();
        if(NextFarola != null)
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
        ActivatingLights = true;
        
    }
    public void NoLights()
    {
        DesactivatingLights = true;
        
    }
}
