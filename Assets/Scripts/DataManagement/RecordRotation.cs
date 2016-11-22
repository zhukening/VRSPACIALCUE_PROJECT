using UnityEngine;
using System.Collections;

public class RecordRotation : MonoBehaviour {

    Quaternion rotationLast; //The value of the rotation at the previous update
    Quaternion CameraRotation; 
    Vector3 rotationDelta; //The difference in rotation between now and the previous update
 
    //Vector3 TotalRotation; - unused due to error

    public float sumRotation = 0;

    //Initialize rotationLast in start, or it will cause an error
    void Start()
    {
        rotationLast = Camera.main.transform.rotation;
    }

    void Update()
    {
        CameraRotation = Camera.main.transform.rotation;
        /* my attempt at calculating the total rotation as a quaternion but it doesn't work

        rotationDelta = CameraRotation.eulerAngles - rotationLast.eulerAngles;

        // make all rotations positive for easy addition
        if (rotationDelta.x < 0){ rotationDelta.x = rotationDelta.x * -1; }
        if (rotationDelta.y < 0) { rotationDelta.y = rotationDelta.y *  - 1; }
        if (rotationDelta.z < 0) { rotationDelta.z = rotationDelta.z *  -1; }

        TotalRotation += rotationDelta;
        Debug.Log(rotationDelta);
        Debug.Log(TotalRotation);

        
        */
        sumRotation += Quaternion.Angle(CameraRotation, rotationLast);
        rotationLast = Camera.main.transform.rotation;
    }
}
