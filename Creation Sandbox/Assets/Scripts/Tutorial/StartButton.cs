using UnityEngine;
using System.Collections;
using VRTK;

public delegate void ButtonCollisionHandler(string collisionTag);

public class StartButton : VRTK_Button {

    public event ButtonCollisionHandler OnCollision;

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision.gameObject.tag);
    }
}
