using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteController : MonoBehaviour
{
    public GameObject explosion;
    public int RotationSpeed;
    public float MovingSpeed;
    public MeteoriteSpawner Spawner;
    private Vector3 _leftOrRight;
    private Vector3 _upOrDown;
    private GameObject Globe;
    private AudioSource _audioSource;
    private bool isplaying = false;
    private Vector3 targetOffset;

    private List<MeepleTemperature> hotList = new List<MeepleTemperature>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MeepleTemperature>())
        {
            hotList.Add(other.GetComponent<MeepleTemperature>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MeepleTemperature>())
        {
            hotList.Remove(other.GetComponent<MeepleTemperature>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (System.DateTime.Now.Millisecond % 2 == 0)
        {
            _leftOrRight = Vector3.right;
            _upOrDown = Vector3.up;
        }
        else
        {
            _leftOrRight = Vector3.left;
            _upOrDown = Vector3.down;
        }
        Globe = GameObject.Find("Globe");
        _audioSource = GetComponent<AudioSource>();

        targetOffset = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f) * Quaternion.Euler(0.0f, 0.0f, Random.Range(-45.0f, 90.0f)) * -Vector3.left * Random.Range(0.0f, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        //Meteorite Rotation
        transform.Rotate(_leftOrRight* Time.deltaTime * RotationSpeed);
        transform.Rotate(_upOrDown * Time.deltaTime  * RotationSpeed, Space.World);

        //Meteorite Movement
        float step =  MovingSpeed * Time.deltaTime; // calculate distance to move

        if (Vector3.Distance(transform.position, Globe.transform.position + targetOffset) < step)
            InstantDeath();
        else
            transform.position = Vector3.MoveTowards(transform.position, Globe.transform.position + targetOffset, step);

        //If we are close enough to Globe
        float dist = Vector3.Distance(Globe.transform.position, transform.position);
        if (dist <= (_audioSource.clip.length * MovingSpeed) && !isplaying)
        {
            isplaying = true;
            _audioSource.Play();
        }
        //Then play audio source
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Globe")
        {
            foreach (MeepleTemperature meep in hotList)
            {
                meep.get_hit_by_meteor();
            }
            foreach (GameObject meep in MeepleManager.allMeeples)
            {
                meep.GetComponent<MeepleTemperature>().get_indirectly_hit_by_meteor();
            }

            InstantDeath();
        }
        else
        {
            StartCoroutine(TimedDeath());
        }
    }

    private void InstantDeath()
    {
        if (transform.childCount > 0)
        {
            Transform trailParticleSystem = transform.GetChild(0);
            trailParticleSystem.SetParent(Spawner.transform, true);
            trailParticleSystem.localScale = new Vector3(
                trailParticleSystem.localScale.x / transform.localScale.x,
                trailParticleSystem.localScale.y / transform.localScale.y,
                trailParticleSystem.localScale.z / transform.localScale.z);
            ParticleSystem parsys = trailParticleSystem.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainmod = parsys.main;
            mainmod.loop = false;
            ParticleSystem.EmissionModule emmod = parsys.emission;
            emmod.rateOverTime = new ParticleSystem.MinMaxCurve(0.0f);
        }

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private IEnumerator TimedDeath()
    {
        float T = 0.0f;

        while (T < 0.5f)
        {
            yield return null;
            T += Time.deltaTime;
        }

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
