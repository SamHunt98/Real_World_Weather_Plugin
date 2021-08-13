using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float defaultSpeed = 8;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movDirection = transform.right * x + transform.forward * z;

        controller.Move(movDirection * speed * Time.deltaTime);
    }

    public void ToggleMovement(bool shouldMove)
    {
        if(shouldMove)
        {
            speed = defaultSpeed;
        }
        else
        {
            speed = 0;
        }
    }
}
