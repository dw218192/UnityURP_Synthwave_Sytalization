using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessBlendPass : ScriptableRenderPass
{
    private Material blendMaterial;

    private RTHandle cameraDepthHandle;
    private RTHandle cameraColorHandle;
    private RTHandle sunColorHandle;

    public PostProcessBlendPass(Material blendMat)
    {
        blendMaterial = blendMat;
    }
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        throw new NotImplementedException();
    }

}
