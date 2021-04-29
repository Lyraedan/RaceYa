﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon;

namespace Photon.Pun.Demo.Asteroids
{
    public class Pause : MonoBehaviour
    {
        bool paused;
        public GameObject PauseMenu;
        public GameObject OptionsMenu;
        public string LoadScene;

        public bool Paused { get => paused; set => paused = value; }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPaused();
            }
        }
        public void SetPaused()
        {
            paused = !paused;
            if (paused)
            {
                //Time.timeScale = 0;
                PauseMenu.SetActive(true);
            }

            else
            {
                //Time.timeScale = 1;
                PauseMenu.SetActive(false);
            }
        }
        public void OptionsButton()
        {
            OptionsMenu.SetActive(true);
            PauseMenu.SetActive(false);
        }
        public void MainMenu()
        {
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel(LoadScene);
            MicrophoneController.instance.canvas.gameObject.SetActive(false);

        }
    }
}
