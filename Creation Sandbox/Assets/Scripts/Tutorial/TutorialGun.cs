using UnityEngine;
using System.Collections;

public class TutorialGun : FireableObject {

    public TutorialManager tutorialManager;

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
        if (!precisionSnap)
        {
            tutorialManager.GunGrabbed();
        }
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        if (!precisionSnap)
        {
            tutorialManager.GunDropped();
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.collider.gameObject.tag == "terrain")
        {
            tutorialManager.GunFloorCollision(gameObject);
        }

    }
}
