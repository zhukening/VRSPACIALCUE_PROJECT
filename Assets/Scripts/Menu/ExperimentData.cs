using UnityEngine;
using System.Collections;

// This class enables the storage of the experiment type data between the menu system and the experiment room

public class ExperimentData : MonoBehaviour {

    static ExperimentData persistantExperimentData;

    public enum Experiment { NULL, Visual, Audio, VibroHaptic }
    public Experiment[] ExperimentOrder = new Experiment[3];  
    private int currentExperimentIndex;
    
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

    public Experiment GetCurrentExperiment()
    {
        if (currentExperimentIndex < (ExperimentOrder.Length ))
        {
            return ExperimentOrder[currentExperimentIndex];
        }
        else
        {
            return Experiment.NULL;
        }

    }

    public void NextScenario()
    {
        if (currentExperimentIndex < (ExperimentOrder.Length))
        {
            // increment current Experiment
            currentExperimentIndex++;
        }
    }

    public int GetExperimentIndex()
    {
        return currentExperimentIndex;
    }
}
