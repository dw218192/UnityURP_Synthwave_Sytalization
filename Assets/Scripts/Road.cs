using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : VertexChanger
{
    public float noiseScale = 1.0f;
    public float noiseFreq = 1.0f;

    protected override Vector3 ModifyVerts(Vector3 pos)
    {
        pos.y = Common.Noise(new Vector2(pos.x, pos.z) * noiseFreq, noiseScale);
        return pos;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
