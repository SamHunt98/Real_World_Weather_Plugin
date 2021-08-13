using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightPreset preset;
    [SerializeField, Range(0,24)] private float currentHours; //how far through the day or night it currently is based on next sunset/rise

    [SerializeField] private callToJs javascriptCaller;

    [SerializeField] private ParticleSystem rainParticle;
    [SerializeField] private ParticleSystem drizzleParticle;
    [SerializeField] private ParticleSystem snowParticle;
    [SerializeField] private GameObject clouds;


    [SerializeField] private string currentWeather; //stores the weather currently acting on the scene, used to stop unnecessary updates

    [SerializeField] private Material clearSkybox;
    [SerializeField] private Material darkSkybox;
    [SerializeField] private bool canChangeTorchesOnUpdate = true; //allows torches to be changed based on time of day rather than the weather condition
    [SerializeField] private Light[] torches;

    [SerializeField] private AudioController audioManager;

 


    public void UpdateLighting(float phasePercentage)
    {
        RenderSettings.ambientLight = preset.AmbientColour.Evaluate(phasePercentage);
        RenderSettings.fogColor = preset.FogColour.Evaluate(phasePercentage);

        Debug.Log("The weather type right now is " + javascriptCaller.ReturnWeatherType());
        if(canChangeTorchesOnUpdate)
        {
            if (currentHours > 6 && currentHours < 18)
            {
                ToggleTorches(false);
            }
            else
            {
                ToggleTorches(true);
            }
        }
        switch (javascriptCaller.ReturnWeatherType())
        {
            case "Fog":
                if(currentWeather != "Fog")
                {
                    ResetWeatherConditions();
                    preset.AmbientColour = preset.AmbientColourRain;
                    RenderSettings.skybox = darkSkybox;
                    directionalLight.intensity = 0.3f;
                    ToggleTorches(true);
                    canChangeTorchesOnUpdate = false;
                    RenderSettings.fogEndDistance = 165;
                    clouds.SetActive(true);
                    currentWeather = "Fog";
                }
                break;
            case "Thunderstorm":
                if (currentWeather != "Thunderstorm")
                {
                    ResetWeatherConditions();
                    preset.AmbientColour = preset.AmbientColourRain;
                    RenderSettings.skybox = darkSkybox;
                    directionalLight.intensity = 0.3f;
                    ToggleTorches(true);
                    canChangeTorchesOnUpdate = false;
                    clouds.SetActive(true);
                    currentWeather = "Thunderstorm";
                }
                SetThunderIntensity();
                break;
            case "Drizzle":
                if (currentWeather != "Drizzle")
                {
                    ResetWeatherConditions();
                    drizzleParticle.gameObject.SetActive(true);
                    clouds.SetActive(true);
                    currentWeather = "Drizzle";
                }
                if (javascriptCaller.ReturnWeatherId() < 305)
                {
                    //the API logos don't actual specify if it's sunny or not here so I did it based on the inclusion of rain in the description
                    preset.AmbientColour = preset.AmbientColourDefault;
                    RenderSettings.skybox = clearSkybox;
                    directionalLight.intensity = 1;
                    canChangeTorchesOnUpdate = true;
                }
                else
                {
                    preset.AmbientColour = preset.AmbientColourRain;
                    RenderSettings.skybox = darkSkybox;
                    directionalLight.intensity = 0.3f;
                    ToggleTorches(true);
                    canChangeTorchesOnUpdate = false;
                }
                SetDrizzleIntensity();
                break;
            case "Rain":
                if (currentWeather != "Rain")
                {
                    ResetWeatherConditions();
                    rainParticle.gameObject.SetActive(true);
                    clouds.SetActive(true);
                    currentWeather = "Rain";
                }
                if (javascriptCaller.ReturnWeatherId() < 505)
                {
                    //values under 505 are shown with a sun in the API logos, so I'm going to set them as having a bright sky
                    preset.AmbientColour = preset.AmbientColourDefault;
                    RenderSettings.skybox = clearSkybox;
                    directionalLight.intensity = 1;
                    canChangeTorchesOnUpdate = true;
                }
                else
                {
                    //values over 505 have a dark sky in the logos, so I will set the sky to be grey for them
                    preset.AmbientColour = preset.AmbientColourRain;
                    RenderSettings.skybox = darkSkybox;
                    directionalLight.intensity = 0.3f;
                    ToggleTorches(true);
                    canChangeTorchesOnUpdate = false;
                }
                SetRainIntensity();
                break;
            case "Snow":
                if (currentWeather != "Snow")
                {
                    ResetWeatherConditions();
                    snowParticle.gameObject.SetActive(true);
                    preset.AmbientColour = preset.AmbientColourRain;
                    RenderSettings.skybox = darkSkybox;
                    ToggleTorches(true);
                    canChangeTorchesOnUpdate = false;
                    directionalLight.intensity = 0.3f;
                    clouds.SetActive(true);
                    currentWeather = "Snow";
                }
                SetSnowIntensity();
                break;
            case "Clouds":
                if(currentWeather != "Clouds")
                {
                    ResetWeatherConditions();
                    clouds.SetActive(true);
                    currentWeather = "Clouds";
                }
                break;
            default:
                //defaulting to "Clear" at the moment since I'm not adding in some of the more niche ones like tornadoes or volcanic eruptions
                if(currentWeather != "Clear")
                {
                    ResetWeatherConditions();
                    currentWeather = "Clear";
                }

                break;

        }
        
        if(directionalLight  != null)
        {
            directionalLight.color = preset.DirectionalColour.Evaluate(phasePercentage);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((phasePercentage * 360f)-120f,170f,0));
            
        }
    }

    private void ResetWeatherConditions()
    {
        preset.AmbientColour = preset.AmbientColourDefault;
        RenderSettings.skybox = clearSkybox;
        directionalLight.intensity = 1;
        RenderSettings.fogEndDistance = 3000;
        rainParticle.gameObject.SetActive(false);
        snowParticle.gameObject.SetActive(false);
        drizzleParticle.gameObject.SetActive(false);
        clouds.SetActive(false);
        audioManager.ToggleRain(false, "null");
        audioManager.ToggleThunder(false);
        canChangeTorchesOnUpdate = true;

    }

    private void SetRainIntensity()
    {
        //sets how heavy the rain will be based on the ID generated by the API
        var particleEmission = rainParticle.emission;
        switch(javascriptCaller.ReturnWeatherId())
        {
            case 500:
                particleEmission.rateOverTime = 250;
                audioManager.ToggleRain(true, "Light");
                break;
            case 501:
                particleEmission.rateOverTime = 500;
                audioManager.ToggleRain(true, "Light");
                break;
            case 502:
                particleEmission.rateOverTime = 750;
                audioManager.ToggleRain(true, "Medium");
                break;
            case 503:
                particleEmission.rateOverTime = 1000;
                audioManager.ToggleRain(true, "Medium");
                break;
            case 504:
                particleEmission.rateOverTime = 2000;
                audioManager.ToggleRain(true, "Heavy");
                break;
            case 511:
                particleEmission.rateOverTime = 1000;
                audioManager.ToggleRain(true, "Medium");
                break;
            case 520:
                particleEmission.rateOverTime = 250;
                audioManager.ToggleRain(true, "Light");
                break;
            case 521:
                particleEmission.rateOverTime = 500;
                audioManager.ToggleRain(true, "Light");
                break;
            case 522:
                particleEmission.rateOverTime = 1000;
                audioManager.ToggleRain(true, "Medium");
                break;
            case 531:
                particleEmission.rateOverTime = 2000;
                audioManager.ToggleRain(true, "Heavy");
                break;
            default:
                particleEmission.rateOverTime = 1000;
                audioManager.ToggleRain(true, "Medium");
                break;

        }
    }

    private void SetSnowIntensity()
    {
        //sets how heavy the rain will be based on the ID generated by the API
        var particleEmission = snowParticle.emission;
        switch (javascriptCaller.ReturnWeatherId())
        {
            case 600:
                particleEmission.rateOverTime = 250;
                break;
            case 601:
                particleEmission.rateOverTime = 1000;
                break;
            case 602:
                particleEmission.rateOverTime = 1500;
                break;
            default:
                particleEmission.rateOverTime = 1000;
                break;
        }
    }

    private void SetDrizzleIntensity()
    {
        //sets how heavy the rain will be based on the ID generated by the API
        var particleEmission = drizzleParticle.emission;
        switch (javascriptCaller.ReturnWeatherId())
        {
            case 300:
                particleEmission.rateOverTime = 50;
                break;
            case 301:
                particleEmission.rateOverTime = 100;
                break;
            case 302:
                particleEmission.rateOverTime = 200;
                break;
            default:
                particleEmission.rateOverTime = 100;
                break;
        }
        audioManager.ToggleRain(true, "Light");
    }
    private void SetThunderIntensity()
    {
        //sets how heavy the thunder will be as well as if it is also raining or drizzling
        var particleEmissionRain = rainParticle.emission;
        var particleEmissionDrizzle = drizzleParticle.emission;
        switch (javascriptCaller.ReturnWeatherId())
        {
            case 200:
                particleEmissionRain.rateOverTime = 250;
                audioManager.ToggleRain(true, "Light");
                rainParticle.gameObject.SetActive(true);
                break;
            case 201:
                particleEmissionRain.rateOverTime = 750;
                audioManager.ToggleRain(true, "Medium");
                rainParticle.gameObject.SetActive(true);
                break;
            case 202:
                particleEmissionRain.rateOverTime = 1000;
                audioManager.ToggleRain(true, "Heavy");
                rainParticle.gameObject.SetActive(true);
                break;
            case 230:
                particleEmissionDrizzle.rateOverTime = 50;
                audioManager.ToggleRain(true, "Light");
                drizzleParticle.gameObject.SetActive(true);
                break;
            case 231:
                particleEmissionDrizzle.rateOverTime = 100;
                audioManager.ToggleRain(true, "Light");
                drizzleParticle.gameObject.SetActive(true);
                break;
            case 232:
                particleEmissionDrizzle.rateOverTime = 200;
                audioManager.ToggleRain(true, "Light");
                drizzleParticle.gameObject.SetActive(true);
                break;
            default:
                //regular thunderstorm sound
                audioManager.ToggleRain(false, "null");
                drizzleParticle.gameObject.SetActive(false);
                rainParticle.gameObject.SetActive(false);
                break;

        }
        audioManager.ToggleThunder(true);
    }

    private void ToggleTorches(bool setOn)
    {
        foreach(Light l in torches)
        {
            l.gameObject.SetActive(setOn);
        }
    }
}
