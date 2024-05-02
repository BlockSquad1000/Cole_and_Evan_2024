using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirboundSlashTrigger : MonoBehaviour
{
    public bool inTriggerZone;
    public bool airboundTrigger;

    public TiraAbilityScript tiraAbilities;

    private void Start()
    {
        tiraAbilities = GetComponentInParent<TiraAbilityScript>();
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy entered trigger zone.");
            if (airboundTrigger)
            {               
                 other.GetComponent<Damage>().DamageCalculation(Damage.DamageType.Physical, tiraAbilities.qDamage, false, true, false, false, tiraAbilities.playerObjectTira);
                 Debug.Log(other + "took damage from Airbound Slash.");
            }
            else
            {
                Debug.Log("Airbound Slash is not triggered.");
            }
        }
    }
}
