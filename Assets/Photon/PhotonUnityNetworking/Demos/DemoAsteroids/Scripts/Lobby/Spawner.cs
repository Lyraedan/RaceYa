using System;
using System.Collections;
using System.Collections.Generic;
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
            if (assignedSpawn < 0)
                throw new ArgumentException("Assigned spawn can not be < 0");
            GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");
            if (assignedSpawn >= spawns.Length)
                throw new ArgumentException("Assigned spawn can not be >= " + spawns.Length);

            selectedSpawn = spawns[assignedSpawn].transform;
        }
    }
}
