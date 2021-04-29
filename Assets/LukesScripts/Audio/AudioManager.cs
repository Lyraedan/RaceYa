using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public Slider masterSlider, musicSlider, sfxSlider;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public float GetSfxVolume()
    {
        return sfxSlider.value;
    }

    public float GetMusicVolume()
    {
        return musicSlider.value;
    }

    public float GetMasterVolume()
    {
        return masterSlider.value;
    }

}
