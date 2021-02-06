using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode forward = KeyCode.W;
    public KeyCode back = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    [Header("Car properties")]
    public float maxSpeedForward = 60f;
    public float maxSpeedBack = 10f;
    [Space(3)]
    public float currentSpeed = 0f;
    [Space(5)]
    public float rateOfAcceleration = 3f;
    public float rateOfReverseAcceleration = 1f;
    [Header("Car Components")]
    public WheelRotator wheelController;

    // Update is called once per frame
    void Update()
    {
        if(isMovingForward)
        {
            wheelController.rotating = true;
            wheelController.rotationSpeed = currentSpeed * 10;
            currentSpeed += rateOfAcceleration;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeedForward);
            transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed, Space.Self);
        } else if(isMovingBack)
        {
            wheelController.rotating = true;
            wheelController.rotationSpeed = currentSpeed * 10;
            currentSpeed -= rateOfReverseAcceleration;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeedBack, 0);
            transform.Translate(-Vector3.forward * Time.deltaTime * currentSpeed, Space.Self);
        } else
        {
            if (currentSpeed > 0)
            {
                currentSpeed--;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeedForward);
            } else
            {
                currentSpeed++;
                currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeedBack, 0);
            }
            wheelController.rotating = false;
        }
    }

    public bool isMovingForward
    {
        get { return Input.GetKey(forward); }
    }

    public bool isMovingBack
    {
        get { return Input.GetKey(forward); }
    }

    public bool isTurningLeft
    {
        get { return Input.GetKey(left); }
    }

    public bool isTurningRight
    {
        get { return Input.GetKey(right); }
    }
}
