using UnityEngine;
using System.Collections;

public class change_sence : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void leave_project()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("start");
    }

    public void change_data()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("data");
    }

	public void visual_project()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Project");
	}

	public void audio_project()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Project2");
	}

	public void tactile_project()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Project3");
	}
}
