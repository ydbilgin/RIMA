Shader "RIMA/TerrainBlend"
{
    Properties
    {
        _SplatMap ("Splat Map", 2D) = "black" {}
        _TerrainTex0 ("Terrain 0", 2D) = "gray" {}
        _TerrainTex1 ("Terrain 1", 2D) = "gray" {}
        _TerrainTex2 ("Terrain 2", 2D) = "gray" {}
        _TerrainTex3 ("Terrain 3", 2D) = "gray" {}
        _NoiseTex ("Noise", 2D) = "gray" {}
        _TerrainTiling ("Terrain Tiling", Float) = 2.0
        _NoiseScale ("Noise Scale", Float) = 0.8
        _NoiseStrength ("Noise Strength", Range(0, 0.5)) = 0.12
        _BlendSharpness ("Blend Sharpness", Range(0.5, 10)) = 2.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" "RenderPipeline"="UniversalRenderPipeline" }

        ZWrite On
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_SplatMap);    SAMPLER(sampler_SplatMap);
            TEXTURE2D(_TerrainTex0); SAMPLER(sampler_TerrainTex0);
            TEXTURE2D(_TerrainTex1); SAMPLER(sampler_TerrainTex1);
            TEXTURE2D(_TerrainTex2); SAMPLER(sampler_TerrainTex2);
            TEXTURE2D(_TerrainTex3); SAMPLER(sampler_TerrainTex3);
            TEXTURE2D(_NoiseTex);    SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _SplatMap_ST;
                float _TerrainTiling;
                float _NoiseScale;
                float _NoiseStrength;
                float _BlendSharpness;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 worldUV : TEXCOORD1;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldUV = worldPos.xy;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample splat map (bilinear filtered = automatic smooth blend)
                half4 splat = SAMPLE_TEXTURE2D(_SplatMap, sampler_SplatMap, IN.uv);

                // Noise perturbation for organic edges
                half noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, IN.worldUV * _NoiseScale).r;
                half noiseDelta = (noise - 0.5) * _NoiseStrength;

                splat.r = saturate(splat.r + noiseDelta);
                splat.g = saturate(splat.g - noiseDelta * 0.8);
                splat.b = saturate(splat.b + noiseDelta * 0.5);
                splat.a = saturate(splat.a - noiseDelta * 0.3);

                // Sharpness: pow then normalize
                splat = pow(max(splat, 0.0001), _BlendSharpness);
                half total = splat.r + splat.g + splat.b + splat.a;
                total = max(total, 0.001);
                splat /= total;

                // Sample tiling terrain textures
                float2 tUV = IN.worldUV * _TerrainTiling;
                half4 c0 = SAMPLE_TEXTURE2D(_TerrainTex0, sampler_TerrainTex0, tUV);
                half4 c1 = SAMPLE_TEXTURE2D(_TerrainTex1, sampler_TerrainTex1, tUV);
                half4 c2 = SAMPLE_TEXTURE2D(_TerrainTex2, sampler_TerrainTex2, tUV);
                half4 c3 = SAMPLE_TEXTURE2D(_TerrainTex3, sampler_TerrainTex3, tUV);

                half4 result = c0 * splat.r + c1 * splat.g + c2 * splat.b + c3 * splat.a;
                result.a = 1.0;
                return result;
            }
            ENDHLSL
        }
    }
}
