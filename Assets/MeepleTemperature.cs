using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleTemperature : MonoBehaviour
{
    public Light light_source;
    public MeepleTemperature[] neighbors;

    private float temperature = 0.0f;
    private Material mat;

    private float spawn_timer = 0.0f;

    private const float cooldown_rate = 0.03f;
    private const float heatup_rate = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = transform.localPosition.normalized / 2.0f;
        transform.localRotation = Quaternion.LookRotation(-transform.localPosition);
        transform.Rotate(-90.0f, 0.0f, 0.0f);
        transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
        mat = GetComponent<MeshRenderer>().material;
        mat.color = Color.black;
        mat.shader = Shader.Find("Unlit/Color");
    }

    // Update is called once per frame
    void Update()
    {
        temperature -= Time.deltaTime * cooldown_rate;

        RaycastHit hit;
        if (Physics.Raycast(light_source.transform.position, transform.position - light_source.transform.position, out hit, light_source.range))
        {
            if (hit.collider.name == transform.name)
            {
                temperature += (Time.deltaTime * heatup_rate * Vector3.Dot(transform.position - transform.parent.position, light_source.transform.position - transform.parent.position)) * (1.0f - ((light_source.transform.position - transform.parent.position).magnitude / light_source.range));
            }
        }

        if (temperature > 0.0f)
        {
            if (temperature > 1.0f)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;

                if (temperature > 1.5f)
                {
                    temperature = 1.5f;
                }
            }

            mat.color = new Color(Mathf.Pow(temperature, 2.0f), 0.0f, 0.0f);
        }
        else if (temperature < 0.0f)
        {
            if (temperature < -1.0f)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;

                if (temperature < -1.5f)
                {
                    temperature = -1.5f;
                }
            }

            mat.color = new Color(Mathf.Pow(-temperature/2.0f, 2.0f), Mathf.Pow(-temperature/2.0f, 2.0f), Mathf.Pow(-temperature, 2.0f));
        }
        else
        {
            mat.color = Color.black;
        }

        if (GetComponent<MeshRenderer>().enabled == true)
        {
            if (spawn_timer > 3.0f)
            {
                spawn_timer -= 3.0f;
                foreach (MeepleTemperature meep in neighbors)
                {
                    if (meep.GetComponent<MeshRenderer>().enabled == false)
                    {
                        if (meep.temperature > -1.0f && meep.temperature < 1.0f)
                        {
                            meep.GetComponent<MeshRenderer>().enabled = true;
                            meep.spawn_init();
                            break;
                        }
                    }
                }
            }
            spawn_timer += Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (MeepleTemperature meep in neighbors)
        {
            if (meep)
            {
                if (meep.enabled == true)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(transform.position, meep.transform.position);
                }
            }
        }
    }

    public void spawn_init()
    {
        spawn_timer = 0.0f;
    }
}
