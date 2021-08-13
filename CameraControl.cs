using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform player;

    float xRot = 0f;
    public bool playerInControl = true;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(playerInControl)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRot -= mouseY;
            xRot = Mathf.Clamp(xRot, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }
 
    }

    public void ToggleCameraLock(bool shouldLock)
    {
        //used to switch between having control of the camera and having control of the mouse cursor when interacting with the UI
        if(shouldLock)
        {
            playerInControl = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            playerInControl = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
}
