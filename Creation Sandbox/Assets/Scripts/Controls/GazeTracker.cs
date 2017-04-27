using UnityEngine;
using System.Collections;
using VRTK;

public struct GazeMarkerEventArgs
{
	public GameObject gazeTarget;
	public Transform gazeTransform;
	public Vector3 gazePoint;
	public float distance;

	public Vector3 position;
}

public delegate void GazeMarkerEventHandler(object sender, GazeMarkerEventArgs e);

public class GazeTracker : VRTK_DestinationMarker {

	public GameObject marker;

	public event GazeMarkerEventHandler GazeMarkerSet;

	public float maxLength = 10f;
	protected LayerMask layersToIgnore = Physics.IgnoreRaycastLayer;

	// Use this for initialization
	protected virtual void Start () {
		marker.layer = LayerMask.NameToLayer("Ignore Raycast");
	}

	// Update is called once per frame
	protected virtual void Update () {

		RaycastHit hit;
		Ray gazeRay = new Ray(transform.position, transform.forward);
		bool hasRayHit = Physics.Raycast(gazeRay, out hit, maxLength, ~layersToIgnore);

		GazeMarkerEventArgs eventArgs = new GazeMarkerEventArgs();

		if (hasRayHit && hit.distance < maxLength)
		{
			marker.transform.localPosition = new Vector3(0f, 0f, hit.distance - (marker.transform.localScale.z / 2));

			eventArgs.gazeTarget = hit.transform.gameObject;
			eventArgs.gazeTransform = hit.transform;
			eventArgs.distance = hit.distance;
		}
		else
		{
			marker.transform.localPosition = new Vector3(0f, 0f, hit.distance - (marker.transform.localScale.z / 2));
			eventArgs.distance = maxLength;
		}

		eventArgs.position = marker.transform.position;

		GazeMarkerSet(this, eventArgs);

	}

	public void Teleport()
	{
		var distance = Vector3.Distance(transform.position, marker.transform.position);
		OnDestinationMarkerSet(SetDestinationMarkerEvent(distance, marker.transform, marker.transform.position, 0));
	}
}