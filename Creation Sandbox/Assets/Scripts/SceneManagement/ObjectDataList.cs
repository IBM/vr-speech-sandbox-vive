using UnityEngine;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

[XmlType("ObjectDataList")]
[XmlInclude(typeof(ObjectData))]
public class ObjectDataList {

    [XmlArray("ObjectList")]
    [XmlArrayItem("ActiveObject")]
    public List<ObjectData> activeObjects = new List<ObjectData>();

    public void Add(ObjectData data)
    {
        activeObjects.Add(data);
    }
}
