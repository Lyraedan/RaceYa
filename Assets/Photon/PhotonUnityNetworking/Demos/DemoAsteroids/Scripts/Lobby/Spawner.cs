using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// I had to put it in the namespace because namespaces are dumb :) - Luke
namespace Photon.Pun.Demo.Asteroids
{
    public class Spawner : MonoBehaviour
    {
        public static Spawner instance;
        public int lobbySize = 0;

        public Transform selectedSpawn;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public void SelectSpawn()
        {
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
            int index = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.PlayerList.ToList().Find(player => player.NickName.Equals(PhotonNetwork.LocalPlayer.NickName)));
            foreach (GameObject s in spawns)
            {
                if (!s.GetComponent<Spawn>())
                {
                    Debug.LogError("Spawn does not have spawn script!");
                }
                else
                {
                    Spawn sp = s.GetComponent<Spawn>();
                    if (sp.id == index)
                    {
                        selectedSpawn = s.transform;
                        Debug.Log("Selected spawn " + sp.id);
                        break;
                    }
                }
            }

        }
    }
}
