using UnityEngine;
using System.Collections;

public class FadingText : MonoBehaviour {

    protected float duration = 100;
    protected float timer = 0;
    protected float deltaTime = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (gameObject.activeSelf) {
            timer += deltaTime;
            if (timer > duration)
            {
                gameObject.SetActive(false);
            }
        }
	}

    public void Show(float duration, float deltaTime)
    {
        gameObject.SetActive(true);
        this.duration = duration;
        this.deltaTime = deltaTime;
    }
}
