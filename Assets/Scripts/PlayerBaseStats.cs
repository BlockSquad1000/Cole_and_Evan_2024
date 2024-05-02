using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Characters", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class PlayerBaseStats : ScriptableObject
{
    public string characterName;

    public float baseHealth;
    public float baseAttackDamage;
    public float attackInterval;
    public float attackSpeed;

    public float baseArmour;
    public float baseResistance;
    public float baseRange;
    public float baseMovementSpeed;
}
