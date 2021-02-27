using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class NetworkedUser : MonoBehaviour
{

    public GameObject camera;
    public GameObject UI;

    public Text lapCounter;
    public Text progressionCounter;

    [Header("Nametag")]
    public Canvas worldspaceCanvas;
    public TMP_Text nametag;

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
        worldspaceCanvas.worldCamera = Camera.main;
        gameObject.name = "Player " + view.Owner.NickName;
        nametag.text = view.Owner.NickName;
    }

    private void Update()
    {
        worldspaceCanvas.transform.LookAt(Camera.main.transform);
        worldspaceCanvas.transform.Rotate(new Vector3(0, 180, 0));
        worldspaceCanvas.gameObject.SetActive(Vector3.Distance(transform.position, worldspaceCanvas.transform.position) < 3);
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
            lapCounter.text = "Race finished";
            progressionCounter.text = string.Empty;
        }
    }
}
