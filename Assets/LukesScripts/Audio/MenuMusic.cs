using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public AudioSource input;

    private void Update()
    {
        input.volume = (AudioManager.instance.GetMasterVolume() * 0.25f) / 1f;
    }
}
