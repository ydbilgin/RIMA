# Animation Step 2 — Production Feasibility Review
## Date: 2026-05-14 (rima-sonnet analysis, S70 gece)

---

## Q1 Production Budget

**VERDICT: MARGINAL — safe only if Ambient Idle + Form Variation cut from Faz 1 critical path**

Detailed numbers:
- PixelLab gen: 200 gen × 1-2 min = ~5-6h realistic (idle periods + credit checks)
- Aseprite cleanup: draft says 12 min avg; weighted avg closer to 13-14 min (hurt/dash ~6-8 min, attack with smear+selout ~15-18 min). 200 sprites × 14 min = **~47h**
- Unity AnimatorController + blend trees: 8-dir blend tree for 9 states on Warblade + mob 4-dir = ~10-12h with Codex assist
- Karar #122 event placement: Beat 3 × 8 dir verify = ~1.5h
- WeaponDatabase SO + HandAnchor setup: ~6-8h Codex
- **GRAND TOTAL: ~5h gen + ~47h Aseprite + ~12h Unity + ~2h event + ~7h weapon = ~73h**

25 days × 3-4h/day = 75-100h. At 3h/day razor-thin (75h vs 73h), zero buffer.

**Specific cuts that recover buffer:**
1. Cut Ambient Idle (Warblade shoulder-rest) from Faz 1: saves 16 gen + 3h Aseprite + timer logic — already Faz 1.5 in draft, **CONFIRM**
2. Cut Karar #124 Unity wiring from Faz 1 (sprite asset OK to generate, zero code): saves ~1h session + Unity complexity
3. Beat 1 smear: NOT mandatory per Bölüm 2 table (Beat 2+Beat 3 only). Remove Beat 1 smear: saves ~40 min × 8 dirs

With three cuts: **~65-67h total, ~8-10h buffer at 3h/day.**

---

## Q2 Karar #122 T1 Event Pattern

**VERDICT: StateMachineBehaviour on Beat 3 state — AnimationClip event REJECTED for 8-dir blend tree**

In a 2D Blend Tree (8 child clips), AnimationClip events from child clips can double-fire at diagonal inputs (50/50 weight split NE/NW/SE/SW). The 1.2s ICD partially mitigates this but the pattern is error-prone.

**Recommended pattern: StateMachineBehaviour attached to the Beat 3 AnimatorState node.**

```csharp
// Beat3CommitTrigger.cs — StateMachineBehaviour on Beat3 AnimatorState
public class Beat3CommitTrigger : StateMachineBehaviour
{
    [SerializeField] private float impactNormalizedTime = 0.417f; // frame 5 of 12
    private bool _fired;
    private CombatHandler _combat;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        _fired = false;
        _combat = animator.GetComponent<CombatHandler>(); // cache once
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

Key points:
- `_fired` bool → exactly one call per Beat 3 playthrough regardless of blend weights
- `normalizedTime` is direction-agnostic
- URP 17.3 + Unity 2D Animation 13.0.2 (confirmed manifest.json) both support StateMachineBehaviour cleanly
- `com.unity.timeline 1.8.10` present but NOT to be used — keeps one animation system

---

## Q3 Karar #120 Death Split

**VERDICT: Faz 1 KEEP (base split) — Lua hash verification DEFER to Faz 1.5**

- Death animation visible on every kill — missing = demo regression
- Practical Faz 1 approach: generate Death 12f, split in Unity Sprite Editor (manual apex frame 6 designation), two AnimationClip assets
- `com.unity.2d.aseprite 3.0.1` already in project — direct Aseprite import pipeline, simplifies split (two .ase files, apex frame shared)
- Lua hash verification: Faz 1.5 defer
- Budget saving: ~2-3h by skipping Lua script

---

## Q4 Smear Frame

**VERDICT: Pilot test first — expect native unusable, plan manual Aseprite (Beat 2+Beat 3 only)**

PixelLab Custom V3 interpolation model resists producing intentionally distorted smear frames (quality objective conflict). Pilot test one Beat 2 direction with clause "one distorted stretch frame at peak weapon velocity, blade morphed into stylized smear arc." If distinct distortion frame: use. If motion-blur approximation (expected): fall back to manual.

Manual cost (Beat 2+Beat 3 only, Beat 1 cut per Q1):
- 2 attacks × 8 directions × 7 min = **~2h for Warblade only** — fits Faz 1.

---

## Q5 Selout Outline

**VERDICT: URP 2D Shader (reverses draft recommendation) — manual sprite-baked REJECT**

Draft recommended manual sprite-baked. This review reverses on budget grounds:
- Manual: 200 sprites × 4 min Aseprite batch = **~13h** (dominant overhead after cleanup)
- URP 2D Shader: `ScriptableRendererFeature` replaces pure black pixels with darkened adjacent color — standard 2D renderer pattern. ~4-6h Codex, applies to ALL sprites automatically, zero per-sprite cost, covers future sprites for free
- Project confirmed: `com.unity.render-pipelines.universal 17.3.0` + standard 2D sprite materials — no conflict

**If URP shader chosen: drop ~13h manual selout from Q1 budget → total drops to ~60-64h, 10-15h buffer.** This is the single highest-leverage decision in Faz 1 budget.

---

## Q6 Ambient Idle

**VERDICT: Faz 2 defer confirmed — Custom V3 Web UI when time comes**

Template `breathing-idle` lacks per-class personality by design. Custom V3 Web UI required for all 10 ambient idles (MCP no First/End Frame inject support — manual Web UI only per MEMORY rules).

10 classes × 16 gen = 160 gen Faz 2. Stub AnimatorController ambient timer with self-transition on base Idle state for Faz 1 demo. Warblade shoulder-rest is Faz 1.5 polish — confirm not in 25-day critical path.

---

## Q7 Form Variation Faz 1

**VERDICT: Faz 2 defer — generate sprite asset only, zero Unity code**

- WeaponDatabase form-lookup branching + SpriteRenderer swap testing across 8 dirs = code complexity not worth it for debug toggle
- T3 trigger is Faz 2 — debug toggle becomes throwaway code at Faz 2 refactor
- **Recommendation:** Generate T2 Rift greatsword sprite in PixelLab (~30 min), commit to Assets, NO Unity wiring until Faz 2. No WeaponDatabase form entry, no debug toggle.

---

## Genel Verdict (rima-design Step 3 için)

**Faz 1 critical path (KEEP):**
- Warblade: Idle (base), Run, Beat 1-3 (Beat 2+3 manual smear), Hurt, Death (12f Unity-split), Dash
- Seam Crawler: Idle, Burst Strike, Submerge (4-dir)
- WeaponDatabase Level 1 OrbitAttach (HandAnchor static)
- Karar #122 T1: StateMachineBehaviour on Beat 3 (NOT AnimationClip event)
- URP 2D selout shader (Codex, 4-6h one-time)
- T2 Rift greatsword sprite asset only (no code)

**Defer from Faz 1:**
- Ambient Idle (all 10 classes incl. Warblade)
- Karar #124 Unity wiring (sprite asset OK)
- Death split Lua hash verification
- Beat 1 smear frame

**En kritik 2 cut suggestion:**
1. **URP selout shader over manual sprite-baked** — saves 7-9h, single highest-leverage decision
2. **Ambient Idle Faz 1.5 confirm** — saves 16 gen + 3h Aseprite + blend tree timer complexity

**En kritik 2 LOCK confirmation:**
1. **StateMachineBehaviour pattern for Karar #122 T1** — AnimationClip event rejected for 8-dir blend tree; Bölüm 4 of spec should be updated accordingly before LOCK
2. **Scope cap LOCK** — no additional states enter Faz 1 after this review; at 3h/day budget fits with ~10h buffer (with cuts)

---

*rima-sonnet analysis — S70 gece. Ready for rima-design Step 3 LOCK synthesis.*
