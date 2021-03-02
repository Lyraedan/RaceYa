using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField]  private Transform respawnPoint;
    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("OtherPlayer"))
        {
            Debug.Log(collision);
            collision.gameObject.transform.position = respawnPoint.transform.position;
            if(collision.attachedRigidbody)
            {
                collision.attachedRigidbody.useGravity = false;
                collision.attachedRigidbody.isKinematic = true;
                collision.attachedRigidbody.useGravity = true;
                collision.attachedRigidbody.isKinematic = false;
            }
        }
    }
}
