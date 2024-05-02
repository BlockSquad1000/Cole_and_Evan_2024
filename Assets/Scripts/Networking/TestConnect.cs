using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestConnect : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server.");
        print(PhotonNetwork.LocalPlayer.NickName);

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server. Reason: " + cause.ToString());
    }

    public override void OnJoinedLobby()
    {

        PhotonNetwork.JoinOrCreateRoom("test", null, null);

        Debug.Log("In a lobby.");
    }
}
