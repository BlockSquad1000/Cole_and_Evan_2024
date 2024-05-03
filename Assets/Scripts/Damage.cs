using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class Damage : MonoBehaviourPun
{
    public enum DamageType
    {
        Physical = 0,
        Magical = 1,
        True = 2
    }

    public GameObject attackerObj;

    public PlayerStatInitializer attackerStats;
    public PlayerStatInitializer defenderStats;

    [SerializeField] float totalDamageValue;
    [SerializeField] float armour;
    [SerializeField] float resistance;
    [SerializeField] float penetration;
    [SerializeField] float percentPenetration;

    public Damage.DamageType damageType;

    public TMP_Text damageText;
    public Slider healthBar;

   /* private void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
    }*/

    private void Start()
    {
        damageText.enabled = false;
        //defenderStats.GetComponent<PhotonView>().RPC("SetHealthBar", RpcTarget.AllBuffered, defenderStats.maxHealth);
        SetHealthBar(defenderStats.maxHealth);
    }

    public void DamageCalculation(DamageType currentDamageType, float damageValue, bool onHit, bool abilityDamage, bool ultimateDamage, bool periodicDamage, GameObject attacker)
    {
        attackerObj = attacker;
        attackerStats = attacker.GetComponent<PlayerStatInitializer>();
        

        if (defenderStats != null && attackerStats != null)
        {
            if (onHit)
            {
                attackerStats.ApplyOnHitEffects();
                defenderStats.ReceiveOnHitEffects();
            }

            if (abilityDamage)
            {
                attackerStats.ApplyAbilityDamageEffects();
                defenderStats.ReceiveAbilityDamageEffects();
            }

            if (ultimateDamage)
            {
                attackerStats.ApplyUltimateDamageEffects();
                defenderStats.ReceiveUltimateDamageEffects();
            }

            if (periodicDamage)
            {
                attackerStats.ApplyPeriodicDamageEffects();
                defenderStats.ReceivePeriodicDamageEffects();
            }

            if (currentDamageType == DamageType.Physical)
            {
                armour = defenderStats.totalArmour;
                penetration = attackerStats.flatArmourPenetration;
                percentPenetration = attackerStats.percentArmourPenetration;

                float calcArmour = (armour * percentPenetration) - penetration;
                totalDamageValue = damageValue / (1 + (calcArmour / 100));

                //defenderStats.Damage(totalDamageValue, currentDamageType);

                defenderStats.GetComponent<PhotonView>().RPC("Damage", RpcTarget.AllBuffered, totalDamageValue, currentDamageType);

                damageText.color = Color.red;
            }

            if (currentDamageType == DamageType.Magical)
            {
                resistance = defenderStats.totalResistance;
                penetration = attackerStats.flatResistancePenetration;
                percentPenetration = attackerStats.percentResistancePenetration;

                float calcResistance = (resistance * percentPenetration) - penetration;
                totalDamageValue = damageValue / (1 + (calcResistance / 100));

                //defenderStats.Damage(totalDamageValue, currentDamageType);

                defenderStats.GetComponent<PhotonView>().RPC("Damage", RpcTarget.AllBuffered, totalDamageValue, currentDamageType);

                damageText.color = Color.green;
            }

            if (currentDamageType == DamageType.True)
            {
                totalDamageValue = damageValue;

                //defenderStats.Damage(totalDamageValue, currentDamageType);

                defenderStats.GetComponent<PhotonView>().RPC("Damage", RpcTarget.AllBuffered, totalDamageValue, currentDamageType);

                damageText.color = Color.blue;
            }

            StartCoroutine(DamageTextUI());

            defenderStats.GetComponent<PhotonView>().RPC("UpdateHealthBar", RpcTarget.AllBuffered, defenderStats.currentHealth);
            //UpdateHealthBar(defenderStats.currentHealth);
        }
        else
        {
            Debug.Log("Missing attacker or defender in the scene.");
        }     
    }

    IEnumerator DamageTextUI()
    {
        damageText.text = totalDamageValue.ToString();
        damageText.enabled = true;

        //float x = Random.Range(minX, maxX);
        //float z = Random.Range(minZ, maxZ);
        damageText.transform.position = this.transform.position + new Vector3(0, 1.5f, 1.5f);
       // damageText.transform.rotation = Quaternion.identity;
        //damageText.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        Animator textAnim = damageText.GetComponent<Animator>();
        textAnim.SetBool("Damaged", true);
        yield return new WaitForSeconds(0.5f);
        damageText.enabled = false;
        textAnim.SetBool("Damaged", false);
    }

    //[PunRPC]
    public void SetHealthBar(float health)
    {
      //  if (photonView.IsMine)
       // {
            healthBar.maxValue = health;
            healthBar.value = health;
      //  }
    }

    [PunRPC]
    public void UpdateHealthBar(float health)
    {
       // if (photonView.IsMine)
       // {
            healthBar.value = health;
      //  }
    }
}
