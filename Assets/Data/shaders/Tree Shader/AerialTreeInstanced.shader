Shader "Custom/AerialTreeInstanced"
{
    Properties
    {
        _BaseMap ("Albedo (RGBA)", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        // Alpha-tested objects should write depth so background doesn't show through
        Tags { "RenderType"="TransparentCutout" "RenderPipeline"="UniversalPipeline" "Queue"="AlphaTest" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            // No blending, depth writes enabled
            ZWrite On
            // Optionally change culling: uncomment for double-sided leaves
            // Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:setup

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseColor;
            float _Cutoff;
            float _Smoothness;
            float _Metallic;

            // StructuredBuffer for procedural instancing transforms
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                StructuredBuffer<float4x4> _TransformBuffer;
            #endif

            void setup()
            {
                #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                    float4x4 data = _TransformBuffer[unity_InstanceID];

                    // Override object/world transforms for this instance
                    unity_ObjectToWorld = data;

                    // Quick inverse/transpose for world->object (rotation + translation)
                    float3x3 w2oRotation = transpose((float3x3)data);
                    unity_WorldToObject._m00_m01_m02 = w2oRotation._m00_m01_m02;
                    unity_WorldToObject._m10_m11_m12 = w2oRotation._m10_m11_m12;
                    unity_WorldToObject._m20_m21_m22 = w2oRotation._m20_m21_m22;
                    unity_WorldToObject._m03_m13_m23 = -mul(w2oRotation, data._m03_m13_m23);
                    unity_WorldToObject._m30_m31_m32_m33 = float4(0, 0, 0, 1);
                #endif
            }

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                setup();

                float3 worldPos = mul(unity_ObjectToWorld, float4(input.positionOS.xyz, 1.0)).xyz;
                output.positionCS = mul(UNITY_MATRIX_VP, float4(worldPos, 1.0));
                output.uv = input.uv;

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                float4 color = tex * _BaseColor;

                // Alpha cutout: discard fragments below cutoff so depth gets written / tested
                clip(color.a - _Cutoff);

                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Simple Lit"
}