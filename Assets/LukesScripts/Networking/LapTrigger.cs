using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// There should only be 1 lap trigger in the scene
/// </summary>
public class LapTrigger : MonoBehaviour
{

    public static LapTrigger instance;
    public bool active = false;

    public AudioSource audioSource;
    public AudioClip lapOne, lapTwo, lapThree;

    public List<LapProgressionTrigger> progressionTriggers = new List<LapProgressionTrigger>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        // Auto assign the progression id's in corrolation to the order in the list
        for (int i = 0; i < progressionTriggers.Count; i++)
        {
            progressionTriggers[i].progressionID = i;
        }
        audioSource.clip = lapOne;
        //GetPercentage(jukebox.volume, slider.maxValue) * Settings.instance.GetMasterVolume();
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        audioSource.volume = (AudioManager.instance.GetMasterVolume() * AudioManager.instance.GetMusicVolume()) / 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject root = other.transform.root.gameObject;
        if (root.GetComponent<PhotonView>())
        {
            if (root.GetComponent<PhotonView>().IsMine)
            {
                NetworkedUser user = root.GetComponent<NetworkedUser>();
                active = user.currentLapProgression >= progressionTriggers.Count;
                if (!active) return;

                user.NextLap();
                if (audioSource.isPlaying)
                    audioSource.Stop();
                switch (user.currentLap)
                {
                    case 1:
                        audioSource.clip = lapOne;
                        break;
                    case 2:
                        audioSource.clip = lapTwo;
                        break;
                    case 3:
                        audioSource.clip = lapThree;
                        break;
                    case 4:
                        audioSource.clip = null;
                        break;
                    default:
                        audioSource.clip = lapOne;
                        break;
                }
                if (audioSource.clip != null)
                {
                    audioSource.loop = true;
                    audioSource.Play();
                }
                if (user.currentLap <= user.maxLaps)
                    user.lapCounter.text = $"Lap: {user.currentLap}/{user.maxLaps}";
                active = false;
            }
        }

    }

}
