﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldLogic : MonoBehaviour
{
    public AudioClip house_music;
    public AudioClip space_music;
    public AudioClip alarm_sound;
    private Material mat;
    [HideInInspector]
    public AudioSource houseSource;
    [HideInInspector]
    public AudioSource spaceSource;
    [HideInInspector]
    public AudioSource alarmSource;

    public float transitionTime = 3.0f;

    public Coroutine co_to_space = null;
    public Coroutine co_to_house = null;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        mat.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        houseSource = gameObject.AddComponent<AudioSource>();
        houseSource.clip = house_music;
        houseSource.loop = true;
        houseSource.playOnAwake = false;
        houseSource.volume = 1.0f;
        houseSource.Play();

        spaceSource = gameObject.AddComponent<AudioSource>();
        spaceSource.clip = space_music;
        spaceSource.loop = true;
        spaceSource.playOnAwake = false;
        spaceSource.volume = 0.0f;
        spaceSource.Play();

        alarmSource = gameObject.AddComponent<AudioSource>();
        alarmSource.clip = alarm_sound;
        alarmSource.loop = false;
        alarmSource.playOnAwake = false;
    }

    public void GoIntoSpaceMode()
    {
        co_to_space = StartCoroutine(go_into_space_mode());
    }

    private IEnumerator go_into_space_mode()
    {
        float T = 0.0f;

        while (T < transitionTime)
        {
            mat.color = new Color(1.0f, 1.0f, 1.0f, T / transitionTime);
            houseSource.volume = 1.0f - (T / transitionTime);
            spaceSource.volume = T / transitionTime;

            yield return null;
            T += Time.deltaTime;
        }

        mat.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        houseSource.volume = 0.0f;
        spaceSource.volume = 1.0f;

        alarmSource.Play();

        co_to_space = null;
    }

    public void GoIntoHouseMode()
    {
        co_to_house = StartCoroutine(go_into_house_mode());
    }

    private IEnumerator go_into_house_mode()
    {
        float T = 0.0f;

        while (T < transitionTime)
        {
            mat.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - (T / transitionTime));
            houseSource.volume = T / transitionTime;
            spaceSource.volume = 1.0f - (T / transitionTime);

            yield return null;
            T += Time.deltaTime;
        }

        mat.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        houseSource.volume = 1.0f;
        spaceSource.volume = 0.0f;

        co_to_house = null;
    }
}
