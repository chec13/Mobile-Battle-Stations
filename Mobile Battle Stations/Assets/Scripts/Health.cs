using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public bool Destroy_on_Death = true;
    public float health = 100;
    public float defense = 1;
    private bool triggerDeath = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ShotBehavior>() != null && !triggerDeath)
        {
            other.gameObject.SetActive(false);
            health = health - (10 / defense);
            if (health <= 0)
            {
                Instantiate(PrefabManager.manager.explosion, transform.position, Quaternion.identity);
                Instantiate(PrefabManager.manager.explosion_Sound,transform.position, Quaternion.identity);
                triggerDeath = true;
                Destroy(gameObject);
            }
        }
    }
    
}
