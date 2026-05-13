# Selout URP 2D Shader — ScriptableRendererFeature — execute every step, commit at end

## Context

Animation Spec LOCKED §5 (STAGING/animation_system_spec_LOCKED.md): URP 2D selout outline shader replaces manual Aseprite sprite-baked approach. Saves ~13h Aseprite work, covers all sprites automatically, zero per-sprite cost. One-time setup.

**Lock decision:** L1 in animation_system_spec_LOCKED.md. No per-sprite manual pass needed.

## Implementation

### STEP 1 — Selout Shader

Create `Assets/Shaders/SeloutOutline.shader`:

Technique: 1px outline pass — for each edge pixel (adjacent to transparent pixel), replace pure black (#000000) with darkened version of the nearest adjacent inner pixel color (~30% value darkening, HSV V * 0.7).

URP 2D compatible shader (SpriteRenderer material override).

```hlsl
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

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; float4 color : COLOR; };
            struct Varyings { float4 positionHCS : SV_POSITION; float2 uv : TEXCOORD0; float4 color : COLOR; };

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
                if (col.a < 0.01) return col;

                // Check if this pixel is a pure black selout candidate
                bool isPureBlack = col.r < 0.04 && col.g < 0.04 && col.b < 0.04;
                if (!isPureBlack) return col;

                // Check 8-neighborhood for non-black, non-transparent pixels
                float2 texelSize = _MainTex_TexelSize.xy;
                half4 best = half4(0,0,0,0);
                float2 offsets[8] = {
                    float2(-1,0), float2(1,0), float2(0,-1), float2(0,1),
                    float2(-1,-1), float2(1,-1), float2(-1,1), float2(1,1)
                };
                for (int i = 0; i < 8; i++)
                {
                    half4 neighbor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + offsets[i] * texelSize);
                    bool neighborNonBlack = neighbor.r > 0.04 || neighbor.g > 0.04 || neighbor.b > 0.04;
                    if (neighbor.a > 0.5 && neighborNonBlack) { best = neighbor; break; }
                }

                if (best.a < 0.5) return col;

                // Darken neighbor color for selout
                half4 selout = best;
                selout.rgb *= _OutlineStrength;
                selout.a = col.a;
                return selout;
            }
            ENDHLSL
        }
    }
}
```

### STEP 2 — Material

Create `Assets/Art/Materials/SeloutSprite.mat`:
- Shader: RIMA/SeloutOutline
- OutlineStrength: 0.7

### STEP 3 — Apply to Warblade sprites

In `Assets/Scripts/Core/` or a utility script, find all SpriteRenderer GameObjects tagged "Player" or "Mob" and assign the SeloutSprite material. OR: leave this as a manual step in scene setup (Inspector assign) since animations import per-clip without runtime material override.

Actually: create `Assets/Editor/Tools/ApplySeloutMaterial.cs` — menu `RIMA > Tools > Apply Selout to All Characters` — finds all SpriteRenderers in Resources/Characters/** and assigns SeloutSprite.mat.

### STEP 4 — Compile check

`read_console` — 0 errors. Fix any shader compilation issues.

### STEP 5 — Test

Open RoomPipelineTest scene. Place a Warblade character SpriteRenderer, assign SeloutSprite.mat, check in Game view: pure black outline pixels should appear darkened from neighbor color, not solid black.

### STEP 6 — Commit

```bash
git add Assets/Shaders/SeloutOutline.shader Assets/Art/Materials/SeloutSprite.mat Assets/Editor/Tools/ApplySeloutMaterial.cs
git commit -m "[anim-spec] SeloutOutline URP shader + material + apply tool

- SeloutOutline.shader: 8-neighbor selout, pure black → darkened inner color
- SeloutSprite.mat: OutlineStrength 0.7
- ApplySeloutMaterial.cs: RIMA > Tools > Apply Selout to All Characters
- Replaces manual Aseprite sprite-baked approach (animation_system_spec_LOCKED.md L1)"
```

### STEP 7 — Report

Write `STAGING/selout_shader_report.md`:
```
# Selout URP Shader Report

## Shader
[compiled Y/N, shader path]

## Material
[created Y/N]

## Apply Tool
[menu item registered Y/N]

## Visual test
[pure black → selout darkened Y/N, or pending manual test]

## Console
[0 errors Y/N]
```

Append `CODEX_DONE_yasinderyabilgin.md`:
```
## [2026-05-14] Selout URP Shader
- Shader compiled: Y/N
- Material: Y/N
- Visual test: Y/N
- Commit: [hash]
```

## Constraints

- URP 17.3.0 uyumlu (manifest.json confirms)
- Shader: standard sprite Transparent queue, SrcAlpha blend
- DO NOT modify existing sprite assets — only material assignment
- OutlineStrength 0.7 is Karar #116(g) implication for 1px darker selout

## Source References

1. `Packages/manifest.json` — URP version confirm
2. `STAGING/animation_system_spec_LOCKED.md` §5 — selout locked spec
3. `Assets/Shaders/` — check for existing shader files first (don't duplicate)
