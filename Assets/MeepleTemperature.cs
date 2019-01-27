using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleTemperature : MonoBehaviour
{
    public Light light_source;
    public MeepleTemperature[] neighbors;

    private Animator anim; 
    private Material mat;

    private float spawn_timer = 0.0f;

    private const float cooldown_rate = 0.03f;
    private const float heatup_rate = 0.7f;

    private ParticleSystem smoke;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = transform.localPosition.normalized / 2.0f;
        transform.localRotation = Quaternion.LookRotation(-transform.localPosition);
        transform.Rotate(-90.0f, 0.0f, 0.0f);
        transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        mat.color = Color.green;
        mat.shader = Shader.Find("Unlit/Color");
        anim = GetComponent<Animator>();

        smoke = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Temperature", anim.GetFloat("Temperature") - Time.deltaTime * cooldown_rate);

        RaycastHit hit;
        if (Physics.Raycast(light_source.transform.position, transform.position - light_source.transform.position, out hit, light_source.range))
        {
            if (hit.collider.name == name)
            {
                anim.SetFloat("Temperature", anim.GetFloat("Temperature") + (Time.deltaTime * heatup_rate * Vector3.Dot(transform.position - transform.parent.position, light_source.transform.position - transform.parent.position)) * (1.0f - ((light_source.transform.position - transform.parent.position).magnitude / light_source.range)));
            }
        }

        if (anim.GetFloat("Temperature") > 0.8f && anim.GetFloat("Temperature") < 1.0f && gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled == true)
        {
            if (smoke.isPlaying != true)
                smoke.Play();
        }
        else
        {
            if (smoke.isPlaying)
                smoke.Stop();
        }

        if (anim.GetFloat("Temperature") > 0.0f)
        {
            if (anim.GetFloat("Temperature") > 1.0f)
            {
                gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

                if (anim.GetFloat("Temperature") > 1.5f)
                {
                    anim.SetFloat("Temperature", 1.5f);
                }
            }

            mat.color = new Color(anim.GetFloat("Temperature"), 1.0f - anim.GetFloat("Temperature"), 0.0f);
        }
        else if (anim.GetFloat("Temperature") < 0.0f)
        {
            if (anim.GetFloat("Temperature") < -1.0f)
            {
                gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

                if (anim.GetFloat("Temperature") < -1.5f)
                {
                    anim.SetFloat("Temperature", -1.5f);
                }
            }

            mat.color = new Color(-anim.GetFloat("Temperature")/2.0f, 1.0f - (-anim.GetFloat("Temperature")/2.0f), -anim.GetFloat("Temperature"));
        }
        else
        {
            mat.color = Color.black;
        }

        if (GetComponentInChildren<SkinnedMeshRenderer>().enabled == true)
        {
            if (spawn_timer > 3.0f)
            {
                spawn_timer -= 3.0f;
                foreach (MeepleTemperature meep in neighbors)
                {
                    if (meep.GetComponentInChildren<SkinnedMeshRenderer>().enabled == false)
                    {
                        if (meep.GetComponent<Animator>().GetFloat("Temperature") > -1.0f && meep.GetComponent<Animator>().GetFloat("Temperature") < 1.0f)
                        {
                            meep.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
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
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, ((transform.position - transform.parent.position).normalized * 0.5f) + transform.parent.position);

        foreach (MeepleTemperature meep in neighbors)
        {
            if (meep)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, meep.transform.position);
            }
        }
    }

    public void spawn_init()
    {
        spawn_timer = 0.0f;
    }

    public void get_hit_by_meteor()
    {
        anim.SetFloat("Temperature", anim.GetFloat("Temperature") + 0.4f);
    }
}
