using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TargetManager : MonoBehaviour {

    public GameObject TargetPrefab;

    private TargetContainer TargetData = new TargetContainer();
    private Target newTarget;

    private string ParticipantID;
    private ExperimentData experimentData;
    private GameObject Player;
    private string TargetPrefix;

    public bool TrainingMode = true; // enables the training mode

    private Text UIText;
    private Text KeyInfo;
    private string ExperimentType;

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

        UIText = GameObject.Find("TextInformer").GetComponent<Text>();
        KeyInfo = GameObject.Find("KeyInstructions").GetComponent<Text>();


        switch (experimentData.GetCurrentExperiment())
        {
            case ExperimentData.Experiment.Visual:
                {
                    TargetPrefix = "Visual_" + TargetPrefix;
                    ExperimentType = "Visual Feedback";
                    break;
                }
            case ExperimentData.Experiment.Audio:
                {
                    TargetPrefix = "Audio_" + TargetPrefix;
                    ExperimentType = "Audio Feedback";
                    break;
                }
            case ExperimentData.Experiment.VibroHaptic:
                {
                    TargetPrefix = "vHaptic_" + TargetPrefix;
                    ExperimentType = "Vibro-Haptic Feedback";
                    break;
                }
            case ExperimentData.Experiment.NULL:
                {
                    break;
                }
        }

        // load target location set
        TargetData = TargetContainer.Load(Path.Combine(Application.dataPath, "Data\\TargetSets\\TargetSet" + experimentData.GetExperimentIndex() + ".xml"));

        UIText.text = "Starting Training: \n" + ExperimentType;
        KeyInfo.enabled = true; // Hide keyinformation

        Time.timeScale = 0; //pause 

    }
	
	// Update is called once per frame
	void Update () {
        // unpause if space pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            if (TrainingMode)
            {
                StartCoroutine(StartTraining());
            }
            else
            {
                StartCoroutine(StartExperiment());
            }
        }
    }

    public void SpawnNextTarget(float Timer)
    {
        if (TrainingMode)
        {
            if (TargetNumber < 5) // 5 practice targets
            {
                TargetNumber++;

                // generate random points on circle -http://answers.unity3d.com/questions/759542/get-coordinate-with-angle-and-distance.html
                Vector3 rndTargetLocation = new Vector3();
                float dist = 10f;
                float a = Random.Range(0, 2f) * Mathf.PI;
                rndTargetLocation.x = Mathf.Sin(a) * dist;
                rndTargetLocation.y = 2;
                rndTargetLocation.z = Mathf.Cos(a) * dist;

                // Create Target based on number stored in Location 
                GameObject newTarget = (GameObject)Instantiate(TargetPrefab, rndTargetLocation, Quaternion.identity);

                // update feedback system with new target
                Player.GetComponentInChildren<FeedbackMain>().target = newTarget;
            }
            else
            {
                StartCoroutine(ExitTraining());
            }
        }
        else
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

                //close connection to the VH board
                Player.GetComponentInChildren<VibroHapticFeedback>().closeConnection();

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


    IEnumerator StartTraining()
    {

        // display text starting 
        UIText.text = "";
        KeyInfo.enabled = false; // Hide keyinformation

        GameObject.Find("FadePlane").GetComponent<Fade>().FadeIn();
        yield return new WaitForSeconds(2);
        // spawn first target

        // generate random points on circle -http://answers.unity3d.com/questions/759542/get-coordinate-with-angle-and-distance.html
        Vector3 rndTargetLocation = new Vector3();
        float dist = 10f;
        float a = Random.Range(0, 2f) * Mathf.PI;
        rndTargetLocation.x = Mathf.Sin(a) * dist;
        rndTargetLocation.y = 2;
        rndTargetLocation.z = Mathf.Cos(a) * dist;

        // Create Target based on number stored in Location 
        GameObject newTarget = (GameObject)Instantiate(TargetPrefab, rndTargetLocation, Quaternion.identity);
        // update feedback system with new target
        Player.GetComponentInChildren<FeedbackMain>().target = newTarget;
        Camera.main.GetComponent<RecordRotation>().sumRotation = 0; //reset sum rotation
    }

    IEnumerator ExitTraining()
    {
        // Reload Experiment Room with new settings
        GameObject.Find("FadePlane").GetComponent<Fade>().FadeOut();
        yield return new WaitForSeconds(2);
        UIText.text = "Training Complete \n \n Experiment: \n" + ExperimentType;
        KeyInfo.enabled = true;
        TrainingMode = false;

        Time.timeScale = 0; // pause the system
    }

    IEnumerator StartExperiment()
    {
        // display text starting 
        UIText.text = "";
        KeyInfo.enabled = false; // Hide keyinformation

        GameObject.Find("FadePlane").GetComponent<Fade>().FadeIn();
        yield return new WaitForSeconds(2);
        // reset target index
        TargetNumber = 0;

        // Create Target based on number stored in Location 
        GameObject newTarget = (GameObject)Instantiate(TargetPrefab, TargetData.Targets[TargetNumber].Location, Quaternion.identity);
        // update feedback system with new target
        Player.GetComponentInChildren<FeedbackMain>().target = newTarget;
        Camera.main.GetComponent<RecordRotation>().sumRotation = 0; //reset sum rotation
    }

}
