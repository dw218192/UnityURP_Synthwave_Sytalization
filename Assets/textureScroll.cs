using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
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
            float offset = -Time.time * scrollSpeed;
            mat.SetTextureOffset("_BaseMap", new Vector2(0, offset));
        }
    }
}
