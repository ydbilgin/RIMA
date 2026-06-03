Shader "RIMA/2D/CliffVoidFade"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _VoidColor("Void Color", Color) = (0.012, 0.014, 0.022, 1)
        _FadeStart("Fade Start", Range(0, 1)) = 0.0
        _FadeEnd("Fade End", Range(0, 1)) = 0.0
        _TopOpaqueBand("Top Opaque Band", Range(0, 1)) = 0.75
        _TopTint("Top Tint", Color) = (1, 1, 1, 1)
        _MidTint("Mid Tint", Color) = (0.96, 0.97, 1, 1)
        _BottomVoidColor("Bottom Void Color", Color) = (0.36, 0.22, 0.48, 1)
        _DarkenStrength("Darken Strength", Range(0, 1)) = 0.22
        _DesaturateStrength("Desaturate Strength", Range(0, 1)) = 0.08
        _AlphaFadeStart("Alpha Fade Start", Range(0, 1)) = 0.2
        _AlphaFadeEnd("Alpha Fade End", Range(0, 1)) = 0.02
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _NoiseScale("Noise Scale", Float) = 3.5
        _NoiseStrength("Noise Strength", Range(0, 1)) = 0.25
        _StrataCount("Strata Count", Range(1, 14)) = 7
        _StrataStrength("Strata Strength", Range(0, 1)) = 0.35
        _StrataJitter("Strata Jitter", Range(0, 1)) = 0.4
        _RockNoiseScale("Rock Noise Scale", Float) = 8
        _RockNoiseStrength("Rock Noise Strength", Range(0, 1)) = 0.08
        _LipLightStrength("Lip Light Strength", Range(0, 1)) = 0.5
        _LipLightBand("Lip Light Band", Range(0.01, 0.25)) = 0.08
        _LipLightColor("Lip Light Color", Color) = (0.72, 0.78, 0.88, 1)
        _AOStrength("AO Strength", Range(0, 1)) = 0.18
        _AOBand("AO Band", Range(0.01, 0.35)) = 0.12
        _RimShadowStrength("Rim Shadow Strength", Range(0, 1)) = 0.18
        [MaterialToggle] _ZWrite("ZWrite", Float) = 0

        [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
        [HideInInspector] PixelSnap("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            #pragma multi_compile_instancing

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half4 _RendererColor;
                float4 _MainTex_ST;
                float4 _NoiseTex_ST;
                half4 _VoidColor;
                half4 _TopTint;
                half4 _MidTint;
                half4 _BottomVoidColor;
                float _FadeStart;
                float _FadeEnd;
                float _TopOpaqueBand;
                float _DarkenStrength;
                float _DesaturateStrength;
                float _AlphaFadeStart;
                float _AlphaFadeEnd;
                float _NoiseScale;
                float _NoiseStrength;
                float _StrataCount;
                float _StrataStrength;
                float _StrataJitter;
                float _RockNoiseScale;
                float _RockNoiseStrength;
                float _LipLightStrength;
                float _LipLightBand;
                half4 _LipLightColor;
                float _AOStrength;
                float _AOBand;
                float _RimShadowStrength;
            CBUFFER_END

            half HashNoise(float2 value)
            {
                return frac(sin(dot(value, float2(12.9898, 78.233))) * 43758.5453);
            }

            half ValueNoise(float2 value)
            {
                float2 cell = floor(value);
                float2 local = frac(value);
                local = local * local * (3.0 - 2.0 * local);

                half a = HashNoise(cell);
                half b = HashNoise(cell + float2(1.0, 0.0));
                half c = HashNoise(cell + float2(0.0, 1.0));
                half d = HashNoise(cell + float2(1.0, 1.0));
                return lerp(lerp(a, b, local.x), lerp(c, d, local.x), local.y);
            }

            half RockFbm(float2 value)
            {
                half noise = ValueNoise(value) * 0.58;
                noise += ValueNoise(value * 2.07 + 17.31) * 0.28;
                noise += ValueNoise(value * 4.11 + 43.17) * 0.14;
                return noise;
            }

            half SmoothBand(float start, float end, float uvY)
            {
                half t = saturate((start - uvY) / max(start - end, 0.0001));
                return t * t * (3.0 - 2.0 * t);
            }

            Varyings Vert(Attributes input)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.positionCS = TransformObjectToHClip(input.positionOS);
                o.uv = TRANSFORM_TEX(input.uv, _MainTex);
                o.color = input.color * _Color * _RendererColor;
                return o;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 sampleUv = input.uv;
                sampleUv.y = lerp(0.22, 1.0, saturate(sampleUv.y));
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, sampleUv);
                half3 vertexShade = lerp(half3(1.0h, 1.0h, 1.0h), input.color.rgb, 0.18h);
                half4 color = half4(texColor.rgb * vertexShade, texColor.a * input.color.a);
                half depth = 1.0 - saturate(input.uv.y);
                half topToMid = smoothstep(0.0, 0.72, depth);
                half bottomBlend = smoothstep(0.76, 1.0, depth);
                half3 tint = lerp(_TopTint.rgb, _MidTint.rgb, topToMid);
                tint = lerp(tint, _BottomVoidColor.rgb, bottomBlend);

                half lipLight = (1.0 - smoothstep(0.0, max(_LipLightBand, 0.0001), depth)) * _LipLightStrength;
                half contactAO = (1.0 - smoothstep(max(_LipLightBand, 0.0001), max(_LipLightBand + _AOBand, 0.0002), depth)) *
                                 smoothstep(0.0, max(_LipLightBand, 0.0001), depth) * _AOStrength;
                half rimShadow = 1.0 - _RimShadowStrength * smoothstep(0.0, max(_TopOpaqueBand, 0.0001), depth);
                half luminance = dot(color.rgb, half3(0.2126, 0.7152, 0.0722));
                half desat = bottomBlend * _DesaturateStrength;
                half darken = 1.0h - bottomBlend * _DarkenStrength;
                half3 shaded = lerp(color.rgb, luminance.xxx, desat) * tint * darken * rimShadow;

                half rockNoise = RockFbm(input.uv * max(_RockNoiseScale, 0.001));
                half mottle = 1.0 + (rockNoise - 0.5) * 2.0 * _RockNoiseStrength;
                half strataWarp = (RockFbm(float2(input.uv.x * 1.35, depth * 0.55) + 9.73) - 0.5) * _StrataJitter;
                half strataCoord = (depth + strataWarp * 0.16) * max(_StrataCount, 1.0);
                half strataPhase = frac(strataCoord);
                half strataThickness = lerp(0.035, 0.18, RockFbm(float2(floor(strataCoord) * 0.37, 2.19)));
                half strataEdge = min(strataPhase, 1.0 - strataPhase);
                half strata = (1.0 - smoothstep(strataThickness, strataThickness + 0.14, strataEdge)) * _StrataStrength;
                shaded *= saturate(mottle - strata - contactAO);
                shaded = lerp(shaded, _LipLightColor.rgb, saturate(lipLight));

                half noise = RockFbm(input.uv * max(_NoiseScale, 0.001) + 31.7) - 0.5;
                half fadeStart = max(_AlphaFadeStart, _AlphaFadeEnd);
                half fadeEnd = min(_AlphaFadeEnd, _AlphaFadeStart);
                half alphaFade = SmoothBand(fadeStart + noise * _NoiseStrength, fadeEnd, input.uv.y);
                alphaFade *= bottomBlend;

                half oldFade = SmoothBand(_FadeStart, _FadeEnd, input.uv.y) * step(0.001, _FadeStart);
                half3 finalRgb = lerp(shaded, _VoidColor.rgb, oldFade * 0.12);
                finalRgb = lerp(finalRgb, _BottomVoidColor.rgb, alphaFade * 0.55);
                return half4(finalRgb, color.a * (1.0 - alphaFade));
            }
            ENDHLSL
        }
    }
}
