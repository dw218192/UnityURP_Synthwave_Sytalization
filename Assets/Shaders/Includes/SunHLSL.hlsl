#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


TEXTURE2D(_CameraColorTexture);
SAMPLER(sampler_CameraColorTexture);
TEXTURE2D(_SunTexture);
SAMPLER(sampler_SunTexture);
TEXTURE2D(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);
float _DepthThreshold;


TEXTURE2D(_sunTexture);
SAMPLER(sampler_sunTexture);

float4 sun_frag() : SV_Target
{

}

float4 blend_frag(v2f i) : SV_Target
{
    // Sample the depth
    float depth = _CameraDepthTexture.Sample(sampler_CameraDepthTexture, i.uv).r;
    depth = clamp(depth, 0.0, 1.0);

    // Determine whether to use the color from the sun texture or the camera color
    float4 sunColor = _SunTexture.Sample(sampler_SunTexture, i.uv);
    float4 cameraColor = _CameraDepthTexture(sampler_CameraDepthTexture, i.uv);
    float4 finalColor = lerp(cameraColor, sunColor, step(_DepthThreshold, depth));

    return finalColor;
}