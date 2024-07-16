using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LashTrigger : MonoBehaviour
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

    public void Trigger()
    {
        foreach (GameObject enemy in enemiesInTrigger)
        {
            enemiesHit++;
            enemy.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, tiraAbilities.wDamage, false, true, false, false, tiraAbilities.playerObjectTira);
            Debug.Log(enemy + " took damage from Airbound Slash.");
            return;
        }
    }
}