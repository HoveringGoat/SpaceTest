using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class NodeList : MonoBehaviour
{

    // Use this for initialization
    public GameObject nodePrefab;
    public int numofnodes = 0;
    private List<GameObject> Nodes = new List<GameObject>();
    //private Transform[] Nodes;
    public Vector3 CenterOfMass = Vector3.zero;
    public float Mass = 0;
    void Start()
    {
        float x, y, z;
        GameObject a;

        a = Instantiate(nodePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        a.name = "Node" + Nodes.Count;
        a.transform.parent = gameObject.transform;
        Mass++;
        Nodes.Add(a);
        
        for (int i = 1; i < numofnodes; i++)
        {
            x = Random.Range(-9.0f, 9.0f);
            y = Random.Range(-9.0f, 9.0f);
            z = Random.Range(-9.0f, 9.0f);
            a = Instantiate(nodePrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            a.transform.parent = gameObject.transform;
            a.name = "Node" + Nodes.Count;
            Mass++;
            //CenterOfMass += (a.transform.position - CenterOfMass) * (1 / (Mass));
            Nodes.Add(a);

        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            int num = 5;
            float x, y, z;
            GameObject a;
            for (int i = 0; i < num; i++)
            {
                Random.seed = i;
                x = Random.Range(-5.0f, 5.0f);
                y = Random.Range(-5.0f, 5.0f);
                z = Random.Range(-5.0f, 5.0f);
                a = Instantiate(nodePrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                a.transform.parent = gameObject.transform;
                Mass++;
                a.name = "Node" + Nodes.Count;
                Nodes.Add(a);

            }
        }
        //calcmesh
        //uhhhhhh needs to like. go through and find the 'surface' of the nodes 
        //then, like, map those nodes to vertices. 
        /*
        if(redrawMesh)
        {
            redrawMesh = false;
            meshVert = new List<Vector3>();

            //int a = (int)Random.Range(0, Nodes.Count - 1);
            //Node child = Nodes[a].GetComponent<Node>() as Node;
            Node child = Nodes[Nodes.Count-1].GetComponent<Node>() as Node;
            FindVertNode(child);
        }

        // update mesh
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        GetComponent<MeshCollider>().sharedMesh = null;


        mesh.name = "Procedural Mesh";
        mesh.vertices = meshVert.ToArray();
        mesh.triangles = meshTri.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;*/

    }
}

    /*
    private void FindVertNode(Node curr)
    {
        List<Vector3> Vertices = new List<Vector3>();
        curr.Meshed = true;
        Vertices.Add(curr.transform.position);
        List<Node> Local = new List<Node>();
        List<int> VertRef = new List<int>();
        Node a;
        Vector3 normal = Vector3.zero;

        // remove self from thigny. add self to current vert list
        //do cast sphere in raduius for all those imediatley in contact , draw normal vector away from all dem.
        Collider[] colliders = Physics.OverlapSphere(Vertices[0], curr.radius*1.1f);
        foreach(Collider hit in colliders)
        {
            a = hit.GetComponent<Node>();
            if(!a.Meshed)
            {
                if (getNodes(a) < 8)//how many nodes does a non surface node connect to?????
                {
                    //not added yet.
                    Local.Add(a);
                    Vertices.Add(a.transform.position);
                    VertRef.Add(-1);
                }
            }
            //update normal vector 
            normal -= a.transform.position;
        }
        // create list of verts. update with master list. check for repeats, update vert list to get from master
        // getting older verts in master list if repeat

        for(int i = 0; i<Vertices.Count-1;i++)
        {
            for(int j = 0; j<meshVert.Count-1; j++ )
            {
                if(Vertices[i]==meshVert[j]) // obviously not working correctly. dups in master list
                {
                    //ref already exist update thingy to do stuff
                    VertRef[i] = j;
                    Debug.Log("NEVER HITS????");
                }

            }
            if(VertRef[i] == -1)
            {
                //if not found in master list
                meshVert.Add(Vertices[i]);
                VertRef[i] = meshVert.Count-1;
            }
        }

        // use normal vector to start drawing tris (clockwise around the vec)
        // merge tri list with master tri
        for(int i=1;i<VertRef.Count-2;i++)
        {
            meshTri.Add(VertRef[0]);
            meshTri.Add(VertRef[i]);
            meshTri.Add(VertRef[i+1]);
            //obviously this isn't picking them in the correct order but i'm just curious to see if i can draw something.
        }

        // pick nect vert to continue
        foreach(Node node in Local)
        {
            FindVertNode(node);
        }


    }
    private int getNodes(Node a)
    {
        Collider[] colliders = Physics.OverlapSphere(a.transform.position, a.radius * 1.1f);
        return colliders.Length;
    }
}
    */