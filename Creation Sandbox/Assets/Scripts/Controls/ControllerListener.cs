
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class ControllerListener : MonoBehaviour
{
    public GameObject DebugScreen;
    public GameObject Menu;
    public Menu MenuCanvas;
    public bool DebugMode = false;
    public bool AllowsMenu = true;
    
    protected bool isTouching = false;
    protected GameObject touchedObject;
    protected bool isHolding = false;
    protected GameObject heldObject;

    public Collider controllerCollider;

    public VRTK_UIPointer uiPointer;
    public VRTK_ControllerEvents controllerEvents;
    public VRTK_InteractTouch interactTouch;
    public VRTK_InteractGrab interactGrab;
    public VRTK_InteractUse interactUse;
    public ControllerSwitcher controllerSwitcher;

    public TouchpadDecal touchpadDecal;

    protected bool sentMenuSignal = false;

    #region Lifecycle and Init
    //------------------------------------------------------------------------------------------------------------------
    // Initialization, Lifecycle, and Utility Functions
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void OnEnable()
    {
        // Setup controller event listeners
        if (controllerEvents != null)
        {
            if (AllowsMenu)
            {
                EnableMenuButton();
                if (MenuCanvas != null)
                {
                    MenuCanvas.MenuEvent += new global::MenuStateChanged(MenuStateChanged);
                }
            }
            EnableTouchpad();
            EnableTrigger();
            EnableGrip();

            EnableTouch();
            EnableGrab();
            EnableUse();

            controllerEvents.AliasPointerOn += new ControllerInteractionEventHandler(DoPointerOn);
            controllerEvents.AliasPointerOff += new ControllerInteractionEventHandler(DoPointerOff);
            
        }
        
    }

    protected virtual void OnDisable()
    {
        // Remove controller event listeners
        if (controllerEvents != null)
        {
            if (AllowsMenu)
            {
                DisableMenuButton();
                if (MenuCanvas != null)
                {
                    MenuCanvas.MenuEvent -= MenuStateChanged;
                }
            }
            DisableTouchpad();
            DisableTrigger();
            DisableGrip();

            DisableTouch();
            DisableGrab();
            DisableUse();

            controllerEvents.AliasPointerOn -= DoPointerOn;
            controllerEvents.AliasPointerOff -= DoPointerOff;
            
        }
    }
    #endregion

    #region Public Interface
    public virtual void EnableMenuButton()
    {
        controllerEvents.ApplicationMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuClicked);
        controllerEvents.ApplicationMenuReleased += new ControllerInteractionEventHandler(DoApplicationMenuUnclicked);
    }

    public virtual void EnableTouchpad()
    {
        controllerEvents.TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadClicked);
        controllerEvents.TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadUnclicked);
        controllerEvents.TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouched);
    }

    public virtual void EnableTrigger()
    {
        controllerEvents.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        controllerEvents.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
    }

    public virtual void EnableGrip()
    {
        controllerEvents.GripPressed += new ControllerInteractionEventHandler(DoGripClicked);
        controllerEvents.GripReleased += new ControllerInteractionEventHandler(DoGripUnclicked);
    }

    public virtual void EnableTouch()
    {
        if (interactTouch != null)
        {
            interactTouch.enabled = true;
            interactTouch.ControllerTouchInteractableObject += new ObjectInteractEventHandler(DoInteractTouch);
            interactTouch.ControllerUntouchInteractableObject += new ObjectInteractEventHandler(DoInteractUntouch);
        }
    }

    public virtual void EnableGrab()
    {
        if (interactGrab != null)
        {
            interactGrab.enabled = true;
            interactGrab.ControllerGrabInteractableObject += new ObjectInteractEventHandler(DoInteractGrab);
            interactGrab.ControllerUngrabInteractableObject += new ObjectInteractEventHandler(DoInteractUngrab);
        }
    }

    public virtual void EnableUse()
    {
        if (interactUse != null)
        {
            interactUse.enabled = true;
            interactUse.ControllerUseInteractableObject += new ObjectInteractEventHandler(DoInteractUse);
            interactUse.ControllerUnuseInteractableObject += new ObjectInteractEventHandler(DoInteractUnuse);
        }
    }

    public virtual void DisableMenuButton()
    {
        controllerEvents.ApplicationMenuPressed -= DoApplicationMenuClicked;
        controllerEvents.ApplicationMenuReleased -= DoApplicationMenuUnclicked;
    }

    public virtual void DisableTouchpad()
    {
        controllerEvents.TouchpadPressed -= DoTouchpadClicked;
        controllerEvents.TouchpadReleased -= DoTouchpadUnclicked;
        controllerEvents.TouchpadTouchStart -= DoTouchpadTouched;
    }

    public virtual void DisableTrigger()
    {
        controllerEvents.TriggerPressed -= DoTriggerPressed;
        controllerEvents.TriggerReleased -= DoTriggerReleased;
    }

    public virtual void DisableGrip()
    {
        controllerEvents.GripPressed -= DoGripClicked;
        controllerEvents.GripReleased -= DoGripUnclicked;
    }

    public virtual void DisableTouch()
    {
        if (interactTouch != null)
        {
            interactTouch.ControllerTouchInteractableObject -= DoInteractTouch;
            interactTouch.ControllerUntouchInteractableObject -= DoInteractUntouch;
        }
    }

    public virtual void DisableGrab()
    {
        if (interactGrab != null)
        {
            interactGrab.ControllerGrabInteractableObject -= DoInteractGrab;
            interactGrab.ControllerUngrabInteractableObject -= DoInteractUngrab;
        }
    }

    public virtual void DisableUse()
    {
        if (interactUse != null)
        {
            interactUse.ControllerUseInteractableObject -= DoInteractUse;
            interactUse.ControllerUnuseInteractableObject -= DoInteractUnuse;
        }
    }
    #endregion

    #region Event Listeners
    #region Menu
    //------------------------------------------------------------------------------------------------------------------
    // Menu Button Listeners
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void DoApplicationMenuClicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "APPLICATION MENU", "pressed down", e.buttonPressure, e.touchpadAxis);
        //DebugScreen.SetActive(!DebugScreen.activeSelf);
        sentMenuSignal = true;
        Menu.SetActive(!Menu.activeSelf);
    }

    protected virtual void DoApplicationMenuUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "APPLICATION MENU", "released", e.buttonPressure, e.touchpadAxis);
    }

    protected virtual void MenuStateChanged(bool isOpen)
    {
        //Debug.Log("Menu State Changed: " + isOpen);
        if (isOpen)
        {
            controllerSwitcher.DisableController();
            EnableMenuButton();
            MenuCanvas.GetComponent<Menu>().MenuEvent += new global::MenuStateChanged(MenuStateChanged);
            if (sentMenuSignal)
            {
                uiPointer.enabled = true;
                GetComponent<CreationPointer>().enabled = true;
            }
        } else
        {
            uiPointer.enabled = false;
            GetComponent<CreationPointer>().enabled = false;
            DisableMenuButton();
            MenuCanvas.GetComponent<Menu>().MenuEvent -= MenuStateChanged;
            if (controllerSwitcher.isObjectController)
            {
                controllerSwitcher.EnableObject();
            }
            else
            {
                controllerSwitcher.EnableTeleport();
            }
        }
        sentMenuSignal = false;
    }
    #endregion

    #region Grip
    //------------------------------------------------------------------------------------------------------------------
    // Grip button Listeners
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void DoGripClicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "GRIP", "pressed down", e.buttonPressure, e.touchpadAxis);
    }

    protected virtual void DoGripUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "GRIP", "released", e.buttonPressure, e.touchpadAxis);
    }
    #endregion

    #region Trigger
    //------------------------------------------------------------------------------------------------------------------
    // Trigger button Listeners
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "pressed", e.buttonPressure, e.touchpadAxis);
    }

    protected virtual void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "released", e.buttonPressure, e.touchpadAxis);
    }
    #endregion

    #region Touchpad
    //------------------------------------------------------------------------------------------------------------------
    // Touchpad Listeners
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void DoTouchpadClicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "pressed down", e.buttonPressure, e.touchpadAxis);
        if (controllerSwitcher != null && !isHolding)
        {
            controllerSwitcher.Switch();
        }
    }

    protected virtual void DoTouchpadUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "released", e.buttonPressure, e.touchpadAxis);
    }

    protected virtual void DoTouchpadTouched(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "touched", e.buttonPressure, e.touchpadAxis);
    }
    #endregion

    #region Touch, Grab, and Use
    //------------------------------------------------------------------------------------------------------------------
    // Interaction Listeners
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void DoInteractTouch(object sender, ObjectInteractEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TOUCHING", e.target);
        isTouching = true;
        touchedObject = e.target;
    }

    protected virtual void DoInteractUntouch(object sender, ObjectInteractEventArgs e)
    {
        DebugLogger(e.controllerIndex, "NO LONGER TOUCHING", e.target);
        isTouching = false;
        touchedObject = null;
    }

    protected virtual void DoInteractGrab(object sender, ObjectInteractEventArgs e)
    {
        DebugLogger(e.controllerIndex, "GRABBING", e.target);
        isHolding = true;
        heldObject = e.target;
        controllerCollider.enabled = false;
    }

    protected virtual void DoInteractUngrab(object sender, ObjectInteractEventArgs e)
    {
        DebugLogger(e.controllerIndex, "NO LONGER GRABBING", e.target);
        isHolding = false;
        heldObject = null;
        controllerCollider.enabled = true;
    }

    protected virtual void DoInteractUse(object sender, ObjectInteractEventArgs e)
    {
        DebugLogger(e.controllerIndex, "USING", e.target);
    }

    protected virtual void DoInteractUnuse(object sender, ObjectInteractEventArgs e)
    {
        DebugLogger(e.controllerIndex, "NO LONGER USING", e.target);
    }
    #endregion

    #region Pointer
    //------------------------------------------------------------------------------------------------------------------
    // Pointer Listeners
    //------------------------------------------------------------------------------------------------------------------
    protected virtual void DoPointerOn(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "Pointer On");
    }

    protected virtual void DoPointerOff(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "Pointer Off");
    }
    #endregion
    #endregion

    #region Debug
    //------------------------------------------------------------------------------------------------------------------
    // Debugger functions
    //------------------------------------------------------------------------------------------------------------------
    protected void DebugLogger(uint index, string button, string action, float buttonPressure, Vector2 touchpadAxis)
    {
        if (DebugMode)
        {
            Debug.Log("Controller on index '" + index + "' " + button + " has been " + action + " with a pressure of " + buttonPressure + " / trackpad axis at: " + touchpadAxis);
        }
    }

    protected void DebugLogger(uint index, string action, Transform target, float distance, Vector3 tipPosition)
    {
        if (DebugMode)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            Debug.Log("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named " + targetName + " - the pointer tip position is/was: " + tipPosition);
        }
    }

    protected void DebugLogger(uint index, string action, GameObject target)
    {
        if (DebugMode)
        {
            Debug.Log("Controller on index '" + index + "' is " + action + " an object named " + target.name);
        }
    }

    protected void DebugLogger(uint index, string action)
    {
        if (DebugMode)
        {
            Debug.Log("Controller on index '" + index + "' is " + action);
        }
    }
    #endregion
}
