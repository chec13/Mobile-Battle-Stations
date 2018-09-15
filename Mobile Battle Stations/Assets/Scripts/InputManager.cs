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
		if (Input.GetMouseButtonDown(0))
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

        //Debug.Log(allCoroutines.Count + " coroutines running");
	}
}
