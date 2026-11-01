Shader "UI/InnerGlowMask"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _BaseColor ("Base UI Color", Color) = (1,1,1,1)
        [HDR] _GlowColor ("Glow Color (HDR)", Color) = (0,1,1,1)
        
        [KeywordEnum(Luminance_And_Alpha, Bounding_Box_UV)] _MaskMode("Glow Mask Mode", Float) = 0
        
        [Header(Text and Sprite Settings)]
        _GlowThreshold ("Alpha/Luma Threshold", Range(0, 1)) = 0.5
        _TextSoftness ("Text Edge Softness", Range(0.01, 0.5)) = 0.1
        
        [Header(Flat Box and Block Settings)]
        _GlowSize ("Box Glow Thickness", Range(0.01, 0.5)) = 0.1
        _BoxSoftness ("Box Edge Softness", Range(0.01, 1.0)) = 0.2

        // Required Stencil / UI Canvas properties
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #pragma shader_feature_local _MASKMODE_LUMINANCE_AND_ALPHA _MASKMODE_BOUNDING_BOX_UV

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 uv       : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 vertex   : SV_POSITION;
                float4 color    : COLOR;
                float2 uv       : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Texture2D _MainTex;
            SamplerState sampler_MainTex;

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _GlowColor;
                float _GlowThreshold;
                float _TextSoftness;
                float _GlowSize;
                float _BoxSoftness;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.worldPosition = input.vertex;
                output.vertex = TransformObjectToHClip(input.vertex.xyz);
                output.uv = input.uv;
                output.color = input.color;
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float4 texColor = _MainTex.Sample(sampler_MainTex, input.uv);
                float alpha = texColor.a * input.color.a;
                float coreMask = 0.0;
                float edgeMask = 0.0;

                #if defined(_MASKMODE_LUMINANCE_AND_ALPHA)
                    // TEXT MODE: Uses letter brightness and alpha fuzz data
                    float luminance = dot(texColor.rgb, float3(0.2126, 0.7152, 0.0722));
                    float blendSrc = max(luminance, texColor.a);
                    coreMask = smoothstep(_GlowThreshold - _TextSoftness, _GlowThreshold + _TextSoftness, blendSrc);
                    edgeMask = max(0.0, texColor.a - coreMask);
                #else
                    // BOX/BLOCK MODE: Uses mathematical layout bounds
                    float2 uvDist = min(input.uv, 1.0 - input.uv);
                    float edgeDist = min(uvDist.x, uvDist.y);
                    coreMask = smoothstep(_GlowSize, _GlowSize + (_GlowSize * _BoxSoftness), edgeDist);
                    edgeMask = 1.0 - coreMask;
                #endif

                // Mix structural core color with outer glowing elements
                float3 finalCore = texColor.rgb * _BaseColor.rgb * input.color.rgb * coreMask;
                float3 finalGlow = _GlowColor.rgb * edgeMask;

                #if !defined(_MASKMODE_LUMINANCE_AND_ALPHA)
                    finalGlow *= alpha;
                #endif

                float4 color = float4(finalCore + finalGlow, alpha);

                #if defined(UNITY_UI_ALPHACLIP)
                clip(color.a - 0.001);
                #endif

                return color;
            }
            ENDHLSL
        }
    }
}