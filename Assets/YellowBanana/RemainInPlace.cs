using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainInPlace : MonoBehaviour
{
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = pos + ((transform.position - pos) * 0.86f);
    }
}
