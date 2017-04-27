using UnityEngine;
using System.Collections;
using VRTK;

public delegate void ControllerSwithcedHandler(bool isObjectController);

public class ControllerSwitcher : MonoBehaviour {

    public event ControllerSwithcedHandler OnSwitch;
    public VRTK_ControllerActions controllerActions;

    public ObjectController objectController;
    public TeleportController teleportController;
    public TouchpadDecal touchpadDecal;
    public bool isObjectController = true;
    public bool isEnabled = true;

    [Header("Controller Highlights", order = 0)]
    public Renderer leftGrip;
    public Renderer rightGrip;
    public Renderer trigger;

    public Material highlightColor;
    public Material defaultColor;

    

    #region Init and Lifecycle
    // Use this for initialization
    void Start () {
        if (isObjectController)
        {
            EnableObject();
        } else
        {
            EnableTeleport();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    #endregion

    #region Event Listeners
    protected virtual void OnSwipeLeft(object sender, SwipeEventArgs e)
    {
        Switch();
    }

    protected virtual void OnSwipeRight(object sender, SwipeEventArgs e)
    {
        Switch();
    }
    #endregion

    #region Public Interface
    public void Switch()
    {
        if (!isObjectController)
        {
            EnableObject();
        } else
        {
            EnableTeleport();
        }
        isObjectController = !isObjectController;
        EmitOnSwitch();
    }

    public void EnableObject()
    {
        objectController.enabled = true;
        teleportController.enabled = false;
        isEnabled = true;
    }

    public void EnableTeleport()
    {
        teleportController.enabled = true;
        objectController.enabled = false;
        isEnabled = true;
    }

    public void DisableController()
    {
        teleportController.enabled = false;
        objectController.enabled = false;
        touchpadDecal.OnHidden();
        isEnabled = false;
    }

    public void VibrateController(ushort rumbleAmount, float duration)
    {
        controllerActions.TriggerHapticPulse(rumbleAmount, duration, 0.01f);
    }

    public void HighlightGrip(bool on)
    {
        if (on)
        {
            leftGrip.material = highlightColor;
            rightGrip.material = highlightColor;
        } else
        {
            leftGrip.material = defaultColor;
            rightGrip.material = defaultColor;
        }
    }

    public void HighlightTrigger(bool on)
    {
        if (on)
        {
            trigger.material = highlightColor;
        } else
        {
            trigger.material = defaultColor;
        }

    }
    #endregion

    #region Private Helpers
    private void EmitOnSwitch()
    {
        if (OnSwitch != null)
        {
            OnSwitch(isObjectController);
        }
    }
    #endregion
}
