using UnityEngine;
using System.Collections;

public class RotateThisObject : MonoBehaviour {
    public float speed;
	
	void Update () {

        transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
       
	}
}
