using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudio : MonoBehaviour {
    AudioSource s;
    // Use this for initialization
    void Start () {
		s = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		if (!s.isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
