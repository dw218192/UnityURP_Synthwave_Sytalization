using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureScroll : MonoBehaviour
{
    private Renderer rend;
    private Material mat;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("No Renderer found on the object.");
            return;
        }

        mat = rend.material;
        if (mat == null)
        {
            Debug.LogError("No Material found on the renderer.");
        }
    }

    void Update()
    {
        if (mat != null)
        {
            float offset = -Time.time * DemoManager.Instance.Speed;
            mat.SetTextureOffset("_BaseMap", new Vector2(0, offset));
        }
    }
}
