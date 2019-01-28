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
    public MeteoriteSpawner spawner;
    public EndFieldLogic endField;
    public Light spotLight;
    public AudioClip sadMusic;
    public AudioClip clunk;
    private AudioSource sadMusicSource;
    private AudioSource clunkSource;

    private Coroutine coEnd = null;

    void Start()
    {
        MeepleManager.allMeeples = GameObject.FindGameObjectsWithTag("Meeple");

        sadMusicSource = gameObject.AddComponent<AudioSource>();
        sadMusicSource.clip = sadMusic;
        sadMusicSource.playOnAwake = false;
        sadMusicSource.loop = false;

        clunkSource = gameObject.AddComponent<AudioSource>();
        clunkSource.clip = clunk;
        clunkSource.playOnAwake = false;
        clunkSource.loop = false;
    }

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

        if (coEnd == null)
        {
            int mCount = MeepleManager.allMeeples.Length;
            foreach (GameObject meeple in MeepleManager.allMeeples)
            {
                if (meeple.GetComponent<MeepleTemperature>().isDead())
                {
                    --mCount;
                }
            }
            if (mCount == 0)
            {
                coEnd = StartCoroutine(EndState());
            }
        }
    }

    private IEnumerator EndState()
    {
        Debug.Log("End state started");
        spawner.gameObject.SetActive(false);
        endField.GoToEndState();
        lampLight.intensity = 0.0f;
        lampLight.range = 0.0f;
        yield return new WaitForSeconds(endField.transitionTime);
        yield return new WaitForSeconds(1.0f);
        clunkSource.Play();
        spotLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        sadMusicSource.Play();
        yield return new WaitForSeconds(4.0f);

        float T = 0.0f;
        float spotInt = spotLight.intensity;

        while (T < 10.0f)
        {
            spotLight.spotAngle = 30.0f - (30.0f * (T / 10.0f));

            if (T > 3.0f)
            {
                spotLight.intensity = spotInt * (1.0f - ((T - 3.0f) / 6.0f));
            }

            yield return null;
            T += Time.deltaTime;
        }

        spotLight.spotAngle = 0.0f;
        spotLight.intensity = 0.0f;
    }
}
