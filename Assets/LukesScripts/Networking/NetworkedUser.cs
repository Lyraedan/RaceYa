using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class NetworkedUser : MonoBehaviour
{

    public GameObject camera;
    public GameObject UI;

    public int currentLap = 0;
    public int maxLaps = 3;
    public int currentLapProgression = 0;
    public bool finished { get; set; } = false;

    public string userID => PhotonNetwork.LocalPlayer.NickName;

    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            Destroy(camera);
            Destroy(UI);
            gameObject.tag = "OtherPlayer";
        }
        else
        {
            gameObject.tag = "Player";
        }
    }

    /// <summary>
    /// Progress your lap for everyone connected
    /// </summary>
    public void NextLap()
    {
        view.RPC("IncreaseLapCounter", RpcTarget.All);
    }

    [PunRPC]
    public void IncreaseLapCounter()
    {
        if (finished) return;

        currentLap++;
        finished = currentLap > maxLaps;
        if(view.IsMine)
        {
            Debug.Log("Resetting progression triggers");
            foreach(LapProgressionTrigger progression in LapTrigger.instance.progressionTriggers)
            {
                progression.gameObject.SetActive(true);
            }
            currentLapProgression = 0;
        }
        if (finished)
        {
            Debug.Log($"{userID} has finished the race");
        }
    }
}
