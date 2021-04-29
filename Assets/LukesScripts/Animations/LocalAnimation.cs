using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAnimation : MonoBehaviour
{
     Vector3 localPos;
    bool wasPlaying = false;
    public new Animation animation;

    void Awake()
    {
        localPos = transform.position;
        wasPlaying = false;
        animation = GetComponent<Animation>();
    }

    void LateUpdate()
    {
        if (!animation.isPlaying && !wasPlaying)
            return;

        transform.localPosition += localPos;

        wasPlaying = animation.isPlaying;
    }
}
