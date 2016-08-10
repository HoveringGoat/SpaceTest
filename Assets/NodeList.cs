using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeList : MonoBehaviour {

    // Use this for initialization
    public GameObject nodePrefab;
    public int numofnodes = 0;
    private List<Object> Nodes = new List<Object>();
	void Start ()
    {
        float x, y, z;
	    for (int i=0; i<numofnodes;i++)
        {
            x = Random.Range(-10.0f, 10.0f);
            y = Random.Range(-10.0f, 10.0f);
            z = Random.Range(-10.0f, 10.0f);
            Nodes.Add(Instantiate(nodePrefab, new Vector3(x, y, z), Quaternion.identity));

        }
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float x, y, z;
            x = Random.Range(-10.0f, 10.0f);
            y = Random.Range(-10.0f, 10.0f);
            z = Random.Range(-10.0f, 10.0f);
            Nodes.Add(Instantiate(nodePrefab, new Vector3(x, y, z), Quaternion.identity));
        }
    }
}
