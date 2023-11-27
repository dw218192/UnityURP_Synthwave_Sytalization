using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public float maxHeight = 1.97f;
    public float noiseFreq = 6.0f;
    public float xthres = 1.87f;

    Vector3 extents; 
    float speed;

    Vector3[] originalVertices;
    Vector3[] modifiedVertices;

    Mesh mesh;
    MeshCollider meshCollider;

    float Frac(float v)
    {
        return v - Mathf.Floor(v);
    }
    Vector2 Frac(Vector2 v)
    {
        return new Vector2(v.x - Mathf.Floor(v.x), v.y - Mathf.Floor(v.y));
    }
    float Rd(Vector2 uv)
    {
        float dt = Vector2.Dot(uv, new Vector2(12.9898f, 78.233f));
        return Frac(Mathf.Sin(dt) * 43758.5453f);
    }

    public float Noise(Vector2 uv)
    {
        Vector2 i = new Vector2(Mathf.Floor(uv.x), Mathf.Floor(uv.y));
        Vector2 f = Frac(uv);
        f = f * f * (new Vector2(3.0f, 3.0f) - 2.0f * f);

        uv = new Vector2(Mathf.Abs(Frac(uv.x) - 0.5f), Mathf.Abs(Frac(uv.y) - 0.5f));
        Vector2 c0 = i + new Vector2(0.0f, 0.0f);
        Vector2 c1 = i + new Vector2(1.0f, 0.0f);
        Vector2 c2 = i + new Vector2(0.0f, 1.0f);
        Vector2 c3 = i + new Vector2(1.0f, 1.0f);

        float r0 = Rd(c0);
        float r1 = Rd(c1);
        float r2 = Rd(c2);
        float r3 = Rd(c3);

        float t = Mathf.Lerp(Mathf.Lerp(r0, r1, f.x), Mathf.Lerp(r2, r3, f.x), f.y);
        return t;
    }

    public float Noise(Vector2 uv, float scale)
    {
        float t = 0.0f;
        float freq = Mathf.Pow(2.0f, 0);
        float amp = Mathf.Pow(0.5f, 3);
        t += Noise(new Vector2(uv.x * scale / freq, uv.y * scale / freq)) * amp;

        freq = Mathf.Pow(2.0f, 1);
        amp = Mathf.Pow(0.5f, 3 - 1);
        t += Noise(new Vector2(uv.x * scale / freq, uv.y * scale / freq)) * amp;

        freq = Mathf.Pow(2.0f, 2);
        amp = Mathf.Pow(0.5f, 3 - 2);
        t += Noise(new Vector2(uv.x * scale / freq, uv.y * scale / freq)) * amp;
        return t;
    }

    Vector3 ModifyVerts(Vector3 pos) {
       Vector2 sp = new Vector2(pos.x, pos.z - speed * Time.time);
        if (Mathf.Abs(pos.x) >= xthres) {
            float t = (Mathf.Abs(pos.x) - xthres) / (extents.x - xthres); // [0,1]
            float farness = 1.0f - (pos.z + extents.z) / (2 * extents.z); // [0,1]
            pos.y += maxHeight * Mathf.SmoothStep(0.0f, 1.0f, t) + Noise(sp * noiseFreq, 2.2f * (farness * farness));
        }
        // else {
        //     pos.y += Noise(sp * noiseFreq * 0.25f) * 0.1f;
        // }

        return pos;
    }

    public void UpdateMesh()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            modifiedVertices[i] = ModifyVerts(originalVertices[i]);
        }
        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
    }

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];

        var rend = GetComponent<Renderer>();
        extents = rend.localBounds.extents;
        var mat = rend.sharedMaterial;
        speed = mat.GetFloat("_speed");
        
        // meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        UpdateMesh();
    }

    void LateUpdate()
    {
        // meshCollider.sharedMesh = null;
        // meshCollider.sharedMesh = mesh;
    }
}
