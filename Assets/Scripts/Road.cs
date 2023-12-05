using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : VertexChanger
{
    public Vector2 minMaxHeight = new Vector2(0.1f, 0.24f);
    public float heightFreq = 1.0f;
    public float noiseFreq = 0.1f;
    public float noiseAmp = 0.1f;

    protected override Vector3 ModifyVerts(Vector3 pos)
    {
        float sp = pos.z - DemoManager.Instance.Speed * Time.time;
        pos.y = (minMaxHeight.x + minMaxHeight.y) / 2.0f +
            (minMaxHeight.y - minMaxHeight.x) / 2.0f * Mathf.Sin(sp * heightFreq * 
            Common.Noise(new Vector2(pos.x, sp) * noiseFreq, noiseAmp));
        return pos;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
