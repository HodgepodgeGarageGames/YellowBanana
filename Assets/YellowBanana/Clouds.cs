using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public Rigidbody globeRB;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = globeRB.transform.position;
        rb.angularVelocity = new Vector3(globeRB.angularVelocity.x / 3.0f, globeRB.angularVelocity.y / 3.0f, globeRB.angularVelocity.z / 3.0f);
    }
}
