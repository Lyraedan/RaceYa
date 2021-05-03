using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class NetworkedUser : MonoBehaviourPunCallbacks
{

    public Camera cam;
    public GameObject UI;
    public Collider carCollider;

    public Text lapCounter;
    public Text progressionCounter;
    public Text positionCounter;
    public Rigidbody body;

    [Header("Nametag")]
    public Canvas worldspaceCanvas;
    public TMP_Text nametag;

    public int currentLap = 0;
    public int maxLaps = 3;
    public int currentLapProgression = 0;
    public int nextProgressionPoint = 1;
    // Has the user finished the race
    public bool finished { get; set; } = false;
    // Has the race started
    public bool started { get; set; } = false;

    public string userID => PhotonNetwork.LocalPlayer.NickName;

    public Color carColour = Color.green;
    public Color characterColour = Color.red;

    public AudioSource audioSource;
    public AudioClip finishedSound;

    private PhotonView view;

    public List<Renderer> bodyRenderers = new List<Renderer>();
    public Renderer character;

    public bool readyForUse { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            Destroy(cam);
            Destroy(UI);
            gameObject.tag = "OtherPlayer";
        }
        else
        {
            gameObject.tag = "Player";
            view.RPC("RPC_LoadCustomization", RpcTarget.All, CarColour.instance.bodyColour.r, CarColour.instance.bodyColour.g, CarColour.instance.bodyColour.b,
                                                             CarColour.instance.characterColour.r, CarColour.instance.characterColour.g, CarColour.instance.characterColour.b);
        }
        worldspaceCanvas.worldCamera = Camera.main;
        gameObject.name = "Player " + view.Owner.NickName;
        nametag.text = view.Owner.NickName;
        StartCoroutine(WaitForAllPlayers());
    }

    IEnumerator WaitForAllPlayers()
    {
        yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length >= Spawner.instance.lobbySize);
        readyForUse = true;
    }

    [PunRPC]
    public void RPC_LoadCustomization(float br, float bg, float bb, float cr, float cg, float cb)
    {
        carColour = new Color(br, bg, bb);
        characterColour = new Color(cr, cg, cb);

        foreach (Renderer renderer in bodyRenderers)
        {
            renderer.material.color = carColour;
        }
        character.material.color = characterColour;

    }

    /// <summary>
    /// Calculate the distance from this user to the next progression point
    /// </summary>
    /// <returns></returns>
    public float DistanceToNextProgressionPoint()
    {
        var progressionPoint = GetNextProgression().gameObject.transform.position;
        return Mathf.Round(Vector3.Distance(progressionPoint, gameObject.transform.position));
    }

    [Obsolete("No longer in use")]
    public float DistanceToProgressionPoint(int id)
    {
        int index = id % LapTrigger.instance.progressionTriggers.Count;
        var point = LapTrigger.instance.progressionTriggers[index];
        return Mathf.Round(Vector3.Distance(point.transform.position, gameObject.transform.position));
    }

    /// <summary>
    /// Get this users next progression point (Loop to 0 if you hit the max limit)
    /// </summary>
    /// <returns></returns>
    public LapProgressionTrigger GetNextProgression()
    {
        int index = (currentLapProgression + 1) % LapTrigger.instance.progressionTriggers.Count;
        return LapTrigger.instance.progressionTriggers[index];
    }

    private void Update()
    {
        if (cam == null)
            Spectate(FindObjectsOfType<NetworkedUser>());

        try
        {
            worldspaceCanvas.transform.LookAt(cam.transform.position);
            worldspaceCanvas.transform.Rotate(new Vector3(0, 180, 0));
            worldspaceCanvas.gameObject.SetActive(Vector3.Distance(transform.position, worldspaceCanvas.transform.position) < 3);
        }
        catch (Exception e)
        {
            // If something goes wrong
            Spectate(FindObjectsOfType<NetworkedUser>());
        }

        if (view.IsMine)
            {
                positionCounter.text = $"Pos: {PositionTracker.instance.yourPosition}/{PhotonNetwork.PlayerList.Length}";

                if (NetworkManager.spectating)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        var players = FindObjectsOfType<NetworkedUser>();
                        NetworkManager.spectatingIndex--;
                        if (NetworkManager.spectatingIndex < 0)
                            NetworkManager.spectatingIndex = players.Length - 1;
                        Spectate(NetworkManager.spectatingIndex);
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        var players = FindObjectsOfType<NetworkedUser>();
                        NetworkManager.spectatingIndex++;
                        if (NetworkManager.spectatingIndex > players.Length - 1)
                            NetworkManager.spectatingIndex = 0;
                        Spectate(NetworkManager.spectatingIndex);
                    }
                }
            }
    }

    public void ProgressLap()
    {
        view.RPC("RPC_ProgressLap", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_ProgressLap()
    {
        currentLapProgression++;
        nextProgressionPoint++;
        if (view.IsMine)
            progressionCounter.text = $"Progression: {currentLapProgression}/{LapTrigger.instance.progressionTriggers.Count}";
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
        currentLap++;
        finished = currentLap > maxLaps;
        if (view.IsMine)
        {
            Debug.Log("Resetting progression triggers");
            foreach (LapProgressionTrigger progression in LapTrigger.instance.progressionTriggers)
            {
                progression.gameObject.SetActive(true);
            }
        }
        currentLapProgression = 0;
        nextProgressionPoint = 1;

        if (finished)
        {
            var players = FindObjectsOfType<NetworkedUser>();
            Debug.Log($"{userID} finished");

            if (view.IsMine)
            {
                Debug.Log($"{userID} has finished the race");
                lapCounter.text = "Race finished";
                progressionCounter.text = string.Empty;

                audioSource.volume = (audioSource.volume * 1f) * AudioManager.instance.GetMasterVolume();
                audioSource.clip = finishedSound;
                audioSource.loop = true;
                audioSource.Play();

                // Teleport the user to their podeum position after finishing
                body.velocity = Vector3.zero;
                foreach (FinishedPosition placement in FindObjectsOfType<FinishedPosition>())
                {
                    if (placement.id == PositionTracker.instance.yourPosition)
                    {
                        gameObject.transform.position = placement.gameObject.transform.position;
                        gameObject.transform.rotation = placement.gameObject.transform.rotation;
                        body.isKinematic = true;
                        Spectate(players);
                    }
                }
            } else
            {
                if (NetworkManager.spectating)
                {
                    // If the person we are spectating finished
                    if (players[NetworkManager.spectatingIndex].finished)
                    {
                        bool everyoneHasFinished = EveryoneHasFinished(players);
                        if (everyoneHasFinished)
                        {
                            MoveToPodiumView();
                        }
                        else
                        {
                            SpectateRandom(players);
                        }
                    }
                }
            }
        }
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        if (!NetworkManager.spectating)
            return;

        var players = FindObjectsOfType<NetworkedUser>();
        bool everyoneFinished = EveryoneHasFinished(players);
        if (everyoneFinished)
        {
            MoveToPodiumView();
        }
        else
        {
            // If our spectating index goes out of range
            if (NetworkManager.spectatingIndex >= players.Length)
            {
                SpectateRandom(players);
            }
            else
            {
                if(player.ActorNumber != view.OwnerActorNr)
                    Spectate(NetworkManager.spectatingIndex);
            }
        }
    }

    void Spectate(NetworkedUser[] players)
    {
        if (!NetworkManager.spectating)
        {
            UI.SetActive(false);
        }

        if(EveryoneHasFinished(players))
        {
            MoveToPodiumView();
            return;
        }

        if (NetworkManager.spectating)
            return;

        SpectateRandom(players);
        NetworkManager.spectating = true;
    }

    void SpectateRandom(NetworkedUser[] players)
    {
        int index = UnityEngine.Random.Range(0, players.Length);
        while (players[index].finished)
        {
            index = UnityEngine.Random.Range(0, players.Length);
        }
        Spectate(index);
    }

    void Spectate(int playerIndex)
    {
        var players = FindObjectsOfType<NetworkedUser>();
        if(players[playerIndex].finished)
        {
            SpectateRandom(players);
            return;
        }
        DisableCam();
        cam = players[playerIndex].cam;
        NetworkManager.spectatingIndex = playerIndex;
        cam.enabled = true;
    }

    public bool IsMine()
    {
        return view.IsMine;
    }

    /// <summary>
    /// Try to disable the camera - Takes into account if a player left
    /// </summary>
    void DisableCam()
    {
        try
        {
            cam.enabled = false;
        }
        catch (Exception e)
        {

        }
    }

    void MoveToPodiumView()
    {
        DisableCam();
        cam = GameObject.FindGameObjectWithTag("EndCamera").GetComponent<Camera>();
        cam.enabled = true;
    }

    bool EveryoneHasFinished(NetworkedUser[] players)
    {
        for(int i = 0; i < players.Length; i++)
        {
            if (!players[i].finished)
                return false;
        }
        return true;
    }
}
