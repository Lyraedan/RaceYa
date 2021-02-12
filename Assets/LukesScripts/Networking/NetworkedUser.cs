using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedUser : MonoBehaviour
{

    public GameObject camera;

    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if(!view.IsMine)
        {
            Destroy(camera);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
