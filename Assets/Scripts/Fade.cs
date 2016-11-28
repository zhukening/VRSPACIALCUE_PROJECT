using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// simple fade in fade out functionality

public class Fade : MonoBehaviour {

    public float FadeTime = 2;
        
	// Use this for initialization
	void Start ()
    {
        FadeIn();
    }
	
    public void FadeIn()
    {
        gameObject.GetComponent<Image>().CrossFadeAlpha(0, FadeTime, false);
    }

    public void FadeOut()
    {
        gameObject.GetComponent<Image>().CrossFadeAlpha(1, FadeTime, false);
    }

}
