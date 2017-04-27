using UnityEngine;
using System.Collections;

public delegate void TargetsDestroyedHandler();

public class TargetGroup : MonoBehaviour {

    public TutorialTarget[] targets;

    public event TargetsDestroyedHandler OnTargetsDestroyed;

    private int targetsShot = 0;

    // Use this for initialization
    void Start () {
        foreach (TutorialTarget target in targets)
        {
            target.OnShot += new TargetShotHandler(OnTargetShot);
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        GetComponent<Animator>().enabled = true;
    }

    private void OnTargetShot()
    {
        if (++targetsShot == 3)
        {
            OnTargetsDestroyed();
        }

    }
}
