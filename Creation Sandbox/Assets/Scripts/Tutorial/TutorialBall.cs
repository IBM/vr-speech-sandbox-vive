using UnityEngine;
using System.Collections;
using VRTK;

public class TutorialBall : CreatableObject
{

    [Header("Tutorial", order = 0)]
    public TutorialManager tutorialManager;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);

        tutorialManager.CorrectObjectGrabbed();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        //base.OnCollisionEnter(collision);

        if (collision.collider.gameObject.tag == "dropPillar")
        {
            Destroy(gameObject, 1);
        } else if (collision.collider.gameObject.tag == "terrain")
        {
            tutorialManager.BallFloorCollision(gameObject);
        }

    }
}
