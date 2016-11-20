using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class data_output : MonoBehaviour {

    public Text data;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	data.text = PlayerPrefs.GetString("all_data");
	}
}
