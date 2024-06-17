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
        /*shields.generalShieldSlider = _healthBar.gameObject.GetChild(3);
        shields.physicalShieldSlider
        shields.magicShieldSlider*/

        _playerUI.transform.SetParent(GameObject.Find("PlayerUICanvas").GetComponent<Transform>(), false);

       /* playerCamera = Instantiate(playerCamera) as Camera;
        CameraFollowPlayer follow = playerCamera.GetComponent<CameraFollowPlayer>();
        follow.view = this.GetComponent<PhotonView>();
        follow.playerPos = this.gameObject.transform;*/
    }
}
