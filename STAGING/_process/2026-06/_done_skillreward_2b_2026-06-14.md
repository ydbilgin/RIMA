# DONE — Skill Reward 2b (2nd skill not arriving after chest) — 2026-06-14

VERDICT: **DONE** · console: **0 errors** (post-compile, verified twice)

## Problem (Phase 1 diagnosis recap)
After picking a skill from a chest reward, the 2nd skill never appeared on the skill bar.
Three independent defects on the chest reward path, each capable of producing the symptom.

## Files changed (2 files)
- `Assets/Scripts/Skills/DraftManager.cs` — FIX 1 (AssignActive guard, +7 lines) + FIX 3 (ShowDraftWithSkill, +9 lines)
- `Assets/Scripts/Core/ChestBehavior.cs` — FIX 2 (BuildChestOffers filter, +6 lines)
- (read-only) `Assets/Scripts/Skills/SkillDatabase.cs` — confirmed filter predicate `if (!s.isImplemented) continue;` in `GetPool` (line 586)

Scope respected: only DraftManager.cs + ChestBehavior.cs touched. SkillBarUI / TooltipSystem untouched.

## FIX 1 — DraftManager.AssignActive (was ~642)
A picked SkillData with `skillType == null` (unimplemented placeholder) bound NO SkillBase
component yet was still appended to `currentActiveSkills`, creating a dead entry → empty slot.
Added an early guard at method top:
```
if (skill != null && skill.skillType == null)
{
    Debug.LogWarning($"[Draft] '{skill.skillName}' atlandı — runtime skill bileşeni yok (skillType=null), slot {slot} boş bırakıldı.");
    return;
}
```
No-op skip: list is not mutated, slot stays usable. Logging matches existing `[Draft]` Turkish style.

## FIX 2 — ChestBehavior.BuildChestOffers (was ~130)
Was pulling UNFILTERED `SkillDatabase.GetAll()`, which includes `isImplemented=false` placeholders
(unlike `GetPool`, which filters them). Replaced with the same predicate `GetPool` uses
(`s.isImplemented`), filtering GetAll() in place. (Did not call GetPool directly because GetPool
additionally class-filters by primary/secondary — would have changed chest behavior beyond scope.)
Placeholders can no longer reach a chest offer.

## FIX 3 — DraftManager.ShowDraftWithSkill (was ~372, chest path)
Unlike the sibling ShowDraft / ShowOpeningKitDraft, it lacked `EnsureDependencies()` and an
IsDraftActive guard, so when `offerUI` was an unresolved Auto instance it silently `return`ed and
the chest skill draft never opened. Now mirrors the siblings: `IsDraftActive || IsDraftPending`
re-entry guard, `EnsureDependencies()`, null-offerUI warning, and sets `IsDraftActive = true`
before `offerUI.Show(...)`.

## Verification (data-proof, NOT screenshot — overlay UI doesn't render in MCP capture)
- Compile: `read_console` types=[error] → **0 entries** (before and after proofs).
- FIX 2 (runtime via execute_code): GetAll total=111, unimplemented=44; filtered chest pool=67
  with **0 placeholders**. ASSERT no-placeholder-in-filtered = PASS; filter-removed-44 = PASS.
- FIX 1 (runtime via reflection on real DraftManager): invoked AssignActive(placeholderSkill, 0)
  with skillType=null → currentActiveSkills before=0 / after=0, placeholder NOT in list,
  warning fired ("[Draft] 'ProofPlaceholderSkill' atlandı …"). ASSERT no-dead-entry = PASS;
  warning-fired = PASS.
- FIX 3 (static IL inspection of compiled ShowDraftWithSkill body): method now references
  EnsureDependencies (call), IsDraftActive (get/set), and IsDraftPending (guard).
  ASSERT calls-EnsureDependencies && references-IsDraftActive = PASS.
  (Used static IL because full runtime sim of the chest UI flow needs Play mode + a live chest;
  IL proof confirms the exact control-flow elements the task required were added.)

## Residual risk
- FIX 2 changes the chest skill pool from 111 → 67 candidates (drops the 44 placeholders).
  Intended; no behavioral risk beyond never offering an unbindable card.
- FIX 1 now skips placeholder picks entirely (no fallback reward substituted). The chest path
  itself can no longer produce a placeholder pick after FIX 2, so FIX 1 is a defense-in-depth
  guard for the normal/forge draft paths too. Slot is left usable, matching the spec ("no-op/skip
  so the slot stays usable").
- FIX 3 adds the IsDraftActive guard: if a draft is already active when a chest skill is picked,
  ShowDraftWithSkill now early-returns (previously it would force-show). This matches sibling
  behavior and is the correct, least-surprising guard; the chest-then-draft sequence is serial in
  practice so no regression expected.
- Live Play-mode end-to-end (open chest → pick skill → see 2nd skill on bar) NOT run here; the
  three unit-level proofs cover each defect surface. Recommend one manual Play-mode chest pick as
  a final smoke before sign-off.
