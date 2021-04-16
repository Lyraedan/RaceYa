using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public GameObject playerObject;

    void Start()
    {
        StartCoroutine(Spawn(new Vector3(0f, 5f, 0f)));
    }

    IEnumerator Spawn(Vector3 position)
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        Spawner.instance.SelectSpawn();
        playerObject = PhotonNetwork.Instantiate(this.playerObject.name, Spawner.instance.selectedSpawn.position, Quaternion.identity, 0);
        playerObject.transform.Rotate(new Vector3(0, 90, 0));
        playerObject.GetComponent<NetworkedUser>().started = false;
        if (PhotonNetwork.PlayerList.Length >= Spawner.instance.lobbySize) //This is the check that everyone is in
        {
            yield return new WaitForSeconds(4);
            playerObject.GetComponent<NetworkedUser>().started = true;
        }
    }
}
