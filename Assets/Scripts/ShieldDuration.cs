using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDuration : MonoBehaviour
{
    public float maxTime;
    public float currentTime;
    public float maxCapacity;
    public float currentCapacity;
    public float decayRate;

    public void Start()
    {
        currentTime = maxTime;
        currentCapacity = maxCapacity;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        currentCapacity = currentCapacity - decayRate;

        if(currentTime <= 0 || currentCapacity <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void DamageShield(float damage, Damage.DamageType damageType)
    {
        currentCapacity -= damage;

        if(currentCapacity < 0)
        {
            this.GetComponentInParent<PlayerStatInitializer>().Damage(-currentCapacity, damageType);
            Destroy(this.gameObject);
        }
    }
}
