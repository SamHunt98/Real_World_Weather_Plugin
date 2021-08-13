using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class callToJs : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void aFunctionImplementedInJavaScriptLibraryFile(string str);

    [DllImport("__Internal")]
    private static extern void GetWeatherInLocation(string query);

    [DllImport("__Internal")]
    private static extern string GetWeatherResult();
    [DllImport("__Internal")]
    private static extern int GetWeatherDescription();

    [DllImport("__Internal")]
    private static extern float GetTimeDecimal();

    [DllImport("__Internal")]
    private static extern string GetTimeReal();

    [DllImport("__Internal")]
    private static extern string GetLocationNameAsString();

    [DllImport("__Internal")]
    private static extern int GetTemperature();

    public int testID; //test values used in the editor to check that functions worked as intended, as javascript could only be called in WebGL
    public string testWeather;

    [SerializeField] private LightManager lightManager;
    [SerializeField] private PopulateLocationBoard locationBoard;
    void Awake()
    {
        GetWeatherInLocation("London");
    }
    void Start()
    {
        //Debug.Log("Start function called please work");
        StartCoroutine(StartWeatherLocation());
    }

    IEnumerator StartWeatherLocation()
    {
        yield return new WaitForSeconds(5);
        locationBoard.UpdateValues();
        lightManager.UpdateLighting((ReturnTimeDecimal() + 1) / 24);
        StartCoroutine(UpdateActiveLocationWeather());
    }


    //public functions for calling the external javascript functions
    public string ReturnWeatherType()
    {
        
        return GetWeatherResult();
    }

    public int ReturnWeatherId()
    {
        
        return GetWeatherDescription();
    }

    public float ReturnTimeDecimal()
    {
        return GetTimeDecimal();
    }

    public string ReturnTimeReal()
    {
        return GetTimeReal();
    }

    public string ReturnTemperatureString()
    {
        return GetTemperature().ToString() + "°c";
    }

    public string ReturnLocationName()
    {
        return GetLocationNameAsString();
    }
    public void PlayerUpdateLocation(string newLocation)
    {
        //called whenever the player types in a new location through the UI.
        StopCoroutine(UpdateActiveLocationWeather());
        GetWeatherInLocation(newLocation);
        StartCoroutine(StartWeatherLocation()); //handles the first update 5 seconds after the player has entered a new value
        StartCoroutine(UpdateActiveLocationWeather()); //starts the 60 second countdown to the next update if the player does not interfere 
    }
    IEnumerator UpdateActiveLocationWeather()
    {
        //will search the API for the weather and time in the current location once per minute to keep it up to date       
        yield return new WaitForSeconds(60);
        GetWeatherInLocation(GetLocationNameAsString());
        yield return new WaitForSeconds(5);
        locationBoard.UpdateValues();
        lightManager.UpdateLighting((ReturnTimeDecimal()+1 )/ 24);
        StartCoroutine(UpdateActiveLocationWeather());
    }
}
