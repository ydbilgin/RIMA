Shader "RIMA/SeloutOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineStrength ("Outline Strength", Range(0.3, 1.0)) = 0.7
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalRenderPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _MainTex_TexelSize;
                half4 _Color;
                half _OutlineStrength;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color * _Color;
                if (col.a < 0.01)
                {
                    return col;
                }

                bool isPureBlack = col.r < 0.04 && col.g < 0.04 && col.b < 0.04;
                if (!isPureBlack)
                {
                    return col;
                }

                float2 texelSize = _MainTex_TexelSize.xy;
                half4 best = half4(0, 0, 0, 0);
                float2 offsets[8] =
                {
                    float2(-1, 0), float2(1, 0), float2(0, -1), float2(0, 1),
                    float2(-1, -1), float2(1, -1), float2(-1, 1), float2(1, 1)
                };

                for (int i = 0; i < 8; i++)
                {
                    half4 neighbor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + offsets[i] * texelSize);
                    bool neighborNonBlack = neighbor.r > 0.04 || neighbor.g > 0.04 || neighbor.b > 0.04;
                    if (neighbor.a > 0.5 && neighborNonBlack)
                    {
                        best = neighbor;
                        break;
                    }
                }

                if (best.a < 0.5)
                {
                    return col;
                }

                half4 selout = best;
                selout.rgb *= _OutlineStrength;
                selout.a = col.a;
                return selout;
            }
            ENDHLSL
        }
    }
}
