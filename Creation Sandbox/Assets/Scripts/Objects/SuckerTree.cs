using UnityEngine;
using System.Collections;

public class SuckerTree : CreatableObject {

    [Header("SuckerTree", order = 0)]
    public GameObject trunk;
    public GameObject foliage;

    public float defaultY = 6.0f;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        if(this.matKey == "")
        {
            float xScale = Random.Range(minScale, maxScale);
            float yOffset = gameObject.transform.position.y - defaultY;

            gameObject.transform.localScale = new Vector3(xScale, xScale, xScale);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, (defaultY * xScale) + yOffset, gameObject.transform.position.z);
            int treeColor = Random.Range(0, _mValues.Count);
            this.matKey = _mKeys[treeColor];
            foliage.GetComponent<Renderer>().material = _mValues[treeColor];
        }
        

    }

    public override void ApplyMaterial(string matKey)
    {
        if (mats == null)
        {
            InitializeMaterials();
        }

        if (mats.ContainsKey(matKey))
        {
            Material mat = mats[matKey];
            foliage.GetComponent<Renderer>().material = mat;
        } else
        {
            Debug.Log("Invlid Material for Object");
        }
    }
}
