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
                NetworkedUser user = root.GetComponent<NetworkedUser>();
                user.currentLapProgression++;
                user.progressionCounter.text = $"Progression: {user.currentLapProgression}/{LapTrigger.instance.progressionTriggers.Count}";
                gameObject.SetActive(false);
            }
        }
    }
}
