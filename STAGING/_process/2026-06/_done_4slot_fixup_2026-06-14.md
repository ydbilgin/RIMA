# DONE — P4b 4-slot council fix-up (2026-06-14)

VERDICT: DONE. Scope honored (only `DraftManager.cs` edited; offerUI/controller read-only). Deferred item (ResolvePrimarySlotHost generalization for Ranger/Shadowblade/Ronin) NOT touched — left as-is per council.

## File written
- `Assets/Scripts/Skills/DraftManager.cs` — 871 lines (was 820). +51 lines net.
  - CHANGE A: `HandleActivePick` now passes `BuildBandReplaceCandidates(secondary)` instead of full `currentActiveSkills` to `ShowReplaceMode`. New private helper `BuildBandReplaceCandidates(bool)` (~45 lines incl. doc) added beside the band-probe helpers.
  - CHANGE B: `OnReplaceChosen` top guard `if (pendingSkill == null || toReplace == null)` now runs `offerUI.Hide(); IsDraftActive = false; pendingSkill = null;` before return (mirrors BUG2 abort / OnSkipChosen).

## Design notes (FIRST READ findings)
- `SkillOfferUI.ShowReplaceMode(List<SkillData> currentActives, ...)` iterates the passed list and binds each card button to `onReplace?.Invoke(captured)` where `captured` is the `SkillData` itself — it does NOT use list index for slot mapping. Slot resolution happens entirely in `DraftManager.OnReplaceChosen` → `FindSlotOf(toReplace)`, which looks up the real slot via `sd.skillType` on the controller. => Passing a filtered SUBSET is safe; no `ShowReplaceMode` API change needed. NOT BLOCKED.
- `BuildBandReplaceCandidates` mirrors host resolution of `HasFreePrimarySlot`/`HasFreeSecondarySlot`/`FindNext*`:
  - secondary band = slots 4-5 on `skillController` (Warblade host)
  - primary band = slots 0-3 on Elementalist host if `UseElementalistPrimary()` else `skillController`
  - For each occupied slot it maps `GetControllerSlot(host,i).GetType()` back to the owning `SkillData` in `currentActiveSkills` by `skillType` — same key `FindSlotOf` uses, so chosen candidate always relocates to its real slot.

## VERIFY — data-proof (reflection via execute_code, NOT screenshot)
console: 0 errors (read_console error filter empty after force recompile + after both proof runs).

CHANGE A (band-full replace, BOTH primary 0-3 and secondary 4-5 populated on Warblade host w/ SecondaryUnlocked):
```
PRIMARY candidates (4): IronCharge,GravityCleave,Earthsplitter,WarStomp
SECONDARY candidates (2): SunderMark,Cleave
[A] primary-pick list == ONLY primary band (4, no secondary): True
[A] secondary-pick list == ONLY secondary band (2, no primary): True
[A] FindSlotOf(primCand[0]=IronCharge) = 0 (expect 0-3)
[A] FindSlotOf(secCand[0]=SunderMark) = 4 (expect 4-5)
[A] filtered candidates still resolve to real slot: True
```
Normal Warblade all-primary replace (incoming IronCrush, choose to replace slot-2 Earthsplitter):
```
[A] normal replace: slot2 now holds 'IronCrush': True
[A] old skill removed from owned list: True · new skill owned: True
[A] IsDraftActive after successful replace = False
```

CHANGE B (OnReplaceChosen with pendingSkill==null, and pending!=null/toReplace==null):
```
[B] before: IsDraftActive=True; after null-guard: IsDraftActive=False
[B] pendingSkill after = null
[B] no softlock — teardown executed without exception: True
[B] pending!=null, toReplace==null → IsDraftActive=False pending=null
```

## Deviations / limitations
- Secondary band populated in test via real `Warblade_SkillController.UnlockSecondarySlots()` + real `SunderMark`/`Cleave` SkillBase components — so CHANGE A was verified with a genuinely populated secondary band, not just logic inference.
- Elementalist-primary path of `BuildBandReplaceCandidates` not exercised in test (PlayerClassManager.Instance null → Warblade host). Logic mirrors `HasFreePrimarySlot`'s `UseElementalistPrimary()` branch exactly (host=elemSlotController, slots 0-3) — same construction the existing `FindNextPrimarySlot` uses, so confidence is high; not independently data-proven.

## Residual risk
- Deferred (by council): Ranger/Shadowblade/Ronin still clobber slot 0 via Warblade host in `HasFree*`/`FindNext*`/`FindSlotOf`. Unreachable in demo (not IsDemoSelectable). `BuildBandReplaceCandidates` shares the same limitation for those classes (primary host = Warblade for non-Elementalist) — consistent with the rest, to be unified in the deferred ResolvePrimarySlotHost ticket.
