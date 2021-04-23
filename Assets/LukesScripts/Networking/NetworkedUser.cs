using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class NetworkedUser : MonoBehaviour
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
    // Has the user finished the race
    public bool finished { get; set; } = false;
    // Has the race started
    public bool started { get; set; } = false;

    public string userID => PhotonNetwork.LocalPlayer.NickName;

    public Color carColour = Color.green;
    public Color characterColour = Color.red;

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
        worldspaceCanvas.transform.LookAt(cam.transform.position);
        worldspaceCanvas.transform.Rotate(new Vector3(0, 180, 0));
        worldspaceCanvas.gameObject.SetActive(Vector3.Distance(transform.position, worldspaceCanvas.transform.position) < 3);

        if (view.IsMine)
            positionCounter.text = $"Pos: {PositionTracker.instance.yourPosition}/{PhotonNetwork.PlayerList.Length}";
    }

    public void ProgressLap()
    {
        view.RPC("RPC_ProgressLap", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_ProgressLap()
    {
        currentLapProgression++;
        if(view.IsMine)
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

            // Teleport the user to their podeum position after finishing
            body.velocity = Vector3.zero;
            foreach(FinishedPosition placement in FindObjectsOfType<FinishedPosition>())
            {
                if(placement.id == PositionTracker.instance.yourPosition)
                {
                    gameObject.transform.position = placement.gameObject.transform.position;
                    gameObject.transform.rotation = placement.gameObject.transform.rotation;
                    body.isKinematic = true;
                    cam.enabled = false;
                    UI.SetActive(false);
                    cam = GameObject.FindGameObjectWithTag("EndCamera").GetComponent<Camera>();
                    cam.enabled = true;
                }
            }
        }
    }
}
