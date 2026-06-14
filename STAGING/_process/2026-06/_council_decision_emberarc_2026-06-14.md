# COUNCIL DECISION — P5 ember-arc (2026-06-14)

Advisors: auditor-opus · ax 3.1 Pro · ax 3.5 Flash · cx (yasinderyabilgin). Synthesis: Opus.

## Verdicts
- auditor-opus: PASS · ax 3.5 Flash: PASS · cx: PASS · ax 3.1 Pro: FAIL (architectural only)

## Change (user-approved direction)
Warblade swing drew TWO overlapping arcs (old blue SlashArcVFX LineRenderer + new ember SkillVfx.MeleeArc sprite). User chose: keep ember (canon #E89020), drop blue, fix muddy tint.
- Removed blue EmitSlashArc calls from MeleeChainBehavior :97/:112 (kept ember MeleeArc).
- MeleeArc base sprite → slash_arc_crescent (lower chroma) + additive blend.
- New ApplyAdditiveSprite: element-agnostic hue-preserving boost `scale = 1.5/max(r,g,b)` (peak>0.0001 zero-div guard).

## Correctness — CONFIRMED (3 PASS)
- Shared MeleeArc has exactly 3 callers (MeleeChainBehavior Physical ×2, GravityCleave Void). 1.5/max uniform scale preserves hue for EVERY element: Physical→ember (1.5,0.93,0.21), Void→purple (1.05,0.63,1.5) — NOT reddened (the bug the orchestrator caught + fixed in P5b). Zero-div guarded + unreachable.
- PlaySweep additiveSprite=false default → backward-compatible, only MeleeArc passes true.
- Blue-arc removal sound: SlashArcVFX/EmitSlashArc still live via MarkPulse (Ravager) — NOT dead code. No broken refs.
- No NRE/whiteout (additive + crescent + Act1 ambient 0.22 dark).

## ax 3.1 Pro FAIL = architectural/post-demo (NOT correctness, NOT P5 regression)
- Physical hardcode in MeleeChainBehavior → data-driven (BasicAttackProfile). Pre-existing (added by VFX-Faz3, not P5); demo MeleeChainBehavior=Warblade=Physical=correct.
- "Parallel VFX systems" (SlashArcVFX vs SkillVfx). Pre-existing; P5 REDUCED duplication; Ravager not in demo.
- Overexposure → production HSV shader. ax Pro itself: "demo için yeterli."

## DECISION: COMMIT P5 as-is. Deferred (post-demo, documented):
- Neutral/white base slash sprite (user art) for full #E89020 saturation (current leans amber/gold).
- Data-driven basic-attack arc element (BasicAttackProfile) + VFX system unification (SlashArcVFX↔SkillVfx) + production HSV-remap shader.
- Cosmetic: stale doc comment MeleeChainBehavior.cs:11 ("defers EmitSlashArc"); optional instance==null guard in ApplyAdditiveSprite.
