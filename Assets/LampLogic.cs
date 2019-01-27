using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLogic : MonoBehaviour
{
    public AudioClip piff;
    public AudioClip chime;
    public AudioClip clickOn;
    public AudioClip clickOff;

    private AudioSource piffSource;
    private AudioSource chimeSource;
    private AudioSource clickSource;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Meteor"))
        {
            if (!piffSource.isPlaying)
            {
                Debug.Log(collision.transform.CompareTag("Meteor") + ": " + collision.transform.tag);
                piffSource.Stop();
                piffSource.Play();

                if (Random.value < 1.0f)
                {
                    if (!chimeSource.isPlaying)
                    {
                        chimeSource.Stop();
                        chimeSource.Play();
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        piffSource = gameObject.AddComponent<AudioSource>();
        piffSource.clip = piff;
        piffSource.loop = false;
        piffSource.playOnAwake = false;

        chimeSource = gameObject.AddComponent<AudioSource>();
        chimeSource.clip = chime;
        chimeSource.loop = false;
        chimeSource.playOnAwake = false;

        clickSource = gameObject.AddComponent<AudioSource>();
        clickSource.loop = false;
        clickSource.playOnAwake = false;
    }

    public void PlayClickOnSound()
    {
        clickSource.Stop();
        clickSource.clip = clickOn;
        clickSource.Play();
    }

    public void PlayClickOffSound()
    {
        clickSource.Stop();
        clickSource.clip = clickOff;
        clickSource.Play();
    }
}
