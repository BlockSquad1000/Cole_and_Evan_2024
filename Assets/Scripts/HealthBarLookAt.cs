using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarLookAt : MonoBehaviour
{
    public GameObject target;
    public Slider healthBar;

    private void Update()
    {
        if (target != null)
        {
            healthBar.transform.position = target.transform.position + healthBar.transform.up + new Vector3(0, 1.5f, 1.5f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
