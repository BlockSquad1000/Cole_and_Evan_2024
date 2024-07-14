using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CanvasInstantiation : MonoBehaviour
{
    public GameObject playerUI;
    public Camera playerCamera;

    private void Awake()
    {
        GameObject _playerUI = Instantiate(playerUI);

        Slider _healthBar = _playerUI.GetComponentInChildren<Slider>();
        TMP_Text _damageText = _playerUI.GetComponentInChildren<TMP_Text>();

        HealthBarLookAt lookAt = _healthBar.GetComponent<HealthBarLookAt>();
        lookAt.target = this.gameObject;

        Damage damage = this.GetComponent<Damage>();
        damage.healthBar = _healthBar;
        damage.damageText = _damageText;

        Shields shields = this.GetComponent<Shields>();

        foreach(Transform transform in _healthBar.transform)
        {
            if(transform.name == "GeneralShieldBar")
            {
                shields.generalShieldSlider = transform.gameObject;
            }

            if (transform.name == "PhysicalShieldBar")
            {
                shields.physicalShieldSlider = transform.gameObject;
            }

            if (transform.name == "MagicShieldBar")
            {
                shields.magicShieldSlider = transform.gameObject;
            }
        }

        _playerUI.transform.SetParent(GameObject.Find("PlayerUICanvas").GetComponent<Transform>(), false);

        GameObject playerRankUI = GameObject.Find("GeneralUICanvas");
        playerRankUI.GetComponentInChildren<RankingSystem>().playerLevel = this.gameObject.GetComponent<LevelUp>();
    }
}
