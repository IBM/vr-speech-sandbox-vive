using UnityEngine;
using System.Collections;

public delegate void MenuStateChanged(bool isOpen);

public class Menu : MonoBehaviour {

    public event MenuStateChanged MenuEvent;
    public Light mainLight;
    public GameObject playerHead;
    public GameObject parentObject;

    private float lightIntensity;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        MenuEvent(true);
        lightIntensity = mainLight.intensity;
        mainLight.intensity = 0.5f;
        if (playerHead != null && parentObject != null)
        {
            parentObject.transform.rotation = Quaternion.Euler(0f, playerHead.transform.rotation.eulerAngles.y, 0f);
        }
    }

    void OnDisable()
    {
        MenuEvent(false);
        if (mainLight != null)
        {
            mainLight.intensity = lightIntensity;
        }
    }

    public bool MenuEventNull()
    {
        return MenuEvent == null;
    }
}
