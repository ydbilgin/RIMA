Shader "RIMA/EnemyReadable"
{
    // Readability outline for colored enemy sprites. Unlike RIMA/SeloutOutline (which only
    // recolors PURE-BLACK edge pixels), this draws a real neighbor-sampled silhouette ring
    // around every opaque sprite pixel regardless of its color, plus a subtle body
    // brightness/saturation lift so dark mobs separate from the slate floor.
    //
    // Telegraph-compatible: exposes _OutlineColor / _OutlineAlpha / _OutlineThickness so
    // EnemyOutlinePulse can pulse a red windup ring through this same material via a
    // MaterialPropertyBlock (it overrides the resting outline only while pulsing).
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        // Resting silhouette outline (always on). Restrained warm light-gray by default.
        _RestOutlineColor ("Rest Outline Color", Color) = (0.92, 0.86, 0.72, 1)
        _RestOutlineAlpha ("Rest Outline Alpha", Range(0,1)) = 0.85
        _OutlineThickness ("Outline Thickness (texels)", Range(0,4)) = 1.4

        // Body readability lift.
        _Brightness ("Body Brightness", Range(1,2)) = 1.18
        _Saturation ("Body Saturation", Range(1,2)) = 1.12

        // Telegraph override (driven by EnemyOutlinePulse via MaterialPropertyBlock).
        // _OutlineAlpha=0 at rest => resting outline used; >0 => telegraph color blended in.
        _OutlineColor ("Telegraph Outline Color", Color) = (1, 0.08, 0.04, 1)
        _OutlineAlpha ("Telegraph Outline Alpha", Range(0,1)) = 0
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
                half4 _RestOutlineColor;
                half _RestOutlineAlpha;
                half _OutlineThickness;
                half _Brightness;
                half _Saturation;
                half4 _OutlineColor;
                half _OutlineAlpha;
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

            // Max alpha of the 8 neighbors at the given texel distance (silhouette test).
            half MaxNeighborAlpha(float2 uv, float2 step)
            {
                half a = 0;
                a = max(a, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-step.x, 0)).a);
                a = max(a, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2( step.x, 0)).a);
                a = max(a, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(0, -step.y)).a);
                a = max(a, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(0,  step.y)).a);
                a = max(a, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-step.x, -step.y)).a);
                a = max(a, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2( step.x, -step.y)).a);
                a = max(a, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-step.x,  step.y)).a);
                a = max(a, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2( step.x,  step.y)).a);
                return a;
            }

            half3 AdjustSaturation(half3 c, half sat)
            {
                half luma = dot(c, half3(0.299, 0.587, 0.114));
                return lerp(half3(luma, luma, luma), c, sat);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half4 vcol = IN.color * _Color;

                // Opaque body pixel: tint + readability lift.
                if (tex.a > 0.5)
                {
                    half3 rgb = tex.rgb * vcol.rgb;
                    rgb = AdjustSaturation(rgb, _Saturation);
                    rgb = saturate(rgb * _Brightness);
                    return half4(rgb, tex.a * vcol.a);
                }

                // Transparent pixel: draw outline if an opaque neighbor exists within thickness.
                float2 step = _MainTex_TexelSize.xy * _OutlineThickness;
                half nbr = MaxNeighborAlpha(IN.uv, step);
                if (nbr < 0.5)
                {
                    return half4(0, 0, 0, 0);
                }

                // Resting outline color, with telegraph color blended in by _OutlineAlpha.
                half3 outRgb = lerp(_RestOutlineColor.rgb, _OutlineColor.rgb, saturate(_OutlineAlpha));
                half restA = _RestOutlineColor.a * _RestOutlineAlpha;
                half outA = lerp(restA, 1.0, saturate(_OutlineAlpha));
                return half4(outRgb, outA * nbr * vcol.a);
            }
            ENDHLSL
        }
    }
}
