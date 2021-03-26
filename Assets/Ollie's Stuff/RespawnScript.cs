using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    public bool showGizmo = true;
    public bool drawLines = false;

    void OnTriggerEnter(Collider collision)
    {

        //collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("OtherPlayer")
        if (collision.gameObject.transform.root.gameObject.CompareTag("Player") || collision.gameObject.transform.root.gameObject.CompareTag("OtherPlayer"))
        {
            Debug.Log(collision);
            collision.gameObject.transform.root.position = respawnPoint.transform.position;
            if (collision.attachedRigidbody)
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

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + transform.forward, 0.1f);
            BoxCollider hitbox = GetComponent<BoxCollider>();
            if (hitbox != null)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.color = new Color(128, 128, 0, 180);
                Gizmos.DrawCube(hitbox.center, hitbox.size);
                if (respawnPoint != null && drawLines)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(hitbox.center, respawnPoint.position);
                }
            }
        }
    }
}
