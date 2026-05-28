---
status: DRAFT (Step 1/3) — Codex Step 2 review BEKLİYOR
faz: 1
tarih: 2026-05-14
ozet: "rima-design Opus draft animation spec; Codex review + rima-design synthesis sonrası LOCK"
source: rima-design Opus dispatch S70
---

# RIMA Animation System Spec — DRAFT (rima-design, S70, 2026-05-14)

**Status:** DRAFT for Codex Step 2 review. rima-design will synthesize and LOCK in Step 3.
**Authority:** rima-design final decision after Codex feasibility review.
**Scope hard cap:** Faz 1 = 25-day school deadline = Warblade primary + seam_crawler mob. Everything else deferred.

---

## Bölüm 1 — Animation State Set per Class

Production phasing aligns with school deadline reality: only Warblade and 1 mob are Faz 1. Other 9 classes deferred to Faz 2+.

**Faz 1 — Warblade only (S70 Hafta 1-3, 25-day window):**

| State | Karar ref | Frames | Cost bracket |
|---|---|---|---|
| Idle (base + ambient layer) | #109 | 8f base + 8f ambient | 2+2 gen/dir |
| Run | #42 + #46 | 8f | 2 gen/dir |
| 3-Beat Combo Beat 1 | #122 T1 + 3-Beat foundation | 8f | 2 gen/dir |
| 3-Beat Combo Beat 2 | #122 T1 | 10-12f (mid-combo polish) | 3 gen/dir |
| 3-Beat Combo Beat 3 (COMMIT-BEAT) | #122 T1 trigger | 10-12f (Echo trigger frame mandatory) | 3 gen/dir |
| Hurt | — | 8f | 2 gen/dir |
| Death | #120 split | 12f (split: pre-fall 6f + ground 6f) | 3 gen/dir (or split 5 gen/dir) |
| Dash | — | 6f | 2 gen/dir |
| **Faz 1 mob: Seam Crawler** | #74 elite skip — grunt 4-dir | Idle 8f + Burst Strike 8f + Submerge 6f | 2+2+2 gen/dir |

**Faz 2 — Elementalist + Shadowblade + Ranger (4 classes total active, Karar #114 mevcut Faz 1.0 scope):**
- Per class: same 8 states as Warblade
- Plus 4 skill animations per class (Q/E/R/F)

**Faz 3-5 — Remaining 6 classes:**
- Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer
- Full state set + skill set per class

**Universal rule for ALL phases:** Karar #114 8-direction direct generation (5 gen + 3 mirror REVOKED per S68 update). 8 directly generated frames per animation, no flip.

**Faz 1.5 polish layer (after Faz 1 functional):** Karar #109 Warblade ambient idle (greatsword shoulder-rest, left hand to belt). Generated in same batch as base idle.

---

## Bölüm 2 — Frame Count Spec (Video Action Items Locked)

Source: `STAGING/animation_video_analysis.md` (85-90% match with RIMA spec).

| Spec | Locked value | Karar align |
|---|---|---|
| FPS | 10-12 | #100, video confirm |
| Resolution | 64x64 chibi | #100 |
| Outlines | 1px selout (darker local color, NOT pure black) | NEW lock — video reference |
| Idle | 4-6f breathing + weapon bob (Ambient 8f) | #109 |
| Run | 6-8f Brian's Extreme Pose (chibi-adapted lean) | #42 |
| Attack Beat 1 | 8f basic, anticipation 1f + smear 1-2f + recovery | NEW smear lock |
| Attack Beat 2 | 10-12f mid combo, mandatory smear frame | NEW |
| Attack Beat 3 (Commit) | 10-12f with **explicit Karar #122 T1 marker on impact frame** | #122 T1 |
| Hurt | 3-4f (knockback recoil + brief hold) | Video confirm |
| Death | 12-16f Karar #120 split (apex = ground plant) | #120 |
| Dash | 4-6f (anticipation 1f + thrust 2f + recovery 1-2f) | NEW |
| Smear frame | Mandatory in attack Beat 2/3, 1-2f stretch arc | NEW — video lock |
| Dust puff VFX | 3-4f at footfall/pivot/landing, separate sprite (animate_object) | NEW |

**Smear technique:** PixelLab Custom V3 prompt explicit: "one distorted stretch frame at peak weapon velocity, blade morphed into stylized smear arc." If PixelLab cannot produce native smear (likely), Aseprite post-process step: duplicate impact frame, manually stretch weapon shape on duplicate. ~5-10 min/attack anim.

---

## Bölüm 3 — Karar #123 Yol A Weapon Attach Timing

**Faz 1 MVP (Level 1 OrbitAttach):**
- Single HandAnchor empty transform on Warblade prefab
- Weapon GameObject parented to HandAnchor
- HandAnchor.localPosition static per character, NOT per-frame
- Weapon visually orbits with body — chibi scale (64×64) makes positioning forgiving
- Setup cost: 6-8h Codex (WeaponDatabase ScriptableObject + spawn logic + HandAnchor prefab system)

**Faz 2 polish (Level 2 per-frame anchor):**
- Unity AnimationCurve on HandAnchor.localPosition per animation clip
- 8 directions × 8-12 frames × Vector2 = ~128 keyframes per anim
- Per-class anim batch: 9 anim × 128 keyframes = ~1.1K keyframes per class
- Memory cost trivial (~1 KB per anim, ~90 KB total all 10 classes)
- Data entry cost: ~10 min/anim manual + Aseprite hand-position pixel mark export to JSON

**Phantom Echo exception (Karar #122 T1 spawn):**
- Phantom is 0.4s brief instance — weapon-baked sprite OK (drift impossible at this duration)
- Phantom does NOT use OrbitAttach system — separate prefab with sprite-baked weapon
- This is the ONLY case where weapon stays baked to body sprite

**Hand anchor JSON schema proposal (Faz 2):**
```json
{
  "anim_name": "warblade_beat3_south",
  "direction": "S",
  "frames": [
    {"frame": 0, "anchor_x": 16, "anchor_y": -4, "rotation": 0},
    {"frame": 1, "anchor_x": 18, "anchor_y": -2, "rotation": 15}
  ]
}
```
Codex generates Aseprite Lua export script (rima-codex Faz 2 dispatch).

---

## Bölüm 4 — Karar #122 Echo Resonance T1 Integration

**T1 Commit-Beat marker = Beat 3 of Universal 3-Beat Combo.**

Implementation chain:
1. **Unity AnimationClip event** at Beat 3 impact frame (frame 5-7 of 10-12f Beat 3 anim) → fires `OnCommitBeat()` callback
2. `CommitBeatHandler` MonoBehaviour: increments combo counter, applies 1.2s ICD lockout, triggers Echo proc
3. Echo proc spawn: facing-relative ±45° front-flank, 24px offset (Karar #122 spawn refinement)
4. Phantom spawn: alpha 0.3, cyan #00FFCC, 0.4s duration, weapon-baked sprite (separate prefab pool)
5. Primary Beat 3 dmg +20% buff applied for that single hit (T1 primary enhancement)

**Faz 1 MVP scope:** Warblade Beat 3 → Elementalist Fireball Echo only (canonized). T2/T3/T4 deferred.

**8-direction facing-relative spawn calc:**
- Player facing vector = (sin(theta), cos(theta)) where theta ∈ {0°, 45°, ..., 315°}
- Front-flank offset = facing + perpendicular(facing) × ±sin(45°) × 24px
- Karar #114 8-yön alignment — phantom spawns in correct relative direction regardless of player facing

**Animation event placement (critical):**
- Frame must be the **impact frame**, NOT the wind-up or recovery
- For 10-12f Beat 3: frame 5 (mid-swing apex = impact)
- Test in Unity AnimationWindow with manual scrubbing before commit
- Karar #120 split-animation MUST NOT split Beat 3 (single apex on impact, frame budget fits in 12f single)

---

## Bölüm 5 — Karar #109 Ambient Idle (Per-Class Personality)

Reference: per-class personality from memory and ambient idle table.

| Class | Ambient action | Frames | Bracket |
|---|---|---|---|
| Warblade | Shoulder-rest greatsword, left hand to belt | 8 | 2 gen |
| Ranger | Bow forearm rest, observation posture, slight scan | 8 | 2 gen |
| Shadowblade | Right dagger reverse-grip wrist flip | 6 | 2 gen |
| Elementalist | Rune disc figure-8 orbit, palm traces glyph | 10 | 3 gen |
| Ravager | Hatchet grip flex, weight shift hip-to-hip | 5 | 1 gen |
| Ronin | 2f motionless + 1f thumb-push tsuba + 3f relax | 6 | 2 gen |
| Gunslinger | Pistol spin in right hand (1 rotation + catch) | 7 | 2 gen |
| Brawler | Alternating air-jab L/R | 5 | 1 gen |
| Summoner | Lantern raise + wisp emerge + whisper | 9 | 3 gen |
| Hexer | Grimoire pull from waist + 3-page flip + close | 8 | 2 gen |

**Total ambient gen cost (10 class × 8 dir):** 17 gen × 8 dir = ~136 gen — Faz 1.5 polish, NOT Faz 1 critical path. Warblade only in Faz 1 (16 gen).

**Karar #71 disipline:** ALL ambient idles maintain weapon-in-hand state. Ronin sheath exception preserved (tsuba thumb-push = sheath-state ambient).

---

## Bölüm 6 — Video Action Items Implementation (5 Items)

These are RIMA spec gaps identified from video analysis. All 5 must be specified before production starts.

| Item | Implementation | Faz 1 scope | Karar align |
|---|---|---|---|
| 1. Smear frame | PixelLab prompt mandatory clause + Aseprite post-fallback (5-10min/anim) | Beat 2 + Beat 3 only | NEW lock |
| 2. Breathing idle + weapon bob | Karar #109 ambient idle base + secondary motion lock | Warblade only | #109 confirmed |
| 3. Run extreme lean | Chibi 64x64 head-40%-height validation; Codex visual test post-gen | Warblade run pass | #100 |
| 4. Selout outline (1px darker local color) | Aseprite post-process: pure black outline → darker local color shade. Script-based or manual. | All Warblade sprites | NEW lock |
| 5. Dust puff VFX | `animate_object` 3-4f loop sprites at footfall/pivot/landing; Unity ParticleSystem or simple SpriteAnimator | Faz 1.5 polish layer | NEW |

**Selout decision recommendation (rima-design judgment):** Manual Aseprite post-process per sprite (no shader). Reasoning: shader-based selout adds URP 2D pipeline complexity, post-process is one-time per gen, low maintenance, deterministic output. Codex Step 2 question #5 will validate.

**Dust puff decision:** Use PixelLab `animate_object` (not character animation). Generates 3-4f loop sprite. Unity spawn at footstep event (AnimationClip event on Run anim foot-down frame). ~30 min per dust variant, 1 variant per biome F1.

---

## Bölüm 7 — Production Cost Matrix

Source: `feedback_pixellab_frame_cost_brackets.md` (4-5f=1, 6-8f=2, 10-12f=3, 14-16f=4 gen).

**Warblade Faz 1 (8-direction, Karar #114):**

| Anim | Frames | Bracket | Per-dir cost | 8-dir total |
|---|---|---|---|---|
| Idle (base) | 8 | 6-8f | 2 gen | 16 gen |
| Ambient Idle (Faz 1.5) | 8 | 6-8f | 2 gen | 16 gen |
| Run | 8 | 6-8f | 2 gen | 16 gen |
| Beat 1 | 8 | 6-8f | 2 gen | 16 gen |
| Beat 2 | 12 | 10-12f | 3 gen | 24 gen |
| Beat 3 (Commit) | 12 | 10-12f | 3 gen | 24 gen |
| Hurt | 4 | 4-5f | 1 gen | 8 gen |
| Death (Karar #120 split: 2 parts + state) | 6+6 | split | (2+2+state 1) ×8 | ~40 gen |
| Dash | 6 | 6-8f | 2 gen | 16 gen |
| **Warblade Faz 1 TOTAL** | | | | **~176 gen** |
| **+ Karar #124 form variation (Base + T2 Rift)** | weapon-only sprite swap | | 1 gen Aseprite × 2 form | **2 sprite, no anim** |
| **Warblade GRAND TOTAL Faz 1** | | | | **~178 gen** |

**Mob Faz 1 — Seam Crawler (4-dir per Karar #74 grunt skip):**
- Idle 8f × 4 dir × 2 gen = 8 gen
- Burst Strike 8f × 4 dir × 2 gen = 8 gen
- Submerge 6f × 4 dir × 2 gen = 8 gen
- **Seam Crawler TOTAL: ~24 gen**

**Faz 1 GRAND TOTAL: ~200 gen** (Warblade 178 + Mob 24)

**Faz 2 (3 additional classes — Elementalist + Shadowblade + Ranger):**
- ~176 gen per class × 3 = ~528 gen
- + 4 skill anim per class × 3 classes = 288 gen
- **Faz 2 TOTAL: ~816 gen**

**Faz 3-5 (6 remaining classes):**
- ~176 gen body + ~96 gen skills per class × 6 = ~1,632 gen
- **Faz 3-5 TOTAL: ~1,632 gen**

**ALL CLASSES GRAND TOTAL: ~2,648 gen** (Karar #114 8-yön direct gen — mirror REVOKED S68)

---

## Bölüm 8 — Pipeline Steps per Class (Production Workflow)

Linear per-class workflow, each step gated on previous completion:

1. **Base body sprite (Create Image Pro)** — DONE for all 10 classes + 6 mobs. South-facing reference.
2. **8-direction character (Create Character)** — `n_directions=8, view=high top-down, proportions=chibi`. ~24 gen per class.
3. **Custom V3 animation per direction** — 8 yön bracket cost (Bölüm 7). Template-first try (`breathing-idle`, `running-8-frames`, `cross-punch`, etc.) before Custom narrative.
4. **Aseprite post-process** per animation:
   - Selout outline conversion (pure black → darker local color)
   - Smear frame manual stretch on Beat 2/3 impact frames
   - Hand anchor mark export (Faz 2 — Level 2 per-frame anchor JSON)
   - Hash verification (Karar #120 split anim apex frames byte-identical across parts)
5. **Unity AnimatorController setup** — Karar #110 Combat Faz 1.0 mimari + hand-tool config. 8 blend tree directions per anim state. Transitions:
   - Idle (loop) → [timer 4-8s] → Ambient Idle (1x) → Idle
   - Idle → Run (movement input) → Idle
   - Idle/Run → Beat1 → Beat2 → Beat3 → recovery → Idle (0.8s reset window)
   - Any → Hurt (damage event) → Idle
   - Any → Death (HP=0) → ground-state (no exit)
6. **Karar #122 T1 marker event** — Unity AnimationClip event on Beat 3 impact frame (frame 5 of 10-12f). Function: `CombatHandler.OnCommitBeat()`.
7. **Yol A weapon attach (Level 1)** — Warblade greatsword sprite parented to HandAnchor. Per-class extension via WeaponDatabase ScriptableObject lookup.
8. **Karar #124 form variation (Faz 1.5 polish)** — Warblade T2 Rift-cracked greatsword sprite. WeaponDatabase swap-by-form during T3 Empowered cast (Faz 2 trigger; Faz 1 = visual showcase only via debug toggle).

---

## Bölüm 9 — Codex Review Soruları (Step 2)

7 questions, ranked by criticality:

1. **CRITICAL — Faz 1 ~200 gen PixelLab credit budget within 25-day school deadline:**
Warblade ~178 gen + Seam Crawler ~24 gen = ~200 gen. PixelLab Web UI bracket pricing. Test order: Idle pilot → Run pilot → Beat 3 pilot (most expensive, drift risk highest). Realistic in 25-day Faz 1 window given Aseprite post-process per sprite ~10-15 min cleanup? Calculate: 200 sprite × 12 min avg = ~40 hours cleanup alone. Add gen wait time (1-2 min/dir × 200 = ~5 hours) + Unity import + AnimatorController setup. Codex feasibility verdict?

2. **CRITICAL — Karar #122 T1 AnimationClip event vs custom marker:**
Beat 3 impact frame event triggers `OnCommitBeat()`. Unity AnimationClip event vs custom timeline marker system — which is lighter weight for the 3-Beat combo state machine? Risk: AnimationClip event fires on direction-blend tree mid-blend (8-dir blend at 45° interpolation) — does event fire ONCE at impact or 8× per direction? Codex implementation pattern recommendation.

3. **Karar #120 split-animation Death 12f hash verification Aseprite workflow:**
Pre-fall 6f part + ground 6f part, apex = ground plant. Apex state generated separately (~3 gen). Hash verification (byte-identical apex frame across both parts) requires Aseprite export discipline. Codex provides Lua export script? Realistic Faz 1 fit or defer Death-split to Faz 1.5?

4. **Smear frame PixelLab native vs Aseprite post-process:**
PixelLab Custom V3 prompt clause "one distorted stretch frame at peak weapon velocity" — does it produce usable smear or generic blur? Codex test recommendation: 1 pilot prompt + visual inspect. If unusable, Aseprite manual smear ~5-10 min/attack anim × 3 attacks (Beat 1/2/3 with smear) × 8 dir = ~2-4 hours per class. Faz 1 fit?

5. **Selout outline implementation — shader vs sprite-baked:**
Pure black outline → darker local color shade. Aseprite manual post-process per sprite (deterministic, no maintenance) vs URP 2D shader pass (one-time setup, all sprites auto-converted). Trade-off: shader = ~4-6h Codex setup + URP pipeline dependency. Manual = ~3-5 min/sprite × 200 sprite Faz 1 = ~12 hours. Codex production-economy recommendation.

6. **Karar #109 ambient idle MCP Custom V3 vs Web UI:**
Per-class 5-10f ambient (Warblade 8f shoulder-rest). Template-first viable? `breathing-idle` template exists but lacks per-class personality. Custom V3 Web UI = ~16 gen per class ambient idle 8-dir. Token economy: 10 classes × 16 gen = 160 gen Faz 2 ambient. Codex feasibility within Faz 2 polish layer.

7. **Karar #124 form variation Faz 1 MVP scope realistic in 25-day window:**
30 min PixelLab (Warblade T2 Rift greatsword 8-dir variant) + 1 hour Unity (WeaponDatabase form lookup + SpriteRenderer swap on T3 cast). MVP shows form swap on debug button (T3 trigger Faz 2). Codex risk verdict: include in Faz 1 Hafta 2 polish or defer entirely to Faz 2?

**Most critical 2 questions: #1 (Faz 1 production budget reality) and #2 (Karar #122 T1 event firing pattern across 8-dir blend tree).**

---

## rima-design Deliverable Summary (Step 1 conclusions)

- **Karar #122 T1 entegrasyon güvenliği:** 5/5. Beat 3 = Commit-Beat marker is canon-aligned. ZERO conflict with locked rules.
- **Faz 1 Warblade scope 25-day fit:** **MARGINAL FIT.** ~60-75 hours focused work needed; 25 calendar days × 3-4h/day = ~75-100 hours available. Recommendation: LOCK Faz 1 = base anim only, ambient idle + form variation = stretch goal.
- **En riskli production bottleneck:** Aseprite manual cleanup (~40h single-threaded). If selout shader path chosen, drops to ~20h — highest-leverage Codex decision.
- **Conflicts with locked rules:** NONE.

**Orchestrator next step:** Codex Step 2 review dispatch using Bölüm 9 questions. Then rima-design Step 3 synthesizes LOCK.
