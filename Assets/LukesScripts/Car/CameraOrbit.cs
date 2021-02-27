using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public int yMinLimit = -20;
    public int yMaxLimit = 80;

    public int zoomRate = 20;

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {

        if (target)
        {
            float test = 0;
            if (Input.GetMouseButton(0))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                test = y;
            }
            distance += -(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
            if (distance < 2.5f)
            {
                distance = 2.5f;
            }
            if (distance > 20f)
            {
                distance = 20f;
            }


            y = ClampAngle(y, yMinLimit, yMaxLimit);

            //Debug.Log("y: "+y+" test: "+test);

            if (y == yMinLimit || test == yMinLimit)
        {
                // This is to allow the camera to slide across the bottom if the player is too low in the y
                distance += -(Input.GetAxis("Mouse Y") * Time.deltaTime) * 10 * Mathf.Abs(distance);
            }

            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0.0f, 2.0f, -distance) + target.position;

            //Debug.Log("Distance: "+distance);
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
