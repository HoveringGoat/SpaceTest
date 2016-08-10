using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
    public int numFrames = 0;
    void FixedUpdate()
    {
        //get other nodes within range (2units)
        float radius = 1, thrust, dist;
        int i = 0;
        bool attract = true;
        int numBonds = 4;
        Vector3 source = transform.position;
        Vector3 dest;
        Vector3 vec;
        Rigidbody body = GetComponent<Rigidbody>();

        // create list and keep refs of bonded bros. check that first before doing sphere check.
        // in sphere check see if they have an open slot before bonding with them.
        // if bond passes both add as a bonded bro.
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2 * radius);
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
                    i++;
                    if (i >= numBonds)
                        attract = false;

                    thrust = (float)(-4 / ((dist + 1.0) * (dist + 1.0)));
                    if (attract)
                        thrust++;
                    thrust *= 1f;
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
