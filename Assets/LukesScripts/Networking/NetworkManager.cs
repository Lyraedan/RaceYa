using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    public byte maxPlayers = 10;
    public string gameVersion = "0.1";
    public GameObject playerObject;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Connect();
        StartCoroutine(Spawn(new Vector3(0f, 5f, 0f)));
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room creating");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
    }

    void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("Already connected. Joining random room");
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            Debug.Log("Connected to server");
        }
    }

    IEnumerator Spawn(Vector3 position)
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        PhotonNetwork.Instantiate(this.playerObject.name, position, Quaternion.identity, 0);
    }
}
