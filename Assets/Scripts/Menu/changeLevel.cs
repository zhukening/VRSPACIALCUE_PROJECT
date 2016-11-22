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

    public void StartVisualFeedbackExperiment()
    {
        // set feedback type and store in the persistant Game Object
        PersistantGameObject.GetComponent<ExperimentData>().VisualFeedback = true;
        PersistantGameObject.GetComponent<ExperimentData>().AudioFeedback = false;
        PersistantGameObject.GetComponent<ExperimentData>().VibroHapticFeedback = false;

        // Load Level
        SceneManager.LoadScene("ExperimentRoom",LoadSceneMode.Single);
    }

    public void StartAudioFeedbackExperiment()
    {
        // set feedback type and store in the persistant Game Object
        PersistantGameObject.GetComponent<ExperimentData>().VisualFeedback = false;
        PersistantGameObject.GetComponent<ExperimentData>().AudioFeedback = true;
        PersistantGameObject.GetComponent<ExperimentData>().VibroHapticFeedback = false;
        // Load Level
        SceneManager.LoadScene("ExperimentRoom", LoadSceneMode.Single);
    }

    public void StartVHapticFeedbackExperiment()
    {
        // set feedback type and store in the persistant Game Object
        PersistantGameObject.GetComponent<ExperimentData>().VisualFeedback = false;
        PersistantGameObject.GetComponent<ExperimentData>().AudioFeedback = false;
        PersistantGameObject.GetComponent<ExperimentData>().VibroHapticFeedback = true;
        // Load Level
        SceneManager.LoadScene("ExperimentRoom", LoadSceneMode.Single);
    }

}
