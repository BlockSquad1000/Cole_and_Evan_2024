using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirboundSlashSweetSpot : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy entered trigger zone.");
            if (GetComponentInParent<AirboundSlashTrigger>().airboundTrigger)
            {
                if (other != null)
                {
                    Debug.Log("Enemy has been knocked airborne");
                    other.GetComponent<PlayerStatInitializer>().Displaced(other.gameObject.transform.position.x - .1f, other.gameObject.transform.position.z - .1f, 1.0f);
                }
            }
        }
    }
}
