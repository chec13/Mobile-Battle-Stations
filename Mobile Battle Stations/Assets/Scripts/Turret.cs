using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Turret : MonoBehaviour {
    SetFireField fireField;
    Vector3 lookVector;
    public float Fire_Range = 10;
    public GameObject fireLocation;
    public GameObject shot_Prefab;
    private GameObject[] shotPool;
    bool firingRoutineRunning = false;
    private int poolCount = 0;
    IEnumerator routine;
    public float fireRate = 5;
    public float rotationSpeed = 5.0f;
    bool isTurning = false;
    bool isFiring = false;
	// Use this for initialization
	void Start () {
        fireField = GetComponentInParent<SetFireField>();
        //fireLocation = transform.Find("Fire").gameObject;
        createShotPool();
	}
	
    void createShotPool()
    {
        shotPool = new GameObject[(int)Mathf.Ceil(fireRate) * 4];
        
        for (int x = 0; x < shotPool.Length; x++)
        {
            shotPool[x] = Instantiate(shot_Prefab);
            shotPool[x].SetActive(false);
        }

        
    }
    GameObject getShotFromPool()
    {
        poolCount++;
        if (poolCount == shotPool.Length)
            poolCount = 0;
        return shotPool[poolCount];
    }
	// Update is called once per frame
	void FixedUpdate () {

        
        lookVector = (fireField.fireField.transform.position - transform.position);
        lookVector.y = 0;
        //Debug.Log(lookVector);
        float angleBetween = Vector3.Angle(lookVector, transform.forward);
        //Debug.Log(angleBetween + " is greater than 0.01f: " + angleBetween.CompareTo(0.01f));
        if (angleBetween.CompareTo(0.01f) == 1)
        {
            isTurning = true;
            
            //Debug.Log("check");
             
            
            Quaternion q = transform.rotation;
            Vector3 euler = q.eulerAngles;
            
            int sign = Vector3.Cross(lookVector, transform.forward).y < 0 ? 1 : -1;

            if (angleBetween > rotationSpeed * Time.fixedDeltaTime)
            {
                euler.y += rotationSpeed * Time.fixedDeltaTime * sign;
                //Debug.Log(rotationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //Debug.DrawRay(transform.position, transform.forward * 10);
                euler.y += angleBetween;
                //Debug.Log("here");
            }
            //Debug.Log(angleBetween);
            //Debug.DrawRay(transform.position, transform.forward * 10);
            q.eulerAngles = euler;
            transform.rotation = q;
        }
        else
        {
            isTurning = false;
            //Debug.Log("Is firing: " + isFiring + "In routine: " + firingRoutineRunning);
            if (isFiring && !firingRoutineRunning)
            {
                Debug.Log("1");
                StartCoroutine(firingRoutine());
                firingRoutineRunning = true;
            }
            if (!isFiring)
            {
                StopCoroutine(firingRoutine());
                firingRoutineRunning = false;
            }
        }
	}
    public void toggleFire()
    {
        isFiring = !isFiring;
        Debug.Log("Button Test");
    }
    public IEnumerator firingRoutine()
    {
        
        while(true)
        {
            Debug.Log("In firing routine");
            if (!isTurning)
            fire();
            

            yield return new WaitForSeconds(1 / fireRate);
        }
        yield return null;
    }
    public void fire()
    {
        //Debug.Log("Hi");
        SetFireField.FireArc arc = fireField.fireArc;
        arc.angle = Mathf.Deg2Rad * arc.angle;
        float adjacent = Mathf.Cos(arc.angle + Mathf.Acos(arc.forward.z));
        float opp = Mathf.Sin(arc.angle + Mathf.Asin(arc.forward.x));
        Vector3 left = new Vector3(opp, 0, adjacent);

        adjacent = Mathf.Cos(Mathf.Acos(arc.forward.z) - arc.angle);
        opp = Mathf.Sin(Mathf.Asin(arc.forward.x) - arc.angle);
        Vector3 right = new Vector3(opp, 0, adjacent);
        Ray r = new Ray(fireField.transform.position, left);
        Ray r1 = new Ray(fireField.transform.position, right);
        Vector3 leftPointInRange = r.GetPoint(Fire_Range);
        Vector3 rightPointInRange = r1.GetPoint(Fire_Range);

        float distance_Between_Vectors = Vector3.Distance(leftPointInRange, rightPointInRange);
        Vector3 directionBetween_Left_Right_vectors = (leftPointInRange + rightPointInRange).normalized;
        Ray between = new Ray(leftPointInRange, directionBetween_Left_Right_vectors);
        Vector3 firePoint = between.GetPoint(Random.Range(0,distance_Between_Vectors));

        //Debug.DrawLine(rightPointInRange, transform.position, Color.red, 2);
        Debug.DrawLine(leftPointInRange, rightPointInRange, Color.blue, 2);
        Debug.Log(leftPointInRange + "Location" + rightPointInRange);
        GameObject shot = getShotFromPool();
        shot.transform.position = fireLocation.transform.position;
        shot.transform.LookAt(firePoint);
        shot.SetActive(true);
    }
}
