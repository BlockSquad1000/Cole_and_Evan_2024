using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class SpawnPlayerUI : MonoBehaviourPunCallbacks
{
    public PhotonView view;
    public Slider healthBar;
    public TMP_Text damageText;
    public GameObject playerUI;

    private void Awake()
    {
        playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
    }

    void Start()
    {
        view = this.GetComponent<PhotonView>();

        Slider _healthBar = Instantiate(healthBar, playerUI.transform);
        TMP_Text _damageText = Instantiate(damageText, playerUI.transform);

        HealthBarLookAt lookAt = _healthBar.GetComponent<HealthBarLookAt>();
        Damage damage = this.GetComponent<Damage>();
        damage.healthBar = _healthBar;
        damage.damageText = _damageText;
        view.RPC("SpawnUI", RpcTarget.All);
    }

    [PunRPC]
    public void SpawnUI()
    {
        Slider _healthBar = Instantiate(healthBar, playerUI.transform);
        TMP_Text _damageText = Instantiate(damageText, playerUI.transform);

        HealthBarLookAt lookAt = _healthBar.GetComponent<HealthBarLookAt>();
        Damage damage = this.GetComponent<Damage>();
        damage.healthBar = _healthBar;
        damage.damageText = _damageText;
    }
}
