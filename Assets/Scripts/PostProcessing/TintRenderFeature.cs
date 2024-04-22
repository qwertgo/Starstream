using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class TintRenderFeature : ScriptableRendererFeature
{
    private TintPass tintPass;

    public override void Create()
    {
        tintPass = new TintPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(tintPass);
    }

    class TintPass : ScriptableRenderPass
    {

        private Material mat;
        private int tintId = Shader.PropertyToID("_Temp"); 
        private RenderTargetIdentifier src, tint;

        public TintPass()
        {
            if (!mat)
            {
                mat = CoreUtils.CreateEngineMaterial("CustomPost/ScreenTint");
            }

            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            
            src = renderingData.cameraData.renderer.cameraColorTarget;

            cmd.GetTemporaryRT(tintId, desc, FilterMode.Bilinear);
            tint = new RenderTargetIdentifier();
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("TintRenderFeature");
            VolumeStack volumes = VolumeManager.instance.stack;
            CustomPostScreenTint customTint = volumes.GetComponent<CustomPostScreenTint>();

            if (customTint.IsActive())
            {
                mat.SetColor("_OverlayColor", (Color)customTint.color);
                mat.SetFloat("_Intensity", (float)customTint.tintIntesity);

                Blit(commandBuffer, src, tint, mat, 0);
                Blit(commandBuffer, tint, src);
            }

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tintId);
        }
    }
}
