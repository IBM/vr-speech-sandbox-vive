using UnityEngine;
using System.Collections;

public class Target : CreatableObject {

    public Material unhitMaterial;
    public Material hitMaterial;

    private bool hit = false;

    // Use this for initialization
    protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}


    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);

        if (hit != false)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);

        if (hit != false)
        {
            hit = false;
            GetComponent<Renderer>().material = unhitMaterial;
        }
    }

    override public void OnCollisionEnter(Collision collision) {
        base.OnCollisionEnter(collision);
        GetComponent<Renderer>().material = hitMaterial;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        hit = true;
    }
}
