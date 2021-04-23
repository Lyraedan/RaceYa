using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCam : MonoBehaviour
{
    public Transform target;
    public float distance;
    public float height;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var targetPosition = target.position - (target.forward * distance) + new Vector3(0, height, 0);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 23f);
        transform.LookAt(target.position);
    }
}
