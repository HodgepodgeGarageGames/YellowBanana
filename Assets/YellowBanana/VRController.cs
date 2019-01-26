using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRController : MonoBehaviour
{
    private List<GrabObject> grab_list = new List<GrabObject>();
    [HideInInspector]
    public GrabObject currentlyGrabbedObject { get; private set; } = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrabObject>())
        {
            grab_list.Add(other.GetComponent<GrabObject>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GrabObject>())
        {
            grab_list.Remove(other.GetComponent<GrabObject>());
        }
    }

    public void Grab()
    {
        foreach (GrabObject go in grab_list)
        {
            if (go.Grab(this))
            {
                currentlyGrabbedObject = go;
            }
        }
    }

    public void Release(bool is_left)
    {
        if (currentlyGrabbedObject)
        {
            Vector3 vel, ang_vel;
            if (is_left)
            {
                vel = SteamVR_Input._default.inActions.Pose.GetVelocity(SteamVR_Input_Sources.LeftHand);
                ang_vel = SteamVR_Input._default.inActions.Pose.GetAngularVelocity(SteamVR_Input_Sources.LeftHand);
            }
            else
            {
                vel = SteamVR_Input._default.inActions.Pose.GetVelocity(SteamVR_Input_Sources.RightHand);
                ang_vel = SteamVR_Input._default.inActions.Pose.GetAngularVelocity(SteamVR_Input_Sources.RightHand);
            }

            currentlyGrabbedObject.Release(vel, ang_vel);
        }

        currentlyGrabbedObject = null;
    }
}
