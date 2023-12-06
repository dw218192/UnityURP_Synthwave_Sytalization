using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    Rigidbody rb;
    Vector3 initialPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialPos = transform.position;
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        // make sure the car is always facing forward
        var fwd = Vector3.forward;
        transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);

        // lock x and z position
        var pos = transform.position;
        pos.x = initialPos.x;
        pos.z = initialPos.z;
        transform.position = pos;
    }
}
