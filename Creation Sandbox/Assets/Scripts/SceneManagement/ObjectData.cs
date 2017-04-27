using UnityEngine;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

[Serializable]
[XmlType("ObjectData")]
public class ObjectData {
    public string id;
    //Transform Data
    public float xPos;
    public float yPos;
    public float zPos;
    public float xRot;
    public float yRot;
    public float zRot;
    public float w;
    public float xScale;
    public float yScale;
    public float zScale;
    //Rigidbody Data
    public float isFrozen;
    //Material Data
    public string mat;
}
