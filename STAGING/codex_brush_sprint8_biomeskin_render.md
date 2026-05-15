# Codex Task — Sprint 8: BiomeSkin + Render Rules (Subtle Alpha, NO Blur)

**Type:** Implementation (V1 close — render preset system)
**Effort:** medium
**Estimated:** 1 day Codex
**Dispatch:** `python cx_dispatch.py --task-file STAGING/codex_brush_sprint8_biomeskin_render.md --effort high`

---

## 0. MUST READ FIRST

1. `STAGING/map_designer_unified_brush_design.md` (§9 BiomeSkin + §17 Soft Alpha Warning)
2. `STAGING/codex_safety_review_output.md` (Q5 Tilemap+SpriteRenderer + Q6 versioning)
3. `STAGING/codex_brush_sprint1_data_layer.md` (BiomeSkinSO + LayerRenderRule defined)
4. `STAGING/codex_brush_sprint5_editor_ui.md` (Skin dropdown in top bar)

---

## 1. Context

Sprint 8 closes V1 by making `BiomeSkinSO` functional. The user picks a skin from the top dropdown → entire active room re-renders in 1–2 seconds with new alpha modes, tints, materials, sorting orders per layer.

**Critical (ChatGPT §17 LOCK):** soft alpha = pixel cluster breakup + organic silhouette + decal overlap. NOT Gaussian blur. NOT bilinear filtering. `AlphaMode.SoftAlpha8` is the maximum allowed in V1.

**Four default skins:**
- **Hades-Net** — Hard pixel everywhere, sharp readable
- **Grimdark-Mix** (DEFAULT) — Hard L1/L2 + SoftAlpha8 L4 patches + Hard L5 + bold L6
- **Soft-Painter** — SoftAlpha8 L3+L4, SoftAlpha8 L5 (still no blur, just edge fade)
- **Bold-Graphic** — Hard tile + thick outline shader on L3 + dense L5 uniform density

---

## 2. Scope — Files to Create

### 2.1 Editor asmdef files

#### 2.1.1 `Assets/Scripts/MapDesigner/Brush/Render/Editor/BiomeSkinApplier.cs`

```csharp
public static class BiomeSkinApplier {
    private static BiomeSkinSO lastApplied;
    private static readonly Dictionary<BiomeSkinSO, RenderCache> cache = new();

    public static void Apply(BiomeSkinSO skin, RoomData room) {
        if (skin == null) return;

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName($"Apply Skin: {skin.skinName}");

        foreach (var rule in skin.layerRenderRules) {
            ApplyRuleToLayer(rule, skin);
        }

        Undo.CollapseUndoOperations(group);
        lastApplied = skin;
    }

    private static void ApplyRuleToLayer(LayerRenderRule rule, BiomeSkinSO skin) {
        var container = GameObject.Find($"Layer_{rule.layer}");
        if (container == null) return;

        foreach (var renderer in container.GetComponentsInChildren<SpriteRenderer>()) {
            Undo.RecordObject(renderer, "Apply Skin Renderer");
            renderer.color = rule.tint * skin.globalTint;
            renderer.sortingOrder = rule.sortingOrder;

            if (rule.overrideMaterial != null) {
                renderer.sharedMaterial = rule.overrideMaterial;
            } else {
                renderer.sharedMaterial = ResolveDefaultMaterialForAlphaMode(rule.alphaMode);
            }
        }

        // Tilemap-level: for L1/L2 only
        if (rule.layer == TargetLayer.L1 || rule.layer == TargetLayer.L2) {
            var tilemap = container.GetComponent<Tilemap>();
            if (tilemap != null) {
                Undo.RecordObject(tilemap, "Apply Skin Tilemap");
                tilemap.color = rule.tint * skin.globalTint;
            }
        }
    }

    private static Material ResolveDefaultMaterialForAlphaMode(AlphaMode mode) {
        // Look up or cache materials. Avoid creating new instances per call.
        return mode switch {
            AlphaMode.Hard => MaterialCache.HardDefault,
            AlphaMode.SoftAlpha8 => MaterialCache.SoftAlpha8,
            AlphaMode.SoftAlpha16 => MaterialCache.SoftAlpha16,
            AlphaMode.MultiplyBlend => MaterialCache.MultiplyBlend,
            _ => MaterialCache.HardDefault
        };
    }
}

internal static class MaterialCache {
    public static readonly Material HardDefault = LoadOrCreate("Assets/Art/Materials/Sprite_HardDefault.mat");
    public static readonly Material SoftAlpha8 = LoadOrCreate("Assets/Art/Materials/Sprite_SoftAlpha8.mat");
    public static readonly Material SoftAlpha16 = LoadOrCreate("Assets/Art/Materials/Sprite_SoftAlpha16.mat");
    public static readonly Material MultiplyBlend = LoadOrCreate("Assets/Art/Materials/Sprite_MultiplyBlend.mat");

    private static Material LoadOrCreate(string path) {
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null) {
            mat = new Material(Shader.Find("Sprites/Default"));
            AssetDatabase.CreateAsset(mat, path);
            AssetDatabase.SaveAssets();
        }
        return mat;
    }
}
```

### 2.2 Shader (single-pass, NO blur)

#### 2.2.1 `Assets/Art/Shaders/Sprite_SoftAlpha.shader`

A simple single-pass shader that uses a UV-distance-to-edge calculation to apply pixel-honest alpha fade ONLY within 8 or 16 pixels of the sprite edge. Implementation note for Codex:

- Vertex/fragment shader
- Sample sprite texture as usual
- Compute distance from current pixel to nearest fully-transparent neighbor pixel
- If distance <= fadeRadius (8 or 16), multiply alpha by (distance/fadeRadius) with NEAREST-neighbor sampling preserved (no bilinear filter on the sprite itself — the shader does the fade in screen space)
- Crucially: shader inputs MUST keep sprite sampler at point filter mode — preserve pixel art

**Alternative simpler approach:** Don't compute distance per pixel. Instead, use a uniform `_FadeRadius` and a `_FadeMask` texture per sprite pre-baked at import time. Codex picks whichever is simpler with the asset pipeline.

**ChatGPT §17 warning:** if this shader produces visible blur on the QC eyeball test, revert to AlphaMode.Hard for that layer and document the regression.

### 2.3 Materials

Create 4 .mat files referencing the shader:
- `Assets/Art/Materials/Sprite_HardDefault.mat` — Sprites/Default shader, no fade
- `Assets/Art/Materials/Sprite_SoftAlpha8.mat` — Sprite_SoftAlpha shader, _FadeRadius=8
- `Assets/Art/Materials/Sprite_SoftAlpha16.mat` — Sprite_SoftAlpha shader, _FadeRadius=16
- `Assets/Art/Materials/Sprite_MultiplyBlend.mat` — Sprites/Default with blend mode multiply

### 2.4 Default BiomeSkin .asset files

`Assets/Data/Brush/Default/BiomeSkin_HadesNet.asset`
`Assets/Data/Brush/Default/BiomeSkin_GrimdarkMix.asset` (DEFAULT)
`Assets/Data/Brush/Default/BiomeSkin_SoftPainter.asset`
`Assets/Data/Brush/Default/BiomeSkin_BoldGraphic.asset`

Each skin has 6 `LayerRenderRule` entries (one per L1-L6) with:
- AlphaMode per design §9.2 table
- sortingOrder per layer band (L1=0, L2=10, L3=100, L4=200, L5=300, L6=400)
- tint adjustments per biome mood
- overrideMaterial = null for default; only Bold-Graphic L3 uses an outline material

### 2.5 UI hookup in MapDesignerBrushWindow

Add Skin dropdown change handler:
```csharp
private void OnSkinChanged(BiomeSkinSO newSkin) {
    activeSkin = newSkin;
    BiomeSkinApplier.Apply(newSkin, currentRoomData);
}
```

This is a minimal addition to the Sprint 5 window code.

### 2.6 EditMode tests

`BiomeSkinTests.cs` — minimum 5 cases:

1. **ApplySkin_ChangesRendererMaterials** — apply Soft-Painter → L4 renderers use SoftAlpha8 material
2. **ApplySkin_PreservesSpriteReferences** — sprite refs unchanged before/after skin swap
3. **ApplySkin_RespectsSortingOrders** — apply Grimdark-Mix → L4 sortingOrder == 200
4. **ApplySkin_UndoRevertsAll** — apply skin, undo → all renderers back to previous state
5. **MaterialCache_NoLeak** — apply same skin 10 times → only 4 unique Material assets exist (no per-call instantiation)

---

## 3. V1 EXCLUSIONS

- Pre-bake all skin variants on save (V2 if needed for performance)
- Multi-sample edge fade shader (V2)
- Bloom / particle / postprocess additions per skin (V2)
- BiomeSkin tied to RoomRecipe (V2 — for now skin is set via UI dropdown manually)
- Skin import/export to JSON (V2)
- Custom shader Graph (V2 — V1 ships hand-written shader)

---

## 4. Acceptance Criteria

A. dotnet build both asmdefs pass 0 errors.
B. 5 EditMode tests PASS.
C. 4 skin .asset files exist and openable in Inspector.
D. Apply skin → entire room renderers update without scene reload.
E. Undo reverts the entire skin application in one step.
F. NO Gaussian blur visible in any skin (ChatGPT §17 LOCK — manual QC by orchestrator after Codex done).
G. Sprite import filter modes remain `Point (no filter)` — skin does NOT modify import settings.
H. MaterialCache prevents leaks (TEST 5).
I. Sprint 1-7 tests still PASS.

---

## 5. Safety Rules

1. **Max 5 files per dispatch.** Sprint 8: 1 applier + 1 shader + 4 materials + 4 skins + 1 test = 11 .asset/files. Split:
   - Sub 1: Shader + 4 materials + applier + cache + test start (5 files counting shader as 1)
   - Sub 2: 4 skin .asset + test extension
2. **Shader simplicity:** prefer single-pass, no multi-sample blur. If multi-sample is needed for SoftAlpha16 quality, defer to V2.
3. **Materials are assets:** create via AssetDatabase, not new Material() at runtime.
4. **Undo discipline** — every renderer mutation wrapped in Undo.RecordObject before assignment.
5. **No painter modifications.**
6. **No commit.**

---

## 6. Codex Self-Review Checklist

1. Read all 4 MUST READ files?
2. Does the shader preserve point filter on sprite sampler (no blur on sprite texture)?
3. Are 4 default skin .asset files created with correct LayerRenderRules per design §9.2?
4. Does MaterialCache return cached instances (not new() per call)?
5. Is Undo.RecordObject called before every renderer mutation?
6. Is the Skin dropdown change handler in Sprint 5 window single-line addition?
7. Did both dotnet build pass?
8. Did 5 + all previous sprint tests pass?
9. Did manual QC eyeball show NO Gaussian blur on any skin variant?
10. Are sortingOrder bands correct (L1=0, L2=10, L3=100, L4=200, L5=300, L6=400)?

---

## 7. V1 CLOSE — Post-Sprint 8

After Sprint 8 PASS:
- Map Designer Brush Tool V1 is COMPLETE
- 8-12 default brushes functional
- 29 PixelLab sprites bound to AssetPools
- 4 BiomeSkins live
- Auto-Dress / Regenerate / Smart Fill operational
- ALL Karar #143-D/E/K rules enforced
- V1 close commit + tag `brush-tool-v1`

Orchestrator post-V1 actions:
1. Update CURRENT_STATUS.md V1 close
2. Update memory `project_brush_tool_v1.md` → "LIVE"
3. Commit + tag
4. Decide V2 dispatch (marketplace, biome brush, standalone migration) — defer to user approval

---

## 8. Dependencies

**Blocked by:** Sprints 1-7 complete.
**Blocks:** V1 close.

---

## 9. Research-Backed Shader (Gemini S85, production-ready)

Full research: `STAGING/research_hades_brushux_softalpha.md` (Q5 — Pixel art soft-alpha shader).

**Verdict:** the soft-edge shader described in §2.2.1 is now LOCKED to use **Screen-Space Ordered Dithering with Bayer Matrix** (no blur, no bilinear filter). This technique is used in *Return of the Obra Dinn* and *Super Mario Odyssey* — proven shipped-game pattern.

**Why this works for our LOCK constraints (ChatGPT §17):**
- NO Gaussian blur
- NO bilinear filtering on sprite (sampler stays at Point)
- Pixel-honest dithering (each output pixel either fully opaque or `clip()`'d — no semi-transparency)
- Runs in `AlphaTest` queue (not `Transparent`) → Z-buffer writes correct, no sort issues
- Screen-aligned 4x4 Bayer matrix → pattern stable at sub-pixel sprite movement

**Critical input requirement:** source PNGs MUST have semi-transparent (0.0-1.0 alpha gradient) baked in at edges. The shader does NOT create softness from hard-edge PNGs — it converts existing alpha gradients into honest dithered pixels. This matches our PixelLab batch (Sprint 3) which already produces soft-edged decals for L4/L5/L6.

### 9.1 Built-in RP shader (USE AS-IS for V1)

Save to `Assets/Art/Shaders/RIMA_DitheredSoftEdge.shader`:

```hlsl
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
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "PreviewType"="Plane" }
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
            float4 _Color;
            float _DitherSpread;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            static const float4x4 bayerMatrix = float4x4(
                 0.0000, 0.5000, 0.1250, 0.6250,
                 0.7500, 0.2500, 0.8750, 0.3750,
                 0.1875, 0.6875, 0.0625, 0.5625,
                 0.9375, 0.4375, 0.8125, 0.3125
            );

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv) * i.color;
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float2 screenPixelPos = screenUV * _ScreenParams.xy;
                uint x = (uint)fmod(screenPixelPos.x, 4);
                uint y = (uint)fmod(screenPixelPos.y, 4);
                float ditherThreshold = bayerMatrix[x][y];
                float clipVal = texColor.a - (ditherThreshold * _DitherSpread);
                clip(clipVal - 0.001);
                return texColor;
            }
            ENDCG
        }
    }
}
```

### 9.2 URP port (if URP 2D Renderer requires it)

RIMA uses URP 2D. The above shader is Built-in RP CGPROGRAM. If it doesn't render correctly in URP 2D, port required:

**Minimal port steps:**
1. Replace `CGPROGRAM`/`ENDCG` with `HLSLPROGRAM`/`ENDHLSL`
2. Replace `#include "UnityCG.cginc"` with:
   ```
   #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
   ```
3. Add `Tags { "RenderPipeline"="UniversalPipeline" }` to SubShader
4. Replace `UnityObjectToClipPos` with `TransformObjectToHClip`
5. Replace `_ScreenParams` with built-in `_ScreenParams` (still works in URP)
6. Keep the Bayer matrix and `clip()` logic identical

**Verification:** Place a known soft-edge sprite (e.g., a moss patch from PixelLab L4 batch) on a quad in Phase1 test scene; toggle between hard alpha material and dithered material — expect dithered to show scattered hard pixels at the edges where alpha gradient existed.

### 9.3 Three material instances Sprint 8 must create

`Assets/Art/Materials/Sprite_HardDefault.mat` — Sprites/Default shader (no dither)
`Assets/Art/Materials/Sprite_SoftAlpha8.mat` — `RIMA/DitheredSoftEdge` shader, `_DitherSpread = 0.5` (subtle)
`Assets/Art/Materials/Sprite_SoftAlpha16.mat` — `RIMA/DitheredSoftEdge` shader, `_DitherSpread = 1.0` (full)

V1 NO `MultiplyBlend` material — deferred to V2 (the design spec mentions it as enum value but per ChatGPT §17 conservatism, defer).

### 9.4 BiomeSkin AlphaMode mapping (revised)

| AlphaMode | Material | Visual Effect |
|---|---|---|
| Hard | Sprite_HardDefault | Sharp pixel edges (default) |
| SoftAlpha8 | Sprite_SoftAlpha8 | Light dither at edges (DitherSpread=0.5) |
| SoftAlpha16 | Sprite_SoftAlpha16 | Heavy dither at edges (DitherSpread=1.0) |
| MultiplyBlend | (defer to V2) | Not implemented in V1 |

### 9.5 Implementation note for Codex / Opus

The shader file is straightforward — copy as-is. The complexity is in the **material creation** + **BiomeSkin asset wiring** + **Apply pipeline** (§2.1.1). Verify visually after applying — eyeball test for "is this still pixel-honest" is the human gate (Opus orchestrator review). If the dither pattern looks like blur on any biome skin, reduce `_DitherSpread` or revert that layer to `Hard`.
