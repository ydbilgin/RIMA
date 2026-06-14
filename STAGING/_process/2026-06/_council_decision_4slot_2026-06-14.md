# COUNCIL DECISION — P4 4-slots-full edge (2026-06-14)

Advisors: auditor-opus · ax 3.1 Pro · ax 3.5 Flash · cx (yasinderyabilgin). Synthesis: Opus.

## Verdicts
- auditor-opus: PASS-with-nits · ax 3.1 Pro: FAIL · ax 3.5 Flash: FAIL · cx: FAIL

## My P4 changes are correct for the demo
auditor (Opus) verified: HasFree*/FindNext* mirror is EXACT for the 2 DEMO classes (Warblade→skillController, Elementalist→elemSlotController), and the normal "primary full→replace / half-empty→free slot" case is preserved. BUG2 abort cleanly mirrors OnSkipChosen. So the two diag'd edge bugs ARE fixed correctly in the demo-reachable flow.

## Why the 3 FAILs do NOT block P4 (but are real, pre-existing)
The FAILs hinge on two issues that are NOT P4 regressions:
1. **Other-class controller gap (ax Pro/cx CRITICAL):** HasFree*/FindNext*/FindSlotOf only branch Warblade/Elementalist; Ranger/Shadowblade/Ronin fall through to Warblade controller → could clobber slot 0. BUT: (a) PRE-EXISTING — old FindNextPrimarySlot had the same gap; my diff only mirrored it. (b) UNREACHABLE in demo — P3 (auditor-confirmed) established IsDemoSelectable hard-gates to Warblade+Elementalist only; ax Pro/cx wrongly assumed the other 3 are selectable.
2. **Cross-band replacement (ax Pro/cx HIGH):** ShowReplaceMode passes ALL currentActiveSkills unfiltered → a primary-full pick could replace a secondary-band skill (or reverse). PRE-EXISTING (ShowReplaceMode untouched by P4).

## DECISION (P4b fix-up)
1. **[FOLD] Cross-band filter:** filter the ShowReplaceMode candidate list to the pending skill's target band (primary pick → only slots 0-3 skills; secondary → 4-5). Completes the band-aware trigger coherently; demo-safe (if secondary never unlocks, list is all-primary → filter is a no-op).
2. **[FOLD] Softlock harden:** OnReplaceChosen top-guard `if(pendingSkill==null||toReplace==null) return;` → also Hide + IsDraftActive=false (ax Flash; mirrors BUG2 abort / OnSkipChosen).
3. **[DEFER → follow-up]** Generalize primary host resolution (HasFree*/FindNext*/FindSlotOf) via ResolvePrimarySlotHost/GetControllerSlot so Ranger/Shadowblade/Ronin work. UNREACHABLE in demo (not selectable); larger refactor (changes slot host resolution across 3+ methods) — post-demo when those classes unlock. This also subsumes the HasFree*-duplicates-FindNext* over-engineering nit (ax Flash) — unify then.

## Deferred follow-up ticket (record in status + memory)
**Skill slot system other-class support:** primary slot host resolution (HasFreePrimarySlot/FindNextPrimarySlot/FindSlotOf) hardcodes Warblade/Elementalist; generalize via ResolvePrimarySlotHost before Ranger/Shadowblade/Ronin become selectable. Until then those classes can clobber slot 0 + bypass replace-mode.
