using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class TiraAbilityScript : MonoBehaviourPun
{
    public GameObject playerObjectTira;

    public PlayerMovement movementScript;
    public PlayerStatInitializer playerStats;

    public GameObject airboundSlashTrigger;

    [SerializeField] AirboundSlashTrigger trigger;

    public float qAbilityCooldown = 0;
    public float wAbilityCooldown = 0;
    public float eAbilityCooldown = 0;
    public float rAbilityCooldown = 0;

    public float qDamage;

    public PhotonView view;
    public Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        airboundSlashTrigger.SetActive(false);
        playerObjectTira = this.gameObject;

        view = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void Update()
    {
        qDamage = 40 + (playerStats.totalAttackDamage * 0.7f);

        if (movementScript.attacking)
        {
            BasicAttack();
            Debug.Log("Executing a basic attack.");
        }

        qAbilityCooldown -= Time.deltaTime;
        wAbilityCooldown -= Time.deltaTime;
        eAbilityCooldown -= Time.deltaTime;
        rAbilityCooldown -= Time.deltaTime;

        playerStats.currentAttackRate -= Time.deltaTime;

        Ray camToNavRay = playerCam.ScreenPointToRay(Input.mousePosition); //A ray will be positioned at the same point where the player's mouse is positioned
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Physics.Raycast(camToNavRay, out hit, 100))
            {
                if (playerStats.canCast && qAbilityCooldown <= 0)
                {
                    if (view.IsMine)
                    {
                        transform.LookAt(hit.point);
                        view.RPC("AirboundSlash", RpcTarget.All);
                        qAbilityCooldown = 8 / (1 + playerStats.abilityHaste);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            this.GetComponent<Shields>().ActivatePhysicalShield(3f, playerStats.maxHealth * 0.1f, 0f);
        }
    }

    [PunRPC]
    public void BasicAttack()
    {     
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;    
        
        movementScript.attackDamage = (playerStats.totalAttackDamage);
        movementScript.attacking = false;

        playerStats.CriticalCheck();

        if(playerStats.hitCount == 1)
        {
            if (playerStats.hit1)
            {
                movementScript.target.transform.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage * 1.5f, true, false, false, false, playerObjectTira);
                Debug.Log("Landed a critical hit.");
            }
            else
            {
                movementScript.target.transform.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage, true, false, false, false, playerObjectTira);
            }
        }
        else if(playerStats.hitCount == 2)
        {
            if (playerStats.hit2)
            {
                movementScript.target.transform.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage * 1.5f, true, false, false, false, playerObjectTira);
                Debug.Log("Landed a critical hit.");
            }
            else
            {
                movementScript.target.transform.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage, true, false, false, false, playerObjectTira);
            }
        }
        else if (playerStats.hitCount == 3)
        {
            if (playerStats.hit3)
            {
                movementScript.target.transform.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage * 1.5f, true, false, false, false, playerObjectTira);
                Debug.Log("Landed a critical hit.");
            }
            else
            {
                movementScript.target.transform.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage, true, false, false, false, playerObjectTira);
            }
        }
        else 
        {
            if (playerStats.hit4)
            {
                movementScript.target.transform.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage * 1.5f, true, false, false, false, playerObjectTira);
                Debug.Log("Landed a critical hit.");
            }
            else
            {
                movementScript.target.transform.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage, true, false, false, false, playerObjectTira);
            }
        }
    }

    [PunRPC]
    public void AirboundSlash()
    {
        playerStats.Rooted(0.75f / playerStats.tenacity);
        playerStats.Disarmed(0.75f / playerStats.tenacity);

        airboundSlashTrigger.SetActive(true);
        // GameObject go = Instantiate(airboundSlashTrigger, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
        //  go.transform.parent = this.transform;

        // trigger = go.GetComponentInChildren<AirboundSlashTrigger>();

        StartCoroutine(AirboundSlashCheck());
        Debug.Log("Beginning Airbound Slash.");
    }

    IEnumerator AirboundSlashCheck()
    {
        yield return new WaitForSeconds(0.75f);
       // trigger.airboundTrigger = true;
        airboundSlashTrigger.GetComponentInChildren<AirboundSlashTrigger>().Trigger();
        Debug.Log("Airbound Slash trigger activated.");
        yield return new WaitForSeconds(0.02f);
       // trigger.airboundTrigger = false;
        airboundSlashTrigger.SetActive(false);
        Debug.Log("Airbound Slash trigger deactivated.");
    }
}
                                                