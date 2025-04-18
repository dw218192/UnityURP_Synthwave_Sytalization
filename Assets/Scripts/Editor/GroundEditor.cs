using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VertexChanger))]
public class GroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update"))
        {
            var ground = target as VertexChanger;
            var rend = ground.GetComponent<Renderer>();
            var groundMat = rend.sharedMaterial;
            groundMat.SetVector("_extent", rend.localBounds.extents);
        }
    }
}