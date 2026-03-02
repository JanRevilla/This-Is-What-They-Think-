Shader "HDRP/Particles/BloodEffectHDRP"
{
    Properties
    {
        [Header(Color Controls)]
        [HDR]_BaseColor("Base Color", Color) = (1,1,1,1)
        _AlphaMin("Alpha Min", Range(0,1)) = 0.1
        _AlphaSoft("Alpha Softness", Range(0.001,1)) = 0.05
        _EdgeDarken("Edge Darkening", Range(0,2)) = 1

        [Header(Mask)]
        _MainTex("Mask", 2D) = "white" {}
        _MaskStr("Mask Strength", Range(0,1)) = 1
        _Columns("Flipbook Columns", Float) = 1
        _Rows("Flipbook Rows", Float) = 1

        [Header(Noise)]
        _NoiseTex("Noise", 2D) = "white" {}
        _NoiseAlphaStr("Noise Strength", Range(0,1)) = 1

        [Header(Warp)]
        _WarpTex("Warp Texture", 2D) = "gray" {}
        _WarpStr("Warp Strength", Float) = 0.1

        [Header(Vertex Physics)]
        _FallOffset("Gravity Offset", Float) = -1
        _FallRandomness("Gravity Randomness", Float) = 0.25
    }

    HLSLINCLUDE
    #pragma target 4.5
    #pragma only_renderers d3d11 metal vulkan

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

    TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
    TEXTURE2D(_NoiseTex); SAMPLER(sampler_NoiseTex);
    TEXTURE2D(_WarpTex); SAMPLER(sampler_WarpTex);

    float4 _BaseColor;
    float _AlphaMin;
    float _AlphaSoft;
    float _EdgeDarken;
    float _MaskStr;
    float _Columns;
    float _Rows;
    float _NoiseAlphaStr;
    float _WarpStr;
    float _FallOffset;
    float _FallRandomness;

    struct Attributes
    {
        float3 positionOS : POSITION;
        float2 uv : TEXCOORD0;
        float4 color : COLOR;
        float4 custom : TEXCOORD1; // x,y custom, z random, w lifetime
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 uv : TEXCOORD0;
        float2 uvFlipbook : TEXCOORD1;
        float4 color : COLOR;
        float random : TEXCOORD2;
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;

        float lifetime = input.custom.w;
        lifetime = lifetime * lifetime +
            (_FallOffset + ((input.custom.z - 0.5) * _FallRandomness)) * lifetime;

        float3 worldPos = TransformObjectToWorld(input.positionOS);
        worldPos.y += lifetime * input.custom.y;

        output.positionCS = TransformWorldToHClip(worldPos);

        output.uv = input.uv;

        float2 flipbookUV = input.uv;
        flipbookUV *= float2(_Columns, _Rows);
        flipbookUV += input.custom.z * float2(3,8);
        output.uvFlipbook = flipbookUV;

        output.color = input.color;
        output.random = input.custom.z;

        return output;
    }

    float4 Frag(Varyings input) : SV_Target
    {
        // Warp
        float2 warpSample = SAMPLE_TEXTURE2D(_WarpTex, sampler_WarpTex, input.uvFlipbook).rg;
        float2 warp = (warpSample * 2 - 1) * _WarpStr;

        // Mask
        float4 mask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + warp);
        mask = lerp(1, mask, _MaskStr);

        // Noise
        float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, input.uvFlipbook + warp).r;
        noise = lerp(1, noise, _NoiseAlphaStr);

        float alpha = mask.r * noise * input.color.a;

        // Soft clip
        alpha = saturate((alpha - _AlphaMin) / _AlphaSoft);

        // Edge darken
        float edge = 1 - alpha;
        edge = pow(edge, 2);
        float3 color = input.color.rgb * _BaseColor.rgb;
        color *= lerp(1, edge, _EdgeDarken);

        return float4(color, alpha * _BaseColor.a);
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline"="HDRenderPipeline" "Queue"="Transparent" }

        Pass
        {
            Name "Forward"
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            ENDHLSL
        }
    }
}