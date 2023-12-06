using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;

public class Car : MonoBehaviour
{
    Rigidbody rb;
    Vector3 initialPos;

    public ParticleSystem[] carSmokes;
    public VolumetricLightBeamSD[] beamSDs;
    public float beamSpeed = 2.3f;
    public float loBeam = 7, hiBeam = 16;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            foreach (var smoke in carSmokes)
            {
                smoke.Play(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (var smoke in carSmokes)
            {
                smoke.Stop(true);
            }
        }

        Debug.Log(beamSDs[0].spotAngle);

        if (Input.GetKey(KeyCode.Space))
        {
            if (!Mathf.Approximately(beamSDs[0].spotAngle, 16))
            {
                foreach (var beamSD in beamSDs)
                {
                    beamSD.spotAngle = Mathf.Min(16, beamSD.spotAngle + beamSpeed * Time.deltaTime);
                    beamSD.UpdateAfterManualPropertyChange();
                }
            }
        }
        else
        {
            if (!Mathf.Approximately(beamSDs[0].spotAngle, 7))
            {
                foreach (var beamSD in beamSDs)
                {
                    beamSD.spotAngle = Mathf.Max(7, beamSD.spotAngle - beamSpeed * Time.deltaTime);
                    beamSD.UpdateAfterManualPropertyChange();
                }
            }
        }
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
