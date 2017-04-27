using UnityEngine;
using System.Collections;

public class TouchpadDecal : MonoBehaviour {

    public Material objectHighlighted;
    public Material teleporterHighlighted;
    public Material objectControlsPlay;
    public Material objectControlsPause;

    public void OnObject()
    {
        gameObject.SetActive(true);
        GetComponent<Renderer>().material = objectHighlighted;
    }

    public void OnTeleporter()
    {
        gameObject.SetActive(true);
        GetComponent<Renderer>().material = teleporterHighlighted;
    }

    public void OnObjectControlsPlay()
    {
        gameObject.SetActive(true);
        GetComponent<Renderer>().material = objectControlsPlay;
    }

    public void OnObjectControlsPause()
    {
        gameObject.SetActive(true);
        GetComponent<Renderer>().material = objectControlsPause;
    }

    public void OnHidden()
    {
        gameObject.SetActive(false);
    }
}
