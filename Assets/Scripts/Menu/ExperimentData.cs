using UnityEngine;
using System.Collections;

// This class enables the storage of the experiment type data between the menu system and the experiment room

public class ExperimentData : MonoBehaviour {

    static ExperimentData persistantExperimentData;

    // configure experiment - stored as individual booleans to enable combination experiments in the future.
    public bool VisualFeedback;
    public bool AudioFeedback;
    public bool VibroHapticFeedback;

    public string participantID; // store the Participant ID

	// Use this for initialization
	void Start () {

        // referance for this code - https://www.youtube.com/watch?v=WchH-JCwVI8

        if (persistantExperimentData != null)
        {
            Destroy(this.gameObject);
            return;
        }

        persistantExperimentData = this;

        GameObject.DontDestroyOnLoad(this.gameObject);  // persistant game object
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
