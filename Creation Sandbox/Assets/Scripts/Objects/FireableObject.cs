using UnityEngine;
using System.Collections;
using VRTK;

public class FireableObject : CreatableObject {

    [Header("Fireable Object", order = 0)]
    public AudioClip firingSound;
    public float projectileSpeed;
    public float projectileLife;

    public bool isAutomatic;
    public float cooldown;
    private float coolDownTimer = 0.0f;

    public ushort rumbleStrength;
    public float rumbleLength;

    protected GameObject projectile;

    protected override void Start()
    {
        base.Start();
        projectile = this.transform.Find("Projectile").gameObject;
        projectile.SetActive(false);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(IsUsing())
        {
            if(isAutomatic && !precisionSnap && coolDownTimer > cooldown)
            {
                Fire();
                coolDownTimer = 0.0f;
            }
        }
        coolDownTimer += Time.fixedDeltaTime;
    }

    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        if (!precisionSnap) {
            Fire();
        }
    }

    public virtual void Fire()
    {
        GetComponent<AudioSource>().clip = firingSound;
        GetComponent<AudioSource>().Play();

        GameObject projectileClone = Instantiate(projectile, projectile.transform.position, projectile.transform.rotation) as GameObject;
        projectileClone.SetActive(true);
        Rigidbody rb = projectileClone.GetComponent<Rigidbody>();
        rb.AddForce(projectile.transform.forward * projectileSpeed);
        Destroy(projectileClone, projectileLife);

        //if (transform.parent != null && transform.parent.GetComponent<ControllerSwitcher>() != null)
        //{
        //    transform.parent.GetComponent<ControllerSwitcher>().VibrateController(rumbleStrength, rumbleLength);
        //}
    }
}
