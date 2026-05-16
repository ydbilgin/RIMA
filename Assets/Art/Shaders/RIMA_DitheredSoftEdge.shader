Shader "RIMA/DitheredSoftEdge"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _DitherSpread ("Dither Spread", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
            "PreviewType"="Plane"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }
        Cull Off
        Lighting Off
        ZWrite On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _DitherSpread;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            // 4x4 Bayer Matrix (normalized 0.0 to 1.0)
            // Per Gemini research §Q5: pixel-honest dithering, no blur.
            // Used in Return of the Obra Dinn + Super Mario Odyssey.
            static const float bayer[16] = {
                 0.0000, 0.5000, 0.1250, 0.6250,
                 0.7500, 0.2500, 0.8750, 0.3750,
                 0.1875, 0.6875, 0.0625, 0.5625,
                 0.9375, 0.4375, 0.8125, 0.3125
            };

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv) * i.color;
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float2 screenPixelPos = screenUV * _ScreenParams.xy;
                int x = (int)fmod(screenPixelPos.x, 4);
                int y = (int)fmod(screenPixelPos.y, 4);
                float ditherThreshold = bayer[y * 4 + x];
                float clipVal = texColor.a - (ditherThreshold * _DitherSpread);
                clip(clipVal - 0.001);
                return texColor;
            }
            ENDCG
        }
    }
}
