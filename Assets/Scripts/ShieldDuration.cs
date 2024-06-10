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

        if(currentTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
