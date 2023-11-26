using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ground))]
public class GroundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update"))
        {
            var ground = target as Ground;
            var rend = ground.GetComponent<Renderer>();
            var groundMat = rend.sharedMaterial;
            groundMat.SetFloat("_x_max", rend.localBounds.max.x);
        }
    }
}