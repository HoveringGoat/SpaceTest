using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Math;

public class Node : MonoBehaviour {
    public int numFrames = 0;
    public int numBonds = 0;
    private List<Object> Bonds = new List<Object>();
    public int openBonds;
    void FixedUpdate()
    {
        //get other nodes within range (2units)
        float radius = 1, thrust, dist;
        openBonds = numBonds - Bonds.Count;
        Vector3 source = transform.position;
        Vector3 dest;
        Vector3 vec;
        Rigidbody body = GetComponent<Rigidbody>();

        // create list and keep refs of bonded bros. check that first before doing sphere check.
        // in sphere check see if they have an open slot before bonding with them.
        // if bond passes both add as a bonded bro.
        int i = 0;
        int[] b = new int[Bonds.Count];
        foreach(Collider a in Bonds)
        {
            if (a != null)
            {
                //make sure they're still in range. if they're not delete ref
                dest = a.transform.position;
                vec = source - dest;
                dist = vec.magnitude;
                if (dist > radius * 2)
                {
                    b[i] = 0;
                }
                else
                {
                    //else add force

                    Rigidbody rb = a.GetComponent<Rigidbody>();
                    thrust = (float)(1 - System.Math.Pow(((dist - 1.5 * radius) * 2 * radius),2));
                    rb.AddForce(vec * thrust);
                    body.AddForce(vec * -thrust);
                    b[i] = 1;
                }
                i++;
            }
        }
        for (i = -1; i >= 0; i--)
        {
            if (b[i] == 0)
            {
                Bonds.RemoveAt(i);
                openBonds++;
            }
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2 * radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                dest = hit.transform.position;
                vec = source - dest;
                dist = vec.magnitude;
                if (dist > 0) // ie not self. unless some int f-ery is going on then some points might think they occupy the same space.
                {

                    thrust = (float)(System.Math.Pow(((dist - .5 * radius) * 2*radius), 2) - 1);
                    //Debug.Log("dist: " + dist + " thrust: " + thrust);
                    if (dist > radius)
                        thrust = 0;

                    if(openBonds>0)
                    {
                        // add bond if other node has open bond
                        if (hit.GetComponent<Node>().openBonds > 0)
                        {
                            Debug.Log(hit.GetComponent<Node>().openBonds + ", " + openBonds);
                            hit.GetComponent<Node>().openBonds -= 1;
                            hit.GetComponent<Node>().Bonds.Add(GetComponent<Collider>());
                            openBonds--;
                            Bonds.Add(hit);
                        }
                    }
                    rb.AddForce(vec * thrust);
                    body.AddForce(vec * -thrust);
                }
            }
            //Debug.Log("dist: " + dist+" thrust: "+thrust);

        }
        if (numFrames > 0 | numFrames < 0)
        {
            //Debug.Log(numFrames);
            numFrames--;
            colliders = Physics.OverlapSphere(transform.position, 100);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                dest = hit.transform.position;
                vec = source - dest;
                dist = vec.magnitude;
                thrust = (float)(1 / (dist + 2.0));
                thrust *= 0.1f;
                if (rb != null & dist > 0)
                {
                    rb.AddForce(vec * thrust);
                    body.AddForce(vec * -thrust);
                }

            }
        }
        
    }

}
