using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessSunFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Shader sunShader;
    [SerializeField]
    private Shader blendShader;

    private Material sunMaterial;
    private Material blendMaterial;

    private PostProcessSunPass m_customPass;
    private PostProcessBlendPass m_blendPass;

    public override void Create()
    {
        sunMaterial = CoreUtils.CreateEngineMaterial(sunShader);
        blendMaterial = CoreUtils.CreateEngineMaterial(blendShader);
        m_customPass = new PostProcessSunPass(sunMaterial);
        m_blendPass = new PostProcessBlendPass(blendMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType != CameraType.Game)
            return;
        renderer.EnqueuePass(m_customPass);
        renderer.EnqueuePass(m_blendPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            //m_customPass.ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color);
            m_customPass.ConfigureInput(ScriptableRenderPassInput.Depth);
            m_customPass.ConfigureInput(ScriptableRenderPassInput.Color);
           // m_customPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(sunMaterial);
        CoreUtils.Destroy(blendMaterial);
    }
}


