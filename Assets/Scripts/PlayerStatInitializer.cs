using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System.Linq;

public class PlayerStatInitializer : MonoBehaviourPun
{
    public string characterName;

    [Header("Health Stats")]
    public float baseHealth;
    public float currentHealth;
    public float bonusHealth;
    public float maxHealth;

    [Header("Attack Stats")]
    public float baseAttackDamage;
    public float bonusAttackDamage;
    public float attackMultiplier;
    public float totalAttackDamage;
    public float attackInterval;
    public float attackSpeed;
    public float attackRate;
    public float currentAttackRate;
    public float abilityPower;
    public float critRate;

    [Header("Armour Stats")]
    public float baseArmour;
    public float bonusArmour;
    public float armourMultiplier;
    public float totalArmour;
    public float baseResistance;
    public float bonusResistance;
    public float resistanceMultiplier;
    public float totalResistance;

    [Header("Range Stats")]
    public SphereCollider triggerRange;
    public float baseRange;
    public float bonusRange;
    public float rangeMultiplier;
    public float totalRange;

    [Header("Movement Stats")]
    public float baseMovementSpeed;
    public float bonusMovementSpeed;
    public float movementSpeedMultiplier;
    public float totalMovementSpeed;

    [Header("Functional Stats")]
    public float abilityHaste;
    public float tenacity;
    public float superiority;
    public float healAndShieldPower;

    [Header("Penetrative Stats")]
    public float flatArmourPenetration;
    public float percentArmourPenetration;
    public float flatResistancePenetration;
    public float percentResistancePenetration;

    public int expDrop;

    public LevelUp statGrowths;

    [Header("Crowd Control Durations")]
    public float slowedDuration;
    public float rootedDuration;
    public float silencedDuration;
    public float disarmedDuration;
    public float stunnedDuration;
    public float charmedDuration;
    public float tauntedDuration;
    public float airborneDuration;

    [Header("Crowd Control Bools")]
    public bool canAttack = true;
    public bool canMove = true;
    public bool canCast = true;

    [Header("Displacement Variables")]
    public float displacementRateCombined;
    public Vector3 startPos;
    public Vector3 newPos;
    public Collider playerCollider;

    [Header("Crit Values")]
    public int hitCount = 0;
    public bool[] hits;
    public bool doesCrit;

    public bool dead;

    public List<GameObject> generalShields = new List<GameObject>();
    public List<GameObject> magicShields = new List<GameObject>();
    public List<GameObject> physicalShields = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = (baseHealth + bonusHealth);
        currentHealth += maxHealth;
        triggerRange.radius = baseRange;
        attackRate = (attackInterval / attackSpeed);
        currentAttackRate = attackRate;
    }

    private void Update()
    {
        maxHealth = (baseHealth + bonusHealth);
         
        if (canAttack)
        {
            totalAttackDamage = (baseAttackDamage + bonusAttackDamage) * attackMultiplier;
        }
        else
        {
            totalAttackDamage = 0;
        }

        totalArmour = (baseArmour + bonusArmour) * armourMultiplier;
        totalResistance = (baseResistance + bonusResistance) * resistanceMultiplier;
        totalRange = (baseRange + bonusRange) * rangeMultiplier;

        if (canMove)
        {
            totalMovementSpeed = (baseMovementSpeed + bonusMovementSpeed) * movementSpeedMultiplier;
        }
        else
        {
            totalMovementSpeed = 0;
        }

        triggerRange.radius = totalRange;
        attackRate = (attackInterval / attackSpeed);

        slowedDuration = slowedDuration - Time.deltaTime;
        rootedDuration = rootedDuration - Time.deltaTime;
        silencedDuration = silencedDuration - Time.deltaTime;
        disarmedDuration = disarmedDuration - Time.deltaTime;
        stunnedDuration = stunnedDuration - Time.deltaTime;
        charmedDuration = charmedDuration - Time.deltaTime;
        tauntedDuration = tauntedDuration - Time.deltaTime;
        airborneDuration = airborneDuration - Time.deltaTime;

        if (slowedDuration == 0)
        {
            movementSpeedMultiplier = (movementSpeedMultiplier * 2f);
        }

        if (rootedDuration <= 0 && stunnedDuration <= 0)
        {
            canMove = true;
        }

        if(disarmedDuration <= 0 && stunnedDuration <= 0)
        {
            canAttack = true;
        }

        if(silencedDuration <= 0 && stunnedDuration <= 0)
        {
            canCast = true;
        }

        if(airborneDuration > 0)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, newPos, displacementRateCombined * Time.deltaTime);
        }

        else
        {
            if (playerCollider.isTrigger)
            {
                //playerCollider.isTrigger = false;
            }
        }

        foreach(GameObject shield in generalShields)
        {
            if(shield == null)
            {
                generalShields.Remove(shield);
            }
        }

        foreach (GameObject shield in physicalShields)
        {
            if (shield == null)
            {
                physicalShields.Remove(shield);
            }
        }

        foreach (GameObject shield in magicShields)
        {
            if (shield == null)
            {
                magicShields.Remove(shield);
            }
        }
    }

    public void AddShieldsToList()
    {
        foreach (Transform transform in this.transform)
        {
            if (transform.CompareTag("GeneralShield"))
            {
                generalShields.Add(transform.gameObject);
            }
            if (transform.CompareTag("PhysShield"))
            {
                physicalShields.Add(transform.gameObject);
            }
            if (transform.CompareTag("MagicShield"))
            {
                magicShields.Add(transform.gameObject);
            }
        }
    }

    public void LevelUpStats()
    {
        baseHealth += statGrowths.baseHealthGrowth;
        baseAttackDamage += statGrowths.baseAttackGrowth;
        baseArmour += statGrowths.baseArmourGrowth;
        baseResistance += statGrowths.baseResistanceGrowth;
        attackSpeed += statGrowths.attackSpeedGrowth;
    }

    [PunRPC]
    public void Damage(float damageAmount, Damage.DamageType damageType)
    {
        SortShieldValues();
        
        if(damageType == global::Damage.DamageType.Physical)
        {
            if(physicalShields.Count > 0)
            {
                physicalShields[0].GetComponent<ShieldDuration>().DamageShield(damageAmount, damageType);
            }
            else if(generalShields.Count > 0)
            {
                generalShields[0].GetComponent<ShieldDuration>().DamageShield(damageAmount, damageType);
            }
            else
            {
                currentHealth -= damageAmount;

                Debug.Log(this.name + " has taken " + damageAmount + " damage.");

                if (currentHealth <= 0)
                {
                    Kill();
                    Debug.Log(this.name + " has been killed.");

                    if (this.gameObject.tag == "Enemy")
                    {
                        Destroy(this.gameObject);

                        statGrowths.currentExp += this.expDrop;
                    }
                }
            }
        }

        if (damageType == global::Damage.DamageType.Magical)
        {
            if (magicShields.Count > 0)
            {
                magicShields[0].GetComponent<ShieldDuration>().DamageShield(damageAmount, damageType);
            }
            else if (generalShields.Count > 0)
            {
                generalShields[0].GetComponent<ShieldDuration>().DamageShield(damageAmount, damageType);
            }
            else
            {
                currentHealth -= damageAmount;

                Debug.Log(this.name + " has taken " + damageAmount + " damage.");

                if (currentHealth <= 0)
                {
                    Kill();
                    Debug.Log(this.name + " has been killed.");

                    if (this.gameObject.tag == "Enemy")
                    {
                        Destroy(this.gameObject);

                        statGrowths.currentExp += expDrop;
                    }
                }
            }
        }

        if (damageType == global::Damage.DamageType.True)
        {       
            if (generalShields.Count > 0)
            {
                generalShields[0].GetComponent<ShieldDuration>().DamageShield(damageAmount, damageType);
            }
            else
            {
                currentHealth -= damageAmount;

                Debug.Log(this.name + " has taken " + damageAmount + " damage.");

                if (currentHealth <= 0)
                {
                    Kill();
                    Debug.Log(this.name + " has been killed.");

                    if (this.gameObject.tag == "Enemy")
                    {
                        Destroy(this.gameObject);

                        statGrowths.currentExp += expDrop;
                    }
                }
            }
        }
    }

    public void SortShieldValues()
    {
        generalShields = generalShields.OrderBy(x => x.GetComponent<ShieldDuration>().currentTime).ToList();
        physicalShields = physicalShields.OrderBy(x => x.GetComponent<ShieldDuration>().currentTime).ToList();
        magicShields = magicShields.OrderBy(x => x.GetComponent<ShieldDuration>().currentTime).ToList();
    }

    [PunRPC]
    public void Kill()
    {
        if (photonView.IsMine)
        {
            if (this.gameObject.tag == "Player")
            {
                this.gameObject.tag = "DeadPlayer";

                var components = gameObject.GetComponents<Component>();

                foreach (var component in components)
                {
                    var collider = component as Collider;
                    if (collider) collider.enabled = false;

                    var navMesh = component as NavMeshAgent;
                    if (navMesh) navMesh.enabled = false;
                    var behaviour = component as Behaviour;
                    if (behaviour) behaviour.enabled = false;
                    GetComponent<PlayerStatInitializer>().enabled = true;
                    GetComponent<Revive>().enabled = true;
                    GetComponent<PhotonView>().enabled = true;
                    GetComponent<PhotonRigidbodyView>().enabled = true;
                    GetComponent<PhotonTransformView>().enabled = true;
                }

                Rigidbody rigidbody = GetComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }

            dead = true;
        }   
    }

    [PunRPC]
    public void Revive()
    {
        currentHealth += baseHealth;

        var components = gameObject.GetComponents<Component>();

        foreach (var component in components)
        {
            var collider = component as Collider;
            if (collider) collider.enabled = true;

            var navMesh = component as NavMeshAgent;
            if (navMesh) navMesh.enabled = true;
            var behaviour = component as Behaviour;
            if (behaviour) behaviour.enabled = true;
        }

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;

        Damage damage = GetComponent<Damage>();
        damage.UpdateHealthBar(maxHealth);

        if (this.gameObject.tag == "DeadPlayer")
        {
            this.gameObject.tag = "Player";
        }

        dead = false;
    }

    public void Slowed(float slowedTime)
    {
        if(slowedTime * tenacity >= slowedDuration)
        {
            if(slowedDuration < 0)
            {
                movementSpeedMultiplier = (movementSpeedMultiplier * 0.5f);
            }

            slowedDuration = (slowedTime * tenacity);
        }
    }

    public void Rooted(float rootedTime)
    {
        if(rootedTime * tenacity >= slowedDuration)
        {
            rootedDuration = (rootedTime * tenacity);

            canMove = false;
        }
    }

    public void Disarmed(float disarmedTime)
    {
        if(disarmedTime * tenacity >= disarmedDuration)
        {
            disarmedDuration = (disarmedTime * tenacity);

            canAttack = false;
        }
    }

    public void Silenced(float silencedTime)
    {
        if(silencedTime * tenacity >= silencedDuration)
        {
            silencedDuration = (silencedTime * tenacity);

            canCast = false;
        }
    }

    public void Stunned(float stunnedTime)
    {
        if(stunnedTime * tenacity >= stunnedDuration)
        {
            stunnedDuration = (stunnedTime * tenacity);

            canMove = false;
            canAttack = false;
            canCast = false;
        }
    }

    public void Displaced(float x, float z, float displacementTime)
    {
        Stunned(displacementTime);
        airborneDuration = displacementTime;
        playerCollider.isTrigger = true;
        startPos = this.transform.position;
        newPos = new Vector3(x, 0.1f, z);
        Vector3 difference = newPos - startPos;

        float displacementRateX = (difference.x / displacementTime);
        float displacementRateZ = (difference.z / displacementTime);
        displacementRateCombined = Mathf.Sqrt(displacementRateX * displacementRateX) + (displacementRateZ * displacementRateZ);       
    }

    public void CriticalCheck()
    {
        if(critRate == 0)
        {
            hits[0] = false;
            hits[1] = false;
            hits[2] = false;
            hits[3] = false;
            Debug.Log("No hit crits");
        }
        else if(critRate == 25)
        {
            hits[0] = true;
            hits[1] = false;
            hits[2] = false;
            hits[3] = false;
            Debug.Log("First hit crits");
        }
        else if(critRate == 50)
        {
            hits[0] = true;
            hits[1] = false;
            hits[2] = true;
            hits[3] = false;
            Debug.Log("Second hit crits");
        }
        else if(critRate == 75)
        {
            hits[0] = true;
            hits[1] = true;
            hits[2] = true;
            hits[3] = false;
            Debug.Log("Third hit crits");
        }
        else if(critRate == 100)
        {
            hits[0] = true;
            hits[1] = true;
            hits[2] = true;
            hits[3] = true;
            Debug.Log("All hits crit");
        }

        hitCount++;

        if(hitCount > 3)
        {
            hitCount = 0;
        }

        doesCrit = hits[hitCount];
    }

    public void ApplyOnHitEffects()
    {
        Debug.Log("You are currently applying on hit effects.");
    }

    public void ReceiveOnHitEffects()
    {
        Debug.Log("You are currently receiving on hit effects.");
    }

    public void ApplyAbilityDamageEffects()
    {
        Debug.Log("You are currently applying ability damage effects.");                       
    }

    public void ReceiveAbilityDamageEffects()
    {
        Debug.Log("You are currently receiving ability damage effects.");
    }

    public void ApplyUltimateDamageEffects()
    {
        Debug.Log("You are currently applying ultimate damage effects.");
    }

    public void ReceiveUltimateDamageEffects()
    {
        Debug.Log("You are currently receiving ultimate damage effects.");
    }

    public void ApplyPeriodicDamageEffects()
    {
        Debug.Log("You are currently applying periodic damage effects.");
    }

    public void ReceivePeriodicDamageEffects()
    {
        Debug.Log("You are currently receiving periodic damage effects.");
    }
}
