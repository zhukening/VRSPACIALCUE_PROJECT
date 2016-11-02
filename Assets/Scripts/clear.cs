using UnityEngine;
using System.Collections;

public class clear : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void clera_data()
    {
        PlayerPrefs.DeleteAll();
    }
}
