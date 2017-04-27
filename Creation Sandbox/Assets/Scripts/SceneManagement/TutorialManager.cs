using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using VRTK;

public class TutorialManager : GameManager {

    [Header("Controllers", order = 0)]
    public GameObject rightController;
    public GameObject leftController;
    public GameObject cameraRig;

    [Header("Lights", order = 0)]
    public GameObject spotLight;
    public GameObject pointLight;
    public GameObject createLight;
    public GameObject dropLight;
    public GameObject gunLight;
    public GameObject targetLight;
    public GameObject exitLight;

    [Header("Props", order = 0)]
    public StartButton button;
    public ButtonPedestal buttonPedestal;
    public GameObject beginText;
    public TutorialTelevision tv;
    public Animator barrierAnimator;
    public Pillar createPillar;
    public Pillar dropPillar;
    public Pillar gunPillar;
    public TargetGroup targetGroup;
    public GameObject exitDoor;

    [Header("AudioClips", order = 0)]
    public AudioClip[] audioClips;
    public AudioClip[] errorClips;
    public AudioClip leftHandedScript;

    [Header("Screens", order = 0)]
    public Material[] screens;

    private GameObject dominantController;
    private GameObject offhandController;

    private int screenIndex = 0;
    private int audioIndex = 0;
    private int currentStep = 0;

    #region Init and Lifecycle
    // Use this for initialization
    protected override void Start () {

        if (gazeTracker != null)
        {
            gazeTracker.GazeMarkerSet += new GazeMarkerEventHandler(DoGazeMarkerSet);
        }

        if (rightPointer != null)
        {
            rightPointer.CreationPointerSet += new CreationPointerEventHandler(DoRightPointerSet);
            rightPointer.CreationPointerOff += new CreationPointerEventHandler(DoRightPointerOff);
        }

        if (leftPointer != null)
        {
            leftPointer.CreationPointerSet += new CreationPointerEventHandler(DoLeftPointerSet);
            leftPointer.CreationPointerOff += new CreationPointerEventHandler(DoLeftPointerOff);
        }

        button.events.OnPush.AddListener(StartTutorial);
        button.OnCollision += new ButtonCollisionHandler(StartButtonCollision);
        createPillar.OnCollision += new CollisionHandler(CreatedCollision);
        dropPillar.OnCollision += new CollisionHandler(DroppedCollision);
        gunPillar.OnCollision += new CollisionHandler(GunPillarCollision);
        targetGroup.OnTargetsDestroyed += new TargetsDestroyedHandler(TargetsDestroyed);
        cameraRig.GetComponent<VRTK_BasicTeleport>().Teleported += new TeleportEventHandler(OnTeleport);
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if (currentStep == 0)
        {
            rightController.GetComponent<ControllerSwitcher>().DisableController();
            leftController.GetComponent<ControllerSwitcher>().DisableController();
        }
	}
    #endregion

    #region State Changes
    private void StartTutorial()
    {
        if (currentStep == 0)
        {
            InitializeDominantHand();

            beginText.SetActive(false);
            barrierAnimator.enabled = true;
            tv.Show();
            TurnOnLights();
            PlayClip(audioClips[audioIndex]);
            tv.ChangeScreen(screens[screenIndex]);
            button.GetComponent<Collider>().enabled = false;
            buttonPedestal.Hide();
            ShowCreatePillar();
            dominantController.GetComponent<ControllerSwitcher>().VibrateController(3000, 2.0f);
            currentStep++;
        }
    }

    private void CorrectObjectCreated()
    {
        if (currentStep == 1)
        {
            AdvanceAudio();
            AdvanceScreen();
            dominantController.GetComponent<ObjectController>().EnableTrigger();
            dominantController.GetComponent<ControllerSwitcher>().HighlightTrigger(true);
            currentStep++;
        }
    }

    public void CorrectObjectGrabbed()
    {
        if (currentStep == 2)
        {
            AdvanceAudio();
            AdvanceScreen();
            ShowDropPillar();
            createLight.SetActive(false);
            createPillar.arrow.SetActive(false);
            dominantController.GetComponent<ControllerSwitcher>().HighlightTrigger(false);
            currentStep++;
        }
    }

    private void ObjectMovedCorrectly()
    {
        if (currentStep == 3)
        {
            AdvanceAudio();
            AdvanceScreen();
            createPillar.Hide();
            dropPillar.Hide();
            dropLight.SetActive(false);
            ShowGunPillar();
            currentStep++;
        }
    }

    private void GunCreated()
    {
        if (currentStep == 4)
        {
            AdvanceAudio();
            AdvanceScreen();
            createPillar.gameObject.SetActive(false);
            dropPillar.gameObject.SetActive(false);
            dominantController.GetComponent<ObjectController>().EnableGrip();
            dominantController.GetComponent<ControllerSwitcher>().HighlightGrip(true);
            currentStep++;
        }
    }

    public void GunGrabbed()
    {
        if (currentStep == 5)
        {
            AdvanceAudio();
            AdvanceScreen();
            gunPillar.Hide();
            gunLight.SetActive(false);
            dominantController.GetComponent<ControllerSwitcher>().HighlightGrip(false);
            dominantController.GetComponent<ObjectController>().DisableGrip();
            ShowTargets();
            currentStep++;
        }
    }

    private void TargetsDestroyed()
    {
        if (currentStep == 6)
        {
            AdvanceAudio();
            AdvanceScreen();
            gunPillar.gameObject.SetActive(false);
            targetLight.SetActive(false);
            dominantController.GetComponent<ObjectController>().EnableGrip();
            currentStep++;
        }
    }

    public void GunDropped()
    {
        if (currentStep == 7)
        {
            AdvanceAudio();
            AdvanceScreen();
            targetGroup.gameObject.SetActive(false);
            currentStep++;
        }
    }

    private void GunDestroyed()
    {
        if (currentStep == 8)
        {
            AdvanceAudio();
            AdvanceScreen();
            dominantController.GetComponent<ControllerSwitcher>().EnableObject();
            dominantController.GetComponent<ObjectController>().DisableTouchpad();
            offhandController.GetComponent<ControllerSwitcher>().EnableTeleport();
            offhandController.GetComponent<TeleportController>().DisableTouchpad();
            offhandController.GetComponent<ControllerSwitcher>().VibrateController(3000, 2.0f);
            offhandController.GetComponent<ControllerSwitcher>().HighlightTrigger(true);
            currentStep++;
        }
    }

    private void Teleported()
    {
        if (currentStep == 9)
        {
            AdvanceAudio();
            AdvanceScreen();
            exitDoor.SetActive(true);
            exitLight.SetActive(true);
            offhandController.GetComponent<ControllerSwitcher>().HighlightTrigger(false);
            currentStep++;
        }

    }
    #endregion

    #region Scene Management
    private void TurnOnLights()
    {
        spotLight.SetActive(false);
        pointLight.SetActive(true);
    }

    private void ShowCreatePillar()
    {
        createPillar.Show();
        createLight.SetActive(true);
    }

    private void ShowDropPillar()
    {
        dropPillar.Show();
        dropLight.SetActive(true);
    }

    private void ShowGunPillar()
    {
        gunPillar.Show();
        gunLight.SetActive(true);
    }

    private void ShowTargets()
    {
        targetGroup.Show();
        targetLight.SetActive(true);
    }

    private void AdvanceScreen()
    {
        screenIndex++;
        tv.ChangeScreen(screens[screenIndex]);
    }

    private void AdvanceAudio()
    {
        audioIndex++;

        //if (audioIndex == 8 && GameData.isLeftHanded)
        if (audioIndex == 8 && PlayerPrefs.GetInt("isLeftHanded") == 0)
        {
            PlayClip(leftHandedScript);
            return;
        }

        PlayClip(audioClips[audioIndex]);
    }
    #endregion

    #region Controller Management
    private void InitializeDominantHand()
    {
        dominantController.GetComponent<ControllerSwitcher>().EnableObject();
        dominantController.GetComponent<ObjectController>().DisableGrip();
        dominantController.GetComponent<ObjectController>().DisableTouchpad();
        dominantController.GetComponent<ObjectController>().DisableTrigger();
    }

    public void SetControllersForCurrentStep()
    {
        if (currentStep == 0)
        {
            return;
        }

        if (currentStep <= 4 || currentStep == 6)
        {
            offhandController.GetComponent<ControllerSwitcher>().DisableController();
            dominantController.GetComponent<ControllerSwitcher>().EnableObject();
            dominantController.GetComponent<ObjectController>().DisableGrip();
            dominantController.GetComponent<ObjectController>().DisableTouchpad();
            return;
        }

        if (currentStep == 5 || currentStep == 7 || currentStep == 8)
        {
            offhandController.GetComponent<ControllerSwitcher>().DisableController();
            dominantController.GetComponent<ControllerSwitcher>().EnableObject();
            dominantController.GetComponent<ObjectController>().DisableTouchpad();
            return;
        }

        if (currentStep >= 9)
        {
            dominantController.GetComponent<ControllerSwitcher>().EnableObject();
            dominantController.GetComponent<ObjectController>().DisableTouchpad();
            offhandController.GetComponent<ControllerSwitcher>().EnableTeleport();
            offhandController.GetComponent<TeleportController>().DisableTouchpad();
        }

    }
    #endregion

    #region Event Listners
    private void StartButtonCollision(string collisionTag)
    {
        if (collisionTag == "leftController")
        {
            dominantController = leftController;
            offhandController = rightController;
            //GameData.isLeftHanded = true;
            PlayerPrefs.SetInt("isLeftHanded", 1);
        } else
        {
            dominantController = rightController;
            offhandController = leftController;
        }
    }

    private void CreatedCollision(string objectType)
    {
        if (objectType == "ball" || objectType == "cube")
        {
            CorrectObjectCreated();
            createPillar.OnCollision -= CreatedCollision;
        } else
        {
            // Handle Incorrect Object with snarky response.
            Debug.Log("Wrong Object");
            PlayError(errorClips[1]);
        }
    }

    private void DroppedCollision(string objectType)
    {
        if (objectType == "ball" || objectType == "cube")
        {
            ObjectMovedCorrectly();
            dropPillar.OnCollision -= DroppedCollision;
        }
    }

    private void GunPillarCollision(string objectType)
    {
        if (objectType == "hand gun")
        {
            GunCreated();
            gunPillar.OnCollision -= GunPillarCollision;
        }
        else
        {
            // Handle Incorrect Object with snarky response.
            Debug.Log("Wrong Object");
            PlayError(errorClips[2]);
        }
    }

    private void OnTeleport(object sender, DestinationMarkerEventArgs e)
    {
        Teleported();
        if (cameraRig.transform.position.z < -5)
        {
            //set tutorial complete
            if (DataManager.dManager != null) {
                DataManager.dManager.tutorialComplete = true;
            } else {
                Debug.LogWarning("No Data Manager.");
            }
            LoadSandbox();
        }
    }

    public void BallFloorCollision(GameObject obj)
    {
        Debug.Log("Ball Floor");

        if (currentStep == 1)
        {
            PlayError(errorClips[0]);
            Destroy(obj, 1);
        }
    }

    public void GunFloorCollision(GameObject obj)
    {
        Debug.Log("Gun Floor");

        if (currentStep == 4)
        {
            PlayError(errorClips[0]);
            Destroy(obj, 1);
        }
    }
    #endregion

    #region Class Overrides
    public override void CreateObject(string key, string matKey, string scaleKey)
    {
        GameObject newObject = objects[key];
        Vector3? tempDestination = rightDestination;
        if (tempDestination == null)
        {
            tempDestination = leftDestination;
        }

        if (newObject != null && tempDestination != null)
        {
            Vector3 destination = (Vector3)tempDestination;
            Vector3 location = new Vector3(destination.x, destination.y + newObject.transform.position.y, destination.z);
            if (matKey != null && newObject.GetComponent<CreatableObject>().isCustomizable)
            {
                newObject.GetComponent<CreatableObject>().ApplyMaterial(matKey);
            }

            if (newObject.GetComponent<TutorialBall>() != null)
            {
                newObject.GetComponent<TutorialBall>().tutorialManager = this;
            }

            if (newObject.GetComponent<TutorialCube>() != null && currentStep == 1)
            {
                newObject.GetComponent<TutorialCube>().tutorialManager = this;
                Instantiate(newObject, location, newObject.transform.rotation);
            }

            if (newObject.GetComponent<TutorialGun>() != null && currentStep == 4)
            {
                newObject.GetComponent<TutorialGun>().tutorialManager = this;
                Instantiate(newObject, location, newObject.transform.rotation);
            }

            
        }
    }

    protected override void DestroyCreatedObject(GameObject createdObject)
    {
        if (currentStep < 8)
        {
            return;
        }

        if (createdObject.tag == "created")
        {
            if (createdObject.GetComponent<TutorialGun>() != null)
            {
                GunDestroyed();
            }

            Destroy(createdObject);
        }
    }
    #endregion
}
