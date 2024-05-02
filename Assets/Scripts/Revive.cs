using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Revive : MonoBehaviourPun
{
    public int revivalItems;
    public GameObject otherPlayer;
    public PlayerStatInitializer playerStats;
    public bool inPlayerRange = false;

    // Start is called before the first frame update
    void Start()
    {
        revivalItems = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //if (inPlayerRange)
            //{
                if (playerStats.dead)
                {
                    if (revivalItems > 0)
                    {
                        otherPlayer.gameObject.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
                        revivalItems--;
                        Debug.Log("Used a healing item to revive.");
                    }
                    else
                    {
                        Debug.Log("Not enought healing items in inventory.");
                    }
                }
                else
                {
                    Debug.Log("Player is already alive.");
                }
            //}
            //else
           // {
           //     Debug.Log("You are not in the range of another player.");
          //  }
        }
    }

    public void AddRevivalItem()
    {
        revivalItems++;
        Debug.Log("Picked up a revival item.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "RevivalItem")
        {
            PhotonNetwork.Destroy(other.gameObject);
            AddRevivalItem();
        }

        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            otherPlayer = other.gameObject;
            playerStats = other.GetComponent<PlayerStatInitializer>();
        }
    }
}
