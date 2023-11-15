using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

class PostProcessSunPass : ScriptableRenderPass
{
    private CameraData m_CameraData;
    private Material sunMaterial;
    private RTHandle sunTexture;


    public PostProcessSunPass(Material sunMat)
    {
        sunMaterial = sunMat;
        sunTexture = RTHandles.Alloc(sunTexture, "_sunTexture");
        renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    // This method is called before executing the render pass.
    // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
    // When empty this render pass will render to the active camera render target.
    // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
    // The render pipeline will ensure target setup and clearing happens in a performant manner.
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        ConfigureTarget(sunTexture);
        ConfigureClear(ClearFlag.All, Color.clear);
    }

    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("PostProcess Sun Effects")))
        {
            // Draw a full-screen quad with the sun material
            Blit(cmd, BuiltinRenderTextureType.None, sunTexture, sunMaterial);

        }

        // Execute the command buffer and release it.
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    // Cleanup any allocated resources that were created during the execution of this render pass.
    public override void OnCameraCleanup(CommandBuffer cmd)
    {

    }
}
