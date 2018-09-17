using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    List<IEnumerator> allCoroutines = new List<IEnumerator>();
  
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.Q) && MovementBehavior.selected.canMove)
        {
            if (MovementBehavior.selected.myCoroutine != null)
            {
                
                StopCoroutine(MovementBehavior.selected.myCoroutine);
                allCoroutines.Remove(MovementBehavior.selected.myCoroutine);
                MovementBehavior.selected.myCoroutine = null;
            }
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane p = new Plane(Vector3.up, Vector3.zero);
            
            float distance = 0;
            if (p.Raycast(r, out distance))
            {
                MovementBehavior.selected.myCoroutine = MovementBehavior.selected.setTarget(r.GetPoint(distance));
                allCoroutines.Add(MovementBehavior.selected.myCoroutine);
                StartCoroutine(MovementBehavior.selected.myCoroutine);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MovementBehavior.selected.canMove = false;
            MovementBehavior.selected.fireField.active_routine = true;
            IEnumerator i = MovementBehavior.selected.fireField.MoveField();
            StartCoroutine(i);
            allCoroutines.Add(i);
            MovementBehavior.selected.GetComponent<SetFireField>().fireField.GetComponent<MeshRenderer>().enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            MovementBehavior.selected.canMove = true;
            MovementBehavior.selected.fireField.active_routine = false;
            StopCoroutine(MovementBehavior.selected.fireField.MoveField());
            allCoroutines.Remove(MovementBehavior.selected.fireField.MoveField());
            MovementBehavior.selected.GetComponent<SetFireField>().fireField.GetComponent<MeshRenderer>().enabled = false;
        }

        //Debug.Log(allCoroutines.Count + " coroutines running");
    }
    public static NullableVector3 Mouse_To_Cross_YAxis()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane(Vector3.up, Vector3.zero);
        float distance = 0;
        if (p.Raycast(r, out distance))
        {
            return new NullableVector3(r.GetPoint(distance));
        }
        return NullableVector3.nullVector;
    }
    
}
