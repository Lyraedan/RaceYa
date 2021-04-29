using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public Slider slider;
    [Range(0, 1)] private float masterVolume = 1f;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        slider.onValueChanged.AddListener(val =>
        {
            masterVolume = slider.value;
        });
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

}
