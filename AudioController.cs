using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource rainAudio;
    [SerializeField] AudioSource thunderAudio;

    [SerializeField] AudioClip lightRain;
    [SerializeField] AudioClip mediumRain;
    [SerializeField] AudioClip heavyRain;

    private string currentRain = "";
    private bool thunderOn = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleThunder(bool shouldPlay)
    {
        if(shouldPlay)
        {
            if(!thunderOn)
            {
                thunderAudio.Play();
                thunderOn = true;
            }
            
        }
        else
        {
            thunderAudio.Stop();
            thunderOn = false;
        }
    }

    public void ToggleRain(bool shouldPlay, string type)
    {
        if(shouldPlay)
        {
            if(type != currentRain)
            {
                switch (type)
                {
                    case "Light":
                        rainAudio.clip = lightRain;
                        currentRain = type;
                        break;
                    case "Medium":
                        rainAudio.clip = mediumRain;
                        currentRain = type;
                        break;
                    case "Heavy":
                        rainAudio.clip = heavyRain;
                        currentRain = type;
                        break;
                    default:
                        rainAudio.clip = mediumRain;
                        currentRain = type;
                        break;
                }
                rainAudio.Play();
            }
            
        }
        else
        {
            rainAudio.Stop();
            currentRain = "";
        }

    }
}
