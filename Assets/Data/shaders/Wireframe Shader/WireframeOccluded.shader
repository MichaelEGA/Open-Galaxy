Shader "Custom/WireframeOccluded"
{
    Properties
    {
        _Color ("Wire Color", Color) = (1, 1, 1, 1)
        _LineWidth ("Line Width", Range(0.001, 0.05)) = 0.01
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

        Pass
        {
            Name "Wireframe"
            Cull Back
            ZWrite On
            ZTest LEqual
            Blend Off

            CGPROGRAM
            #pragma target 5.0
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;
            float _LineWidth;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2g
            {
                float4 vertex : SV_POSITION;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float3 bary : TEXCOORD0;
            };

            v2g vert(appdata v)
            {
                v2g o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2g input[3], inout TriangleStream<g2f> stream)
            {
                float3 bary[3] = {
                    float3(1, 0, 0),
                    float3(0, 1, 0),
                    float3(0, 0, 1)
                };

                for (int i = 0; i < 3; i++)
                {
                    g2f o;
                    o.pos = input[i].vertex;
                    o.bary = bary[i];
                    stream.Append(o);
                }

                stream.RestartStrip();
            }

            fixed4 frag(g2f i) : SV_Target
            {
                // Distance to nearest triangle edge in barycentric space
                float edgeDist = min(i.bary.x, min(i.bary.y, i.bary.z));

                // 1 near edges, 0 in the middle of the triangle
                float edgeMask = 1.0 - smoothstep(_LineWidth * 0.25, _LineWidth * 1.5, edgeDist);

                // Keep only the wireframe and discard the rest
                clip(edgeMask - 0.01);

                return _Color;
            }
            ENDCG
        }
    }
}