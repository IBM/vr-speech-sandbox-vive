using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class CreatableObject : VRTK_InteractableObject
{
    [Header("Creatable Object", order = 0)]

    public string id;
    public string matKey;

    public bool isCustomizable = false;
    public bool isLarge = false;
    public bool isFreezable = true;
    public bool isFood = false;
    public float minScale;
    public float maxScale;
    public float minDistance;
    public float maxDistance;
    public AudioClip collisionClip;
    public AudioClip consumeClip;

    [Header("Materials Dictionary", order = 1)]
    public List<string> _mKeys = new List<string>();
    public List<Material> _mValues = new List<Material>();
    protected Dictionary<string, Material> mats;

    private string[] silentObjects = { "terrain", "leftController", "rightController" };
    protected GrabAttachType grabAttachDefault;
    protected bool defaultPrecision;
    public bool isFrozen = false;

    protected Vector3 initialScale;

    #region InitAndLifecycle
    //------------------------------------------------------------------------------------------------------------------
    // Initialization and Lifecycle
    //------------------------------------------------------------------------------------------------------------------
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        defaultPrecision = precisionSnap;
        grabAttachDefault = grabAttachMechanic;
        isSwappable = false;
        initialScale = transform.localScale;
    }

    protected override void Update()
    {
        base.Update();
        // Destroy if falls through floor.
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void InitializeMaterials()
    {
        mats = new Dictionary<string, Material>();

        for (var i = 0; i != Math.Min(_mKeys.Count, _mValues.Count); i++)
        {
            mats.Add(_mKeys[i], _mValues[i]);
        }
    }
    #endregion

    #region Using Object
    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
    }
    #endregion

    #region Grabbing Object
    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        if (!precisionSnap)
        {
            FreezeRigidBody(false);
            isFrozen = false;
        }
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        precisionSnap = defaultPrecision;
    }
    #endregion

    #region Modifying Object
    public virtual void ApplyMaterial(string matKey)
    { 
        if (mats == null)
        {
            InitializeMaterials();
        }

        if (mats.ContainsKey(matKey))
        {
            Material mat = mats[matKey];
            GetComponent<Renderer>().material = mat;
        } else
        {
            Debug.Log("Invlid Material for Object");
        }
    }

    public virtual void ApplyScale(string scale)
    {
        float currentMass = GetComponent<Rigidbody>().mass;
        if (scale == "big")
        {
            if (maxScale > 0)
            {
                transform.localScale = new Vector3(maxScale, maxScale, maxScale);
                GetComponent<Rigidbody>().mass = currentMass * maxScale * 10;
            } else
            {
                transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);
                GetComponent<Rigidbody>().mass = currentMass * 20;
            }
        } else if (scale == "small")
        {
            if (minScale > 0)
            {
                transform.localScale = new Vector3(minScale, minScale, minScale);
                GetComponent<Rigidbody>().mass = currentMass / (minScale * 10);
            } else
            {
                transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2);
                GetComponent<Rigidbody>().mass = currentMass / 20;
            }
        }
    }

    public Vector3 GetInitialScale()
    {
        return initialScale;
    }

    public void ToggleFreeze()
    {
        if (isFreezable)
        {
            FreezeRigidBody(!isFrozen);
            isFrozen = !isFrozen;
        }
    }

    public void FreezeRigidBody(bool freeze)
    {
        if (freeze)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            grabAttachMechanic = GrabAttachType.Child_Of_Controller;
            //GetComponent<Rigidbody>().isKinematic = true;
        } else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            grabAttachMechanic = grabAttachDefault;
            //GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    #endregion

    #region Collisions
    public virtual void OnCollisionEnter(Collision collision)
    {

        // need to check if object is being held. I feel like there's a better way...
        if (transform.GetComponent<FixedJoint>())
        {
            Debug.Log(transform.GetComponent<FixedJoint>().connectedBody);
            transform.GetComponent<FixedJoint>().connectedBody.GetComponent<ControllerSwitcher>().VibrateController(800, 0.08f); ;
        }

        if (collisionClip != null && System.Array.IndexOf(silentObjects, collision.collider.tag) == -1)
        {
            //GetComponent<AudioSource>().clip = collisionClip;
            //GetComponent<AudioSource>().Play();
        }
    }
    #endregion

    #region Triggers
    public virtual void OnTriggerEnter(Collider collider)
    {
        if ((collider.CompareTag("MainCamera") || collider.CompareTag("Animal")) && isFood)
        {
            if (consumeClip != null)
            {
                collider.gameObject.GetComponent<AudioSource>().clip = consumeClip;
            }
            collider.gameObject.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }
    #endregion
}
