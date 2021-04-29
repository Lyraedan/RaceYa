using Dissonance;
using Dissonance.Integrations.PhotonUnityNetworking2;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MicrophoneController : MonoBehaviour
{

    public static MicrophoneController instance;

    [Header("Components")]
    public DissonanceComms dissonanceComms;
    public PhotonCommsNetwork photonCommsNetwork;
    public VoiceReceiptTrigger voiceReceiptTrigger;
    public VoiceBroadcastTrigger voiceBroadcastTrigger;
    public Button button;
    public Canvas canvas;

    [Header("Events")]
    public UnityEvent OnMicTransmitting;
    public UnityEvent OnMicNotTransmitting;
    public UnityEvent OnMicMuted;
    public UnityEvent OnMicUnmuted;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if (!voiceBroadcastTrigger.IsMuted)
            {
                voiceBroadcastTrigger.IsMuted = true;
                OnMicMuted.Invoke();
            }
            else
            {
                voiceBroadcastTrigger.IsMuted = false;
                OnMicUnmuted.Invoke();
            }
        });
    }

    void Update()
    {
        if(!voiceBroadcastTrigger.IsMuted)
        {
            if(voiceBroadcastTrigger.IsTransmitting)
            {
                OnMicTransmitting.Invoke();
            } else
            {
                OnMicNotTransmitting.Invoke();
            }
        }
    }
}
