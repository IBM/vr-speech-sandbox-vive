using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : GameManager {

    [Header("Menu Manager", order = 0)]
    //public StartButton newButton;
    //public StartButton continueButton;
    //public StartButton tutorialButton;

    public ControllerSwitcher rightSwitcher;
    public ControllerSwitcher leftSwitcher;

    #region Init and Lifecycle
    // Use this for initialization
    protected override void Start()
    {
        DataManager.dManager.Load();
        if (!DataManager.dManager.tutorialComplete)
        {
            LoadTutorialWithoutSaving();
        }
        //newButton.events.OnPush.AddListener(NewPressed);
        //continueButton.events.OnPush.AddListener(ContinuePressed);
        //tutorialButton.events.OnPush.AddListener(TutorialPressed);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (rightSwitcher.isEnabled)
        {
            rightSwitcher.DisableController();
        }

        if (leftSwitcher.isEnabled)
        {
            leftSwitcher.DisableController();
        }
    }
    #endregion

    #region Button Actions

    public void NewPressed()
    {
        DataManager.dManager.ClearSandbox();
        LoadSandbox();
    }

    public void ContinuePressed()
    {
        LoadSandbox();
    }

    public void TutorialPressed()
    {
        LoadTutorial();
    }

    #endregion
}
