using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System;

public class DataManager : MonoBehaviour {

    public static DataManager dManager;

    public bool tutorialComplete;
    public ObjectDataList activeObjects;
    public bool isLoaded;

	void Awake() {
        if (dManager == null)
        {
            DontDestroyOnLoad(gameObject);
            dManager = this;
        }
        else if(dManager != this)
        {
            Destroy(gameObject);
        }
	}

    void Start()
    {
        if(!PlayerPrefs.HasKey("isLeftHanded"))
        {
            PlayerPrefs.SetInt("isLeftHanded", 0);
        }

        if(!PlayerPrefs.HasKey("isRigCam"))
        {
            PlayerPrefs.SetInt("isRigCam", 0);
        }

        if(!PlayerPrefs.HasKey("isCamMenuOpen"))
        {
            PlayerPrefs.SetInt("isCamMenuOpen", 1);
        }
    }

    public void Save()
    {
        Type[] types = { typeof(ObjectDataList), typeof(ObjectData) };
        XmlSerializer xml = new XmlSerializer(typeof(GameData), types);
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/gameData.xml"))
        {
            file = File.Open(Application.persistentDataPath + "/gameData.xml", FileMode.Truncate);
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/gameData.xml");
        }

        GameData data = new GameData();
        data.tutorialComplete = tutorialComplete;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Sandbox"))
        {
            activeObjects = new ObjectDataList();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("created");
            foreach (GameObject obj in objects) {
                if(obj.GetComponent<CreatableObject>().id != null && obj.GetComponent<CreatableObject>().id != "")
                {
                    ObjectData objectData = new ObjectData();
                    objectData.id = obj.GetComponent<CreatableObject>().id;
                    objectData.mat = obj.GetComponent<CreatableObject>().matKey;
                    Transform trans = obj.GetComponent<Transform>();
                    objectData.xPos = trans.position.x;
                    objectData.yPos = trans.position.y;
                    objectData.zPos = trans.position.z;
                    objectData.xRot = trans.rotation.x;
                    objectData.yRot = trans.rotation.y;
                    objectData.zRot = trans.rotation.z;
                    objectData.w = trans.rotation.w;
                    objectData.xScale = trans.localScale.x;
                    objectData.yScale = trans.localScale.y;
                    objectData.zScale = trans.localScale.z;
                    objectData.isFrozen = (float) obj.GetComponent<Rigidbody>().constraints;
                    activeObjects.Add(objectData);
                }
            }
        }
        data.objectList = activeObjects;
        xml.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.xml"))
        {
            Type[] types = { typeof(ObjectDataList), typeof(ObjectData) };
            XmlSerializer xml = new XmlSerializer(typeof(GameData), types);
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.xml", FileMode.Open);
            GameData data = (GameData)xml.Deserialize(file);
            file.Close();
            //set local values to those of data
            tutorialComplete = data.tutorialComplete;
            Debug.Log(data.objectList.activeObjects.Count);
            activeObjects = data.objectList;
            isLoaded = true;

        }
    }

    public void setActiveObjects(ObjectDataList objects)
    {
        activeObjects = objects;
    }

    public ObjectDataList getActiveObjects()
    {
        return activeObjects;
    }

    public void ClearSandbox()
    {
        activeObjects = new ObjectDataList();
        isLoaded = false;
    }
}
