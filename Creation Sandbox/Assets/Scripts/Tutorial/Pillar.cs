using UnityEngine;
using System.Collections;


public delegate void CollisionHandler(string collisionType);

public class Pillar : MonoBehaviour {

    public event CollisionHandler OnCollision;
    public Animation hide;
    public GameObject arrow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        GetComponent<Animator>().enabled = true;
        if (arrow != null)
        {
            arrow.SetActive(true);
        }
    }

    public void Hide()
    {
        GetComponent<Animator>().SetBool("up", false);
        if (arrow != null)
        {
            arrow.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (OnCollision != null)
        {

            if (collision.collider.gameObject.GetComponent<TutorialBall>() != null)
            {
                OnCollision("ball");
            }
            else if (collision.collider.gameObject.GetComponent<TutorialCube>() != null)
            {
                OnCollision("cube");
            }
            else if (collision.collider.gameObject.GetComponent<TutorialGun>() != null)
            {
                OnCollision("hand gun");
            }
        }
    }
}
