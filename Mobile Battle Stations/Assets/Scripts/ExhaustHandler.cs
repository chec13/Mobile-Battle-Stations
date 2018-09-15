using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustHandler : MonoBehaviour {
    ParticleSystem[] systems;
    float[] loadedValues;
    Light light;
	// Use this for initialization
	void Start () {
        systems = GetComponentsInChildren<ParticleSystem>();
        light = GetComponentInChildren<Light>();
        loadedValues = new float[systems.Length + 1];
        for (int x = 0; x < loadedValues.Length; x++)
        {
            if (x < systems.Length)
            {
                loadedValues[x] = systems[x].main.maxParticles;
            }
            else
            {
                loadedValues[x] = light.intensity;
            }
        }
        setExhaustRate(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void setExhaustRate(float percentage)
    {
        percentage = percentage / 10;
        for (int x = 0; x < loadedValues.Length; x++)
        {
            if (x < systems.Length)
            {
                ParticleSystem.MainModule main = systems[x].main;
                main.maxParticles = (int)(loadedValues[x] * percentage);
                
            }
            else
            {
                light.intensity = loadedValues[x] * percentage;
            }
        }
    }
}
