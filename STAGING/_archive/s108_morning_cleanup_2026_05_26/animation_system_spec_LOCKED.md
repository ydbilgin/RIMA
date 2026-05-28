---
status: LOCKED (Step 3/3 — rima-design synthesis)
faz: 1
tarih: 2026-05-14
ozet: "Final animation system spec; Codex Faz 1 implementation kaynak dosyası. Bu spec sonrası kapsam SABİT."
source: rima-design Opus S70 synthesis of draft + rima-sonnet feasibility review
locks:
  - StateMachineBehaviour pattern Karar #122 T1 (AnimationClip event REJECT)
  - URP 2D Shader selout (sprite-baked REJECT)
  - Beat 1 smear CUT (Beat 2+3 only)
  - Ambient Idle Faz 1.5 defer
  - Karar #124 Unity wiring Faz 2 defer (sprite asset Faz 1 OK)
  - Death split com.unity.2d.aseprite (Lua hash Faz 1.5 defer)
  - Scope cap: ZERO new states enter Faz 1 after this LOCK
---

# RIMA Animation System Spec — LOCKED
## Date: 2026-05-14 | Step 3/3 rima-design synthesis

This document is the LOCKED implementation spec for RIMA Faz 1 animation system. It synthesizes the rima-design Step 1 draft (`STAGING/animation_spec_draft.md`) and the rima-sonnet Step 2 production review (`STAGING/animation_step2_review_output.md`). Where the two disagree, this document supersedes both. Codex Faz 1 animation dispatch tasks consume this file as their authoritative source.

**Scope envelope LOCK:** Faz 1 = Warblade primary + Seam Crawler mob. No additional states, classes, or polish layers enter Faz 1 after this LOCK. Re-dispatching rima-design Opus is required to amend any decision below.

---

## 1. Scope (Faz 1 Critical Path)

### 1.1 Warblade — 8 states, 8 directions each (Karar #114)

| State | Frames | Cost bracket | Per-dir gen | 8-dir total | Notes |
|---|---|---|---|---|---|
| Idle (base) | 8 | 6-8f | 2 gen | 16 gen | Karar #109 base layer only — ambient deferred |
| Run | 8 | 6-8f | 2 gen | 16 gen | Karar #42 + #46, chibi-adapted Brian's Extreme Pose lean |
| Beat 1 | 8 | 6-8f | 2 gen | 16 gen | No smear (CUT from draft) |
| Beat 2 | 12 | 10-12f | 3 gen | 24 gen | Manual smear Aseprite (Beat 2+3 only) |
| Beat 3 (COMMIT-BEAT) | 12 | 10-12f | 3 gen | 24 gen | StateMachineBehaviour Karar #122 T1 marker |
| Hurt | 4 | 4-5f | 1 gen | 8 gen | Knockback recoil + brief hold |
| Death | 6+6 split | special | ~5 gen/dir | ~40 gen | Karar #120 split via com.unity.2d.aseprite |
| Dash | 6 | 6-8f | 2 gen | 16 gen | Anticipation 1f + thrust 2f + recovery 1-2f |

**Warblade Faz 1 TOTAL: ~160 gen**

### 1.2 Seam Crawler mob — 4-direction

| State | Frames | Per-dir gen | 4-dir total |
|---|---|---|---|
| Idle | 8 | 2 gen | 8 gen |
| Burst Strike | 8 | 2 gen | 8 gen |
| Submerge | 6 | 2 gen | 8 gen |

**Seam Crawler TOTAL: ~24 gen.**

### 1.3 Faz 1 Grand Total

**~184 PixelLab generations** + Karar #124 Warblade T2 Rift greatsword **sprite asset only** (no animation, no Unity wiring — 8 sprites static reference for Faz 2 wiring).

### 1.4 Budget (LOCK)

| Phase | Hours |
|---|---|
| PixelLab generation | 5h |
| Aseprite cleanup (~14 min avg × ~184 sprites) | ~43h |
| Unity AnimatorController + 8-dir blend trees | ~10h |
| StateMachineBehaviour Beat3CommitTrigger + Echo wire | ~2h |
| WeaponDatabase Level 1 OrbitAttach + HandAnchor | ~6h |
| URP 2D Selout shader (Codex one-time) | ~5h |
| **Faz 1 TOTAL** | **~65-67h** |

25 days × 3h/day = 75h → **~8-10h buffer**. At 4h/day: ~33h buffer.

---

## 2. PixelLab Generation Plan

### 2.1 Generation order (risk-first)

1. Warblade Idle base 8-dir (16 gen) — drift baseline reference
2. Warblade Beat 3 commit 8-dir (24 gen) — highest drift + smear risk
3. Warblade Run 8-dir (16 gen)
4. Warblade Beat 2 8-dir (24 gen)
5. Warblade Beat 1 8-dir (16 gen)
6. Warblade Hurt + Dash 8-dir (24 gen total)
7. Warblade Death split 8-dir (~40 gen)
8. Seam Crawler 4-dir all states (24 gen)
9. Warblade T2 Rift greatsword sprite-only 8-dir (8 gen)

### 2.2 Per-direction count (LOCK — Karar #114)

8 directions DIRECTLY generated. Mirror trick REVOKED (S68 Karar #99 weapon-hand conflict).

### 2.3 Mode selection

- **Template-first** when matching template exists (`breathing-idle`, `cross-punch`) — 1 gen/dir
- **Custom V3 Web UI** for combat states, Death split apex anchoring — bracket cost
- **MCP `animate_character` Custom** when First/End Frame anchoring not required
- Reference: `feedback_pixellab_template_vs_custom_v3.md` + `feedback_pixellab_frame_cost_brackets.md`

### 2.4 Prompt discipline (LOCK)

- Narrative format only (per `feedback_pixellab_idle_prompt_style.md`)
- Pixel count, frame number, loop instruction PROHIBITED in prompt
- Motion-only descriptors — "blade sways", NOT "warrior holds greatsword"
- Magnitude vocab: subtle / minimal / gentle / explosive
- Karar #71: NEVER write "falls/drops/releases" — write "stays gripped/remains anchored"
- Karar #99: NEVER mention which hand; sprite ground truth governs

---

## 3. Aseprite Workflow

### 3.1 Per-sprite cleanup pipeline

1. Import PixelLab PNG batch → Aseprite
2. Trim transparent margins → re-center to 64×64 canvas
3. Visual drift scan vs reference frame (Karar #80 silhouette, Karar #99 weapon visibility)
4. Color palette quantize to F1 Shattered Ruins palette
5. NO manual selout pass — URP 2D shader handles outline (see §5)
6. Export per-frame PNG → `Assets/Art/Characters/Warblade/<state>/<dir>/`
7. Import via `com.unity.2d.aseprite 3.0.1` → AnimationClip generated automatically

### 3.2 Smear technique (LOCK — Beat 2 + Beat 3 only)

**Beat 1 smear CUT.**

**Pilot test first:** One Beat 2 South-direction with clause `"one distorted stretch frame at peak weapon velocity, blade morphed into stylized smear arc"`. Expected: PixelLab gives motion-blur, not true smear → fall back to manual.

**Manual Aseprite smear (expected path):**
- Duplicate impact frame on Beat 2 (frame 5-6) and Beat 3 (frame 5-7)
- Stretch weapon shape on duplicate using Transform + Liquify
- ~7 min/direction × 8 dir × 2 attacks = **~2h Warblade total**

### 3.3 Death split (LOCK — com.unity.2d.aseprite, Faz 1 KEEP)

- Generate 12-frame Death, apex = ground plant (frame 6)
- Split in Unity Sprite Editor: pre-fall (frames 1-6) + ground (frames 6-12)
- Apex frame 6 shared across both clips (Karar #120)
- `com.unity.2d.aseprite 3.0.1` native import — no Lua scripting Faz 1
- **Lua hash byte-identical verification: DEFER to Faz 1.5**

---

## 4. Unity AnimatorController Architecture

### 4.1 8-direction Blend Tree (per state)

- Each Warblade state = 2D Freeform Cartesian Blend Tree
- 8 child motion clips at positions (sin(θ), cos(θ)) for θ ∈ {0°, 45°, ..., 315°}
- Blend parameters: `MoveX`, `MoveY`
- Seam Crawler: 4-dir threshold

### 4.2 State transitions

```
Idle (loop) → Run (input magnitude)
Idle/Run → Beat1 → [0.8s combo window] → Beat2 → [0.8s] → Beat3 → Recovery → Idle
Any → Hurt (damage event) → Idle
Any → Death (HP=0) → Death.Part2 (ground, no exit)
Idle/Run → Dash → Idle
```

### 4.3 Beat3CommitTrigger — Karar #122 T1 LOCK

**LOCK: StateMachineBehaviour. AnimationClip event REJECTED for 8-dir blend tree (double-fire at diagonal blends).**

```csharp
// Beat3CommitTrigger.cs — attach to Beat3 AnimatorState (not child clips)
public class Beat3CommitTrigger : StateMachineBehaviour
{
    [SerializeField] private float impactNormalizedTime = 0.417f; // frame 5 of 12
    private bool _fired;
    private CombatHandler _combat;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        _fired = false;
        _combat = animator.GetComponent<CombatHandler>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        if (!_fired && info.normalizedTime >= impactNormalizedTime)
        {
            _fired = true;
            _combat?.OnCommitBeat();
        }
    }
}
```

Echo proc semantics on `OnCommitBeat()`:
- Increment combo counter + 1.2s ICD lockout
- Spawn Phantom Echo (alpha 0.3, cyan #00FFCC, 0.4s) at facing-relative ±45° front-flank, 24px offset
- Primary Beat 3 dmg +20% buff for this hit (T1 primary enhancement)

### 4.4 WeaponDatabase Level 1 OrbitAttach (LOCK)

- `WeaponDatabaseSO` ScriptableObject — Faz 1: Warblade base greatsword only
- Single `HandAnchor` empty transform on Warblade prefab
- Weapon GameObject parented to HandAnchor at spawn
- `HandAnchor.localPosition` static per character (Level 1)
- Faz 2 → Level 2 per-frame AnimationCurve anchor

### 4.5 Karar #124 (Faz 1 = sprite asset only)

- T2 Rift greatsword 8-dir sprite → `Assets/Art/Weapons/Warblade/T2_Rift/`
- ZERO Unity code/wiring in Faz 1

---

## 5. Selout Shader (URP 2D — LOCK)

**LOCK: URP 2D ScriptableRendererFeature. Sprite-baked manual REJECTED (saves ~13h).**

### 5.1 Implementation (Codex dispatch ~4-6h)

- `SeloutOutlineFeature : ScriptableRendererFeature` injected into URP 2D Renderer
- Shader pass: replaces pure black edge pixels with darkened adjacent inner color (~30% value reduction)
- Applied via `SeloutMaterial` tag on default sprite material
- Edge detection: 1px outline, 8-neighborhood sample
- One-time setup, automatic coverage for all present + future sprites

### 5.2 Project compatibility

- `com.unity.render-pipelines.universal 17.3.0` confirmed in manifest
- Standard 2D sprite materials — no conflict

---

## 6. Faz 1.5 Defer List

| Item | Notes |
|---|---|
| Karar #109 Warblade Ambient Idle | 16 gen + ~3h Aseprite, Faz 1.5 P1 |
| Beat 1 smear | Only if Beat 2+3 smear quality validates well |
| Karar #120 Death Lua hash verification | ~2-3h Codex, com.unity.2d.aseprite native used Faz 1 |
| Dust puff VFX | `animate_object` 3-4f loop, footfall trigger |

---

## 7. Faz 2 Scope

1. Karar #122 T2/T3/T4 Echo tiers
2. Karar #124 Unity wiring (form-lookup + T3 swap trigger)
3. Karar #109 ambient idle full batch (9 remaining classes × 16 gen)
4. Karar #123 Level 2 per-frame HandAnchor
5. 3 additional playable classes (Elementalist + Shadowblade + Ranger)
6. 4 skill animations per active class (Q/E/R/F)

---

## 8. Production Timeline (25-Day Faz 1)

### Hafta 1 (Day 1-8) — Pipeline + Warblade base motion
- Day 1-2: Selout shader Codex (~5h) + WeaponDatabase Level 1 (~6h)
- Day 3-4: Warblade Idle 8-dir gen + Aseprite + Unity import; validate shader
- Day 5-6: Warblade Beat 3 8-dir gen + smear pilot test + Aseprite
- Day 7-8: Warblade Run 8-dir + AnimatorController scaffold

### Hafta 2 (Day 9-16) — Combat states + mob
- Day 9-10: Beat 2 8-dir + manual smear; Beat 1 8-dir
- Day 11-12: Hurt + Dash 8-dir
- Day 13-14: Death 12f split — generate + Aseprite import
- Day 15-16: Seam Crawler 4-dir all 3 states

### Hafta 3 (Day 17-25) — Integration + LOCK
- Day 17-19: 8-dir Blend Trees wired; Karar #122 T1 StateMachineBehaviour + tested
- Day 20-21: WeaponDatabase Level 1 + OrbitAttach 8-dir validated
- Day 22: T2 Rift greatsword 8-dir sprite-only gen
- Day 23: Echo Phantom prefab + spawn logic + 8-dir test
- Day 24: Demo scene integration + playthrough
- Day 25: Buffer / overflow / phase-close

---

## 9. LOCKED Decisions

| # | Decision | Source |
|---|---|---|
| L1 | URP 2D Selout shader (sprite-baked REJECT) | review Q5 |
| L2 | StateMachineBehaviour Beat3CommitTrigger (AnimationClip event REJECT) | review Q2 |
| L3 | Beat 1 smear CUT (Beat 2+3 only) | review Q1 |
| L4 | Karar #109 Ambient Idle Faz 1.5 defer | review Q1+Q6 |
| L5 | Karar #124 Unity wiring Faz 2 defer (sprite Faz 1 OK) | review Q7 |
| L6 | Death split via com.unity.2d.aseprite (Lua hash Faz 1.5 defer) | review Q3 |
| L7 | Smear pilot test first → manual Beat 2+3 only | review Q4 |
| L8 | 8-dir directly generated, mirror REVOKED | Karar #114 |
| L9 | Faz 1 scope cap: ZERO new states after this LOCK | review verdict |
| L10 | Budget: ~65-67h total, ~8-10h buffer at 3h/day | review Q1 |

**No conflicts with MASTER_KARAR_BELGESI.** Karar #71/#80/#99/#100/#108/#109/#114/#120/#122/#123/#124 all preserved.

*rima-design Opus synthesis — S70 LOCK. Step 3/3 complete. Codex Faz 1 animation dispatch unblocked.*
