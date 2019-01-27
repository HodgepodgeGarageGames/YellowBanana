using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteController : MonoBehaviour
{
    public GameObject explosion;
    public int RotationSpeed;
    public float MovingSpeed;
    private Vector3 _leftOrRight;
    private Vector3 _upOrDown;
    private GameObject Globe;
    private AudioSource _audioSource;
    bool isplaying = false;

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

    void Initialize(int rotationSpeed, int movingSpeed)
    {
        RotationSpeed = rotationSpeed;
        MovingSpeed = movingSpeed;
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
    }

    // Update is called once per frame
    void Update()
    {
        //Meteorite Rotation
        transform.Rotate(_leftOrRight* Time.deltaTime * RotationSpeed);
        transform.Rotate(_upOrDown * Time.deltaTime  * RotationSpeed, Space.World);

        //Meteorite Movement
        float step =  MovingSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, Globe.transform.position, step);

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

            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(TimedDeath());
        }
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
