using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Enemy_AI_Movement : MonoBehaviour
{
    public Vector3 point;
    public NavMeshAgent myAgent;
    public Transform playerPos;

    public PlayerStatInitializer enemyStats;

    public bool walkPointSet;
    public bool following;
    public bool patrolling;

    public bool moving = true;

    PhotonView view;

    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<PlayerStatInitializer>();

        view = GetComponent<PhotonView>();
    }

    [PunRPC]
    private void Update()
    {
        GetComponent<NavMeshAgent>().speed = enemyStats.totalMovementSpeed;

        if (!following)
        {
            if (!walkPointSet)
            {
                view.RPC("FindDestination", RpcTarget.AllBuffered);
            }

            if (walkPointSet)
            {
                myAgent.destination = point;

                Vector3 distanceToDestination = transform.position - point;

                if (distanceToDestination.magnitude < 1f)
                {
                    walkPointSet = false;
                }
            }
        }
       
        else
        {
            if(playerPos != null)
            {
                myAgent.destination = playerPos.transform.position;
                transform.LookAt(playerPos.transform.position);
                //Debug.Log("Currently following " + playerPos.name);
            }
            else
            {
                Debug.Log("The player is missing from the scene.");
                following = false;
            }
        }
    }

    [PunRPC]
    public void FindDestination()
    {
        float randomZ = Random.Range(-5, 5);
        float randomX = Random.Range(-5, 5);

        point = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
        //Debug.Log("Finding a new target destination of " + point);
    }

    public void Stop()
    {

    }
}
