using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using TMPro;

public class TiraAbilityScript : MonoBehaviourPun
{
    public GameObject playerObjectTira;

    public PlayerMovement movementScript;
    public PlayerStatInitializer playerStats;

    public GameObject airboundSlashTrigger;
    public GameObject lashTrigger;

    [SerializeField] AirboundSlashTrigger airboundTriggerVar;
    [SerializeField] LashTrigger lashTriggerVar;

    public float qAbilityCooldown = 0;
    public float wAbilityCooldown = 0;
    public float eAbilityCooldown = 0;
    public float rAbilityCooldown = 0;

    public bool speedBoostIsActive = false;

    public Transform eTarget;

    public float qDamage;
    public float wDamage;
    public float eDamage;
    public float rInstant;
    public float rDoT;

    public PhotonView view;
    public Camera playerCam;

    public RankingSystem ranks;

    // Start is called before the first frame update
    void Awake()
    {
        airboundSlashTrigger.SetActive(false);
        lashTrigger.SetActive(false);
        playerObjectTira = this.gameObject;

        view = GetComponent<PhotonView>();

        ranks = GameObject.Find("GeneralUICanvas").GetComponentInChildren<RankingSystem>();
    }

    [PunRPC]
    public void Update()
    {
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
                if (playerStats.canCast && qAbilityCooldown <= 0 && ranks.qRank > 0)
                {
                    if (view.IsMine)
                    {
                        qDamage = 40 + (playerStats.totalAttackDamage * 0.7f) + (ranks.qRank * 30);
                        transform.LookAt(hit.point);
                        view.RPC("AirboundSlash", RpcTarget.All);
                        qAbilityCooldown = 8 / (1 + playerStats.abilityHaste);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (wAbilityCooldown <= 0 && ranks.wRank > 0)
            {
                playerStats.stunnedDuration = 0;
                playerStats.rootedDuration = 0;
                playerStats.silencedDuration = 0;
                playerStats.disarmedDuration = 0;

                StartCoroutine(SpeedBoost());

                wAbilityCooldown = (15f - ranks.wRank) / (1 + playerStats.abilityHaste);
            }
            else if (wAbilityCooldown > 0)
            {
                if (speedBoostIsActive)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (playerStats.canCast)
                        {
                            Lash();
                            speedBoostIsActive = false;
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(camToNavRay, out hit, 100))
            {           
                if (playerStats.canCast && eAbilityCooldown <= 0 && ranks.eRank > 0)
                {
                    if(hit.collider.tag == "Enemy")
                    {
                        eTarget = hit.transform;
                        eTarget.GetComponent<PlayerStatInitializer>().Displaced(eTarget.gameObject.transform.position.x - .1f, eTarget.gameObject.transform.position.z - .1f, 1.0f);
                    }
                    else if (hit.collider.tag == "Player")
                    {
                        eTarget = hit.transform;   
                    }
                }
            }
        }
    }

    IEnumerator SpeedBoost()
    {
        speedBoostIsActive = true;
        float tempAbilityPower = playerStats.abilityPower;
        playerStats.movementSpeedMultiplier = (playerStats.movementSpeedMultiplier + 0.5f + (0.01f * tempAbilityPower));
        yield return new WaitForSeconds(4.0f);
        playerStats.movementSpeedMultiplier = (playerStats.movementSpeedMultiplier - (0.5f + (0.01f * tempAbilityPower)));
        speedBoostIsActive = false;
    }

    /*IEnumerator LashAbility()
    {
        yield return new WaitForSeconds(0.5f);

        if(speedBoostIsActive)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (playerStats.canCast)
                {
                    Lash();
                    speedBoostIsActive = false;
                }
            }
        }
    }*/

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
                movementScript.target.transform.GetComponentInParent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage * 1.5f, true, false, false, false, playerObjectTira);
                Debug.Log("Landed a critical hit.");
            }
            else
            {
                movementScript.target.transform.GetComponentInParent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage, true, false, false, false, playerObjectTira);
            }
        }
        else if(playerStats.hitCount == 2)
        {
            if (playerStats.hit2)
            {
                movementScript.target.transform.GetComponentInParent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage * 1.5f, true, false, false, false, playerObjectTira);
                Debug.Log("Landed a critical hit.");
            }
            else
            {
                movementScript.target.transform.GetComponentInParent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage, true, false, false, false, playerObjectTira);
            }
        }
        else if (playerStats.hitCount == 3)
        {
            if (playerStats.hit3)
            {
                movementScript.target.transform.GetComponentInParent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage * 1.5f, true, false, false, false, playerObjectTira);
                Debug.Log("Landed a critical hit.");
            }
            else
            {
                movementScript.target.transform.GetComponentInParent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage, true, false, false, false, playerObjectTira);
            }
        }
        else 
        {
            if (playerStats.hit4)
            {
                movementScript.target.transform.GetComponentInParent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage * 1.5f, true, false, false, false, playerObjectTira);
                Debug.Log("Landed a critical hit.");
            }
            else
            {
                movementScript.target.transform.GetComponentInParent<Damage>().DamageCalculation(Damage.DamageType.Physical, movementScript.attackDamage, true, false, false, false, playerObjectTira);
            }
        }
    }

    [PunRPC]
    public void AirboundSlash()
    {
        playerStats.Rooted(0.75f / playerStats.tenacity);
        playerStats.Disarmed(0.75f / playerStats.tenacity);

        airboundSlashTrigger.SetActive(true);

        StartCoroutine(AirboundSlashCheck());
        Debug.Log("Beginning Airbound Slash.");
    }

    public void Lash()
    {
        lashTrigger.SetActive(true);
        StartCoroutine(LashCheck());
        Debug.Log("Beginning Lash.");
    }

    IEnumerator LashCheck()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Lash trigger activated.");
        wDamage = 20 + (20 * ranks.wRank) + (playerStats.totalAttackDamage * 0.3f);
        lashTriggerVar.Trigger();
        yield return new WaitForSeconds(0.01f);
        float shieldStrength;
        if (lashTriggerVar.enemiesHit > 0)
        {
            shieldStrength = 20f + (ranks.wRank * 30) + (playerStats.abilityPower * 0.5f) + (lashTriggerVar.enemiesHit * playerStats.maxHealth * 0.04f);
        }
        else
        {
            shieldStrength = 0f;
        }     
        this.GetComponent<Shields>().ActivateGeneralShield(5f, shieldStrength, 0f);
        lashTrigger.SetActive(false);
        Debug.Log("Lash trigger deactivated.");
    }

    IEnumerator AirboundSlashCheck()
    {
        yield return new WaitForSeconds(0.75f);
        airboundTriggerVar = GetComponentInChildren<AirboundSlashTrigger>();
        airboundTriggerVar.airboundTrigger = true;
        airboundTriggerVar.Trigger();
        airboundSlashTrigger.GetComponentInChildren<AirboundSlashSweetSpot>().Trigger();
        Debug.Log("Airbound Slash trigger activated.");
        yield return new WaitForSeconds(0.02f);
        airboundTriggerVar.airboundTrigger = false;
        airboundTriggerVar.enemiesInTrigger.Clear();
        airboundTriggerVar.GetComponentInChildren<AirboundSlashSweetSpot>().enemiesInTrigger.Clear();
        airboundSlashTrigger.SetActive(false);
        Debug.Log("Airbound Slash trigger deactivated.");
    }
}
                                                