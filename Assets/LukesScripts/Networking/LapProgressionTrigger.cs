using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapProgressionTrigger : MonoBehaviour
{

    public int progressionID { get; set; } = -1;

    private void OnTriggerEnter(Collider other)
    {
        GameObject root = other.transform.root.gameObject;
        if(root.GetComponent<PhotonView>())
        {
            if(root.GetComponent<PhotonView>().IsMine)
            {
                NetworkedUser user = root.GetComponent<NetworkedUser>();
                user.ProgressLap();
                gameObject.SetActive(false);
            }
        }
    }
}
