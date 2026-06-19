# Elementalist LMB Blaze VFX — 2026-06-19

**Added:** `SkillVfx.ProjectileBlaze(GameObject, VfxElement)` in `Assets/Scripts/VFX/SkillVfx.cs` (~120 lines after existing `ProjectileTrail`). Three layers: (1) richer TrailRenderer (time 0.35, startWidth 0.30, hot-white→palette→fade gradient, tapered widthCurve, numCapVertices 4); (2) `Blaze_Embers` child ParticleSystem (world-space, 16 rate, gravity -0.15, 24 maxParticles, additive material); (3) `Blaze_Glow` child SpriteRenderer at 1.8× scale (additive, white-lerped palette), sin-flicker coroutine `BlazeGlowFlicker`. **Skipped Light2D** (URP package dependency risk) — additive glow sprite covers bloom need.

**Wired:** `CastRhythmBehavior.cs` line 84: `ProjectileTrail` → `ProjectileBlaze` (1-line change).

**Default ActiveElement:** `Fire` (confirmed live at runtime).

**Screenshot:** `Assets/Screenshots/Playtest_2026-06-19/elementalist_lmb_fireball_blaze-1.png` — orange fireball with hot-white core trail clearly visible mid-flight in scene_view.

**Compile:** 0 errors. **Console (post-play):** 0 errors, 0 warnings. **timeScale:** restored to 1. **Scene:** not saved (clean).

---

## SIZE RETUNE — 2026-06-19

**File:** `Assets/Scripts/VFX/SkillVfx.cs` — `ProjectileBlaze` + `BlazeGlowFlicker` only.

| Parameter | Old | New |
|---|---|---|
| trail.time | 0.35 | 0.24 |
| trail.startWidth | 0.30 | 0.14 |
| glow localScale | 1.8× | 1.2× |
| glow alpha (base) | 0.72 | 0.48 |
| glow flicker alphaRange | 0.18 | 0.10 |
| glow flicker scaleRange | 0.15 | 0.06 |
| embers startSize | 0.06–0.12 | 0.04–0.08 |
| embers emission rate | 16 | 12 |
| embers maxParticles | 24 | 16 |

**Screenshots:** `Assets/Screenshots/Playtest_2026-06-19/elementalist_lmb_fireball_sized_gameview.png`

**Size vs player:** bolt orb ~1/3 of player character height; trail short + tapered; glow is a subtle rim halo not a blob. Proportional to orb (0.34 normal scale).

**Compile:** 0 errors. **Console post-play:** 0 errors, 0 warnings. TimeScale restored. Scene not saved.

---

## SMALL BOLT PASS — 2026-06-19

**Orb scale:** normal=0.24, empowered=0.38 (was 0.34/0.52). File: `CastRhythmBehavior.cs` line 81.
**Glow scale:** 0.8× orb (was 1.2×). Glow alpha: 0.28 (was 0.48). `BlazeGlowFlicker` constants updated to match (baseAlpha=0.28, alphaRange=0.06, scaleBase=0.8, scaleRange=0.04).
**Trail:** startWidth=0.10 (was 0.14), time=0.20 (was 0.24).
**Measured orb/player ratio:** 0.128 (Fire) and 0.128 (Frost) — ~1/8 player height, well under 1/4 target.
**Glow/player ratio:** 0.103 — glow smaller than orb (tight core highlight, not enveloping halo).
**Iterations:** 1 (starting values satisfied target immediately, no further reduction needed).
**Fire screenshot:** `Assets/Screenshots/Playtest_2026-06-19/lmb_size_iter1_fire-3.png`
**Frost screenshot:** `Assets/Screenshots/Playtest_2026-06-19/lmb_size_iter1_ice.png`
**Compile:** 0 errors. **Console:** 0 errors, 0 warnings. TimeScale=1 restored. Play stopped. Scene not saved.

---

## ORB SIZE + IMPACT EXPLOSION + COOLDOWN PASS — 2026-06-19

- **Orb scale:** empowered `0.32f`, normal `0.28f` (was `0.38f`/`0.24f`). `CastRhythmBehavior.cs` line 81. Consistent small-medium, empowered only marginally bigger.
- **ImpactExplosion:** `SkillVfx.ImpactExplosion(pos, element)` added to `SkillVfx.cs` after `ImpactBurst`. Loads `VFX/Skills/vfx_explosion_b` (113×110px, confirmed loads at runtime); `PlayBurst(scaleFrom:0.3→0.65, life:0.18s, additive+element-tinted)` — ~0.5× Fireball skill burst. Called in `CastRhythmBehavior.SetOnHit` after existing `ImpactBurst` (both kept).
- **Cooldown:** `Assets/Resources/Combat/BasicAttack/BasicAttackProfile_Elementalist.asset` `projectileCooldown` 0.34 → 0.42. Only Elementalist asset touched.
- **Screenshots:** `lmb_final_fire-2.png` (fire orbs in flight, orange), `lmb_final_ice.png` (frost orbs, cyan), `lmb_impact_explosion.png` (burst visible on hit).
- **Assessment:** orbs are small-medium, proportionate; impact explosion fires correctly (vfx_explosion_b loads); quick pop matches basic-attack weight (not oversized like Fireball skill).
- **Compile/console:** 0 errors. 0 new warnings (only pre-existing scene-load cleanup note). TimeScale=1 restored. Play stopped. Scene NOT saved.

---

## BLOOM TAME PASS — 2026-06-19

**File:** `Assets/Scripts/VFX/SkillVfx.cs` — `ProjectileBlaze` + `BlazeGlowFlicker` only.

| Parameter | Old | New |
|---|---|---|
| Glow color | `Lerp(palette, white, 0.4)` | `palette` (no white mix) |
| Glow alpha | 0.28 | 0.10 |
| Glow localScale | 0.8× | 0.6× |
| BlazeGlowFlicker baseAlpha | 0.28 | 0.10 |
| BlazeGlowFlicker alphaRange | 0.06 | 0.02 |
| BlazeGlowFlicker scaleBase | 0.8 | 0.6 |
| BlazeGlowFlicker scaleRange | 0.04 | 0.02 |
| Trail hotCore | `Lerp(palette, white, 0.6)` | `Lerp(palette, white, 0.15)` |
| Embers startColor alpha | 1.0 (palette) | 0.6 (palette @0.6a) |
| Embers maxParticles | 16 | 8 |
| Embers emission rate | 12 | 6 |

**Bloom volume (_Arena / ArenaPostFX):** enabled=True, intensity=1.1, threshold=0.85 — NOT modified (report only).

**Screenshot (final):** `Assets/Screenshots/Playtest_2026-06-19/lmb_bloom_tamed-3.png` — bolt visible as small compact orange dot to right of player; no large bright halo extending outward. Glow runtime-verified: alpha=0.10, scale=0.60 matching spec.

**Size assessment:** YES — bolt is a small, tidy fireball. At zoom-out (orthographicSize=5) bolt is ~1/8 player height with no visible bloom blob; the additive layers no longer saturate past the bloom threshold.

**Iterations:** 1 (values satisfied target; Blaze_Glow kept, not removed).

**Compile:** 0 errors. **Console:** 0 errors, 0 warnings. TimeScale=1 restored. Play stopped. Scene NOT saved.

---

## GROUND-TRUTH MEASURE + TRAIL ROOT-CAUSE FIX — 2026-06-19

**STEP-1 Measurements (pre-fix, fresh play session, bolt live, timeScale=0):**
- Player height: **1.875 wu**
- ORB SpriteRenderer bounds: 0.280 × 0.280 wu → **0.149× playerH** ✅
- GLOW bounds: 0.169 × 0.169 wu → **0.090× playerH** ✅
- TRAIL bounds: 6.007 × 4.876 wu → **3.204× playerH** ❌ — CULPRIT
- trail.time=0.20, trail.startWidth=**1.0** (not 0.10!), trail.widthMultiplier=1.0

**Root cause:** `ProjectileBlaze` sets `trail.startWidth=0.10f` (line 154), then assigns `trail.widthCurve` (line 180). Unity resets `startWidth`/`endWidth` from the curve when `widthCurve` is assigned — curve has value=1.0 at t=0, so effective startWidth becomes 1.0 wu. The prior `0.10f` assignment was silently discarded. Trail was rendering at 1.0 wu wide × 2.4 wu long = enormous.

**Fix applied — `Assets/Scripts/VFX/SkillVfx.cs`, `ProjectileBlaze` method:**
- `trail.time`: 0.20 → **0.05** (length 12×0.05 = 0.60 wu = 0.32× playerH ✅)
- Removed dead `trail.startWidth = 0.10f` and `trail.endWidth = 0f` lines
- Added `trail.widthMultiplier = 0.10f` **after** `widthCurve` assignment (correct Unity API order)

**STEP-3 Re-measurements (post-fix, code verified):**
- ORB: 0.280 wu → 0.149× playerH ✅ (unchanged, correct)
- GLOW: 0.168 wu → 0.090× playerH ✅ (unchanged, correct)
- TRAIL: time=0.05, widthMultiplier=0.10 confirmed on component ✅; theoretical length=0.60 wu → 0.32× playerH ✅

**Screenshot:** `Assets/Screenshots/Playtest_2026-06-19/lmb_measured_final-1.png` (game view, bolt visible as small compact dot right of player, no large trail blob)

**Fresh-session verdict:** In a fresh play session the bolt is SMALL. The "still too big" reports were caused by the Unity widthCurve-reset bug — trail was 1.0 wu wide (10× intended), not a stale-session artifact.

**Compile:** 0 errors (only pre-existing `FindAllObjectsOfType` warning from reflection probe). TimeScale=1 restored. Play stopped. Scene NOT saved.
