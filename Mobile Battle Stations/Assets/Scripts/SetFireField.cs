using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFireField : MonoBehaviour {
    [HideInInspector]
    public GameObject fireField;
    public FireArc fireArc;
    public float maxSize;
    public bool active_routine = false;
	// Use this for initialization
	void Start () {
        fireField = transform.Find("FireField").gameObject;
        fireField.GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public IEnumerator MoveField()
    {
        Vector3 fieldPosition;
        Vector3 shipPosition;
        NullableVector3 mousePosition;

        Vector3 direction_ShipToField;
        Vector3 direction_ShipToMouse;
        fireField.GetComponent<MeshRenderer>().enabled = true;
        Mesh m = fireField.GetComponent<MeshFilter>().mesh;
        yield return new WaitForFixedUpdate();

        while (active_routine)
        {
            
            fieldPosition = fireField.transform.position;
            shipPosition = transform.position;
            direction_ShipToField = (fieldPosition - shipPosition).normalized;
            mousePosition = InputManager.Mouse_To_Cross_YAxis();
            if (mousePosition.is_Null())
            {
                
            }
            else // set fire field location around ship
            {
                
                direction_ShipToMouse = (mousePosition.vector - shipPosition).normalized;

                float angle = Vector3.Angle(direction_ShipToField, direction_ShipToMouse);
                int sign = Vector3.Cross(direction_ShipToField, direction_ShipToMouse).y > 0 ? 1 : -1;
                fireField.transform.RotateAround(transform.position, Vector3.up, angle * sign);

            }
            fireArc.forward = direction_ShipToField;
            if (Input.GetMouseButton(0)) // modify fire field arc
            {
                Vector3 mouseStartPosition = Input.mousePosition;
                while (Input.GetMouseButton(0))
                {
                    float sensitivity = 0.5f;
                    float distance_from_Start = Vector3.Distance(mouseStartPosition, Input.mousePosition) * sensitivity;
                    float distanceBuffer = 10.0f;
                    Vector3[] editVertices = m.vertices;
                    for (int x = 0; x < m.vertexCount; x++)
                    {
                        
                        if (m.vertices[x].x == 0.5)
                        {
                           
                            if (editVertices[x].z > 0)
                            {
                                editVertices[x].z = 0.5f * Mathf.Min(25, Mathf.Max(1, distance_from_Start - distanceBuffer));
                                fireArc.angle = calculateFireAngle(editVertices[x].z);
                                fireArc.forward = direction_ShipToField;
                                //Debug.Log(calculateFireAngle(editVertices[x].z));
                            }
                            else
                            {
                                editVertices[x].z = -0.5f * Mathf.Min(25, Mathf.Max(1, distance_from_Start - distanceBuffer));
                            }
                            
                        }
                    }
                    m.vertices = editVertices;
                    fireField.GetComponent<MeshFilter>().mesh = m;


                    yield return new WaitForFixedUpdate();
                }
            }
                
            yield return new WaitForFixedUpdate();
        }
        
        GetComponent<MovementBehavior>().canMove = true;
        yield return null;
    }
    public float calculateFireAngle(float outer_fireRange_distance)
    {
        Vector3 backVector = new Vector3(0.5f * fireField.transform.localScale.x, 0f, 0.5f ).normalized;
        Vector3 frontVector = new Vector3(0.5f * fireField.transform.localScale.x, 0f, outer_fireRange_distance).normalized;
        

        return Vector3.Angle(backVector, frontVector);
    }
    public Vector3 calculateFireVector(float outer_fireRange_distance)
    {
        Vector3 backVector = new Vector3(0.5f * fireField.transform.localScale.x, 0f, 0.5f);
        Vector3 frontVector = new Vector3(0.5f * fireField.transform.localScale.x, 0f, outer_fireRange_distance);

        return (frontVector - backVector).normalized;
    }
    public struct FireArc
    {
        public Vector3 forward;
        public float angle;
        
        
    }
}
