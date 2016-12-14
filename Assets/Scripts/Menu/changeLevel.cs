using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class changeLevel : MonoBehaviour {

    private GameObject PersistantGameObject;

    public InputField participantIDField;

    void Start()
    {
        PersistantGameObject = GameObject.Find("persistantGO");
    }

    public void updateParticipantID()
    {
        // update participant ID in persistant Object
        PersistantGameObject.GetComponent<ExperimentData>().participantID = participantIDField.text;
    }

    public void StartExperiment()
    {
        // get participandID
        int ParticipantID = int.Parse(PersistantGameObject.GetComponent<ExperimentData>().participantID);

        // configure experiment Order
        if (ParticipantID % 6 == 1)
        {
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[0] = ExperimentData.Experiment.Visual;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[1] = ExperimentData.Experiment.Audio;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[2] = ExperimentData.Experiment.VibroHaptic;
        }
        else if (ParticipantID % 6 == 2)
        {
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[0] = ExperimentData.Experiment.Audio;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[1] = ExperimentData.Experiment.VibroHaptic;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[2] = ExperimentData.Experiment.Visual;
        }
        else if (ParticipantID % 6 == 3)
        {
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[0] = ExperimentData.Experiment.VibroHaptic;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[1] = ExperimentData.Experiment.Visual;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[2] = ExperimentData.Experiment.Audio;
        }
        else if (ParticipantID % 6 == 4)
        {
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[0] = ExperimentData.Experiment.VibroHaptic;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[1] = ExperimentData.Experiment.Audio;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[2] = ExperimentData.Experiment.Visual;
        }
        else if (ParticipantID % 6 == 5)
        {
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[0] = ExperimentData.Experiment.Visual;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[1] = ExperimentData.Experiment.VibroHaptic;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[2] = ExperimentData.Experiment.Audio;
        }
        else if (ParticipantID % 6 == 0)
        {
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[0] = ExperimentData.Experiment.Audio;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[1] = ExperimentData.Experiment.Visual;
            PersistantGameObject.GetComponent<ExperimentData>().ExperimentOrder[2] = ExperimentData.Experiment.VibroHaptic;
        }

        // Load Level
        SceneManager.LoadScene("ExperimentRoom", LoadSceneMode.Single);
    }
}
