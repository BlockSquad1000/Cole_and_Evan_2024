using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy_Behaviour_Melee : MonoBehaviour
{
    public GameObject enemyObj;

    public Transform enemyPos;
    public PlayerStatInitializer enemyStats;
    public float moveSpeed;
    public NavMeshAgent myAgent;

    public Transform player;
    public bool targeting;
    public bool attacking;

    public Damage damage;

    public float attackDamage;

    [SerializeField] float time = 0f;

    public Enemy_AI_Movement enemyAI;
    [SerializeField] private PlayerStatInitializer playerStats;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<PlayerStatInitializer>();
        enemyAI = GetComponent<Enemy_AI_Movement>();
        moveSpeed = enemyStats.totalMovementSpeed;
        //enemyStats.triggerRange.enabled = false;
    }

    void Update()
    {
        if (player != null)
        {
            enemyAI.playerPos = player.transform;
            enemyAI.following = true;
            enemyAI.walkPointSet = false;
        }

       /* if (!enemyStats.canMove)
        {
            myAgent.destination = gameObject.transform.position;
        }*/

        time += Time.deltaTime;

        if (targeting)
        {
            while (time >= enemyStats.attackRate)
            {
                if (enemyStats.canAttack)
                {
                    if (player.transform.tag == "Player")
                    {
                        attacking = true;
                        enemyAI.moving = false;
                        time = 0f;
                    }
                    else if (player.transform.tag == "DeadPlayer")
                    {
                        Debug.Log("Player is dead.");
                        targeting = false;
                        player = null;
                        enemyAI.moving = true;
                        enemyAI.following = false;
                    }
                }
                else
                {
                    Debug.Log(this.name + "cannot attack right now.");
                }
            }
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
            attacking = false;
            enemyAI.following = false;
            //enemyAI.FindDestination();
        }

        if (attacking)
        {
            BasicAttack();
            //Debug.Log(this.name + "is executing a basic attack.");
            attacking = false;
        }
    }

    public void BasicAttack()
    {
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;

        attackDamage = (enemyStats.totalAttackDamage);
        player.gameObject.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, attackDamage, true, false, false, false, enemyObj);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject.transform;
            //Debug.Log("Found player.");
            {
                if (player != null)
                {
                    targeting = true;
                    enemyAI.following = true;
                    
                }
                else
                {
                    Debug.Log("Player is missing from scene.");
                    targeting = false;
                    player = null;
                    enemyAI.following = false;
                }
            }      
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = null;
            targeting = false;
            enemyAI.following = false;
        }
    }
}
