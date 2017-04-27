
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class TeleportController : ControllerListener
{
    [Header("Teleport Controller", order = 0)]
    public VRTK_BezierPointer bezierPointer;
    public GameObject teleporter;

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
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        Debug.Log("Teleport Enable");
        SetControls();
        bezierPointer.enabled = true;
        teleporter.SetActive(true);
        touchpadDecal.OnTeleporter();
    }

    override protected void OnDisable()
    {
        base.OnDisable();
        bezierPointer.enabled = false;
        teleporter.SetActive(false);
    }

    void SetControls()
    {
        if (controllerEvents != null)
        {
            controllerEvents.pointerToggleButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;
            controllerEvents.pointerSetButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;
            controllerEvents.grabToggleButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
            controllerEvents.useToggleButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
            controllerEvents.uiClickButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;
            controllerEvents.menuToggleButton = VRTK_ControllerEvents.ButtonAlias.Application_Menu;
        }
    }
    #endregion

    #region Touchpad
    //------------------------------------------------------------------------------------------------------------------
    // Touchpad Listeners
    //------------------------------------------------------------------------------------------------------------------
    //override protected void DoTouchpadClicked(object sender, ControllerInteractionEventArgs e)
    //{
    //    base.DoTouchpadClicked(sender, e);
    //}
    #endregion

}
