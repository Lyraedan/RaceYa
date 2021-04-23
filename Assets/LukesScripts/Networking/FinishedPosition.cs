using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedPosition : MonoBehaviour
{
    public int id = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.forward * 3f, 0.25f);
    }
}
