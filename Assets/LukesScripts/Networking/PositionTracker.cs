using Photon.Pun;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun.Demo.Asteroids;

public class PositionTracker : MonoBehaviourPunCallbacks
{

    public static PositionTracker instance;

    public int yourPosition { get; private set; }

    int position = 1;
    float distance = 100f;
    int myProgression = 0;
    int myLap = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        yourPosition = CalculatePosition();
    }

    int CalculatePosition()
    {
        position = 1;
        foreach (NetworkedUser user in FindObjectsOfType<NetworkedUser>())
        {
            if (user.nametag.text.Equals(PhotonNetwork.LocalPlayer.NickName))
            {
                distance = user.DistanceToNextProgressionPoint();
                myProgression = user.currentLapProgression;
                myLap = user.currentLap;
            }
            else
            {
                float theirDistance = user.DistanceToNextProgressionPoint();
                int theirProgression = user.currentLapProgression;
                int theirLap = user.currentLap;

                bool increasePosition = distance < theirDistance || theirProgression > myProgression || theirLap > myLap;

                Debug.Log("Their distance " + theirDistance + " | " + distance);
                Debug.Log("Their progression " + theirProgression + " | " + myProgression);
                Debug.Log("Their lap " + theirLap + " | " + myLap);

                if (increasePosition)
                {
                    Debug.Log($"{user.nametag.text} is ahead! Increasing position");
                    position++;
                }
            }
        }
        return position;
    }
}
