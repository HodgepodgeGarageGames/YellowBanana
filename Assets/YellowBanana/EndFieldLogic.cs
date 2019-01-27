using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFieldLogic : MonoBehaviour
{
    public StarfieldLogic starfield;
    public float transitionTime = 2.0f;

    private Material mat;
    private AudioSource scaryMusic;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        mat.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        scaryMusic = GetComponent<AudioSource>();
    }

    public void GoToEndState()
    {
        if (starfield.co_to_house != null)
        {
            StopCoroutine(starfield.co_to_house);
            starfield.co_to_house = null;
        }
        if (starfield.co_to_space != null)
        {
            StopCoroutine(starfield.co_to_space);
            starfield.co_to_space = null;
        }
        StartCoroutine(go_to_end_state());
    }

    private IEnumerator go_to_end_state()
    {
        float T = 0.0f;

        starfield.houseSource.volume = 0.0f;
        starfield.spaceSource.volume = 0.0f;

        while (T < starfield.transitionTime)
        {
            float ratio = T / starfield.transitionTime;

            mat.color = new Color(0.0f, 0.0f, 0.0f, ratio);
            Debug.Log(mat.color);

            yield return null;
            T += Time.deltaTime;
        }

        mat.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    }
}
