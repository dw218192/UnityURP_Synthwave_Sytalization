using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : VertexChanger
{
    public float maxHeight = 1.97f;
    public float noiseFreq = 6.0f;
    public float xthres = 1.87f;

    Vector3 extents; 
    float speed;

    protected override Vector3 ModifyVerts(Vector3 pos) {
       Vector2 sp = new Vector2(pos.x, pos.z - speed * Time.time);
        if (Mathf.Abs(pos.x) >= xthres) {
            float t = (Mathf.Abs(pos.x) - xthres) / (extents.x - xthres); // [0,1]
            float farness = 1.0f - (pos.z + extents.z) / (2 * extents.z); // [0,1]
            pos.y += maxHeight * Mathf.SmoothStep(0.0f, 1.0f, t) + Common.Noise(sp * noiseFreq, 2.2f * (farness * farness));
        }
        return pos;
    }

    protected override void Start()
    {
        base.Start();
        var rend = GetComponent<Renderer>();
        extents = rend.localBounds.extents;
        var mat = rend.sharedMaterial;
        speed = mat.GetFloat("_speed");
    }
}
