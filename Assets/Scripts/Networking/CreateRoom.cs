using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text roomName;

    public void OnClickCreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 5;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully.", this);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message, this);
    }
}

