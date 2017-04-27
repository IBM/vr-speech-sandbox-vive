using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("GameData")]
[XmlInclude(typeof(ObjectDataList))]
public class GameData
{
    public bool tutorialComplete;
    public ObjectDataList objectList = new ObjectDataList();
    //public List<ObjectData> activeObjects = new List<ObjectData>();
}