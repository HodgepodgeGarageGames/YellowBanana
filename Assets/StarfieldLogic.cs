using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldLogic : MonoBehaviour
{
    public AudioClip house_music;
    public AudioClip space_music;
    private Material mat;
    private AudioSource houseSource;
    private AudioSource spaceSource;

    private const float transitionTime = 3.0f;

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
    }

    public void GoIntoSpaceMode()
    {
        StartCoroutine(go_into_space_mode());
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
    }

    public void GoIntoHouseMode()
    {
        StartCoroutine(go_into_house_mode());
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
    }
}
