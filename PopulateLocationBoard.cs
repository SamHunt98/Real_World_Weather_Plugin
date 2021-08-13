using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulateLocationBoard : MonoBehaviour
{
    [SerializeField] TextMeshPro locationText;
    [SerializeField] TextMeshPro weatherText;
    [SerializeField] TextMeshPro temperatureText;
    [SerializeField] TextMeshPro timeText;

    [SerializeField] callToJs javascriptCode;


    public void UpdateValues()
    {
        locationText.text = javascriptCode.ReturnLocationName();
        weatherText.text = javascriptCode.ReturnWeatherType();
        temperatureText.text = javascriptCode.ReturnTemperatureString();
        timeText.text = javascriptCode.ReturnTimeReal();
    }

}
