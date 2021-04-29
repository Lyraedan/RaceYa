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

        masterSlider.value = PlayerPrefs.HasKey("VOLUME_MASTER") ? PlayerPrefs.GetFloat("VOLUME_MASTER") : masterSlider.maxValue;
        musicSlider.value = PlayerPrefs.HasKey("VOLUME_MUSIC") ? PlayerPrefs.GetFloat("VOLUME_MUSIC") : musicSlider.maxValue;
        sfxSlider.value = PlayerPrefs.HasKey("VOLUME_SFX") ? PlayerPrefs.GetFloat("VOLUME_SFX") : sfxSlider.maxValue;

        masterSlider.onValueChanged.AddListener(val =>
        {
            PlayerPrefs.SetFloat("VOLUME_MASTER", masterSlider.value);
        });

        musicSlider.onValueChanged.AddListener(val =>
        {
            PlayerPrefs.SetFloat("VOLUME_MUSIC", musicSlider.value);
        });

        sfxSlider.onValueChanged.AddListener(val =>
        {
            PlayerPrefs.SetFloat("VOLUME_SFX", sfxSlider.value);
        });
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
