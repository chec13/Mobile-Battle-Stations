using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Turret : MonoBehaviour {
    SetFireField fireField;
    Vector3 lookVector;
    IEnumerator routine;
    public float rotationSpeed = 5.0f;
    bool isTurning = false;
	// Use this for initialization
	void Start () {
        fireField = GetComponentInParent<SetFireField>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        
        lookVector = (fireField.fireField.transform.position - transform.position);
        lookVector.y = 0;
        //Debug.Log(lookVector);
		if (lookVector != transform.forward)
        {
            //Debug.Log("check");
            float angleBetween = Vector3.Angle(lookVector, transform.forward);
            Debug.Log(angleBetween);
            Quaternion q = transform.rotation;
            Vector3 euler = q.eulerAngles;
            
            int sign = Vector3.Cross(lookVector, transform.forward).y < 0 ? 1 : -1;

            if (angleBetween > rotationSpeed * Time.fixedDeltaTime)
            {
                euler.y += rotationSpeed * Time.fixedDeltaTime * sign;
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.forward * 10);
                euler.y += angleBetween;
            }
            //Debug.DrawRay(transform.position, transform.forward * 10);
            q.eulerAngles = euler;
            transform.rotation = q;
        }
	}
}
