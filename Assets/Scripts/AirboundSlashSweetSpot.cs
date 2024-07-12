using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirboundSlashSweetSpot : MonoBehaviour
{
    public List<GameObject> enemiesInTrigger;
    [SerializeField] private AirboundSlashTrigger trigger;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy entered sweet spot trigger zone.");
            enemiesInTrigger.Add(other.gameObject);   
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy left trigger zone.");
            enemiesInTrigger.Remove(other.gameObject);
        }
    }

    public void Trigger()
    {
        foreach (GameObject enemy in enemiesInTrigger)
        {
            Debug.Log("Enemy has been knocked airborne");            
            enemy.GetComponent<PlayerStatInitializer>().Displaced(enemy.gameObject.transform.position.x - .1f, enemy.gameObject.transform.position.z - .1f, 1.0f);
            return;
        }
    }
}
           
