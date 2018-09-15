using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullableVector3 {
    public Vector3 vector;
    public static NullableVector3 nullVector = new NullableVector3();
    private bool isNull = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public NullableVector3(Vector3 v)
    {
        vector = v;
        this.isNull = false;
    }
    
    public NullableVector3()
    {
        isNull = true;
    }
    public bool is_Null()
    {
        return isNull;
    }
    
    

    
 

    
}
