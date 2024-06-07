using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollowPlayer : MonoBehaviourPun
{
    //public GameObject player;
    [SerializeField] GameObject playerPos;

    bool followingPlayer;
    bool canEdgeScroll;

    Vector3 cameraPos = new Vector3(0, 21, -5);

    [SerializeField] GameObject myCam;

    public float edgeSize = 30f;
    public float moveAmount = 100f;

    [SerializeField] PhotonView view;

    private void Start()
    {
        canEdgeScroll = true;

        //myCam = GetComponentInChildren<Camera>();
        //playerPos = GetComponentInChildren
        //view = GetComponentInChildren<PhotonView>();
    }

    void FindPlayer()
    {
        myCam.transform.position = playerPos.transform.position + cameraPos;
        //Debug.Log("Finding player...");
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
            if (view.IsMine)
            {
                FindPlayer();
            }
        }

       // if (!followingPlayer)
       // {
            //playerPos = null;
       // }

        if (canEdgeScroll)
        {
            float mousePosX = Input.mousePosition.x;
            float mousePosY = Input.mousePosition.y;

            if (mousePosX < edgeSize)
            {
                myCam.transform.Translate(Vector3.right * -moveAmount * Time.deltaTime);
            }

            if (mousePosX >= Screen.width - edgeSize)
            {
                myCam.transform.Translate(Vector3.right * moveAmount * Time.deltaTime);
            }

            if (mousePosY < edgeSize)
            {
                myCam.transform.position = new Vector3(myCam.transform.position.x, myCam.transform.position.y, myCam.transform.position.z + 1 * -moveAmount * Time.deltaTime);
            }

            if (mousePosY >= Screen.height - edgeSize)
            {
                myCam.transform.position = new Vector3(myCam.transform.position.x, myCam.transform.position.y, myCam.transform.position.z + 1 * moveAmount * Time.deltaTime);
            }
        }

        //if(player = null)
        //{
          //  Debug.Log("Camera cannot find player right now");
        //}
    }
}
