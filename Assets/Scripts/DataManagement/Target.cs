using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

// source ref: http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer

public class Target
{
    // XML Class to store all Target information
    [XmlAttribute("Name")]
    public string Name;

    // participant ID to be gathered at home screen
    public string ParticipantID;

    // Time of 
    public float Time;
    public float DistanceTravelled;

    // Save RAW data incase there is a problem in our in engine computation of distance.
    // Add Target absolute Location
    public Vector3 Location;

    // player angle relative to player at spawn time
    public Quaternion PlayerDirection;
    // which targetSet was used
    public int TargetSet;

}