using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public GameObject player;
    public Transform spawnPoint;

    public GameObject roomCam;

    public GameObject nameUI;
    public GameObject connectingUI;

    public GameObject playerUI;

    private string name = "Default";

    public void ChangeNickname(string _name)
    {
        name = _name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();

        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Connected an in a room.");

        roomCam.SetActive(false);

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);

        _player.GetComponent<PlayerSetup>().IsLocalPlayer();

        GameObject _playerUI = PhotonNetwork.Instantiate(playerUI.name, new Vector3(0,0,0), Quaternion.identity);

        Slider _healthBar = _playerUI.GetComponentInChildren<Slider>();
        TMP_Text _damageText = _playerUI.GetComponentInChildren<TMP_Text>();

        HealthBarLookAt lookAt = _healthBar.GetComponent<HealthBarLookAt>();
        lookAt.target = _player;

        Damage damage = _player.GetComponent<Damage>();
        damage.healthBar = _healthBar;
        damage.damageText = _damageText;
    }
}
