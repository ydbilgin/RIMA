# DONE — Skill-reward 2b fix-up (2026-06-14)

VERDICT: **DONE**. Council-driven two-change surgical fix. Compile clean, data-proven via reflection in play mode.

Source of truth: `STAGING/_process/2026-06/_council_decision_skillreward2b_2026-06-14.md`

## Files changed
- `Assets/Scripts/Core/ChestBehavior.cs` — `BuildChestOffers()` (~line 130-143)
- `Assets/Scripts/Skills/DraftManager.cs` — `AssignActive()` (~line 652-694)

(Read-only for signatures: `Assets/Scripts/Skills/SkillDatabase.cs`, `Assets/Scripts/Systems/PlayerClassManager.cs`)

## CHANGE A — ChestBehavior.BuildChestOffers
Replaced the inline `GetAll() + isImplemented` filter (and its wrong "to match GetPool" comment)
with `SkillDatabase.Instance.GetPool(primary, secondary)`.
- primary/secondary obtained from `PlayerClassManager.Instance` (`PrimaryClass` / `SecondaryClass`),
  with null-safe fallback to `Warblade` / `None` (matches the API's PlayerClassManager defaults).
- GetPool signature confirmed: `List<SkillData> GetPool(ClassType primary, ClassType secondary)` —
  it filters retired offers (`IsRetiredOfferSkill`) + `isImplemented` + class scope
  (`None || primary || secondary`) together.
- Added 1-line comment: room-depth/rarity gating intentionally deferred (separate, larger surface).

## CHANGE B — DraftManager.AssignActive
1. Added defensive `if (skill == null) return;` at method top.
2. Kept the existing `skillType == null` placeholder early-return + warning (now reads `skill.skillType`
   since the null check above already guarantees non-null).
3. Introduced `bool bound` set true ONLY when the SkillBase component is actually attached
   (`comp != null`) and `SetControllerSlot` ran. Gated the append:
   `if (bound && !currentActiveSkills.Contains(skill)) currentActiveSkills.Add(skill);`
   Previously the append ran unconditionally — even when `host == null` (no controller resolved)
   or component attach failed — creating a dead/empty slot entry.
- Pick FLOW untouched: FinishPick / replace / skip logic in callers unchanged. Only the
  `currentActiveSkills` bookkeeping inside AssignActive was gated.

## Verification (data-proof, play mode reflection — NOT screenshot)
Compile: **console 0 errors** (0 CS errors). One benign "objects not cleaned up when closing
the scene" notice = probe-GameObject teardown artifact at play-mode exit, not from changed code.

### CHANGE A proof — GetPool(Warblade, None)
- GetAll = **111** total skills → GetPool(Warblade, None) = **23**.
- Breakdown: Warblade=13, None(neutral)=10. offClass=0, notImpl=0, retired=0, null=0 → CLEAN.
- Cross-checks:
  - Ranger skills: inGetAll=11, **inPool=0** (off-class correctly excluded).
  - "Cleave" (retired): inGetAll=True, **inPool=False** (retired correctly excluded).

### CHANGE B proof — AssignActive bookkeeping (currentActiveSkills count)
Failure cases (must add NO dead entry), base count = 0:
- (a) skill == null → adds 0 ✓
- (b) skillType == null (placeholder) → adds 0 ✓
- (c) host == null (slot>=4, skillController null) → adds 0 ✓ (bindable type = IronCharge)
- deadEntriesAdded = **0**.
Positive case (must bind + add):
- (d) valid "Iron Charge" (slot>=4, real Warblade_SkillController host) → list 0→1, added=True,
  SkillBase actually attached on host=True ✓

## Deviations from spec
None. Both changes match the council DECISION exactly; deferred items (room-depth/rarity gating,
SkillRuntime DRY) left untouched as instructed.

## Residual risk
- Low. Behavioral change is strictly narrowing: chest can no longer offer off-class/retired/
  placeholder skills, and AssignActive never records an unbound skill. Normal draft already used
  GetPool, so chest now matches it.
- The `bound` gate means if a player somehow picks a skill while no valid host exists (e.g. wrong
  scene state), the skill is silently NOT recorded rather than producing a dead slot — this is the
  intended council outcome (FinishPick still runs in callers, slot stays usable).
- Deferred (noted, not done): chest room-depth/rarity gating via SkillOfferGenerator.
