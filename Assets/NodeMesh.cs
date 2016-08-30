using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeMesh : MonoBehaviour {
    public Mesh mesh;
    public bool DoUpdate = false;
    public bool DebugOn = false;
    public List<Vector3> Vertices = new List<Vector3>();
    public List<Vector3> Verts = new List<Vector3>();
    public List<Vector3> TransVerts = new List<Vector3>();
    public List<int> Triangles = new List<int>();
    public float[] angles = new float[10];

	void FixedUpdate () 
    {
        // update mesh
        if (DoUpdate)
        {
            UpdateMesh();
            DoUpdate = false;
        }
	}

    private void UpdateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();

        OrderVertices();
        GetVerts();
        GetTriangles();

        mesh.name = "Procedural Mesh";
        mesh.vertices = Verts.ToArray();
        mesh.triangles = Triangles.ToArray();
        mesh.RecalculateNormals();
    }
    private void OrderVertices()
    {
        Vector3 TransformedVert;
        Vector3 normal = Vector3.zero;
        Vector3 axis;
        Quaternion rotation;
        //Quaternion a = Quaternion.identity;
        float angle, right;
        TransVerts = new List<Vector3>();
        foreach (Vector3 i in Vertices)
        {
            normal -= i;
            TransVerts.Add(i);
        }
        normal.Normalize();
        axis = Vector3.Cross(normal, Vector3.up);  //get rot angly thingy
        angle = Vector3.Angle(normal, Vector3.up); //angle between normal and "up"
        rotation = Quaternion.AngleAxis(angle, axis); 

        if (DebugOn)
        {
            Debug.Log("Normal: " + " (" + normal.x + ", " + normal.y + ", " + normal.z + ")");
            Debug.Log("Axis: " + " (" + axis.x + ", " + axis.y + ", " + axis.z + ")");
            Debug.Log("Angle: " + angle);
            TransformedVert = rotation * normal;
            Debug.Log("Normalized: " + " (" + TransformedVert.x + ", " + TransformedVert.y + ", " + TransformedVert.z + ")");
        }

        // transform all the local vectors store modyifyed vectors in temp
        // go through and find the angle (should be looking down on it (ignore y))

        angles = new float[TransVerts.Count];
        for (int i = 0; i < TransVerts.Count; i++)
        {
            TransformedVert = rotation * TransVerts[i];
            TransformedVert.y = 0;
            if(DebugOn)
                Debug.Log("Vector"+i+" (" + TransformedVert.x + ", " + TransformedVert.y + ", " + TransformedVert.z + ")");
            right = Vector3.Angle(TransformedVert, Vector3.right);
            if(TransformedVert.z<0)
            {
                right = 360 - right;
            }
            angles[i] = right;
        }

        bubbleSort();
        // sort vectors by the angles 

    }
    private void bubbleSort()//prob chnge angles from bein public
    {
        float temp;
        Vector3 vec;
        for (int i = (TransVerts.Count - 1); i >= 0; i--)
        {
            for (int j = 1; j <= i; j++)
            {
                if (angles[j - 1] < angles[j])
                {
                    //if element to the left of curr is smaller swap
                    temp = angles[j - 1];
                    angles[j - 1] = angles[j];
                    angles[j] = temp;
                    // do for our vectors in Vertices too
                    vec = TransVerts[j - 1];
                    TransVerts[j - 1] = TransVerts[j];
                    TransVerts[j] = vec;
                }
            }
        }
    }
    private void GetVerts()
    {
        Verts = new List<Vector3>();
        //assumes vertices are ordered
        Verts.Add(Vector3.zero);
        for (int i = 0; i < TransVerts.Count; i++)
        {
            Verts.Add(TransVerts[i] * .5f);
            if (i + 2 < TransVerts.Count)
                Verts.Add((TransVerts[i] + TransVerts[i + 1]) * .5f);
            else
                Verts.Add((TransVerts[i] + TransVerts[1]) * .5f);
        }

    }

    private void GetTriangles()
    {
        //create triangles
        Triangles = new List<int>();
        for (int i = 1; i < Verts.Count; i++)
        {
            Triangles.Add(0);
            Triangles.Add(i);
            if (i + 2 < Verts.Count)
                Triangles.Add(i + 1);
            else
                Triangles.Add(1);
        }
    }
}
