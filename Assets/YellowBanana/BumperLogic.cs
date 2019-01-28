using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BumperLogic : MonoBehaviour
{
    public GoalLogic goal;
    private VideoPlayer vid;

    // Start is called before the first frame update
    void Start()
    {
        vid = GetComponent<VideoPlayer>();
        
        StartCoroutine(playvid());
    }

    private IEnumerator playvid()
    {
        vid.Play();

        yield return new WaitForSeconds(1.0f);

        while (vid.isPlaying)
        {
            yield return null;
        }

        Destroy(vid);
        goal.ShowGoal();
    }
}
