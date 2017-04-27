using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.IO;

public class GameManager : MonoBehaviour {


    [Header("Objects Dictionary", order = 0)]
    public List<string> _oKeys = new List<string>();
    public List<GameObject> _oValues = new List<GameObject>();
    protected Dictionary<string, GameObject> objects;

    [Header("Pointers", order = 3)]
    public GazeTracker gazeTracker;
    protected Vector3 gazeDestination;
    protected GameObject gazeTarget;

    public CreationPointer rightPointer;
    protected Vector3? rightDestination;
    protected GameObject rightTarget;

    public CreationPointer leftPointer;
    protected Vector3? leftDestination;
    protected GameObject leftTarget;

    [Header("AudioSources", order = 4)]
    public AudioSource musicSource;
    public AudioSource voiceSource;
    public AudioSource errorSource;

    [Header("AudioClips", order = 5)]
    public AudioClip welcomeMessage;

    protected enum Action
    {
        Create,
        Destroy
    }
    protected enum Location
    {
        Here,
        There
    }

    #region InitAndLifecycle
    //------------------------------------------------------------------------------------------------------------------
    // Initialization and Lifecycle
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void Awake()
    {
        InitializeObjects();
        Screen.SetResolution(1280, 720, false);
    }

    protected virtual void Start () {
        if (gazeTracker != null)
        {
            gazeTracker.GazeMarkerSet += new GazeMarkerEventHandler(DoGazeMarkerSet);
        } else
        {
            Debug.Log("GameManager: Gaze Tracker is null.");
        }

        if (rightPointer != null)
        {
            rightPointer.CreationPointerSet += new CreationPointerEventHandler(DoRightPointerSet);
            rightPointer.CreationPointerOff += new CreationPointerEventHandler(DoRightPointerOff);
        } else
        {
            Debug.Log("GameManager: Right Pointer is null.");
        }

        if (leftPointer != null)
        {
            leftPointer.CreationPointerSet += new CreationPointerEventHandler(DoLeftPointerSet);
            leftPointer.CreationPointerOff += new CreationPointerEventHandler(DoLeftPointerOff);
        } else
        {
            Debug.Log("GameManager: Left Pointer is null.");
        }

        if (PlayerPrefs.GetInt("isLeftHanded") != 0)
        {
            leftPointer.gameObject.GetComponent<ControllerSwitcher>().EnableObject();
            rightPointer.gameObject.GetComponent<ControllerSwitcher>().EnableTeleport();
        } else
        {
            leftPointer.gameObject.GetComponent<ControllerSwitcher>().EnableTeleport();
            rightPointer.gameObject.GetComponent<ControllerSwitcher>().EnableObject();
        }

        //load saved objects
        if (DataManager.dManager != null)
        {
            //We don't do real serialization, so this is a workaround to make sure we don't load certain objects twice
            if (DataManager.dManager.isLoaded)
            {
                GameObject[] initialObjects = GameObject.FindGameObjectsWithTag("created");
                foreach (GameObject obj in initialObjects)
                {
                    obj.SetActive(false);
                }
                DataManager.dManager.isLoaded = false;

                if (DataManager.dManager.activeObjects != null && DataManager.dManager.activeObjects.activeObjects.Count > 0)
                {
                    foreach (ObjectData obj in DataManager.dManager.activeObjects.activeObjects)
                    {
                        LoadObject(obj);
                    }
                }

            }

        }

        PlayClip(welcomeMessage);
    }

    // Update is called once per frame
    protected virtual void Update () {
	
	}

    void InitializeObjects()
    {
        objects = new Dictionary<string, GameObject>();

        for (var i = 0; i != Math.Min(_oKeys.Count, _oValues.Count); i++)
        {
            objects.Add(_oKeys[i], _oValues[i]);
        }

        _oKeys.Clear();
        _oValues.Clear();
    }
    #endregion

    #region Public
    //------------------------------------------------------------------------------------------------------------------
    // Public Interface Functions
    //------------------------------------------------------------------------------------------------------------------

    #region Right
    public void ClearRightDestination()
    {
        rightDestination = null;
        rightTarget = null;
    }

    public void DestroyAtRight()
    {
        if (rightTarget != null)
        {
            DestroyCreatedObject(rightTarget);
        }
    }

    #endregion

    #region Left
    public void ClearLeftDestination()
    {
        leftDestination = null;
        leftTarget = null;
    }

    public void DestroyAtLeft()
    {
        if (leftTarget != null)
        {
            DestroyCreatedObject(leftTarget);
        }
    }

    #endregion

    #region Gaze
    public void DestroyAtGaze()
    {
        if (gazeTarget != null)
        {
            DestroyCreatedObject(gazeTarget);
        }
    }

    public void TeleportToGaze()
    {
        if (gazeDestination != null)
        {
            gazeTracker.Teleport();
        }
    }

    #endregion

    public void PlayClip(AudioClip clip)
    {
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    public void PlayError(AudioClip clip)
    {
        if (!errorSource.isPlaying && !voiceSource.isPlaying)
        {
            errorSource.clip = clip;
            errorSource.Play();
        }
    }

    public GameObject GetObject(string key)
    {
        return objects[key];
    }

    public virtual void CreateObject(string key, string matKey)
    {
        GameObject newObject = objects[key];
   
        // Check Right
        Vector3? tempDestination = rightDestination;
        string hand = "right";
        // Check Left
        if (tempDestination == null)
        {
            tempDestination = leftDestination;
            hand = "left";
        }
        // Check Gaze
        if (tempDestination == null)
        {
            tempDestination = gazeDestination;
            hand = "gaze";
        }

        if (newObject != null && tempDestination != null)
        {
            Vector3 destination = (Vector3)tempDestination;
            Vector3 location = new Vector3(destination.x, destination.y + newObject.transform.position.y, destination.z);     
            Debug.Log(Analytics.CustomEvent("createdObject", new Dictionary<string, object> { {"objectKey", key }, {"pointerHand", hand } }));

            if (matKey != null && newObject.GetComponent<CreatableObject>().isCustomizable)
            {
                newObject.GetComponent<CreatableObject>().ApplyMaterial(matKey);
                newObject.GetComponent<CreatableObject>().matKey = matKey;
            }

            Instantiate(newObject, location, newObject.transform.rotation);
            
        }
    }

    // Modified for scale
    public virtual void CreateObject(string key, string matKey, string scale)
    {
        GameObject newObject = objects[key];

        // Check Right
        Vector3? tempDestination = rightDestination;
        string hand = "right";
        // Check Left
        if (tempDestination == null)
        {
            tempDestination = leftDestination;
            hand = "left";
        }
        // Check Gaze
        if (tempDestination == null)
        {
            tempDestination = gazeDestination;
            hand = "gaze";
        }

        if (newObject != null && tempDestination != null)
        {
            Vector3 destination = (Vector3)tempDestination;
            Vector3 location = new Vector3(destination.x, destination.y + newObject.transform.position.y, destination.z);
            Debug.Log(Analytics.CustomEvent("createdObject", new Dictionary<string, object> { { "objectKey", key }, { "pointerHand", hand } }));

            if (matKey != null && newObject.GetComponent<CreatableObject>().isCustomizable)
            {
                newObject.GetComponent<CreatableObject>().ApplyMaterial(matKey);
                newObject.GetComponent<CreatableObject>().matKey = matKey;
            }

            GameObject objectInstance = (GameObject) Instantiate(newObject, location, newObject.transform.rotation);

            if (scale != null)
            {
                objectInstance.GetComponent<CreatableObject>().ApplyScale(scale);
            }
        }
    }

    private void LoadObject(ObjectData data)
    {
        GameObject newObject = objects[data.id];

        if (newObject != null)
        {
            Vector3 location = new Vector3(data.xPos, data.yPos, data.zPos);

            GameObject objectInstance = (GameObject)Instantiate(newObject, location, new Quaternion(data.xRot, data.yRot, data.zRot, data.w));

            if (data.mat != "" && newObject.GetComponent<CreatableObject>().isCustomizable)
            {
                objectInstance.GetComponent<CreatableObject>().ApplyMaterial(data.mat);
                objectInstance.GetComponent<CreatableObject>().matKey = data.mat;
            }

            objectInstance.GetComponent<Transform>().localScale = new Vector3(data.xScale, data.yScale, data.zScale);
            objectInstance.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)data.isFrozen;
        }
    }
    #endregion

    #region Menu Functions
    //------------------------------------------------------------------------------------------------------------------
    // Menu Functions
    //------------------------------------------------------------------------------------------------------------------
    public virtual void LoadSandbox()
    {
        if (DataManager.dManager != null) {
            if(!DataManager.dManager.tutorialComplete)
            {
                DataManager.dManager.tutorialComplete = true;
            }
            DataManager.dManager.Save();
        } else {
            Debug.LogWarning("No Data Manager.");
        }
        SceneManager.LoadScene(2);
    }

    public virtual void ClearAndLoadSandbox()
    {
        if (DataManager.dManager != null) {
            DataManager.dManager.ClearSandbox();
        } else {
            Debug.LogWarning("No Data Manager.");
        }
        SceneManager.LoadScene(2);
    }

    public virtual void LoadTutorial()
    {
        if (DataManager.dManager != null) {
            if(DataManager.dManager.activeObjects != null && DataManager.dManager.activeObjects.activeObjects.Count > 0)
            {
                DataManager.dManager.isLoaded = true;
            }
        
            DataManager.dManager.Save();
        } else {
            Debug.LogWarning("No Data Manager.");
        }
        SceneManager.LoadScene(1);
    }

    public virtual void ClearAndLoadTutorial()
    {
        if (DataManager.dManager != null) {
            DataManager.dManager.ClearSandbox();
        } else {
            Debug.LogWarning("No Data Manager.");
        }
        LoadTutorialWithoutSaving();
    }

    public virtual void LoadTutorialWithoutSaving()
    {
        SceneManager.LoadScene(1);
    }

    public virtual void ExitGame()
    {
        if (DataManager.dManager != null) {
            DataManager.dManager.Save();
        } else {
            Debug.LogWarning("No Data Manager.");
        }
        Application.Quit();
    }

    public virtual void ExitWithoutSaving()
    {
        Application.Quit();
    }
    #endregion

    #region EventHandlers
    //------------------------------------------------------------------------------------------------------------------
    // Event Handlers
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void DoGazeMarkerSet(object sender, GazeMarkerEventArgs e)
    {
        gazeDestination = e.position;
        gazeTarget = e.gazeTarget;
    }

    protected virtual void DoRightPointerSet(object sender, CreationPointerEventArgs e)
    {
        rightDestination = e.position;
        rightTarget = e.targetObject;
    }

    protected virtual void DoRightPointerOff(object sender, CreationPointerEventArgs e)
    {
        ClearRightDestination();
    }

    protected virtual void DoLeftPointerSet(object sender, CreationPointerEventArgs e)
    {
        leftDestination = e.position;
        leftTarget = e.targetObject;
    }

    protected virtual void DoLeftPointerOff(object sender, CreationPointerEventArgs e)
    {
        ClearLeftDestination();
    }
    #endregion

    #region Protected
    //------------------------------------------------------------------------------------------------------------------
    // Protected Class Functions
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void DestroyCreatedObject(GameObject createdObject)
    {
        if (createdObject.tag == "created")
        {
            Destroy(createdObject);
        }
    }
    #endregion
}
