using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasInstantiation : MonoBehaviour
{
    public GameObject playerUI;
    public GameObject playerCamera;

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

        _playerUI.transform.SetParent(GameObject.Find("PlayerUICanvas").GetComponent<Transform>(), false);

        GameObject _playerCam = Instantiate(playerCamera);
        CameraFollowPlayer follow = _playerCam.GetComponent<CameraFollowPlayer>();
        follow.player = this.gameObject;
    }
}
