using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {
    public static PrefabManager manager;
    public GameObject explosion;
    public AudioSource explosion_Sound;
    public AudioSource turretFire_Sound;
	// Use this for initialization
	void Awake () {
        manager = this;
        if (explosion == null)
        {
            throw new System.Exception("No Explosion Prefab Set");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
