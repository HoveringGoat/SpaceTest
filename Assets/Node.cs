using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// get list of SURFACE nodes at radius range .8*rad - 1.2 rad????  for making vertices (include self???) 
// when getting bonded bros if new check if hes frozen (it should be new ref for him too)
// if he is unfeeze him so he can get updated ref too
// should make mesh? uh spmetimes

// how would we determine if a mesh overlaps its neighbor. since the mesh is made up of the nearby vectors
// each node tries to build a mesh. if neighbor has built one then dont use them as a vertice??? but holes!
// each node cuts the length of the vertices by half. so instead of (0,0) - (0,4) use the point (0,2)
// each node now can complete their thingy by selves.
// get normal of all local vectors to see which way is 'out'
// apply triangles in correct way so mesh is visable on that side



[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Node : MonoBehaviour
{
    public float radius = 5;
    public float dragVel = 1.0f;
    public float frozeSpeed = .01f;
    public int isFrozen = 0;
    public int MaxSurfaceConnections = 10;
    public bool DebugOn = false;
    public bool isSurface = false;
    public bool isPreSurface = false;
    public int SurfaceConnections = 0;
    public bool UpdateMesh = false;
    public List<Vector3> Local = new List<Vector3>();
    public GameObject NodeMeshPrefab;
    private GameObject Child;
    void FixedUpdate()
    {
        if (isFrozen < 100)
            DoCollisions();
        else if (GetComponent<Rigidbody>().velocity.magnitude > 0)
            isFrozen = 98;

        //make mesh
        if (UpdateMesh)
            GetLocal();
        if (isSurface|isPreSurface)
            DrawMesh();


    }
    void Awake()
    {

        float dist;
        Vector3 source = transform.position;
        Vector3 dest;
        Vector3 vec;
        Rigidbody body = GetComponent<Rigidbody>();
        if (body.transform.position == Vector3.zero)
        {
            isFrozen = 100;
            isSurface = true;
        }
        else
        {

            dest = Vector3.zero;//center of mass eh
            vec = dest - source;
            vec.Normalize();
            vec *= .5f;
            body.velocity = vec;
        }
        // create child to hold mesh
        Child = Instantiate(NodeMeshPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        Child.name = "Mesh";
        Child.transform.position = transform.position;
        Child.transform.parent = gameObject.transform;

    }

    private void GetLocal()
    {
        Vector3 source = transform.position;
        Vector3 dest;
        Vector3 vec;
        Rigidbody body = GetComponent<Rigidbody>();
        Local = new List<Vector3>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f * radius);
        GetFlags(colliders.Length);
        SurfaceConnections = 0;
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                dest = hit.transform.position;
                vec = source - dest;
                if (vec.magnitude > 0)
                {
                    if (hit.GetComponent<Node>().isSurface)
                    {
                        Local.Add(-vec);
                        SurfaceConnections++;
                    }
                    else if(hit.GetComponent<Node>().isPreSurface && isPreSurface)
                    {
                        Local.Add(-vec);
                    }
                }
            }
        }
    }
    private void DoCollisions()
    {
        //get other nodes within range (2units)
        float thrust, dist;
        Vector3 temp;
        Vector3 source = transform.position;
        Vector3 dest;
        Vector3 vec;
        Rigidbody body = GetComponent<Rigidbody>();
        UpdateMesh = false;

        //check frozen
        if (body.velocity.magnitude < frozeSpeed)
        {
            isFrozen++;
        }
        else
        {
            isFrozen = 0;
        }

        if (isFrozen >= 100)
        {
            body.velocity = Vector3.zero;
        }
        else
        {
            Local = new List<Vector3>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f * radius);
            GetFlags(colliders.Length);
            SurfaceConnections = 0;
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    dest = hit.transform.position;
                    vec = source - dest;
                    dist = vec.magnitude;
                    if (dist > 0)
                    {
                        if (dist > radius)
                        {
                            thrust = (float)(1 - System.Math.Pow(((dist - 1.25 * radius) * (4 / radius)), 2)); //attract
                            thrust *= 2.0f;
                            temp = (body.velocity - rb.velocity);
                            temp *= -1f;
                            temp *= (float)(dragVel / (temp.magnitude * 10));
                            body.AddForce(temp);

                            //body.velocity = (body.velocity+temp)*.5f; //normalize vel
                            //rb.velocity -= (temp * (float)(dragVel / 100.0)); //normalize vel
                        }
                        else
                        {
                            thrust = (float)((1 / (dist * dist)) - (1 / (radius * radius))) * -100f; //repel
                            //thrust = -100;
                        }
                        body.AddForce(vec * -thrust);

                        //add to local list if surface
                        if (hit.GetComponent<Node>().isSurface)
                        {
                            Local.Add(-vec);
                            if (hit.GetComponent<Node>().isFrozen == 100)
                                hit.GetComponent<Node>().UpdateMesh = true;
                            //hit.GetComponent<Node>().Local.Add(vec); // adds self to their thingy if frozen they'll update, if not they will erase
                                SurfaceConnections++;
                        }
                        else if (hit.GetComponent<Node>().isPreSurface && isPreSurface)
                        {
                            Local.Add(-vec);
                        }
                    }
                }
            }
        }
    }
    private void DrawMesh()
    {
        if (isPreSurface)
        {
            Child.GetComponent<NodeMesh>().Vertices = new List<Vector3>();
            Child.GetComponent<NodeMesh>().mesh = null;

            if (Local.Count >= 3)
            {
                isSurface = true;
            }
        }
        else
        {
            Child.GetComponent<NodeMesh>().Vertices = Local;
        }
        Child.GetComponent<NodeMesh>().DoUpdate = true;
        Child.GetComponent<NodeMesh>().transform.position = GetComponent<Rigidbody>().position;
    }
    private void GetFlags(int length)
    {
        if (length > MaxSurfaceConnections)
        {
            isSurface = false;
            isPreSurface = false;
        }
        else if (SurfaceConnections >= 3)
        {
            isSurface = true;
            isPreSurface = false;
        }
        else if (length > 0)
        {
            isSurface = false;
            isPreSurface = true;
        }
        else
        {
            isSurface = false;
            isPreSurface = false;
        }
    }
}
//grav
//Collider[] colliders = Physics.OverlapSphere(transform.position, 100);
//foreach (Collider hit in colliders)
//{
//    Rigidbody rb = hit.GetComponent<Rigidbody>();
//    dest = hit.transform.position;
//    vec = source - dest;
//    dist = vec.magnitude;
//    thrust = (float)(1 / (dist + 2.0));
//    thrust *= 5f;
//    if (rb != null & dist > 0)
//    {
//        body.AddForce(vec * -thrust);
//    }
//    //Debug.Log("impulse " + rb.impulse.magnitude);
//}