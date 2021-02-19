using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometerarrow : MonoBehaviour
{
    public Rigidbody target;

    public float maxspeed = 0.0f; // max speed of car
    public float minangle;
    public float maxangle;

    [Header("ui")]
    public RectTransform arrow;

    private float speed = 0.0f;

    private void Update()
    {
        speed = target.velocity.magnitude * 2.237f;

        if (arrow != null)
            arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minangle, maxangle, speed / maxspeed));
    }
}
