using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shields : MonoBehaviour
{
    public PlayerStatInitializer stats;
    public List<GameObject> shields;

    public List<float> times;

    public GameObject generalShieldSlider;
    public GameObject magicShieldSlider;
    public GameObject physicalShieldSlider;

    public GameObject generalShield;
    public GameObject physicalShield;
    public GameObject magicShield;

    // Start is called before the first frame update
    void Start()
    {
        stats = this.GetComponent<PlayerStatInitializer>();

        generalShieldSlider.SetActive(false);
        physicalShieldSlider.SetActive(false);
        magicShieldSlider.SetActive(false);
    }

    private void Update()
    {
        if(stats.generalShields.Count <= 0)
        {
            generalShieldSlider.SetActive(false);
        }
        if (stats.physicalShields.Count <= 0)
        {
            physicalShieldSlider.SetActive(false);
        }
        if (stats.magicShields.Count <= 0)
        {
            magicShieldSlider.SetActive(false);
        }
    }

    public void ActivateGeneralShield(float time, float capacity, float decayRate)
    {
        GameObject spawnedShield = Instantiate(generalShield, this.transform) as GameObject;
        spawnedShield.GetComponent<ShieldDuration>().maxTime = time;
        spawnedShield.GetComponent<ShieldDuration>().maxCapacity = capacity;
        spawnedShield.GetComponent<ShieldDuration>().decayRate = decayRate;

        generalShieldSlider.SetActive(true);
        generalShieldSlider.GetComponent<Slider>().maxValue = stats.maxHealth * 0.33f;
        generalShieldSlider.GetComponent<Slider>().value = spawnedShield.GetComponent<ShieldDuration>().maxCapacity;

        stats.generalShields.Add(spawnedShield);
    }

    public void ActivatePhysicalShield(float time, float capacity, float decayRate)
    {
        GameObject spawnedShield = Instantiate(physicalShield, this.transform) as GameObject;
        spawnedShield.GetComponent<ShieldDuration>().maxTime = time;
        spawnedShield.GetComponent<ShieldDuration>().maxCapacity = capacity;
        spawnedShield.GetComponent<ShieldDuration>().decayRate = decayRate;

        physicalShieldSlider.SetActive(true);
        physicalShieldSlider.GetComponent<Slider>().maxValue = stats.maxHealth * 0.33f;
        physicalShieldSlider.GetComponent<Slider>().value = spawnedShield.GetComponent<ShieldDuration>().maxCapacity;

        stats.physicalShields.Add(spawnedShield);
    }

    public void ActivateMagicShield(float time, float capacity, float decayRate)
    {
        GameObject spawnedShield = Instantiate(magicShield, this.transform) as GameObject;
        spawnedShield.GetComponent<ShieldDuration>().maxTime = time;
        spawnedShield.GetComponent<ShieldDuration>().maxCapacity = capacity;
        spawnedShield.GetComponent<ShieldDuration>().decayRate = decayRate;

        magicShieldSlider.SetActive(true);
        magicShieldSlider.GetComponent<Slider>().maxValue = stats.maxHealth * 0.33f;
        magicShieldSlider.GetComponent<Slider>().value = spawnedShield.GetComponent<ShieldDuration>().maxCapacity;

        stats.magicShields.Add(spawnedShield);
    }
}
