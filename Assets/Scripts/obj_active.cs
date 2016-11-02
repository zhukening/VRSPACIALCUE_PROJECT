using UnityEngine;
using System.Collections;

public class obj_active : MonoBehaviour {

    public GameObject[] obj_on;
    public GameObject[] obj_off;

 	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void on (bool x)
    {
        for(int i = 0; i <= obj_on.Length - 1; i++)
        {
            obj_on[i].SetActive(x);            
        }
        for (int i = 0; i <= obj_off.Length - 1; i++)
        {
            obj_off[i].SetActive(!x);
        }
    }
}
