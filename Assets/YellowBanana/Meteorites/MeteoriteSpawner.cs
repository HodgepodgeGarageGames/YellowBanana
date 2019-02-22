using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{
    public GameObject Meteorite;
    public int RotationSpeed;
    public float MovingSpeed;
    public StarfieldLogic starField;
    public GameObject globe;
    private float _timeForNextWave = 60.0f;
    private float _meteoriteCount = 2;
    private float _timeBetweenMeteorites = 1.0f;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private GameObject Spawn()
    {
        Vector3 position = RandomPoint(globe.transform.position, 5.0f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, transform.position - position);
        var meteorite = Instantiate(Meteorite, position, rotation) as GameObject;

        var controller = meteorite.GetComponent<MeteoriteController>();
        controller.RotationSpeed = RotationSpeed;
        controller.MovingSpeed = MovingSpeed;
        controller.Spawner = this;

        return meteorite;
    }

    Vector3 RandomPoint(Vector3 center, float radius)
    {
        return center + (Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f) * Quaternion.Euler(0.0f, 0.0f, Random.Range(-45.0f, 90.0f)) * -Vector3.left * radius);
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(_timeForNextWave-starField.transitionTime);
        starField.GoIntoSpaceMode();
        yield return new WaitForSeconds(starField.transitionTime);
        //After the first two minutes, we will send waves once per minute
        _timeForNextWave = 25.0f;
        while (true)
        {
            List<GameObject> activeMeteors = new List<GameObject>();

            for (int i = 0; i < _meteoriteCount; i++)
            {
                activeMeteors.Add(Spawn());
                yield return new WaitForSeconds(_timeBetweenMeteorites);
            }

            while (true)
            {
                int count = activeMeteors.Count;

                foreach (GameObject go in activeMeteors)
                {
                    if (go == null)
                    {
                        --count;
                    }
                }

                if (count == 0)
                    break;

                yield return null;
            }

            starField.GoIntoHouseMode();

            //Next wave will have two more meteorites
            _meteoriteCount += 2;
            //Next wave will also spawn meteorites faster
            _timeBetweenMeteorites *= 0.9f;

            yield return new WaitForSeconds(_timeForNextWave - starField.transitionTime);
            starField.GoIntoSpaceMode();
            yield return new WaitForSeconds(starField.transitionTime);
        }
    }
}