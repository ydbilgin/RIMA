# DONE — P2b tooltip fix-up (council fix) — 2026-06-14

Source decision: `STAGING/_process/2026-06/_council_decision_tooltip_2026-06-14.md`
Scope (surgical, as mandated): ONLY `SkillBarUI.cs` + `TooltipSystem.cs`. Read-only: `BasicAttackProfile.cs` + all `Assets/Scripts/Combat/BasicAttack/*Behavior*.cs` + Warblade/Ranger assets.

## Files written
- `Assets/Scripts/UI/SkillBarUI.cs` — CHANGE A: behaviorType-aware LMB+RMB damage. `BuildBasicAttackTooltip` now calls two new private static helpers `GetLmbTooltipDamage(profile)` / `GetRmbTooltipDamage(profile)`. Markup/format unchanged. `BuildBasicAttackTooltip` stays in SkillBarUI (encapsulation refactor DEFERRED per council). (~+40 lines net)
- `Assets/Scripts/UI/TooltipSystem.cs` — CHANGE B: `FormatSkill` description append now guarded by `if (!string.IsNullOrWhiteSpace(skill.description))` (description line + its trailing blank both omitted for name-only skills). (~+4 lines)

## behaviorType -> damage field mapping (derived by reading each behavior class)
Verified against actual behavior execution code:

| behaviorType | LMB hit field (verified) | RMB hit field (verified) |
|---|---|---|
| Melee (MeleeChainBehavior) | comboDamage[step] -> tooltip comboDamage[0] (ApplyMeleeHit) | rmbDamage (ExecuteRageOutlet AoE) |
| MarkPulse (Ravager) | comboDamage[step] -> comboDamage[0] (ApplyMeleeHitWithMarks) | rmbDamage* (Blood Pact = HP self-cost, NO enemy-damage field) |
| IaidoStance (-> MeleeChainBehavior) | comboDamage[0] | rmbDamage |
| CastRhythm (Elementalist) | projectileDamage (ExecuteBolt) | rmbDamage* (RMB = element switch / Lightbreak, NO damage field) |
| ShotCadence (Ranger) | projectileDamage (ExecuteArrow) | projectileDamage (RMB Tactical Roll -> ExecuteArrow) |
| VeilStrike (Shadowblade) | projectileDamage (ExecuteStrike) | rmbDamage* (RMB = Veil Flicker phase-step, NO damage field) |
| HeatGauge (Gunslinger) | projectileDamage (ExecuteDualFire) | projectileDamage (ExecuteHipShot base; x1.5 with heat) |

Implemented selection:
- LMB: {CastRhythm, ShotCadence, VeilStrike, HeatGauge} -> projectileDamage; else (Melee/MarkPulse/IaidoStance) -> comboDamage[0] (fallback projectileDamage if combo array empty).
- RMB: {ShotCadence, HeatGauge} -> projectileDamage; else -> rmbDamage.

*Ambiguity note (NOT blocking): MarkPulse/CastRhythm/VeilStrike RMBs deal no field-based enemy damage (HP-cost / element-switch / phase-step). No behavior-read damage field exists for them, so the tooltip falls back to the asset-configured `rmbDamage` (the field an author would set to surface a number). This is no worse than the prior behavior and is the least-surprising choice; flagged here per instructions. The two HIGH-bug classes the council called out (Ranger RMB, all ranged LMB) are now exact.

## Verification (data-proof via execute_code reflection; tooltip is overlay UI, not screenshottable)
- Compile: console 0 errors (only artifact: "Client handler error: Cannot access a disposed object" = MCP domain-reload artifact, not a compile error). editor_state = idle after compile.
- Helpers found via reflection: lmbHelperFound=True rmbHelperFound=True.
- Driven against REAL loaded assets:
  - Warblade (beh=0 Melee): combo0=25 proj=18 rmbField=34 => TOOLTIP LMB=25 RMB=34
  - Ranger (beh=2 ShotCadence): combo0=18 proj=18 rmbField=0 => TOOLTIP LMB=18 RMB=18
  - Ranger RMB non-zero? True; equals projectileDamage? True  (BUG FIXED: was 0, now 18)
- CHANGE B: FormatSkill(name-only, description="") => 3 split-elements [header, divider, trailing-newline], blankLines artifact = the divider's own line terminator only; NO description line, NO description-trailing blank. With-desc control = 5 elements / 2 blanks (description block present). Confirmed: name-only produces no blank description line.

## State discipline
- Did NOT enter Play mode. Edit-mode only (refresh_unity + execute_code). Nothing left null/polluted.

## Deferred (per council, untouched)
- BuildBasicAttackTooltip -> BasicAttackProfile.GetTooltipText(isLmb) encapsulation refactor.
- TooltipSystem.Awake Destroy(gameObject)->Destroy(this).
- Explicit per-profile tooltip damage fields.

## Residual risk
- Low. RMB tooltip for the 3 non-damaging RMB behaviors (MarkPulse/CastRhythm/VeilStrike) shows asset `rmbDamage`, which is a display convention choice not a behavior mirror (those RMBs have no damage field). If a future asset sets a misleading rmbDamage there, tooltip would show it. No Gunslinger/Ravager assets exist yet, so HeatGauge/MarkPulse paths are code-verified but not asset-verified.
