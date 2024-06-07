using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirboundSlashTrigger : MonoBehaviour
{
    public bool inTriggerZone;
    public bool airboundTrigger;

    public TiraAbilityScript tiraAbilities;

    public List<GameObject> enemiesInTrigger;

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
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy left trigger zone.");

            enemiesInTrigger.Remove(other.gameObject);
        }
    }

    public void Trigger()
    {
        foreach (GameObject enemy in enemiesInTrigger)
        {
            enemy.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, tiraAbilities.qDamage, false, true, false, false, tiraAbilities.playerObjectTira);
            Debug.Log(enemy + " took damage from Airbound Slash.");
            return;
        }
    }
}
