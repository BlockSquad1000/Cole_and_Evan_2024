using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    Transform playerPos;

    bool followingPlayer;
    bool canEdgeScroll;

    Vector3 cameraPos = new Vector3(0, 21, -5);

    public Camera myCam;

    public float edgeSize = 30f;
    public float moveAmount = 100f;

    private void Start()
    {
        canEdgeScroll = true;
        myCam = GetComponent<Camera>();
        
    }

    void FindPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PhotonView>().IsMine)
        {
            player = GameObject.FindGameObjectWithTag("PlayerCam");
            playerPos = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            followingPlayer = true;
            canEdgeScroll = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            followingPlayer = false;
            canEdgeScroll = true;
        }

        if (followingPlayer)
        {
            FindPlayer();
            transform.position = playerPos.position + cameraPos;
        }

        if (!followingPlayer)
        {
            player = null;
        }

        if (canEdgeScroll)
        {
            float mousePosX = Input.mousePosition.x;
            float mousePosY = Input.mousePosition.y;

            if (mousePosX < edgeSize)
            {
                transform.Translate(Vector3.right * -moveAmount * Time.deltaTime);
            }

            if (mousePosX >= Screen.width - edgeSize)
            {
                transform.Translate(Vector3.right * moveAmount * Time.deltaTime);
            }

            if (mousePosY < edgeSize)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1 * -moveAmount * Time.deltaTime);
            }

            if (mousePosY >= Screen.height - edgeSize)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1 * moveAmount * Time.deltaTime);
            }
        }

        if(player = null)
        {
            Debug.Log("Camera cannot find player right now");
        }
    }
}
