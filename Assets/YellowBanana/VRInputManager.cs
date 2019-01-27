using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRInputManager : MonoBehaviour
{
    public VRController left_hand;
    public VRController right_hand;

    public Light lampLight;
    public MeshRenderer lampBulb;

    [SteamVR_DefaultAction("TrackPad")]
    public SteamVR_Action_Vector2 trackPadAction;

    public LampLogic lamp;

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

        Vector2 trackPadValue = trackPadAction.GetAxis(SteamVR_Input_Sources.Any);

        if (SteamVR_Input._default.inActions.Teleport.GetLastStateDown(SteamVR_Input_Sources.Any))
        {
            if (trackPadValue != Vector2.zero)
            {
                if (trackPadValue.y > 0.333f)
                {
                    lampLight.range += 0.2f;
                    if (lampLight.range > 6.0f)
                        lampLight.range = 6.0f;
                    lamp.PlayClickOnSound();
                }
                else if (trackPadValue.y < -0.333f)
                {
                    lampLight.range -= 0.2f;
                    if (lampLight.range < 0.6f)
                        lampLight.range = 0.6f;
                    lamp.PlayClickOffSound();
                }
            }
        }

        lampBulb.material.color = new Color(lampLight.range, lampLight.range, lampLight.range, lampLight.range * 2.0f);

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
