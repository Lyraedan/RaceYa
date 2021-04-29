using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButton : MonoBehaviour
{
    public AudioSource input;

    private void Update()
    {
        input.volume = (AudioManager.instance.GetMasterVolume() * AudioManager.instance.GetSfxVolume()) / 1f;
    }
}
