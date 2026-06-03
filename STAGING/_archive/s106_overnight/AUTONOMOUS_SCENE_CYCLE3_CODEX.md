# Scene Composition Cycle 3 (Polish) — Codex (xhigh)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Cycle 2 verdict = POLISH-CYCLE-3. Antigravity identified CRITICAL regression: lighting collapsed to pitch-black because Light2D Target Sorting Layers not configured. 3 focused polish items. Small targeted edits only.

## PRIMARY INPUT
- **`STAGING/s106_overnight/SCENE_V3_REVIEW_VERDICT_AGY.md`** — full verdict + scoring + top 3 polish list
- Cycle 2 state: PlayableArena.unity (4 braziers, central portal, 4 pillars, BG tinted, 59-cell arena, particles)

## CRITICAL FIX: Light2D Target Sorting Layers

In URP 2D Renderer, each `Light2D` component has a **Target Sorting Layers** field (default may be empty or only "Default"). If sprites are on `Floor`, `Ground`, `Characters` layers, those lights DON'T affect them unless explicitly included.

### Fix all Light2D components in scene:
- `Brazier_NW`, `Brazier_NE`, `Brazier_SW`, `Brazier_SE` (4 corner lights)
- `CentralPortal/Light2D` (central glow)
- Cyan rim lights S/E/W (3 cliff-edge lights)
- `Global Light 2D`

For each: set `m_TargetSortingLayers` to include ALL these layer IDs:
- Default (id=0)
- Ground (id=2024493761)
- Floor (id=1843609376)
- Decals (id=1200000001)
- Walls (id=593505845)
- Entities (id=1293760285)
- BackwallLandmark (id=657081444)
- Characters (id=1200000003)
- Props (id=1200000004)

(skip VFX 200, UI 1200000002 — they're effects/UI)

The Light2D component in URP has `m_TargetSortingLayers` as a `List<int>`. Set via SerializedObject + serializedProperty access. Example:
```csharp
var so = new SerializedObject(light2DComp);
var prop = so.FindProperty("m_TargetSortingLayers");
prop.ClearArray();
foreach (int id in layerIds) {
    prop.InsertArrayElementAtIndex(prop.arraySize);
    prop.GetArrayElementAtIndex(prop.arraySize - 1).intValue = id;
}
so.ApplyModifiedProperties();
```

## POLISH ITEM 1 — Brazier flame sprites
- Each Brazier_* GameObject currently has only the mounting apparatus sprite + Light2D
- Add a child GameObject "Flame" with:
  - SpriteRenderer using an existing flame/fire sprite (search `Assets/Sprites/AssetPackV3/` for `flame`, `fire`, `torch_flame`, `brazier_fire` keywords)
  - If no flame sprite found, use the existing PixelLab flame inventory or create a simple yellow→orange→red triangular sprite via runtime (procedural)
  - Position: slightly above the brazier mounting (offset y = +0.3)
  - sortingLayer "Floor", order 6 (just above mounting prop)
  - Optional: tint emissive orange-yellow

## POLISH ITEM 2 — Brighten BG layers + lightning
1. **Increase L1_Nebula tint brightness:**
   - Current: ~(0.45, 0.20, 0.65) (per Cycle 2)
   - New: (0.65, 0.30, 0.95) — brighter magenta-violet
2. **LightningStreaks particle system:**
   - Increase sorting order to render ABOVE L0_Void (above -800), e.g. order -350 on Ground
   - Set Renderer Material to a sprite/additive shader for HDR-like emissive
   - Increase emission rate from 2/s → 4-6/s
   - Make particles BRIGHTER: color (1.0, 0.5, 1.0, 1.0) magenta-pink, additive blend
   - Make trails: enable Trails module, gradient cyan-to-magenta, lifetime 0.3
3. **L0_Void slight brightness:**
   - Bump from very dark to (0.18, 0.12, 0.25) — readable void

## POLISH ITEM 3 — Scale + illuminate pillars
1. Find the 4 pillar GameObjects (named "Pillar_*" or similar)
2. Scale each by 1.5x: `transform.localScale = Vector3.one * 1.5f`
3. Position them at outer corners closer to screen edges (current position OK, may need ±0.5 unit outward)
4. Add a child Light2D to each:
   - Type: Point Light
   - Color: warm amber (1.0, 0.7, 0.3)
   - Intensity: 0.6
   - Falloff: 0.8
   - Radius: 1.2
   - sortingLayers: same as brazier (all gameplay layers)
   - Y offset: +1.5 (light catches top of pillar)

## TONE BALANCE POLISH (HIGH PRIORITY)
After above changes:
- Test result. If still too dark, raise Global Light 2D from 0.15 → **0.22** (small bump)
- If too bright, keep at 0.15

## PROCESS

1. Phase 0: Read agy verdict + open scene
2. Phase 1: Apply Light2D Target Sorting Layers fix (THE CRITICAL ONE — without this nothing else matters)
3. Phase 2: Add brazier flame sprites (if asset found)
4. Phase 3: Brighten L1_Nebula + lightning particles
5. Phase 4: Scale pillars 1.5x + add pillar lights
6. Phase 5: Tone balance check; adjust Global if needed
7. Phase 6: Save scene, screenshot, side-by-side, report

## DELIVERABLES
- Modified `Assets/Scenes/Test/PlayableArena.unity`
- `STAGING/s106_overnight/scene_v4_match_attempt.png` (1280×720)
- `STAGING/s106_overnight/scene_v4_vs_M3.png` (2560×720)
- `STAGING/s106_overnight/SCENE_V4_REPORT.md` (self-assessment + Light2D Target fix verification)
- Final `CODEX_DONE_<profile>.md` with `STATUS: DONE`

## CONSTRAINTS
- NO PixelLab API calls
- 0 error 0 warning required
- Single scene save at end
- Don't regress arena painting or cliff placement (Cycle 2 those were ★+½ improvements)

## TIME ESTIMATE
~30-50 min at xhigh.

After this DONE: Opus checks. If lighting works + visible improvements → likely SHIP-NOW. If still issues, micro Cycle 4 (small).
