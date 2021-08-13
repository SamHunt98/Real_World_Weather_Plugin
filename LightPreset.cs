using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName ="Light Preset",menuName="Scriptables/Light Preset",order =1)]
public class LightPreset : ScriptableObject
{
    public Gradient AmbientColourDefault;
    public Gradient AmbientColourRain; //for when the sky is darker during rain or thunder
    public Gradient AmbientColour;
    public Gradient DirectionalColour;
    public Gradient FogColour;
}
