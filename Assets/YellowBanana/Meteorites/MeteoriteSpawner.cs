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
    private float _timeForNextWave = 20.0f;
    private float _meteoriteCount = 2;
    private float _timeBetweenMeteorites = 1.0f;
    private float MinXPosition = -3f;
    private float MaxXPosition = 3f;
    private float MinZPosition = -3.0f;
    private float MaxZPosition = 3.0f;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private GameObject Spawn()
    {
        Vector3 position = RandomPoint(transform.position, 5.0f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, transform.position - position);
        var meteorite = Instantiate(Meteorite, position, rotation) as GameObject;

        var controller = meteorite.GetComponent<MeteoriteController>();
        controller.RotationSpeed = RotationSpeed;
        controller.MovingSpeed = MovingSpeed;

        return meteorite;
    }

    Vector3 RandomPoint(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;

        pos.x = Random.Range(MinXPosition, MaxXPosition);
        pos.y = 4;
        pos.z = Random.Range(MinZPosition, MaxZPosition);

        return pos;
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(_timeForNextWave-starField.transitionTime);
        starField.GoIntoSpaceMode();
        yield return new WaitForSeconds(starField.transitionTime);
        //After the first two minutes, we will send waves once per minute
        _timeForNextWave = 40.0f;
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
            yield return new WaitForSeconds(_timeForNextWave - starField.transitionTime);
            starField.GoIntoSpaceMode();
            yield return new WaitForSeconds(starField.transitionTime);
        }
    }
}