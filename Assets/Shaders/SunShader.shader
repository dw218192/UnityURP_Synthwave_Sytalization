Shader "Custom/SunShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        Pass
        {
            Tags { "LightMode" = "_Outline" }
            Name "Outline"
            Cull Back

            HLSLPROGRAM

            #pragma vertex Outline_Vert;
            #pragma fragment Outlint_Frag;

            #include "Assets/Shaders/Includes/SunHLSL.hlsl"
            ENDHLSL
        }

    }
    FallBack "Diffuse"
}
