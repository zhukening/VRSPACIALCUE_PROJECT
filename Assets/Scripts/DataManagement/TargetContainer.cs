using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

// source ref: http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer

[XmlRoot("TargetCollection")]
public class TargetContainer
{
    [XmlArray("Targets"), XmlArrayItem("Target")]
    //public Target[] Targets;
    public List<Target> Targets = new List<Target>();

    // save data to XML
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(TargetContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    // Load Data from XML
    public static TargetContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(TargetContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as TargetContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static TargetContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(TargetContainer));
        return serializer.Deserialize(new StringReader(text)) as TargetContainer;
    }

}