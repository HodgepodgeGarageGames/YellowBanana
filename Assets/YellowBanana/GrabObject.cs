using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrabObject : MonoBehaviour
{
    private VRController CurrentlyGrabbedBy = null;
    private Rigidbody rb = null;
    private FixedJoint fj = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public bool Grab(VRController grabber)
    {
        if (CurrentlyGrabbedBy == null)
        {
            CurrentlyGrabbedBy = grabber;

            if (fj != null)
            {
                fj.connectedBody = null;
                Destroy(fj);
            }
            fj = CurrentlyGrabbedBy.gameObject.AddComponent<FixedJoint>();
            fj.connectedBody = rb;

            return true;
        }
        else
            return false;
    }

    public void Release(Vector3 vel, Vector3 ang_vel)
    {
        rb.velocity = vel;
        rb.angularVelocity = ang_vel;

        CurrentlyGrabbedBy = null;

        if (fj != null)
        {
            fj.connectedBody = null;
            Destroy(fj);
        }
    }

}
