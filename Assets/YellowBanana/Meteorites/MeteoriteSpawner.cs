using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
    public GameObject Meteorite;
    public int RotationSpeed;
    public int MovingSpeed;
    public float SpawnRate = 2.0f;
    private float _timeSinceLastSpawn;

    void Start()
    {  
    }

    void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
	    if (_timeSinceLastSpawn > SpawnRate)
	    {
	        Spawn();
	        _timeSinceLastSpawn = 0.0f;
	    }
    }

    private void Spawn()
    {
        Vector3 position = RandomCircle(transform.position, 5.0f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, transform.position - position);
        var meteorite = Instantiate(Meteorite, position, rotation) as GameObject;

        var controller =  meteorite.GetComponent<MeteoriteController>();
        controller.RotationSpeed = RotationSpeed;
        controller.MovingSpeed = MovingSpeed;
    }
 
     Vector3 RandomCircle ( Vector3 center ,   float radius  ){
         float ang = Random.value * 360;
         Vector3 pos;
         pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
         pos.y = Mathf.Abs(center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad));
         pos.z = center.z;
         return pos;
     }
}
