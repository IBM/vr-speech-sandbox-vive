using UnityEngine;
using System.Collections;

public delegate void TargetShotHandler();

public class TutorialTarget : MonoBehaviour {

    public Material hitMaterial;

    public event TargetShotHandler OnShot;

    private bool hit = false;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        if (!hit && collision.collider.gameObject.tag != "terrain")
        {
            GetComponent<Renderer>().material = hitMaterial;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            OnShot();
            hit = true;
        }
    }
}
