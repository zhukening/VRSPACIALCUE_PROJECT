using UnityEngine;
using System.Collections;

public class change_sence : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void change_project()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Project");
    }

    public void change_data()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("data");
    }
}
