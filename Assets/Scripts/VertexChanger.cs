using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VertexChanger : MonoBehaviour
{
    Vector3[] originalVertices;
    Vector3[] modifiedVertices;

    Mesh mesh;
    MeshCollider meshCollider;

    protected abstract Vector3 ModifyVerts(Vector3 pos);

    protected void UpdateMesh()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            modifiedVertices[i] = ModifyVerts(originalVertices[i]);
        }
        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
    }

    protected virtual void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        UpdateMesh();
    }

    void FixedUpdate()
    {
        if (meshCollider != null && meshCollider.sharedMesh != null)
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = mesh;
        }
    }
}
