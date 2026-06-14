# DONE — Blue/Void arc fixup (SkillVfx.ApplyAdditiveSprite element-agnostic)

Date: 2026-06-14
Verdict: DONE
Scope (surgical): ONLY `Assets/Scripts/VFX/SkillVfx.cs` — `ApplyAdditiveSprite`. No other VFX path touched.

## Problem
P5 fix had hardcoded ember-biased per-channel multipliers in `ApplyAdditiveSprite`:
`new Color(color.r * 1.6f, color.g * 1.15f, color.b * 0.6f, a)`.
`MeleeArc` is a SHARED helper. `MeleeChainBehavior.cs:99/:115` pass `VfxElement.Physical` (ember, correct),
but `GravityCleave.cs:39` passes `VfxElement.Void` (purple). The boost-R / cut-B bias reddened and
de-purpled the Void arc -> regression.

## Fix (element-agnostic, hue-preserving)
Replaced the per-channel hardcode with a uniform scale that brings the color's MAX channel up to a
constant peak while keeping channel RATIOS intact (hue preserved) and alpha untouched:

```csharp
private const float AdditiveTargetPeak = 1.5f;
float peak = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
float factor = peak > 0.0001f ? AdditiveTargetPeak / peak : 1f;
Color boosted = new Color(color.r * factor, color.g * factor, color.b * factor);
// per-renderer: sharedMaterial = additive; color = boosted with original alpha preserved
```

Additive-blend approach and crescent-sprite choice (approved earlier) UNCHANGED.

## Verification
- Compile: refresh+compile requested; `read_console` error / `error CS` filters = 0 entries. CONSOLE: 0 ERRORS.
- Data-proof (computed in-Editor against real `SkillVfx.Palette`, and read back off the LIVE spawned arc):
  - Physical base #E89020 = (0.910, 0.565, 0.125) -> boosted = **(1.500, 0.931, 0.207)** | R dominant=True, low B (B<0.4R)=True -> ember/amber-orange. Confirmed on live `MeleeArc_SkillVfx`: (1.500,0.931,0.207).
  - Void base #B36BFF = (0.702, 0.420, 1.000) -> boosted = **(1.053, 0.629, 1.500)** | B>R=True, B>G=True, NOT reddened (B>=R)=True -> purple/violet.
  - (For contrast: old formula on Void would give (1.123, 0.483, 0.600) -> R>B, reddened. Fixed.)
- Visual (SCENE_VIEW, real crescent sprite + real `SkillVfx_SharedAdditive` material over identical dark backdrop):
  - Physical ember: `STAGING/_process/2026-06/_shots/bluearc_fixup_PHYSICAL_ember.png` -> warm amber/gold crescent.
  - Void purple: `STAGING/_process/2026-06/_shots/bluearc_fixup_VOID_purple_v2.png` -> lavender/purple crescent, NOT reddish.
- Play STOPPED. Scene `_Arena` isDirty=false, rootCount=8 (unchanged); proof objects were runtime-only and destroyed. NOT saved.

## Notes / limitations
- Full game flow (MainMenu->CharacterSelect->spawn) does not auto-run from a direct Play of `_Arena`, so no
  PlayerAttack existed to trigger a real input-driven swing. Instead the EXACT patched code path was exercised
  directly: `SkillVfx.MeleeArc(..., Physical/Void)` -> `PlaySweep(additiveSprite:true)` -> `ApplyAdditiveSprite`,
  with the real crescent sprite and the real shared additive material. Both element callers route through this
  same single function, so the proof covers both real combat cases (Warblade swing + GravityCleave cast).
- One benign teardown log on play-stop ("Some objects were not cleaned up when closing the scene") — not an
  error, fires on normal play cycles (SkillVfxRunner/coroutine teardown), unrelated to this change.
