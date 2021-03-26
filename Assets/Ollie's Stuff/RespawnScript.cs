using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField]  private Transform respawnPoint;
    void OnTriggerEnter(Collider collision)
    {

        //collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("OtherPlayer")
        if (collision.gameObject.transform.root.gameObject.CompareTag("Player") || collision.gameObject.transform.root.gameObject.CompareTag("OtherPlayer"))
        {
            Debug.Log(collision);
            collision.gameObject.transform.root.position = respawnPoint.transform.position;
            if(collision.attachedRigidbody)
            {
                StartCoroutine(PlaceBack(collision));
            }
        }
    }

    IEnumerator PlaceBack(Collider collision)
    {
        collision.attachedRigidbody.useGravity = false;
        collision.attachedRigidbody.isKinematic = true;
        yield return new WaitForSeconds(0.1f);
        collision.attachedRigidbody.useGravity = true;
        collision.attachedRigidbody.isKinematic = false;
    }
}
