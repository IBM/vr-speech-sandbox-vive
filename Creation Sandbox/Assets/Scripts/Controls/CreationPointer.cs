using UnityEngine;
using System.Collections;
using VRTK;

public struct CreationPointerEventArgs
{
    public float distance;
    public GameObject targetObject;
    public Transform target;
    public Vector3 position;
    public uint controllerIndex;
}

public delegate void CreationPointerEventHandler(object sender, CreationPointerEventArgs e);

public class CreationPointer : VRTK_WorldPointer {

    [Header("Creation Pointer", order = 0)]
    public GameObject pointer;
    public GameObject pointerTip;
    public float maxLength;

    public event CreationPointerEventHandler CreationPointerSet;
    public event CreationPointerEventHandler CreationPointerOn;
    public event CreationPointerEventHandler CreationPointerOff;

    protected LayerMask layersToIgnore = Physics.IgnoreRaycastLayer;

    protected bool isOn = true;

    // Use this for initialization
    void Start () {
        pointer.layer = LayerMask.NameToLayer("Ignore Raycast");
        pointerTip.layer = LayerMask.NameToLayer("Ignore Raycast");
        TurnOn(true);
    }

    override protected void OnEnable()
    {
        TurnOn(true);
    }

    override protected void OnDisable()
    {
        TurnOn(false);
    }

    protected void TurnOn(bool on)
    {

        pointer.SetActive(on);
        pointerTip.SetActive(on);
        isOn = on;

        if (on)
        {
            if (CreationPointerOn != null)
            {
                CreationPointerOn(this, new CreationPointerEventArgs());
            }
        } else
        {
            if (CreationPointerOff != null)
            {
                CreationPointerOff(this, new CreationPointerEventArgs());
            }
        }
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        if (!isOn)
        {
            return;
        }

        RaycastHit hit;
        Ray gazeRay = new Ray(transform.position, transform.forward);
        bool hasRayHit = Physics.Raycast(gazeRay, out hit, maxLength, ~layersToIgnore);

        CreationPointerEventArgs eventArgs = new CreationPointerEventArgs();

        if (hasRayHit && hit.distance < maxLength)
        {
            SetPointerTransform(hit.distance);

            eventArgs.targetObject = hit.transform.gameObject;
            eventArgs.target = hit.transform;
            eventArgs.distance = hit.distance;

            pointerContactDistance = hit.distance;
            pointerContactTarget = hit.transform;
            destinationPosition = pointerTip.transform.position;

            base.PointerIn();

        } else
        {
            SetPointerTransform(maxLength);
            eventArgs.distance = maxLength;

            base.PointerOut();

            pointerContactDistance = 0f;
            pointerContactTarget = null;
            destinationPosition = Vector3.zero;
        }

        eventArgs.position = pointerTip.transform.position;
        eventArgs.controllerIndex = (uint)controller.GetComponent<SteamVR_TrackedObject>().index;

        CreationPointerSet(this, eventArgs);
        
    }

    private void SetPointerTransform(float setLength)
    {
        var beamPosition = setLength / (2 + 0.00001f);

        pointer.transform.localScale = new Vector3(0.002f, 0.002f, setLength);
        pointer.transform.localPosition = new Vector3(0f, 0.0025f, beamPosition + 0.0231f);

        pointerTip.transform.localPosition = new Vector3(0f, 0f, setLength - (pointerTip.transform.localScale.z / 2));
        //base.SetPlayAreaCursorTransform(pointerTip.transform.position);
    }
}
