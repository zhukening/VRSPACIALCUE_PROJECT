using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RecordRotation : MonoBehaviour {

    Quaternion rotationLast; //The value of the rotation at the previous update
    Quaternion CameraRotation; 
    Vector3 rotationDelta; //The difference in rotation between now and the previous update

    public int TargetNumber; 

    private ExperimentData experimentData;

    private RotationContainer RotationList = new RotationContainer();

    //Vector3 TotalRotation; - unused due to error

    public float sumRotation = 0;

    //Initialize rotationLast in start, or it will cause an error
    void Start()
    {
        GameObject PersistantGameObject = GameObject.Find("persistantGO");
        experimentData = PersistantGameObject.GetComponent<ExperimentData>();
        
        rotationLast = Camera.main.transform.rotation;
    }

    void Update()
    {  
        // update the rotation Log for this session

        RotationList.RotationLog.Add(new RotationData());
        RotationList.RotationLog[RotationList.RotationLog.Count - 1].Name = experimentData.GetCurrentExperiment() + "_" + TargetNumber;
        RotationList.RotationLog[RotationList.RotationLog.Count - 1].deltaTime = Time.deltaTime;
        RotationList.RotationLog[RotationList.RotationLog.Count - 1].qRotation = Camera.main.transform.rotation;

        CameraRotation = Camera.main.transform.rotation;

        sumRotation += Quaternion.Angle(CameraRotation, rotationLast);
        rotationLast = Camera.main.transform.rotation;
    }

    public void SaveData()
    {
        Debug.Log("rotationLogSaved");
        GameObject PersistantGameObject = GameObject.Find("persistantGO");
        string ParticipantID = PersistantGameObject.GetComponent<ExperimentData>().participantID;
        // filename is RotationLog_EXPINDEX_PARTID_CUETYPE.xml
        RotationList.Save(Path.Combine(Application.dataPath, "Data\\RotationLog_"+ experimentData.GetExperimentIndex() + "_" + ParticipantID + "_" + experimentData.GetCurrentExperiment() +  ".xml"));
    }
}
