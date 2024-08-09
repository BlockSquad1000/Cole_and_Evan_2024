using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChannelingChecker : MonoBehaviour
{
    [SerializeField] PlayerStatInitializer playerStats;
    [SerializeField] PlayerMovement playerMovement;
    public float maxChannelTimer;
    public float currentTime;
    public bool isChanneling;

    // Start is called before the first frame update
    void Start()
    {
        isChanneling = false;
        playerStats = this.GetComponent<PlayerStatInitializer>();
        playerMovement = this.GetComponent<PlayerMovement>();
    }

    public void SetChanneling(float channelTime)
    {
        maxChannelTimer = channelTime;
        currentTime = maxChannelTimer;
        isChanneling = true;
        playerMovement.moving = false;
        this.GetComponent<NavMeshAgent>().isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChanneling)
        {
            currentTime -= Time.deltaTime;

            if(currentTime <= 0 || !playerStats.canCast || this.GetComponent<NavMeshAgent>().isStopped)
            {
                isChanneling = false;
            }
        }
    }
}
