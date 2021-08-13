using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Open_UI : MonoBehaviour
{
    [SerializeField] GameObject uiCanvas;
    [SerializeField] TMP_InputField textBox;
    [SerializeField] Movement movementScript;
    [SerializeField] CameraControl cameraScript;
    [SerializeField] callToJs javascriptFunctions;

    bool uiIsOpen = false;
    bool playerInTrigger = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E) && !uiIsOpen)
            {
                uiIsOpen = true;
                movementScript.ToggleMovement(false);
                cameraScript.ToggleCameraLock(true);
                uiCanvas.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Tab) && uiIsOpen)
            {
                uiIsOpen = false;
                movementScript.ToggleMovement(true);
                cameraScript.ToggleCameraLock(false);
                uiCanvas.SetActive(false);
            }
        }
    }

    public void OnAcceptButtonClicked()
    {
        javascriptFunctions.PlayerUpdateLocation(textBox.textComponent.text);
        textBox.textComponent.text = "";
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInTrigger = true;
        }
        else
        {
            playerInTrigger = false;
        }
    }
}
