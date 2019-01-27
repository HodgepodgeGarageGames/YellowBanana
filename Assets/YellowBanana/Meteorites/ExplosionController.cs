using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private ParticleSystem ps ;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, ps.main.duration);
    }

    private void Update()
    {
        if(ps)
        {
            if(!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
