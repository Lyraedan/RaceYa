using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Image timer;

    public Sprite timerThree;
    public Sprite timerTwo;
    public Sprite timerOne;
    public Sprite timerGo;

    public AudioSource sound;

    public void Start()
    {
        StartTimer();
    }

    public void Update()
    {
        sound.volume = (AudioManager.instance.GetMasterVolume() * AudioManager.instance.GetSfxVolume()) / 1f;
    }

    public void StartTimer()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length >= Spawner.instance.lobbySize);
        sound.Play();
        timer.sprite = timerThree;
        yield return new WaitForSeconds(1);
        timer.sprite = timerTwo;
        yield return new WaitForSeconds(1);
        timer.sprite = timerOne;
        yield return new WaitForSeconds(1);
        timer.sprite = timerGo;

        foreach (NetworkedUser user in (NetworkedUser[]) FindObjectsOfType(typeof(NetworkedUser))) {
            user.started = true;
        }
        yield return new WaitForSeconds(1f);
        timer.gameObject.SetActive(false);
    }

}
