﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthLogic : MonoBehaviour
{
    public AudioClip[] woah;
    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isGoingTooFast = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.angularVelocity.magnitude > 4.0f)
        {
            if (isGoingTooFast == false)
            {
                isGoingTooFast = true;

                if (!audioSource.isPlaying)
                {
                    int mCount = MeepleManager.allMeeples.Length;
                    foreach (GameObject meeple in MeepleManager.allMeeples)
                    {
                        if (meeple.GetComponent<MeepleTemperature>().isDead())
                        {
                            --mCount;
                        }
                    }
                    if (mCount > 4)
                    {
                        audioSource.clip = woah[Random.Range(0, woah.Length)];
                        audioSource.Play();
                    }
                    
                }
            }
        }
        else
        {
            if (!audioSource.isPlaying)
                isGoingTooFast = false;
        }
    }
}
