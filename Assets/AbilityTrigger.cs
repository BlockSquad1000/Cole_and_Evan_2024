using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrigger : MonoBehaviour
{
    public bool trigger;
    public bool lashTrigger;

    public int enemiesHit;

    public TiraAbilityScript tiraAbilities;

    public List<GameObject> enemiesInTrigger;

    private void Awake()
    {
        enemiesHit = 0;
    }

    private void Start()
    {
        tiraAbilities = GetComponentInParent<TiraAbilityScript>();
    }

    private void Update()
    {
        if (this.gameObject.transform.parent.tag == "DeadPlayer")
        {
            this.gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy entered trigger zone.");

            enemiesInTrigger.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy left trigger zone.");

            enemiesInTrigger.Remove(other.gameObject);
        }
    }

    public void LashTrigger()
    {
        foreach (GameObject enemy in enemiesInTrigger)
        {
            enemiesHit++;
            enemy.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, tiraAbilities.wDamage, false, true, false, false, tiraAbilities.playerObjectTira);
            Debug.Log(enemy + " took damage from Airbound Slash.");
         //   return;
        }
    }

    public void ScorpionTailTrigger()
    {
        foreach(GameObject enemy in enemiesInTrigger)
        {
            enemiesHit++;
            enemy.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, tiraAbilities.eDamage, false, true, false, false, tiraAbilities.playerObjectTira);
            Debug.Log(enemy + " took damage from Scorpion Tail.");
            //return;
        }
    }

    public void UltimateTrigger()
    {
        foreach (GameObject enemy in enemiesInTrigger)
        {
            enemiesHit++;
            enemy.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, tiraAbilities.rInstant, false, true, false, false, tiraAbilities.playerObjectTira);
            enemy.GetComponent<PlayerStatInitializer>().Displaced(enemy.gameObject.transform.position.x - .1f, enemy.gameObject.transform.position.z - .1f, 1.5f);
            Debug.Log(enemy + " took damage and was knocked airborne from Tira's Ultimate.");
          //  return;
        }
    }

    public void DoTTrigger()
    {
        foreach (GameObject enemy in enemiesInTrigger)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<PlayerStatInitializer>().airborneDuration > 0)
                {
                    enemy.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, (tiraAbilities.rDoT + (tiraAbilities.rDoTHealth * enemy.GetComponent<PlayerStatInitializer>().maxHealth)) * 1.1f, false, true, false, true, tiraAbilities.playerObjectTira);
                }
                else
                {
                    enemy.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, (tiraAbilities.rDoT + (tiraAbilities.rDoTHealth * enemy.GetComponent<PlayerStatInitializer>().maxHealth)), false, true, false, true, tiraAbilities.playerObjectTira);
                }
            }
            else
            {
                Debug.Log("Enemy is not found.");
            }
        }
    }
}
