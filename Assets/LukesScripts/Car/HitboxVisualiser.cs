using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxVisualiser : MonoBehaviour
{

    public bool displayHitboxes = true;
    [SerializeField] private List<Hitbox> hitboxes = new List<Hitbox>();

    private void OnDrawGizmos()
    {
        if (displayHitboxes)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + transform.forward, 0.1f);
            foreach (Hitbox hitbox in hitboxes)
            {
                if (hitbox.collider != null)
                {
                    Gizmos.matrix = transform.localToWorldMatrix;
                    Gizmos.color = hitbox.isValid ? hitbox.visualColor : Color.red;
                    if (hitbox.wireframe) Gizmos.DrawWireCube(hitbox.collider.center, hitbox.collider.size);
                    else Gizmos.DrawCube(hitbox.collider.center, hitbox.collider.size);
                }
            }
        }
    }
}

[Serializable]
class Hitbox
{
    public BoxCollider collider;
    public Color visualColor = Color.green;
    public bool wireframe = false;
    public bool isValid { get { return collider.size.x > 0 && collider.size.y > 0 && collider.size.z > 0; } }
}
