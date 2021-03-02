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

        public int assignedSpawn = 0;
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

        public void AssignSpawn(int spawn) => assignedSpawn = spawn;

        public void SelectSpawn()
        {
            Player[] sorted = PhotonNetwork.PlayerList;
            Array.Sort(sorted, (userA, userB) => string.Compare(userA.NickName, userB.NickName));
            int index = Array.IndexOf(sorted, sorted.ToList().Find(player => player.NickName.Equals(PhotonNetwork.LocalPlayer.NickName)));
            AssignSpawn(index);

            if (assignedSpawn < 0)
                throw new ArgumentException("Assigned spawn can not be < 0");
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
            if (assignedSpawn >= spawns.Length)
                throw new ArgumentException("Assigned spawn can not be >= " + spawns.Length);

            foreach(GameObject s in spawns)
            {
                if (!s.GetComponent<Spawn>())
                {
                    Debug.LogError("Spawn does not have spawn script!");
                }
                else
                {
                    Spawn sp = s.GetComponent<Spawn>();
                    if (sp.id == assignedSpawn)
                    {
                        selectedSpawn = s.transform;
                        break;
                    }
                }
            }

        }
    }
}
