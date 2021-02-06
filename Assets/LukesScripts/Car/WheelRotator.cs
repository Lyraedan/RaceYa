using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    public bool rotating = false;
    public float rotationSpeed = 5f;
    [SerializeField] private List<Wheel> pivots = new List<Wheel>();

    // Update is called once per frame
    void Update()
    {
        if(rotating)
        {
            foreach(Wheel wheel in pivots)
            {
                wheel.RotateFromPivot(rotationSpeed);
            }
        }
    }
}

[Serializable]
class Wheel
{
    public enum WheelAxis
    {
        FORWARD, BACK, LEFT, RIGHT
    }

    public Transform pivot;
    public WheelAxis rotationAxis = WheelAxis.FORWARD;
    public Vector2 leftRightClamp = new Vector2(-15f, 15f);
    public Transform[] wheels { get { return pivot.GetComponentsInChildren<Transform>(); } }

    private float rotY;

    public void RotateFromPivot(float rot)
    {
        rotY += rot * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, leftRightClamp.x, leftRightClamp.y);

        switch(rotationAxis)
        {
            case WheelAxis.FORWARD:
                pivot.Rotate(GetAxis(), rot * Time.deltaTime);
                break;
            case WheelAxis.BACK:
                pivot.Rotate(GetAxis(), rot * Time.deltaTime);
                break;
            case WheelAxis.LEFT:
                pivot.rotation = Quaternion.Euler(pivot.rotation.x, -rotY, pivot.rotation.z);
                break;
            case WheelAxis.RIGHT:
                pivot.rotation = Quaternion.Euler(pivot.rotation.x, rotY, pivot.rotation.z);
                break;
            default:
                break;
        }
        
    }

    Vector3 GetAxis()
    {
        switch(rotationAxis)
        {
            case WheelAxis.FORWARD:
                return Vector3.right;
            case WheelAxis.BACK:
                return -Vector3.right;
            default:
                return Vector3.right;
        }
    }
}
