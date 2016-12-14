using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class RotationData {
    // target name
    public string Name;
    // rotation this frame
    public Quaternion qRotation;
    // frame time duration
    public float deltaTime;
}
