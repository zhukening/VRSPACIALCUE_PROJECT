using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RecordRotation : MonoBehaviour {

    Quaternion rotationLast; //The value of the rotation at the previous update
    Quaternion CameraRotation; 
    Vector3 rotationDelta; //The difference in rotation between now and the previous update

    private RotationContainer RotationList = new RotationContainer();

    //Vector3 TotalRotation; - unused due to error

    public float sumRotation = 0;

    //Initialize rotationLast in start, or it will cause an error
    void Start()
    {
        rotationLast = Camera.main.transform.rotation;
    }

    void Update()
    {  
        // update the rotation Log for this session
        RotationList.RotationLog.Add(new RotationData());
        RotationList.RotationLog[RotationList.RotationLog.Count - 1].deltaTime = Time.deltaTime;
        RotationList.RotationLog[RotationList.RotationLog.Count - 1].qRotation = Camera.main.transform.rotation;

        CameraRotation = Camera.main.transform.rotation;

        sumRotation += Quaternion.Angle(CameraRotation, rotationLast);
        rotationLast = Camera.main.transform.rotation;
    }

    public void SaveData()
    {
        Debug.Log("data saved");
        GameObject PersistantGameObject = GameObject.Find("persistantGO");
        string ParticipantID = PersistantGameObject.GetComponent<ExperimentData>().participantID;
        RotationList.Save(Path.Combine(Application.dataPath, "Data\\RotationLog_" + ParticipantID + ".xml"));
    }
}
