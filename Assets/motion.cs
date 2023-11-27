using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class Motion : MonoBehaviour
{
    public float beginZ; 
    public float endZ;   
    public float speed = 1.0f; 

    private float startTime; 
    private Vector3 origin;
    void Start()
    {
        startTime = Time.time; 
        origin = transform.position;
    }

    void Update()
    {
        float distance = (Time.time - startTime) * speed;
        
        float newZ = transform.position.z - distance;

        if (newZ >= endZ)
        {
            transform.position = new Vector3(origin.x, transform.position.y, newZ);
        }
        else
        {   
            startTime = Time.time;
            transform.position = new Vector3(origin.x, transform.position.y, beginZ);
        }
        if (transform.position.y < 0){
            transform.position = new Vector3(origin.x, origin.y, beginZ);
        }
    }
}
