using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteController : MonoBehaviour
{
    public int RotationSpeed;
    public int MovingSpeed;
    private Vector3 _leftOrRight;
    private Vector3 _upOrDown;
    private GameObject Globe;

    public MeteoriteController()
    {
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
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
