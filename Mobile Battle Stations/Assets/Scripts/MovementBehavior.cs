using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MovementBehavior : MonoBehaviour {
    public static MovementBehavior selected;
    public bool isSelected = false;
    public bool canMove = true;
    public SetFireField fireField;
    public IEnumerator myCoroutine = null;
    public float turnVelocity = 5.0f;
    private Vector3 target;
    private float distance;
    public float acceleration = 0.00001f;
    public float maxSpeed = 0.1f;
    public float stopRange = 0.1f;
    private ExhaustHandler exhaust;
    Rigidbody r;
	// Use this for initialization
	void Start () {
        r = GetComponent<Rigidbody>();
        fireField = GetComponent<SetFireField>();
        if (isSelected)
        {
            selected = this;
        }
        exhaust = GetComponentInChildren<ExhaustHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public IEnumerator setTarget( Vector3 t) // set target and move
    {
        target = t;
        distance = Vector3.Distance(target, transform.position);
        //float turnAngle = Vector3.Angle(transform.position, target);
        Vector3 direction = (t - transform.position).normalized;
        //Debug.Log("1");
        
        while (distance > stopRange)
        {
            float currentRange = r.velocity.magnitude;
            direction = (t - transform.position).normalized;
            float turnAngle = Vector3.Angle(transform.forward, direction);
            float turnSpeed = 1.0f;
            Debug.DrawRay(transform.position, direction * 10);
            if (turnAngle > 0.0)
            {
                turnSpeed = Mathf.Cos(turnAngle / 2);
                Quaternion q = transform.rotation;
                Vector3 rotation = q.eulerAngles;
                Debug.Log(turnAngle);
                exhaust.setExhaustRate(r.velocity.magnitude / maxSpeed);
                int sign = Vector3.Cross(transform.forward, direction).y < 0 ? -1 : 1;
               
                    if (turnAngle < turnVelocity)
                    {
                        rotation.y += turnAngle * Time.fixedDeltaTime * sign;
                    }
                    else
                    {
                        rotation.y += turnVelocity * Time.fixedDeltaTime * sign;
                    }
               
                
                q.eulerAngles = rotation;
                transform.rotation = q;
                turnSpeed = Mathf.Abs(Mathf.Cos(turnAngle / 2));
            }
            else
            {
                turnSpeed = 1;
            }
            distance = Vector3.Distance(target, transform.position);
            if (distance >= maxSpeed) // if distance is greater than max speed, continue to accelerate.
            {
                if (currentRange < maxSpeed * turnSpeed)
                {
                    r.velocity = transform.forward * (r.velocity.magnitude + (acceleration * Time.fixedDeltaTime));
                }
                if (currentRange > maxSpeed * turnSpeed)
                {
                    r.velocity = transform.forward * maxSpeed * turnSpeed;
                }
                
            }
            else if (distance > currentRange)
            {
                
                r.velocity = transform.forward * (r.velocity.magnitude + (acceleration * Time.fixedDeltaTime));
            }
            else
            {
                r.velocity = transform.forward * distance;
            }
            //Debug.Log("2");
            yield return new WaitForFixedUpdate();
        }
        r.velocity = Vector3.zero;
        exhaust.setExhaustRate(0);
        yield return null;
    }
}
