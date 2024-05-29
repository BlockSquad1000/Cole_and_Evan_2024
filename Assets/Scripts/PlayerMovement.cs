using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Transform playerPos;

    UnityEngine.AI.NavMeshAgent myAgent;
    public bool moving;

    public PlayerStatInitializer playerStats;

    public Transform target;
    public bool targeting;
    public bool attacking;

    [SerializeField] float time = 0f;

    public Damage damage;

    [SerializeField] Camera myCam;
    //public Damage.DamageType attackType;
    public float attackDamage;

    public string[] attackTags;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStatInitializer>();
        myAgent = GetComponent<NavMeshAgent>();
        //GetComponent<NavMeshAgent>().speed = moveSpeed;
        //moveSpeed = playerStats.totalMovementSpeed;
        playerStats.triggerRange.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            playerStats.Revive();
        }

        GetComponent<NavMeshAgent>().speed = playerStats.totalMovementSpeed;
       // moveSpeed = playerStats.totalMovementSpeed;

        Ray camToNavRay = myCam.ScreenPointToRay(Input.mousePosition); //A ray will be positioned at the same point where the player's mouse is positioned
        RaycastHit hit;

        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(camToNavRay, out hit, 100))
            {
                if (playerStats.canMove)
                {
                    if (hit.collider.tag != "Blocked")
                    {
                        moving = true;
                        myAgent.destination = hit.point; //The NavMeshAgent will move towards the point where the Raycast is hit
                        myAgent.Resume();
                        transform.LookAt(hit.point); //The player's transform will turn to look in the direction of the Raycast point
                        playerStats.triggerRange.enabled = false;
                        targeting = false;
                        attacking = false;
                    }

                    if (hit.collider.tag == "Enemy")
                    {
                        if (moving)
                        {
                            myAgent.destination = hit.transform.position;
                            transform.LookAt(hit.transform.position);
                            target = hit.transform;
                            playerStats.triggerRange.enabled = true;
                        }
                        else
                        {
                            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                        }
                    }
                }
            }
        }
        if (!playerStats.canMove)
        {
            myAgent.destination = gameObject.transform.position;
        }

        time += Time.deltaTime;

        if (targeting)
        {
            while (time >= playerStats.attackRate)
            {

                if (playerStats.canAttack)
                {
                    if (target != null)
                    {
                        attacking = true;
                        moving = false;
                        time = 0f;
                        
                    }
                }
                else
                {
                    Debug.Log("You cannot attack right now.");
                    //time = 0f;
                }
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(other.transform == target)
            {
                if(target != null)
                {
                    targeting = true;
                    damage = target.gameObject.GetComponent<Damage>();
                }
                else
                {
                    Debug.Log("Enemy is missing from scene.");
                    damage = null;
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            target = null;
            targeting = false;
            damage = null;
        }
    }
}
