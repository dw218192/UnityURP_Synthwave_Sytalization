using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class Motion : MonoBehaviour
{
    public float beginZ; 
    public float endZ;   

    private Vector3 origin;
    void Start()
    {
        origin = transform.position;
    }

    void Update() {
        float newZ = transform.position.z - Time.deltaTime * DemoManager.Instance.Speed;

        if (newZ >= endZ)
        {
            transform.position = new Vector3(origin.x, transform.position.y, newZ);
        }
        else
        {   
            transform.position = new Vector3(origin.x, transform.position.y, beginZ);
        }
        if (transform.position.y < 0){
            transform.position = new Vector3(origin.x, origin.y, beginZ);
        }
    }
}
