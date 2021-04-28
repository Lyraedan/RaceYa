using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public GameObject playerObject;

    void Start()
    {
        StartCoroutine(Spawn(new Vector3(0f, 5f, 0f)));
    }

    IEnumerator Spawn(Vector3 position)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name.Equals("Test"));
        Spawner.instance.SelectSpawn();
        string prefabName = this.playerObject.name;
        Debug.Log($"Spawning {prefabName}");
        playerObject = PhotonNetwork.Instantiate(prefabName, Spawner.instance.selectedSpawn.position, Quaternion.identity, 0);
        playerObject.transform.Rotate(new Vector3(0, 90, 0));
        playerObject.GetComponent<NetworkedUser>().started = false;
    }
}
