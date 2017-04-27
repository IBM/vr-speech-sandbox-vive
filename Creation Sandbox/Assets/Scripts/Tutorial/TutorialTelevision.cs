using UnityEngine;
using System.Collections;

public class TutorialTelevision : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        animator.enabled = true;
    }

    public void ChangeScreen(Material newScreen)
    {
        //Material[] tempMaterials = GetComponent<Renderer>().materials;
        //tempMaterials[1] = newScreen;
        //GetComponent<Renderer>().materials = tempMaterials;
        GetComponent<Renderer>().material = newScreen;
    }
}
