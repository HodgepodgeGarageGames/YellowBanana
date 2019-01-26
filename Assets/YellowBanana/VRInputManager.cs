using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRInputManager : MonoBehaviour
{
    public VRController left_hand;
    public VRController right_hand;

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Input._default.inActions.GrabPinch.GetLastStateDown(SteamVR_Input_Sources.LeftHand))
        {
            left_hand.Grab();
        }
        else if (SteamVR_Input._default.inActions.GrabPinch.GetLastStateUp(SteamVR_Input_Sources.LeftHand))
        {
            left_hand.Release(true);
        }
        
        if (SteamVR_Input._default.inActions.GrabPinch.GetLastStateDown(SteamVR_Input_Sources.RightHand))
        {
            right_hand.Grab();
        }
        else if (SteamVR_Input._default.inActions.GrabPinch.GetLastStateUp(SteamVR_Input_Sources.RightHand))
        {
            right_hand.Release(false);
        }
    }
}
