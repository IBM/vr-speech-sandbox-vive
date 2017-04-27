
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class ObjectController : ControllerListener
{
    [Header("Object Controller", order = 0)]
    public CreationPointer creationPointer;
    public GameObject objectTool;
    public float triggerThrowMultiplier = 2.0f;
    public float pushSpeed = 0.1f;
    public float pullSpeed = 0.1f;
    public float scaleSpeed = 0.9f;

    private SteamVR_Controller.Device controller;
    private bool increaseSize = false;
    private bool decreaseSize = false;
    private bool pushObject = false;
    private bool pullObject = false;

    #region Lifecycle and Init
    //------------------------------------------------------------------------------------------------------------------
    // Initialization, Lifecycle, and Utility Functions
    //------------------------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
        SetControls();
    }

    protected override void Update()
    {
        base.Update();
        if (heldObject == null)
        {
            interactGrab.ForceRelease();
            interactTouch.ForceStopTouching();
            isTouching = false;
            isHolding = false;
        }
    }

    protected override void FixedUpdate()
    {
        if (isHolding)
        {
            CheckControls();
        }
    }

    private void CheckControls()
    {
        if (increaseSize)
        {
            float scaleBoundMax;
            if (heldObject.GetComponent<CreatableObject>().maxScale > 0)
            {
                scaleBoundMax = heldObject.GetComponent<CreatableObject>().maxScale;
            }
            else
            {
                scaleBoundMax = heldObject.GetComponent<CreatableObject>().GetInitialScale().x * 3;
            }

            if (heldObject.transform.localScale.x <= scaleBoundMax)
            {
                heldObject.transform.localScale += new Vector3(scaleSpeed / 100 * heldObject.transform.localScale.x, scaleSpeed / 100 * heldObject.transform.localScale.y, scaleSpeed / 100 * heldObject.transform.localScale.z);
            }
        }
        else if (decreaseSize)
        {
            float scaleBoundMin;
            if (heldObject.GetComponent<CreatableObject>().minScale > 0)
            {
                scaleBoundMin = heldObject.transform.GetComponent<CreatableObject>().minScale;
            }
            else
            {
                scaleBoundMin = heldObject.GetComponent<CreatableObject>().GetInitialScale().x * 0.3f;
            }

            if (heldObject.transform.localScale.x >= scaleBoundMin)
            {
                heldObject.transform.localScale += new Vector3(-scaleSpeed / 100 * heldObject.transform.localScale.x, -scaleSpeed / 100 * heldObject.transform.localScale.y, -scaleSpeed / 100 * heldObject.transform.localScale.z);
            }
        }
        else if (pushObject)
        {
            float maxDistance;
            if (heldObject.GetComponent<CreatableObject>().maxDistance > 0)
            {
                maxDistance = heldObject.GetComponent<CreatableObject>().maxDistance;
            } else
            {
                // Max distance is defaulted to be less than 1 less than pointer range.
                maxDistance = 24f;
            }

            if (heldObject.transform.localPosition.z < maxDistance)
            {
                heldObject.transform.localPosition += new Vector3(0, 0, pushSpeed);
            }

        }
        else if (pullObject)
        {
            float minDistance;
            if (heldObject.GetComponent<CreatableObject>().minDistance > 0)
            {
                minDistance = heldObject.GetComponent<CreatableObject>().minDistance;
            }
            else
            {
                minDistance = 1f;
            }

            if (heldObject.transform.localPosition.z > minDistance)
            {
                heldObject.transform.localPosition += new Vector3(0, 0, -pullSpeed);
            }
        }
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        //Debug.Log("Object Enable");
        SetControls();
        creationPointer.CreationPointerSet += new CreationPointerEventHandler(OnPointerSet);
        creationPointer.enabled = true;
        objectTool.SetActive(true);
        touchpadDecal.OnObject();
    }

    override protected void OnDisable()
    {
        base.OnDisable();
        creationPointer.CreationPointerSet -= OnPointerSet;
        creationPointer.enabled = false;
        objectTool.SetActive(false);
    }

    void SetControls()
    {
        if (controllerEvents != null)
        {
            controllerEvents.pointerToggleButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
            controllerEvents.pointerSetButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
            controllerEvents.grabToggleButton = VRTK_ControllerEvents.ButtonAlias.Grip;
            controllerEvents.useToggleButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;
            controllerEvents.uiClickButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;
            controllerEvents.menuToggleButton = VRTK_ControllerEvents.ButtonAlias.Application_Menu;
        }
    }
    #endregion

    #region Public Overrides
    public override void EnableTrigger()
    {
        base.EnableTrigger();
        controllerEvents.useToggleButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;
    }

    public override void EnableGrip()
    {
        base.EnableGrip();
        controllerEvents.grabToggleButton = VRTK_ControllerEvents.ButtonAlias.Grip;
    }

    public override void DisableTrigger()
    {
        base.DisableTrigger();
        controllerEvents.useToggleButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
    }

    public override void DisableGrip()
    {
        base.DisableGrip();
        controllerEvents.grabToggleButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
    }
    #endregion

    #region Grip
    //------------------------------------------------------------------------------------------------------------------
    // Grip button Listeners
    //------------------------------------------------------------------------------------------------------------------
    //override protected void DoGripClicked(object sender, ControllerInteractionEventArgs e)
    //{
    //    base.DoGripClicked(sender, e);
    //    if (isTouching && !touchedObject.GetComponent<CreatableObject>().isLarge)
    //    {
    //        GetComponent<VRTK_InteractGrab>().AttemptGrab();
    //    }
    //}
    #endregion

    #region Trigger
    //------------------------------------------------------------------------------------------------------------------
    // Trigger button Listeners
    //------------------------------------------------------------------------------------------------------------------
    override protected void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        base.DoTriggerPressed(sender, e);
        if (touchedObject == null)
        {
            interactTouch.ForceStopTouching();
            interactGrab.ForceRelease();
            isTouching = false;
            isHolding = false;
        }

        if (isTouching && !isHolding)
        {
            touchedObject.GetComponent<CreatableObject>().precisionSnap = true;
            interactGrab.hideControllerOnGrab = false;
            interactGrab.AttemptGrab();
        }
    }

    override protected void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        base.DoTriggerReleased(sender, e);
        if (heldObject == null)
        {
            interactGrab.ForceRelease();
            interactTouch.ForceStopTouching();
            isTouching = false;
            isHolding = false;
        }

        if (isHolding && heldObject.GetComponent<CreatableObject>().precisionSnap == true)
        {
            interactGrab.ForceRelease();
        }
    }
    #endregion

    #region Touchpad
    //------------------------------------------------------------------------------------------------------------------
    // Touchpad Listeners
    //------------------------------------------------------------------------------------------------------------------
    override protected void DoTouchpadClicked(object sender, ControllerInteractionEventArgs e)
    {
        base.DoTouchpadClicked(sender, e);
        
        if(isHolding && heldObject.GetComponent<CreatableObject>())
        {
            float magnitude = Mathf.Sqrt(e.touchpadAxis.x * e.touchpadAxis.x + e.touchpadAxis.y * e.touchpadAxis.y);
            Debug.Log(magnitude);
            Debug.Log(e.touchpadAngle);
            if (magnitude < 0.25)
            {
                // Pause/Unpause
                heldObject.GetComponent<CreatableObject>().ToggleFreeze();
                if (heldObject.GetComponent<CreatableObject>().isFrozen)
                {
                    touchpadDecal.OnObjectControlsPlay();
                }
                else
                {
                    touchpadDecal.OnObjectControlsPause();
                }
            } else
            {
                if (e.touchpadAngle >= 315 || e.touchpadAngle < 45)
                {
                    // Push Away
                    pushObject = true;
                } else if (e.touchpadAngle >= 45 && e.touchpadAngle < 135)
                {
                    // Size Increase
                    increaseSize = true;
                } else if (e.touchpadAngle >= 135 && e.touchpadAngle < 225)
                {
                    // Pull Toward
                    pullObject = true;
                } else if (e.touchpadAngle >= 225 && e.touchpadAngle < 315)
                {
                    // Size Decrease
                    decreaseSize = true;
                }
            }
            
        }
    }

    protected override void DoTouchpadUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        base.DoTouchpadUnclicked(sender, e);
        pushObject = false;
        pullObject = false;
        increaseSize = false;
        decreaseSize = false;
    }
    #endregion

    #region Touch and Grab
    //------------------------------------------------------------------------------------------------------------------
    // Interaction Listeners
    //------------------------------------------------------------------------------------------------------------------

    override protected void DoInteractGrab(object sender, ObjectInteractEventArgs e)
    {
        base.DoInteractGrab(sender, e);
        if (heldObject == null)
        {
            interactGrab.ForceRelease();
            interactTouch.ForceStopTouching();
            isTouching = false;
            isHolding = false;
            touchpadDecal.OnObject();
        }

        if (heldObject.GetComponent<CreatableObject>().precisionSnap == false)
        {
            // Grip Snap
            creationPointer.enabled = false;
        } else
        {
            // Trigger Grab
            interactGrab.throwMultiplier = triggerThrowMultiplier;
            if (heldObject.GetComponent<CreatableObject>().isFrozen)
            {
                touchpadDecal.OnObjectControlsPlay();
            } else
            {
                touchpadDecal.OnObjectControlsPause();
            }
        }

        controller = SteamVR_Controller.Input((int)e.controllerIndex);
    }

    override protected void DoInteractUngrab(object sender, ObjectInteractEventArgs e)
    {
        base.DoInteractUngrab(sender, e);
        interactGrab.throwMultiplier = 1.0f;
        interactGrab.hideControllerOnGrab = true;
        creationPointer.enabled = true;
        touchpadDecal.OnObject();

        controller = null;
    }
    #endregion

    #region Destination Marker Events
    //------------------------------------------------------------------------------------------------------------------
    // Interaction Listeners
    //------------------------------------------------------------------------------------------------------------------

    protected virtual void OnPointerSet(object sender, CreationPointerEventArgs e)
    {
        if (!isHolding && isTouching && (e.targetObject == null || !e.targetObject.CompareTag("created")))
        {
            interactTouch.ForceStopTouching();
            return;
        }

        if (e.targetObject != null)
        {
            if (!isHolding && !isTouching && e.targetObject.CompareTag("created"))
            {
                interactTouch.ForceTouch(e.targetObject);
            }
        }
    }
    #endregion
}
