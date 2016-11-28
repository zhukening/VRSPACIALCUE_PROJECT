using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class TargetManager : MonoBehaviour {

    public GameObject TargetPrefab;

    private TargetContainer TargetData = new TargetContainer();
    private Target newTarget;

    private string ParticipantID;
    private ExperimentData experimentData;
    private GameObject Player;
    private string TargetPrefix;

    // maxiumum number of targets to spawn
    public int MaxTargets = 10;

    // the current targetNumber
    private int TargetNumber;

    // Use this for initialization
    void Start() {

        // set target Default
        TargetNumber = 0;
        // get useful objects
        Player = GameObject.Find("Player");
        GameObject PersistantGameObject = GameObject.Find("persistantGO");
        experimentData = PersistantGameObject.GetComponent<ExperimentData>();
        ParticipantID = experimentData.participantID;

        // get the type of experiment being conducted
        bool visualFeedback = experimentData.VisualFeedback;
        bool audioFeedback = experimentData.AudioFeedback;
        bool vibroHapticFeedback = experimentData.VibroHapticFeedback;

        // update targetPrefix to match experiment
        if (visualFeedback){
            TargetPrefix = "Visual_" + TargetPrefix;
        }
        if (audioFeedback){
            TargetPrefix = "Audio_" + TargetPrefix;
        }
        if (vibroHapticFeedback){
            TargetPrefix = "vHaptic_" + TargetPrefix;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnNextTarget(float Timer)
    {
            // Collect data and store in Data Structure
            TargetData.Targets.Add(new Target());    
            TargetData.Targets[TargetNumber].Name = TargetPrefix + TargetNumber;
            TargetData.Targets[TargetNumber].Time = Timer;
            TargetData.Targets[TargetNumber].ParticipantID = ParticipantID;
            TargetData.Targets[TargetNumber].PlayerDirection = Camera.main.transform.rotation;
            TargetData.Targets[TargetNumber].DistanceTravelled = Camera.main.GetComponent<RecordRotation>().sumRotation;  // in degrees

            // should we wish to use a fixed set of locations we can replace the random numbers here with a file read.
            TargetData.Targets[TargetNumber].Location = new Vector3(Random.Range(-10, 10), Random.Range(1, 10), Random.Range(-10, 10));
            
            if (TargetNumber < MaxTargets -1)
            {
                // Create Target based on number stored in Location 
                GameObject newTarget = (GameObject)Instantiate(TargetPrefab, TargetData.Targets[TargetNumber].Location, Quaternion.identity);
                // update feedback system with new target
                Player.GetComponentInChildren<FeedbackMain>().target = newTarget;

                Camera.main.GetComponent<RecordRotation>().sumRotation = 0; //reset sum rotation
                // increment Target Number
                TargetNumber++;
            }
            else
            {
                // Save the results to Data\Results_XXXX.xml and reload main menu
                string resultsFile = Path.Combine(Application.dataPath, "Data\\Results_" + ParticipantID + ".xml");

                if (File.Exists(resultsFile))
                {
                    // load existing Data
                    var existingData = TargetContainer.Load(Path.Combine(Application.dataPath, "Data\\Results_" + ParticipantID + ".xml"));
                    // Append new data to existing file
                    existingData.Targets.AddRange(TargetData.Targets);
                    // Save appended File
                    existingData.Save(Path.Combine(Application.dataPath, "Data\\Results_" + ParticipantID + ".xml"));
                }
                else
                {
                    // save the participants File
                    TargetData.Save(Path.Combine(Application.dataPath, "Data\\Results_" + ParticipantID + ".xml"));

                }

                // return to main Menu
                Camera.main.GetComponent<RecordRotation>().SaveData();
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
    }

}
