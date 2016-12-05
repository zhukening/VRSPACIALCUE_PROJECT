using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

// source ref: http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer

[XmlRoot("RotationLog")]
public class RotationContainer
{
    [XmlArray("Rotation"), XmlArrayItem("Log")]
    //public Target[] Targets;
    public List<RotationData> RotationLog = new List<RotationData>();

    // save data to XML
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(RotationContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

}