Shader "Custom/URP/Terrain Tile Blend"
{
    Properties
    {
        [Header(Current Tile)]
        _GroundMap ("Ground", 2D) = "white" {}
        _CliffMap ("Shared Cliff", 2D) = "gray" {}

        [Header(Neighbor Ground Textures)]
        _RightGroundMap ("Right Ground", 2D) = "white" {}
        _FrontGroundMap ("Front Ground", 2D) = "white" {}

        [Header(Procedural Border Blending)]
        _BorderBlend ("Border Blend Width", Range(0.001, 0.5)) = 0.12
        _BlendRoughness ("Blend Roughness", Range(0, 1)) = 0.35
        _BlendNoiseTiling ("Blend Noise Tiling", Float) = 3

        [Header(Cliff)]
        _CliffStart ("Cliff Slope Start", Range(0, 1)) = 0.65
        _CliffSoftness ("Cliff Slope Softness", Range(0.001, 1)) = 0.2

        [Header(Texture Settings)]
        _TextureTiling ("Texture Tiling", Float) = 1
        _Brightness ("Brightness", Range(0, 2)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardLit"

            Tags
            {
                "LightMode" = "UniversalForward"
            }

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_GroundMap);
            SAMPLER(sampler_GroundMap);

            TEXTURE2D(_CliffMap);
            SAMPLER(sampler_CliffMap);

            TEXTURE2D(_RightGroundMap);
            SAMPLER(sampler_RightGroundMap);

            TEXTURE2D(_FrontGroundMap);
            SAMPLER(sampler_FrontGroundMap);

            CBUFFER_START(UnityPerMaterial)

                half _BorderBlend;
                half _BlendRoughness;
                half _BlendNoiseTiling;

                half _CliffStart;
                half _CliffSoftness;

                half _TextureTiling;
                half _Brightness;

            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs positionInputs =
                    GetVertexPositionInputs(input.positionOS.xyz);

                VertexNormalInputs normalInputs =
                    GetVertexNormalInputs(input.normalOS);

                output.positionCS = positionInputs.positionCS;
                output.positionWS = positionInputs.positionWS;
                output.normalWS =
                    NormalizeNormalPerVertex(normalInputs.normalWS);

                output.uv = input.uv * _TextureTiling;

                return output;
            }

            half3 SampleGround(
                TEXTURE2D_PARAM(groundTexture, groundSampler),
                float2 uv)
            {
                return SAMPLE_TEXTURE2D(
                    groundTexture,
                    groundSampler,
                    uv
                ).rgb;
            }

            half3 SampleCurrentGround(float2 uv)
            {
                return SampleGround(
                    TEXTURE2D_ARGS(_GroundMap, sampler_GroundMap),
                    uv
                );
            }

            half3 SampleSharedCliff(float2 uv)
            {
                return SAMPLE_TEXTURE2D(
                    _CliffMap,
                    sampler_CliffMap,
                    uv
                ).rgb;
            }

            // Procedural hash function.
            float Hash21(float2 position)
            {
                position = frac(
                    position * float2(123.34, 456.21)
                );

                position += dot(
                    position,
                    position + 45.32
                );

                return frac(position.x * position.y);
            }

            // Smooth procedural value noise.
            float ValueNoise(float2 position)
            {
                float2 cell = floor(position);
                float2 local = frac(position);

                local = local * local * (3.0 - 2.0 * local);

                float bottomLeft =
                    Hash21(cell);

                float bottomRight =
                    Hash21(cell + float2(1.0, 0.0));

                float topLeft =
                    Hash21(cell + float2(0.0, 1.0));

                float topRight =
                    Hash21(cell + float2(1.0, 1.0));

                float bottom = lerp(
                    bottomLeft,
                    bottomRight,
                    local.x
                );

                float top = lerp(
                    topLeft,
                    topRight,
                    local.x
                );

                return lerp(bottom, top, local.y);
            }

            float GetBlendNoise(float3 positionWS)
            {
                float2 noisePosition =
                    positionWS.xz *
                    _BlendNoiseTiling *
                    0.01;

                // Convert noise from 0..1 to -1..1.
                return ValueNoise(noisePosition) * 2.0 - 1.0;
            }

            // Only blend to right (+X) and front (+Y) neighbors to avoid mutual blending seams.
            half3 BlendNeighborGround(
                float2 tileUV,
                float3 positionWS,
                half3 currentGround)
            {
                half noise = (half)GetBlendNoise(positionWS);

                // Irregularly offset the blend boundary.
                half edgeOffset =
                    noise *
                    _BlendRoughness *
                    _BorderBlend;

                // Do NOT blend with -X/-Y neighbors (left/back).
                half leftWeight = 0.0;
                half backWeight = 0.0;

                // Blend outward only in +X (right) and +Y (front).
                half rightWeight = smoothstep(
                    1.0 - _BorderBlend,
                    1.0,
                    tileUV.x + edgeOffset
                );

                half frontWeight = smoothstep(
                    1.0 - _BorderBlend,
                    1.0,
                    tileUV.y + edgeOffset
                );

                // Sample only the right and front neighbor textures.
                half3 rightGround = SampleGround(
                    TEXTURE2D_ARGS(
                        _RightGroundMap,
                        sampler_RightGroundMap
                    ),
                    float2(tileUV.x, tileUV.y)
                );

                half3 frontGround = SampleGround(
                    TEXTURE2D_ARGS(
                        _FrontGroundMap,
                        sampler_FrontGroundMap
                    ),
                    float2(tileUV.x, tileUV.y)
                );

                half3 color = currentGround;

                color = lerp(
                    color,
                    rightGround,
                    rightWeight
                );

                color = lerp(
                    color,
                    frontGround,
                    frontWeight
                );

                return color;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half3 normalWS = normalize(input.normalWS);

                // Horizontal surfaces have normal.y near 1.
                // Vertical surfaces have normal.y near 0.
                half cliffAmount = saturate(
                    (_CliffStart - normalWS.y) /
                    _CliffSoftness
                );

                // Coordinates within the current tile.
                float2 tileUV = frac(input.uv);

                // Blend only the ground textures.
                half3 groundColor =
                    SampleCurrentGround(input.uv);

                groundColor = BlendNeighborGround(
                    tileUV,
                    input.positionWS,
                    groundColor
                );

                // One shared cliff texture is used everywhere.
                half3 cliffColor =
                    SampleSharedCliff(input.uv);

                half3 color = lerp(
                    groundColor,
                    cliffColor,
                    cliffAmount
                );

                Light mainLight = GetMainLight();

                half diffuse = saturate(
                    dot(normalWS, mainLight.direction)
                );

                half lighting = 0.35 + diffuse * 0.65;

                color *= mainLight.color;
                color *= lighting;
                color *= _Brightness;

                return half4(color, 1.0);
            }

            ENDHLSL
        }
    }
}