# COUNCIL DECISION — Skill-reward 2b fix (2026-06-14)

Topic: Phase-1 skill-reward fix review (DraftManager.cs + ChestBehavior.cs, uncommitted).
Advisors: auditor-opus · ax 3.1 Pro · ax 3.5 Flash · cx (yasinderyabilgin). Synthesis: Opus orchestrator.

## Verdicts
- auditor-opus: PASS-with-nits · ax 3.1 Pro: FAIL · ax 3.5 Flash: PASS · cx: PASS-with-nits

## Agreements
- **FIX3 (ShowDraftWithSkill IsDraftActive guard+set):** ALL 4 → SAFE, no soft-lock. Every OnOfferSelected branch (pick/replace/skip/force-close) resets IsDraftActive=false via FinishPick. Chest card has no cancel. → SHIP AS-IS.
- **FIX1 main path:** 3/4 (auditor, cx, Flash) → safe; HandleActivePick still calls FinishPick, slot stays usable.
- **FIX2:** ALL 4 → the comment "to match GetPool" is FALSE; GetPool additionally filters retired offers + class scope. Inline isImplemented-only filter still lets chest offer off-class / retired skills.

## Key disagreement + call
- ax 3.1 Pro (FAIL) raised 2 deeper points:
  1. **Chest class/tier bypass (HIGH):** chest can offer Ranger skill to Warblade / Legendary in room 1.
  2. **AssignActive replace-loss (latent):** OnReplaceChosen removes old skill BEFORE AssignActive; placeholder early-return → net slot loss.
- Resolution: both are real but #2 is UNREACHABLE once offers are filtered (normal draft already uses GetPool; chest will too after fix). ax Flash's host-null finding shares #2's root: bookkeeping writes a slot even when bind fails.

## DECISION (fix-up P1b)
1. **FIX2 → `SkillDatabase.Instance.GetPool(primary, secondary)`** (classes from PlayerClassManager). Subsumes isImplemented + retired + class filtering. Fix wrong comment. DEFER room-depth/rarity gating (via SkillOfferGenerator) — pre-existing, larger, separate.
2. **FIX1 → keep early-return; add: only append to currentActiveSkills when a SkillBase is actually bound** (host!=null && bound) + defensive `if (skill==null) return;`. Eliminates dead-entry class (host-null + placeholder), makes replace-loss unreachable.
3. **FIX3 → no change.**

## Deferred (noted, not done)
- Chest room-depth/rarity gating through SkillOfferGenerator (design + larger surface).
- SkillRuntime DRY recipe layer (cx feasibility audit, separate post-demo track).
