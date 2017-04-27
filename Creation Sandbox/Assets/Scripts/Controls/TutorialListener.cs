using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using VRTK;

public class TutorialListener : ControllerListener {

    [Header("Tutorial Listener", order = 0)]
    public TutorialManager tutorialManager;

    // Use this for initialization
    protected override void Start () {
        EnableMenuButton();
        if (MenuCanvas != null)
        {
            MenuCanvas.MenuEvent += new global::MenuStateChanged(MenuStateChanged);
        }
    }

    protected override void OnEnable()
    {
        //EnableMenuButton();
        //if (MenuCanvas != null)
        //{
        //    MenuCanvas.MenuEvent += new global::MenuStateChanged(MenuStateChanged);
        //}
    }

    protected override void OnDisable()
    {
        // Remove controller event listeners
        //DisableMenuButton();
        //if (MenuCanvas != null)
        //{
        //    MenuCanvas.MenuEvent -= MenuStateChanged;
        //}
    }

    #region Menu
    //------------------------------------------------------------------------------------------------------------------
    // Menu Button Listeners
    //------------------------------------------------------------------------------------------------------------------
    protected override void MenuStateChanged(bool isOpen)
    {
        //Debug.Log("Menu State Changed: " + isOpen);
        if (isOpen)
        {
            controllerSwitcher.DisableController();
            //EnableMenuButton();
            //MenuCanvas.GetComponent<Menu>().MenuEvent += new global::MenuStateChanged(MenuStateChanged);
            if (sentMenuSignal)
            {
                uiPointer.enabled = true;
                GetComponent<CreationPointer>().enabled = true;
            }
        }
        else
        {
            uiPointer.enabled = false;
            GetComponent<CreationPointer>().enabled = false;
            //DisableMenuButton();
            //MenuCanvas.GetComponent<Menu>().MenuEvent -= MenuStateChanged;
            tutorialManager.SetControllersForCurrentStep();
        }
        sentMenuSignal = false;
    }
    #endregion
}
