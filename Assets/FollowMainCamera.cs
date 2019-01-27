using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMainCamera : MonoBehaviour
{
    public Camera otherCamera;

    // Update is called once per frame
    void Update()
    {
        transform.position = otherCamera.transform.position;
        transform.rotation = otherCamera.transform.rotation;
    }
}
