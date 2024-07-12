using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public int currentLevel = 1;

    public int maxExp;
    public int currentExp;

    public float baseHealthGrowth;
    public float baseAttackGrowth;
    public float baseArmourGrowth;
    public float baseResistanceGrowth;
    public float attackSpeedGrowth;

    public PlayerStatInitializer playerBaseStats;

    // Update is called once per frame
    void Update()
    {
        if(currentExp >= maxExp)
        {
            currentExp -= maxExp;
            currentLevel++;
            playerBaseStats.LevelUpStats();
        }
    }
}
