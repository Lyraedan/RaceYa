using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapProgressionTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameObject root = other.transform.root.gameObject;
        if(root.GetComponent<PhotonView>())
        {
            if(root.GetComponent<PhotonView>().IsMine)
            {
                root.GetComponent<NetworkedUser>().currentLapProgression++;
                gameObject.SetActive(false);
            }
        }
    }
}
