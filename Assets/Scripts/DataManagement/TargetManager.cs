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

    // the current targetNumber
    private int TargetNumber;

    // Use this for initialization
    void Start()
    {

        // set target Default
        TargetNumber = 0;
        // get useful objects
        Player = GameObject.Find("Player");
        GameObject PersistantGameObject = GameObject.Find("persistantGO");
        experimentData = PersistantGameObject.GetComponent<ExperimentData>();
        ParticipantID = experimentData.participantID;

        switch (experimentData.GetCurrentExperiment())
        {
            case ExperimentData.Experiment.Visual:
                {
                    TargetPrefix = "Visual_" + TargetPrefix;
                    break;
                }
            case ExperimentData.Experiment.Audio:
                {
                    TargetPrefix = "Audio_" + TargetPrefix;
                    break;
                }
            case ExperimentData.Experiment.VibroHaptic:
                {
                    TargetPrefix = "vHaptic_" + TargetPrefix;
                    break;
                }
            case ExperimentData.Experiment.NULL:
                {
                    break;
                }
        }

        // load target location set
        TargetData = TargetContainer.Load(Path.Combine(Application.dataPath, "Data\\TargetSets\\TargetSet" + experimentData.GetExperimentIndex() + ".xml"));

        // spawn first target

        // Create Target based on number stored in Location 
        GameObject newTarget = (GameObject)Instantiate(TargetPrefab, TargetData.Targets[TargetNumber].Location, Quaternion.identity);
        // update feedback system with new target
        Player.GetComponentInChildren<FeedbackMain>().target = newTarget;
        Camera.main.GetComponent<RecordRotation>().sumRotation = 0; //reset sum rotation

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnNextTarget(float Timer)
    {
        // Collect data and store in Data Structure for the last target destroyed
        TargetData.Targets[TargetNumber].Name = TargetPrefix + "_" + TargetNumber;
        TargetData.Targets[TargetNumber].TargetSet = experimentData.GetExperimentIndex();
        TargetData.Targets[TargetNumber].Time = Timer;
        TargetData.Targets[TargetNumber].ParticipantID = ParticipantID;
        TargetData.Targets[TargetNumber].PlayerDirection = Camera.main.transform.rotation;
        TargetData.Targets[TargetNumber].DistanceTravelled = Camera.main.GetComponent<RecordRotation>().sumRotation;  // in degrees

        // load in next Target 
        TargetNumber++;

        if (TargetNumber < TargetData.Targets.Count)
        {


            // Create Target based on number stored in Location 
            GameObject newTarget = (GameObject)Instantiate(TargetPrefab, TargetData.Targets[TargetNumber].Location, Quaternion.identity);
            // update feedback system with new target
            Player.GetComponentInChildren<FeedbackMain>().target = newTarget;

            Camera.main.GetComponent<RecordRotation>().sumRotation = 0; //reset sum rotation
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

            // update current Scenario
            experimentData.NextScenario();
                
            // Fade out and load the next scene
            if (experimentData.GetCurrentExperiment() != ExperimentData.Experiment.NULL)
            {
                StartCoroutine(DelayLoad(false));
            }
            else
            {
                StartCoroutine(DelayLoad(true));
            }

        }
    }

    IEnumerator DelayLoad(bool expFinnished)
    {
        // Reload Experiment Room with new settings
        GameObject.Find("FadePlane").GetComponent<Fade>().FadeOut();
        yield return new WaitForSeconds(2);

        if (expFinnished)
        {
            Camera.main.GetComponent<RecordRotation>().SaveData();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("ExperimentRoom", LoadSceneMode.Single);
        }
    }
}
