using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Node : MonoBehaviour {
    public float radius = 5;
    public float dragVel = 1.0f;
    public float frozeSpeed = .01f;
    public int isFrozen = 0;
    public bool DebugOn = false;
    public bool Meshed = false;
    public List<Vector3> Local = new List<Vector3>();
    void FixedUpdate()
    {
        if (Meshed)
            Meshed = false;
        if (isFrozen<100)
        {
            //get other nodes within range (2units)
            float thrust, dist;
            Vector3 temp;
            Vector3 source = transform.position;
            Vector3 dest;
            Vector3 vec;
            Rigidbody body = GetComponent<Rigidbody>();

            //check frozen
            if (body.velocity.magnitude < frozeSpeed)
            {
                isFrozen++;
            }
            else 
            {
                isFrozen = 0;
            }
            
            if(isFrozen >= 100)
            {
                body.velocity = Vector3.zero;
            }
            else
            {
                Local = new List<Vector3>();
                Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f * radius);
                foreach (Collider hit in colliders)
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        dest = hit.transform.position;
                        vec = source - dest;
                        dist = vec.magnitude;
                        Local.Add(-vec);
                        if (dist > 0)
                        {
                            if (dist > radius)
                            {
                                thrust = (float)(1 - System.Math.Pow(((dist - 1.25 * radius) * (4/radius)), 2)); //attract
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
                        }
                    }
                }
            }
        }
        if ((GetComponent<Rigidbody>().velocity.magnitude > 0) & isFrozen >= 100)
        {
            if(DebugOn)
                Debug.Log("vel = " + GetComponent<Rigidbody>().velocity.magnitude);
            isFrozen = 0;
        }

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
        }
        else
        {

            dest = Vector3.zero;//center of mass eh
            vec = source - dest;
            dist = vec.magnitude;
            vec=(-vec*(1/dist));
            vec *= 1.0f;
            body.velocity = vec;
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
        }
    }
    

}
