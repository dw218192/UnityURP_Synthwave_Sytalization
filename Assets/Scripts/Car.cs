using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;
using static UnityEditor.PlayerSettings;

public class Car : MonoBehaviour
{
    Rigidbody rb;
    Vector3 initialPos;

    public ParticleSystem[] carSmokes;
    public VolumetricLightBeamSD[] beamSDs;
    public float beamSpeed = 2.3f;
    public float speedSpeed = 5.0f;
    public float loBeam = 7, hiBeam = 16;
    public float loSpeed = 8, hiSpeed = 50;
    public float loX = -0.5f, hiX = 0.5f;

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
            DemoManager.Instance.Speed = hiSpeed;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (var smoke in carSmokes)
            {
                smoke.Stop(true);
            }
            DemoManager.Instance.Speed = loSpeed;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (!Mathf.Approximately(beamSDs[0].spotAngle, hiBeam))
            {
                foreach (var beamSD in beamSDs)
                {
                    beamSD.spotAngle = Mathf.Min(hiBeam, beamSD.spotAngle + beamSpeed * Time.deltaTime);
                    beamSD.UpdateAfterManualPropertyChange();
                }
//                DemoManager.Instance.Speed = Mathf.Min(hiSpeed, DemoManager.Instance.Speed + speedSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (!Mathf.Approximately(beamSDs[0].spotAngle, loBeam))
            {
                foreach (var beamSD in beamSDs)
                {
                    beamSD.spotAngle = Mathf.Max(loBeam, beamSD.spotAngle - beamSpeed * Time.deltaTime);
                    beamSD.UpdateAfterManualPropertyChange();
                }
//               DemoManager.Instance.Speed = Mathf.Max(loSpeed, DemoManager.Instance.Speed - speedSpeed * Time.deltaTime);
            }
        }


        var pos = transform.position;
        if (Input.GetKey(KeyCode.A))
        {
            pos.x = Mathf.Max(loX, pos.x - speedSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x = Mathf.Min(hiX, pos.x + speedSpeed * Time.deltaTime);
        }

        transform.position = pos;
    }

    void LateUpdate()
    {
        // make sure the car is always facing forward
        var fwd = Vector3.forward;
        transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);

        // lock x and z position
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, loX, hiX);
        pos.z = initialPos.z;
        transform.position = pos;
    }
}
