using UnityEngine;
using System.Collections;

public struct SwipeEventArgs
{
    public float velocity;
    public float magnitude;
    public float angle;
}

public delegate void SwipeEventHandler(object sender, SwipeEventArgs e);

public class SwipeListener : MonoBehaviour {

    protected SteamVR_TrackedObject trackedObj;

    private readonly Vector2 mXAxis = new Vector2(1, 0);
    private readonly Vector2 mYAxis = new Vector2(0, 1);
    private bool trackingSwipe = false;
    private bool checkSwipe = false;

    private const float mAngleRange = 30;
    private const float mMinSwipeDist = 0.2f;
    private const float mMinVelocity = 4.0f;

    private Vector2 mStartPosition;
    private Vector2 endPosition;

    private float mSwipeStartTime;

    public event SwipeEventHandler SwipedLeft;
    public event SwipeEventHandler SwipedRight;
    public event SwipeEventHandler SwipedUp;
    public event SwipeEventHandler SwipedDown;

    // Use this for initialization
    void Start () {

        if (GetComponent<SteamVR_TrackedObject>() == null)
        {
            Debug.LogError("SwipeListener needs to be on a GameObject with a SteamVR_TrackedObject script.");
            return;
        }

        trackedObj = GetComponent<SteamVR_TrackedObject>(); 
	}
	
	// Update is called once per frame
	void Update () {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        // Touch down, possible chance for a swipe
        if ((int)trackedObj.index != -1 && device.GetTouchDown(Valve.VR.EVRButtonId.k_EButton_Axis0))
        {
            trackingSwipe = true;
            // Record start time and position
            mStartPosition = new Vector2(device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x,
                device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y);
            mSwipeStartTime = Time.time;
        }
        // Touch up , possible chance for a swipe
        else if (device.GetTouchUp(Valve.VR.EVRButtonId.k_EButton_Axis0))
        {
            trackingSwipe = false;
            trackingSwipe = true;
            checkSwipe = true;
            //Debug.Log("Tracking Finish");
        }
        else if (trackingSwipe)
        {
            endPosition = new Vector2(device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x,
                                      device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y);

        }

        if (checkSwipe)
        {
            checkSwipe = false;

            float deltaTime = Time.time - mSwipeStartTime;
            Vector2 swipeVector = endPosition - mStartPosition;

            float velocity = swipeVector.magnitude / deltaTime;

            if (velocity > mMinVelocity &&
                swipeVector.magnitude > mMinSwipeDist)
            {
                swipeVector.Normalize();

                float angleOfSwipe = Vector2.Dot(swipeVector, mXAxis);
                angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;

                SwipeEventArgs eventArgs = new SwipeEventArgs();
                eventArgs.velocity = velocity;
                eventArgs.magnitude = swipeVector.magnitude;
                eventArgs.angle = angleOfSwipe;

                if (angleOfSwipe < mAngleRange)
                {
                    SwipedRight(this, new SwipeEventArgs());
                }
                else if ((180.0f - angleOfSwipe) < mAngleRange)
                {
                    SwipedLeft(this, new SwipeEventArgs());
                }
                else {
                    angleOfSwipe = Vector2.Dot(swipeVector, mYAxis);
                    angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;
                    if (angleOfSwipe < mAngleRange)
                    {
                        //SwipedUp(this, new SwipeEventArgs());
                    }
                    else if ((180.0f - angleOfSwipe) < mAngleRange)
                    {
                        //SwipedDown(this, new SwipeEventArgs());
                    }
                }
            }
        }

    }

}
