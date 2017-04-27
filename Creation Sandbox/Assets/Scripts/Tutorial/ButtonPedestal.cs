using UnityEngine;
using System.Collections;

public class ButtonPedestal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Hide()
    {
        GetComponent<Animator>().enabled = true;
    }
}
